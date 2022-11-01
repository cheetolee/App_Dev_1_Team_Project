using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using BusinessLayer;
using Inventory_Management.Model;
using EntityLayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using Inventory_Management.Service;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace Inventory_Management.ViewModel
{
    class NewTransactionViewModel : EditItemModel<TransactionHeadListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NewTransactionViewModel() : base(null, true, null)
        {
            this.TransactionBody.CollectionChanged += this.OnCollectionChanged;

            List<UserEntity> Users = UserLogin.ListUsers();

            if (SelectedUser == null)
                foreach (var record in Users.Where(p => p.Username == UserLogin.LoginedUser))
                    SelectedUser = record;

            SelectedProductCategory = new ProductCategoryEntity() { Category = " - All product categories - ", Id = 0 };
            ProductCategories.Add(SelectedProductCategory);

            ProductCategories.AddRange(ManageProducts.ListProductCategories());
        }

   
        // Transaction date
        private DateTime _transactionDate;
        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { SetProperty(ref _transactionDate, value); }
        }

        private UserEntity _selectedUser;
        public UserEntity SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        // Products
        private List<ProductCategoryEntity> _productCategories;


        public List<ProductCategoryEntity> ProductCategories
        {
            get
            {
           
                    if (_productCategories == null) _productCategories = new List<ProductCategoryEntity>();
                    return _productCategories;
                
            }
            set { SetProperty(ref _productCategories, value); }
        }

        private ProductCategoryEntity _selectedProductCategory;
        public ProductCategoryEntity SelectedProductCategory
        {
            get { return _selectedProductCategory; }
            set
            {
                if (_selectedProductCategory != value) Products = ManageProducts.ListProducts(value.Id);
                SetProperty(ref _selectedProductCategory, value);
            }
        }

        private List<ProductListEntity> _products;
        public List<ProductListEntity> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        private ProductListEntity _selectedProduct;
        public ProductListEntity SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (value != null) ProductPrice = value.Price;
                SetProperty(ref _selectedProduct, value);
            }
        }

        private decimal _productQuantity;
        public decimal ProductQuantity
        {
            get { return _productQuantity; }
            set { SetProperty(ref _productQuantity, value); }
        }

        private decimal _productPrice;
        public decimal ProductPrice
        {
            get { return _productPrice; }
            set { SetProperty(ref _productPrice, value); }
        }

        // Transaction body
        private ObservableCollection<BindableTransactionBodyListEntity> _transactionBody;
        public ObservableCollection<BindableTransactionBodyListEntity> TransactionBody
        {
            get
            {
                if (_transactionBody == null) _transactionBody = new ObservableCollection<BindableTransactionBodyListEntity>();
                return _transactionBody;
            }
            set { SetProperty(ref _transactionBody, value); }
        }

        private BindableTransactionBodyListEntity _selectedBody;
        public BindableTransactionBodyListEntity SelectedBody
        {
            get { return _selectedBody; }
            set { SetProperty(ref _selectedBody, value); }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            decimal sum = 0m;
            foreach (var record in TransactionBody)
                sum += record.SumPrice;
            TotalPrice = sum;
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }

        private ICommand _addProductCommand;
        public ICommand AddProductCommand
        {
            get
            {
                if (_addProductCommand == null) _addProductCommand = new RelayCommand(new Action<object>(AddProduct), new Predicate<object>(CanAddProduct));
                return _addProductCommand;
            }
            set { SetProperty(ref _addProductCommand, value); }
        }
        private void AddProduct(object parameter)
        {
            var rec = new TransactionBodyListEntity();
            rec.Product = new ProductEntity();
            rec.Product.Id = SelectedProduct.Id;
            rec.Product.Name = SelectedProduct.Name;
            rec.Product.Code = SelectedProduct.Code;
            rec.Product.Price = SelectedProduct.Price;
            rec.Body = new TransactionBodyEntity();
            rec.Body.ProductId = SelectedProduct.Id;
            rec.Body.Price = ProductPrice;
            rec.Body.Quantity = ProductQuantity;
            TransactionBody.Add(new BindableTransactionBodyListEntity(rec));
        }
        private bool CanAddProduct(object parameter)
        {
            return (SelectedProduct != null);
        }

        private ICommand _removeProductCommand;

        public ICommand RemoveProductCommand
        {
            get
            {
                if (_removeProductCommand == null) _removeProductCommand = new RelayCommand(new Action<object>(RemoveProduct), new Predicate<object>(CanRemoveProduct));
                return _removeProductCommand;
            }
            set { SetProperty(ref _removeProductCommand, value); }
        }
        private void RemoveProduct(object parameter)
        {
            TransactionBody.Remove(SelectedBody);
        }
        private bool CanRemoveProduct(object parameter)
        {
            return (SelectedBody != null);
        }

     
        protected override bool Save(object parameter)
        {
            if (SelectedUser == null)
            {
                log.Error("Save transaction error Pleace select a user.");
                MessageBox.Show("Pleace select a user", "Save transaction error ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                TransactionHeadListEntity Item = new TransactionHeadListEntity();
                Item.Head.Username = SelectedUser.Username;
                Item.Head.TotalPrice = TotalPrice;
                Item.Head.Date = TransactionDate;
                Item.User = SelectedUser;
                var list = new List<TransactionBodyListEntity>();
                foreach (var record in TransactionBody)
                    list.Add(record.Item);
                bool result = ManageTransactions.AddOrModifyTransaction(Item.Head, list);
                if (result)
                {
                    MessageBox.Show("Order create", "Save transaction success ",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    EmailConfirmation.sendOrderConfirmation(SelectedUser.Email);
                }
                return result;
            }
        }
    }
}
