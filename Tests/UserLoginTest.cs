using BusinessLayer;
using System;
using System.Windows;
namespace Tests
{
    [TestClass]
    public class UserLoginTest

    {
        [TestMethod]
        public void IsValidUserIdTest()
        {
           
            bool result = UserLogin.IsValidUserID("1234");
            Assert.AreEqual(true, result);
        }
    }
}