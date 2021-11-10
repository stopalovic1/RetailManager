using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppDesktopUI.Library.Api;
using WpfAppDesktopUI.Library.Helpers;
using WpfAppDesktopUI.Library.Models;

namespace WpfAppDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndpoint _productEndpoint;
        IConfigHelper _confgiHelper;
        ISaleEndpoint _saleEndpoint;
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper,ISaleEndpoint saleEndpoint)
        {
            _productEndpoint = productEndpoint;
            _confgiHelper = configHelper;
            _saleEndpoint = saleEndpoint;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }
        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }



        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private CartItemModel _selectedCartProduct;

        public CartItemModel SelectedCartProduct
        {
            get { return _selectedCartProduct; }
            set
            {
                _selectedCartProduct = value;
                NotifyOfPropertyChange(() => SelectedCartProduct);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }



        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }
        private decimal CalculateSubTotal()
        {
            decimal subtotal = 0;


            foreach (var item in Cart)
            {
                subtotal += item.Product.RetailPrice * item.QuantityInCart;
            }
            return subtotal;
        }
        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _confgiHelper.GetTaxRate() / 100;
            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);
            /*foreach (var item in Cart)
            {
                taxAmount += item.Product.RetailPrice * item.QuantityInCart * taxRate;
            }*/
            return taxAmount;
        }
        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                if (SelectedProduct?.QuantityInStock >= ItemQuantity && ItemQuantity > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.ResetBindings();
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };

                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            Products.ResetBindings();
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Products);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                if (SelectedCartProduct != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public void RemoveFromCart()
        {
            if (SelectedCartProduct != null)
            {
                var cartItem = Cart.FirstOrDefault(x => x.Product == SelectedCartProduct.Product);

                if (cartItem.QuantityInCart > 1)
                {
                    cartItem.QuantityInCart -= 1;
                }
                else
                {
                    Cart.Remove(cartItem);
                }
                Products.FirstOrDefault(x => x == cartItem.Product).QuantityInStock += 1;
                Cart.ResetBindings();
                Products.ResetBindings();
                NotifyOfPropertyChange(() => SubTotal);
                NotifyOfPropertyChange(() => Tax);
                NotifyOfPropertyChange(() => Total);
                NotifyOfPropertyChange(() => CanCheckOut);
            }
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                if(Cart.Count>0)
                {
                    output = true;
                }
                return output;
            }
        }

        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();
            foreach(var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel 
                {
                    ProductId=item.Product.Id,
                    Quantity=item.QuantityInCart
                });
            }
            await _saleEndpoint.PostSale(sale);
        }
    }
}
