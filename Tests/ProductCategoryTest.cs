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

        [DataRow("fruits")]
        [TestMethod]
        public void AddCategory_Should_Success(string name)
        {
            ProductCategoryEntity Item = new ProductCategoryEntity();
            Item.Category = name;
        }

        [TestMethod]
        public void ListProductCategories_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.ListProductCategories());
        }

        [DataRow("fruits", 3)]
        [TestMethod]
        public void NewProductCategory_Should_Success(string category, int id)
        {
            ProductCategoryEntity productCategoryEntity = new ProductCategoryEntity();
            productCategoryEntity.Category = category;
            productCategoryEntity.Id = id;
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.NewProductCategory(productCategoryEntity));
        }

        [DataRow("", 3)]
        [TestMethod]
        public void NewProductCategoryEmpty_Should_Fail(string category, int id)
        {
            ProductCategoryEntity productCategoryEntity = new ProductCategoryEntity();
            productCategoryEntity.Category = category;
            productCategoryEntity.Id = id;
            Assert.IsFalse(ManageProducts.NewProductCategory(productCategoryEntity));
        }

        [TestMethod]
        public void DeleteProductCategory_Should_Success()
        {
            ProductCategoryEntity productCategoryEntity = new ProductCategoryEntity();
            productCategoryEntity.Category = "fruits";
            productCategoryEntity.Id = 3;
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.DeleteProductCategory(productCategoryEntity));
        }

        [DataRow("", 3)]
        [TestMethod]
        public void DeleteProductCategoryEmpty_Should_Fail(string category, int id)
        {
            ProductCategoryEntity productCategoryEntity = new ProductCategoryEntity();
            productCategoryEntity.Category = category;
            productCategoryEntity.Id = id;
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.DeleteProductCategory(productCategoryEntity));
        }

        [TestMethod]
        public void ModifyProductCategory_Should_Success()
        {
            ProductCategoryEntity productCategoryEntity = new ProductCategoryEntity();
            productCategoryEntity.Category = "fruits";
            productCategoryEntity.Id = 3;
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.ModifyProductCategory(productCategoryEntity));
        }

        [DataRow("", 3)]
        [TestMethod]
        public void ModifyProductCategoryEmpty_Should_Fail(string category, int id)
        {
            ProductCategoryEntity productCategoryEntity = new ProductCategoryEntity();
            productCategoryEntity.Category = category;
            productCategoryEntity.Id = id;
            Assert.IsFalse(ManageProducts.ModifyProductCategory(productCategoryEntity));
        }
    }
}