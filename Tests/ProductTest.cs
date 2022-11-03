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

      
        
        [TestMethod]
        public void AddProduct_Should_Fail_For_InvalidData()
        {
            

        }

    }
}