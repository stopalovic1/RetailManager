using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            decimal output = 0;
            string rateText = ConfigurationManager.AppSettings["taxRate"];
            bool isValid = Decimal.TryParse(rateText, out output);
            if (isValid == false)
            {
                throw new ConfigurationErrorsException("Taksa nije postavljnea ispravno");
            }
            return output;
        }
    }
}
