using System.Collections.Generic;
using System.Net.Mail;
using DataLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class UserLogin
    {
        private static string _googleUser;
        public static string GoogleUser
        {
            get
            {
                if (_googleUser == null) _googleUser = "";
                return _googleUser;
            }
        }

        private static string _loginedUser;
        public static string LoginedUser
        {
            get
            {
                if (_loginedUser == null) _loginedUser = "";
                return _loginedUser;
            }
        }

        private static bool _loginedAdmin = false;
        public static bool LoginedAdmin
        {
            get
            {
                return _loginedAdmin;
            }
        }

       
        private static bool IsValidEmail(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }

        public static bool IsEmptyUserDatabase()
        {
            return UsersProvider.IsEmptyUserDatabase();
        }
        public static bool IsValidUserID(string userID)
        {

            return UsersProvider.IsValidUserID(userID);
        }
        public static bool IsValidPassword(string userID, string password)
        {
            return UsersProvider.IsValidPassword(userID, password);
        }
        public static bool AddUser(string firstname, string lastname, string address, string email, string phone, string userID, string password)
        {
            if (string.IsNullOrWhiteSpace(firstname))
            {
                throw new System.ArgumentException("Wrong firsntmae.", "firsntmae");
            }
            if (string.IsNullOrWhiteSpace(lastname))
            {
                throw new System.ArgumentException("Wrong lastname.", "lastname");
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new System.ArgumentException("Wrong address.", "address");
            }
            if (string.IsNullOrWhiteSpace(phone) || phone.Length != 10)
            {
                throw new System.ArgumentException("Wrong phone. Phone should have length equal at 10", "phone");
            }

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                throw new System.ArgumentException("Wrong email. Email is not valid", "email");
            }
            if (string.IsNullOrWhiteSpace(userID) || userID.Length < 5)
            {
                throw new System.ArgumentException("Wrong username. Min length for username is 5", "userID");
            }
            else if (string.IsNullOrWhiteSpace(password) || password.Length < 5)
            {
                throw new System.ArgumentException("Wrong password. Min length for username is 5", "password");
            }
            return UsersProvider.NewUser( firstname,  lastname,  address,  email,  phone, userID, password);
        }
        public static bool RemoveUser(string userID)
        {
            return UsersProvider.DeleteUser(userID);
        }

        public static void LoginGoogle(string userId)
        {
            _googleUser = userId;
          
        }

        public static void Login(string userID, string password)
        {
            if (userID.Equals("admin") && password.Equals("admin"))
            {
                _loginedAdmin = true;
                _loginedUser = userID;

            }
            else if (string.IsNullOrWhiteSpace(userID) || !UserLogin.IsValidUserID(userID))
            {
                throw new System.ArgumentException("Wrong username.", "userID");
            }
            else if (string.IsNullOrWhiteSpace(password) || !UserLogin.IsValidPassword(userID, password))
            {
                throw new System.ArgumentException("Wrong password.", "password");
            }
            _loginedUser = userID;
        }
        public static List<UserEntity> ListUsers()
        {
            return UsersProvider.ListUsers();
        }

        public static bool ModifyUser(string oldUserID, string oldPassword, string newUserId, string password, string confirm)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new System.ArgumentException("Wrong password.", "password");
            }
            else if (string.IsNullOrWhiteSpace(confirm) || password != confirm)
            {
                throw new System.ArgumentException("Wrong confirm password.", "confirm");
            }
            else if (string.IsNullOrWhiteSpace(oldUserID) || !UserLogin.IsValidUserID(oldUserID))
            {
                throw new System.ArgumentException("Wrong old username", "oldUserID");
            }
            else if (string.IsNullOrWhiteSpace(oldPassword) || !UserLogin.IsValidPassword(oldUserID, oldPassword))
            {
                throw new System.ArgumentException("Wrong old password.", "oldPassword");
            }
            else if (string.IsNullOrWhiteSpace(newUserId) || (oldUserID != newUserId && UserLogin.IsValidUserID(newUserId)))
            {
                throw new System.ArgumentException("Wrong new username.", "newUserId");
            }
            else if (UsersProvider.Modify(oldUserID, newUserId, password))
            {
                if (_loginedUser == oldUserID) _loginedUser = newUserId;
                return true;
            }
            else
                return false;
        }
    }
}
