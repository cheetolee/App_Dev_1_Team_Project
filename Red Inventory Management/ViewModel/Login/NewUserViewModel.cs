using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using BusinessLayer;
using Inventory_Management.Model;
using Inventory_Management.Service;
using MessageBox = System.Windows.Forms.MessageBox;


namespace Inventory_Management.ViewModel
{
    class NewUserViewModel : BindableBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string _firstname;
        private string _lastname;
        private string _address;
        private string _phone;
        private string _email;
        private string _userID;
        private string _password;
        private string _confirm;
        public Window NewUserWindow { get; set; }

        public string Firstname
        {
            get
            {
                if (_firstname == null) _firstname = "";
                return _firstname;
            }
            set { SetProperty(ref _firstname, value); }
        }

        public string Lastname
        {
            get
            {
                if (_lastname == null) _lastname = "";
                return _lastname;
            }
            set { SetProperty(ref _lastname, value); }
        }

        public string Address
        {
            get
            {
                if (_address == null) _address = "";
                return _address;
            }
            set { SetProperty(ref _address, value); }
        }


        public string Phone
        {
            get
            {
                if (_phone == null) _phone = "";
                return _phone;
            }
            set { SetProperty(ref _phone, value); }
        }

        public string Email
        {
            get
            {
                if (_email == null) _email = "";
                return _email;
            }
            set { SetProperty(ref _email, value); }
        }


        public string UserID
        {
            get
            {
                if (_userID == null) _userID = "";
                return _userID;
            }
            set { SetProperty(ref _userID, value); }
        }
        public string Password
        {
            get
            {
                if (_password == null) _password = "";
                return _password;
            }
            set { SetProperty(ref _password, value); }
        }
        public string Confirm
        {
            get
            {
                if (_confirm == null) _confirm = "";
                return _confirm;
            }
            set { SetProperty(ref _confirm, value); }
        }

        private ICommand _click_AddUserCommand;

        public ICommand Click_AddUserCommand
        {
            get
            {
                if (_click_AddUserCommand == null) _click_AddUserCommand = new RelayCommand(new Action<object>(AddUser));
                return _click_AddUserCommand;
            }
            set { SetProperty(ref _click_AddUserCommand, value); }
        }
        private void AddUser(object parameter)
        {
            log.Debug("Add user button");

            if (Password != Confirm)
            {
                log.Error("New user error : Password does not match the confirm password.");
            }
            else
            {
                try
                {
                    if (UserLogin.AddUser(Firstname,Lastname,Address,Phone,Email,UserID, Password))
                    {
                        log.Info("New user added:" + String.Format("Username: {0}", UserID));
                        MessageBox.Show("Account created", "Registration success",
                                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                        EmailConfirmation.sendRegistrationConfirmation(Email);
                        NewUserWindow.Close();
                    }
                    else
                    {
                        log.Error("New user error: Username already exist.");
                        MessageBox.Show("Username already exist", "Registration fail",
                                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                catch(Exception e)
                {
                    log.Error("New user error : Please fill the Username and Password fieleds.");
                    MessageBox.Show(e.Message, "Registration fail",
                                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
