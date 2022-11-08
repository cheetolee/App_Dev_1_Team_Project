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
    public class TransactionTest
    {

        [TestInitialize()]
        public void Startup()
        {
          
        }

        [TestMethod]
        public void ListHead_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ManageTransactions.ListHead());
        }

        [TestMethod]
        public void ListHeadUser_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ManageTransactions.ListHead("john"));
        }

        [TestMethod]
        public void ListBody_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ManageTransactions.ListBody(12345));
        }

        [TestMethod]
        public void AddModifyTransaction_Should_Success()
        {
            TransactionHeadEntity transactionHeadEntity = new TransactionHeadEntity();
            List<TransactionBodyListEntity> body = new List<TransactionBodyListEntity>();
            Assert.ThrowsException<InvalidOperationException>(() => ManageTransactions.AddOrModifyTransaction(transactionHeadEntity, body));
        }

        [TestMethod]
        public void RemoveTransaction_Should_Success()
        {
            TransactionHeadEntity transactionHeadEntity = new TransactionHeadEntity();
            Assert.ThrowsException<InvalidOperationException>(() => ManageTransactions.RemoveTransaction(transactionHeadEntity));
        }

        [TestMethod]
        public void ListInventory_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ManageTransactions.ListInventory());
        }

        [TestMethod]
        public void ListInventoryDetails_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ManageTransactions.ListInventoryDetails(1));
        }

        [TestMethod]
        public void ListPartnerTransactions_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => ManageTransactions.ListPartnerTransactions());
        }

    }
}