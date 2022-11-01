using System;
using System.IO;
using System.Windows;
using BusinessLayer;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using System.Collections.Generic;

namespace Tests
{

    [TestClass]
    public class UserLoginTest
    {
        public UserLogin userLogin;

        [TestInitialize()]
        public void Startup()
        {
             userLogin = new UserLogin();
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
        [DataRow("john", "Doe", "15500 peltrie", "tuto@gmail.com", "4389448978", "username", "password")]
        [DataTestMethod]
        public void AddUser_Should_Fail_For_InvalidData(string fn, String ln, String ad, String em, String ph, String uname, String pwd )
        {

            Assert.ThrowsException<ArgumentException>(() => UserLogin.AddUser(fn, ln, ad, em, ph, uname, pwd));

        }

        [DataRow("","")]
        [DataRow("test","")]
        [DataRow("test","test")]
        [DataTestMethod]
        public void UserLogin_Should_Fail_For_InvalidCredentials(string username, String password)
        {
            var UserLoginMock = new Mock<UserLogin>();

            UserLoginMock.Setup(u => UserLogin.IsValidUserID(username)).Returns(false);
            Assert.ThrowsException<ArgumentException>(() => UserLogin.Login(username,password));

        }
 
        [TestMethod]
        public void UserLogin_Should_Success_For_ValidCredentials()
        {
            String username = "admin";
            String password = "admin";

            UserLogin.Login(username, password);

            Assert.IsTrue(UserLogin.LoginedUser == username);

        }
    }
}