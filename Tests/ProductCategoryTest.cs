using System;
using System.IO;
using System.Windows;
using BusinessLayer;
using Inventory_Management;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using System.Collections.Generic;
using Inventory_Management.ViewModel;
using EntityLayer;
using Inventory_Management.Model;

namespace Tests
{

    [TestClass]
    public class ProductCategoryTest
    {
        public EditProductCategoryViewModel EPVM;
       
        [TestInitialize()]
        public void Startup()
        {
         
        }


        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public void AddCategory_Should_Fail_For_InvalidName(string name)
        {
            ProductCategoryEntity Item = new ProductCategoryEntity();
            Item.Category = name;
            EPVM = new EditProductCategoryViewModel(Item, true, "ProdCategory");


        }




    }
}