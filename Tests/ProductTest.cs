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
    public class ProductTest
    {
        public ProductListEntity p1;

        [TestInitialize()]
        public void Startup()
        {
          
        }


        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public void AddProduct_Should_Fail_For_InvalidData(string name)
        {
            ProductEntity Item = new ProductEntity();
            Item.Name = name;
            p1 = new ProductListEntity(Item);
        }

        [TestMethod]
        public void AddProduct_Should_Success()
        {
            ProductEntity Item = new ProductEntity();
            Item.Id = 1;
            Item.Code = "E4r";
            Item.Name = "tomato";
            Item.Quantity = 5;
            Item.CategoryId = 2;
            Item.Price = Convert.ToDecimal(20);
            p1 = new ProductListEntity(Item);
        }

        [TestMethod]
        public void ListProducts_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.ListProducts(0));
        }

        [DataRow("2", "", 0, "", 0, 0)]
        [DataRow("2", "R4t", 0, "", 0, 0)]
        [DataRow("2", "R4t", 1, "", 0, 0)]
        [DataRow("2", "R4t", 1, "name", 0, 0)]
        [DataRow("2", "R4t", 1, "name", 20, 0)]
        [DataRow("2", "R4t", 1, "name", 20, 5)]
        [TestMethod]
        public void NewProduct_Should_Fail(string category, string code, int id, string name, int price, int quantity)
        {
            ProductListEntity productListEntity = new ProductListEntity();
            productListEntity.Category = category;
            productListEntity.Code = code;
            productListEntity.Id = id;
            productListEntity.Name = name;
            productListEntity.Price = price;
            productListEntity.Quantity = quantity;
            Assert.IsFalse(ManageProducts.NewProduct(productListEntity));
        }

        [DataRow("", "", 0, "", 0, 0)]
        [TestMethod]
        public void NewProductEmpty_Should_Fail(string category, string code, int id, string name, int price, int quantity)
        {
            ProductListEntity productListEntity = new ProductListEntity();
            productListEntity.Category = category;
            productListEntity.Code = code;
            productListEntity.Id = id;
            productListEntity.Name = name;
            productListEntity.Price = price;
            productListEntity.Quantity = quantity;
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.NewProduct(productListEntity));
        }

        [TestMethod]
        public void DeleteProduct_Should_Success()
        {
            ProductListEntity productListEntity = new ProductListEntity();
            productListEntity.Category = "2";
            productListEntity.Code = "R4t";
            productListEntity.Id = 1;
            productListEntity.Name = "name";
            productListEntity.Price = 20;
            productListEntity.Quantity = 5;
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.DeleteProduct(productListEntity));
        }

        [DataRow("2", "", 0, "", 0, 0)]
        [DataRow("2", "R4t", 0, "", 0, 0)]
        [DataRow("2", "R4t", 1, "", 0, 0)]
        [DataRow("2", "R4t", 1, "name", 0, 0)]
        [DataRow("2", "R4t", 1, "name", 20, 0)]
        [DataRow("2", "R4t", 1, "name", 20, 5)]
        [TestMethod]
        public void ModifyProduct_Should_Fail(string category, string code, int id, string name, int price, int quantity)
        {
            ProductListEntity productListEntity = new ProductListEntity();
            productListEntity.Category = category;
            productListEntity.Code = code;
            productListEntity.Id = id;
            productListEntity.Name = name;
            productListEntity.Price = price;
            productListEntity.Quantity = quantity;
            Assert.IsFalse(ManageProducts.ModifyProduct(productListEntity));
        }

        [DataRow("", "", 0, "", 0, 0)]
        [TestMethod]
        public void ModifyProductEmpty_Should_Fail(string category, string code, int id, string name, int price, int quantity)
        {
            ProductListEntity productListEntity = new ProductListEntity();
            productListEntity.Category = category;
            productListEntity.Code = code;
            productListEntity.Id = id;
            productListEntity.Name = name;
            productListEntity.Price = price;
            productListEntity.Quantity = quantity;
            Assert.ThrowsException<InvalidOperationException>(() => ManageProducts.ModifyProduct(productListEntity));
        }

        [TestMethod]
        public void ModifyProduct_Should_Success()
        {
            ProductListEntity productListEntity = new ProductListEntity();
            productListEntity.Category = "2";
            productListEntity.Code = "R4t";
            productListEntity.Id = 1;
            productListEntity.Name = "name";
            productListEntity.Price = 20;
            productListEntity.Quantity = 5;
            Assert.IsFalse(ManageProducts.ModifyProduct(productListEntity));
        }



    }
}