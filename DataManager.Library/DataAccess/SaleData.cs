using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.DataAccess
{
    public class SaleData
    {
        private readonly IConfiguration _config;

        public SaleData(IConfiguration config)
        {
            _config = config;
        }

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData(_config);
            var taxRate = ConfigHelper.GetTaxRate() / 100;
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                var productInfo = products.GetProductById(item.ProductId);
                if (productInfo == null)
                {
                    throw new Exception("Product could not be found");
                }
                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;
                if (productInfo.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * taxRate;
                }
                details.Add(detail);
            }
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;

            using (SqlDataAccess sql = new SqlDataAccess(_config))
            {
                try
                {
                    sql.StartTransaction("Database1");

                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);

                    }
                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }

            }

        }

        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "Database1");
            return output;

        }
    }
}
