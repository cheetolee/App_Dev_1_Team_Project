using System;
using System.IO;
using System.Windows;
using BusinessLayer;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using EntityLayer;
using System.Net;
using DataLayer;

namespace Tests
{

    [TestClass]
    public class UserTest
    {
        public UserLogin userLogin;

        [TestInitialize()]
        public void Startup()
        {
             userLogin = new UserLogin();
        }

        [DataRow("", "")]
        [DataRow("", "test")]
        [DataTestMethod]
        public void UserLogin_Should_Fail_For_InvalidCredentials(string username, string password)
        {
            Assert.ThrowsException<ArgumentException>(() => UserLogin.Login(username, password));

        }

        [DataRow("test", "test")]
        [DataRow("test", "")]
        [DataTestMethod]
        public void UserLogin_Should_Fail(string username, string password)
        {
            Assert.ThrowsException<InvalidOperationException>(() => UserLogin.Login(username, password));
        }

        [TestMethod]
        public void UserLogin_Should_Success_For_ValidCredentials()
        {
            string username = "admin";
            string password = "admin";

            UserLogin.Login(username, password);

            Assert.IsTrue(UserLogin.LoginedUser == username);

        }

        [DataRow("", "", "", "","","", "")]
        [DataRow("john", "", "", "", "", "", "")]
        [DataRow("john", "Doe", "", "", "", "", "")]
        [DataRow("john", "Doe", "15500 peltrie", "", "", "", "")]
        [DataRow("john", "Doe", "15500 peltrie", "tuto@gmail.com", "", "", "")]
        [DataRow("john", "Doe", "15500 peltrie", "tutotete", "", "", "")]
        [DataRow("john", "Doe", "15500 peltrie", "tuto@gmail.com", "438", "", "")]
        [DataRow("john", "Doe", "15500 peltrie", "tuto@gmail.com", "4389448978", "", "")]
        [DataRow("john", "Doe", "15500 peltrie", "tuto@gmail.com", "4389448978", "546", "")]
        [DataRow("john", "Doe", "15500 peltrie", "tuto@gmail.com", "4389448978", "username", "")]
        [DataRow("john", "Doe", "15500 peltrie", "tuto@gmail.com", "4389448978", "username", "435")]
        [DataTestMethod]
        public void AddUser_Should_Fail(string fn, string ln, string ad, string em, string ph, string uname, string pwd )
        {
            Assert.ThrowsException<ArgumentException>(() => UserLogin.AddUser(fn, ln, ad, em, ph, uname, pwd));
        }

        [TestMethod]
        public void DatabaseConnection_Should_Fail()
        {
            DataLayer.DatabaseConnection.InitializeConnection("E:\\Personal Documents\\Personal Projects\\Grocery_App_K_CIM-master(1)\\Grocery_App_K_CIM-master\\Red Inventory Management\\bin\\Debug\\", "Database");
            Assert.IsFalse(BusinessLayer.DatabaseConnection.TestConnection());
        }

        [TestMethod]
        public void DatabaseEmpty_Should_Fail()
        {
            Assert.ThrowsException<InvalidOperationException>(() => UserLogin.IsEmptyUserDatabase());
        }

        [TestMethod]
        public void ValidEmail_Should_Success()
        {
            Assert.IsTrue(UserLogin.IsValidEmail("test@test.com"));
        }

        [TestMethod]
        public void ValidEmail_Should_Fail()
        {
            Assert.IsFalse(UserLogin.IsValidEmail("test"));
        }

        [TestMethod]
        public void ValidUser_Should_Fail()
        {
            Assert.ThrowsException<InvalidOperationException>(() => UserLogin.IsValidUserID("john"));
        }

        [DataRow("", "")]
        [DataRow("john", "")]
        [DataRow("", "john")]
        [DataRow("john", "john")]
        [TestMethod]
        public void ValidPassword_Should_Fail(string username, string password)
        {
            Assert.ThrowsException<InvalidOperationException>(() => UserLogin.IsValidPassword(username, password));
        }

        [TestMethod]
        public void EmptyEmail_Should_Success()
        {
            Assert.ThrowsException<NotImplementedException>(() => UserLogin.IsValidEmail());
        }

        [TestMethod]
        public void DeleteUser_Should_Fail()
        {
            Assert.ThrowsException<InvalidOperationException>(() => UserLogin.RemoveUser("john"));
        }

        [TestMethod]
        public void SetGoogleLogin_Should_Success()
        {
            UserLogin.LoginGoogle("john");
            Assert.IsTrue(UserLogin.GoogleUser == "john");
        }

        [TestMethod]
        public void ListUsers_Should_Fail()
        {
            Assert.ThrowsException<InvalidOperationException>(() => UserLogin.ListUsers());
        }

        [DataRow("", "", "", "", "")]
        [DataRow("", "john", "john", "", "john")]
        [DataRow("", "john", "john", "john", "")]
        [DataRow("", "john", "john", "john", "doe")]
        [DataRow("", "john", "john", "john", "john")]
        [DataRow("", "john", "", "john", "john")]
        [DataTestMethod]
        public void ModifyUser_Should_Fail(string oldUserID, string oldPassword, string newUserId, string password, string confirm)
        {
            Assert.ThrowsException<ArgumentException>(() => UserLogin.ModifyUser(oldUserID, oldPassword, newUserId, password, confirm));
        }

        [DataRow("john", "john", "john", "john", "john")]
        [DataTestMethod]
        public void ModifyUserException_Should_Fail(string oldUserID, string oldPassword, string newUserId, string password, string confirm)
        {
            Assert.ThrowsException<InvalidOperationException>(() => UserLogin.ModifyUser(oldUserID, oldPassword, newUserId, password, confirm));
        }


        [TestMethod]
        public void AddUser_Should_Success()
        {
            Assert.ThrowsException<InvalidOperationException>(() => UserLogin.AddUser("john", "Doe", "15500 peltrie", "4389448978", "tuto@gmail.com" , "username", "password"));
        }

        
    }
}