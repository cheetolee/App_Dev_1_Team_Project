using System;
using Inventory_Management.Model;
using System.Windows.Input;
using System.Windows.Controls;
using BusinessLayer;
using System.Windows;

namespace Red_Inventory_Management.ViewModel
{
    class EditUserViewModel : BindableBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EditUserViewModel() { }
        public EditUserViewModel(string oldUserID) { this._oldUserID = oldUserID; }

        private Window _editWindow;
        public Window EditWindow
        {
            get { return _editWindow; }
            set { SetProperty(ref _editWindow, value); }
        }

        private string _oldUserID;
        private string _newUserID;
        private string _password;
        private string _confirm;
        public string UserID
        {
            get
            {
                if (_newUserID == null) _newUserID = _oldUserID;
                return _newUserID;
            }
            set { SetProperty(ref _newUserID, value); }
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

        private ICommand _modifyUserCommand;
        public ICommand ModifyUserCommand
        {
            get
            {
                if (_modifyUserCommand == null) _modifyUserCommand = new RelayCommand(new Action<object>(ModifyUser));
                return _modifyUserCommand;
            }
            set { SetProperty(ref _modifyUserCommand, value); }
        }
        private void ModifyUser(object parameter)
        {
            log.Debug("Modify user button");

            PasswordBox pwBox = (PasswordBox)parameter;
            string OldPassword = pwBox.Password;

            if (string.IsNullOrWhiteSpace(_oldUserID) ||
                string.IsNullOrWhiteSpace(OldPassword) ||
                string.IsNullOrWhiteSpace(UserID) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Confirm))
            {
                MessageBox.Show("Please fill the Username and Password fieleds.","Edit user error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    if (UserLogin.ModifyUser(_oldUserID, OldPassword, UserID, Password, Confirm))
                    {

                        MessageBox.Show(String.Format("New username: {0}", UserID), String.Format("User modified: {0}", _oldUserID), MessageBoxButton.OK, MessageBoxImage.Information);
                        EditWindow?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Database error", "Edit user error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                catch (ArgumentException e)
                {
                    switch (e.ParamName)
                    {
                        case "oldUserID":
                            MessageBox.Show("The original username is missing from the database.", "Edit user error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case "oldPassword":
                            MessageBox.Show("The old password is wrong.", "Edit user error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case "newUserId":
                            MessageBox.Show("The new username already exist.", "Edit user error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case "password":
                            MessageBox.Show("Please fill the password field.", "Edit user error",  MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case "confirm":
                            MessageBox.Show("Password does not match the confirm password.", "Edit user error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        default:
                            MessageBox.Show("UserLogin error", "Edit user error",  MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }

            }
        }
    }
}
