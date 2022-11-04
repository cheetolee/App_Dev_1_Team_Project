using System;
using Inventory_Management.Model;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;
using Inventory_Management.Views;
using Inventory_Management.ViewModel;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Inventory_Management.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructors
        public MainWindowViewModel(Window mainwindow)
        {
            log.Debug(">>> Program started. <<<");

            MainWindow = mainwindow;

            try
            {
                // Set database connection.
                if (!DatabaseConnection.TestConnection())
                {
                    log.Debug("Can't connect to database. => Opening connection setup window.");
                    SetupConnectionWindow SCW = new SetupConnectionWindow();
                    SCW.ShowDialog();
                    if (!DatabaseConnection.TestConnection())
                    {
                        log.Debug("Database connection wasn't set.");
                        CloseWindow();
                        return;
                    }
                }

                // New user.
              /*  if (UserLogin.IsEmptyUserDatabase())
                {
                    log.Debug("User datatable is empty. => Opening new user window.");
                    NewUserWindow NUW = new NewUserWindow();
                    NUW.ShowDialog();
                    if (UserLogin.IsEmptyUserDatabase())
                    {
                        log.Debug("User wasn't added.");
                        CloseWindow();
                        return;
                    }
                }*/

                // Login.
                log.Debug("Opening login window.");
                LoginWindow LW = new LoginWindow();
                LW.ShowDialog();
                if (UserLogin.LoginedUser == "")
                {
                    log.Debug("Not logged in.");
                    CloseWindow();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Program initialization error.", ex);
                MessageBox.Show("Program initialization error.\nCheck InventoryManagementLog.txt for more information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CloseWindow();
                return;
            }
        }
        #endregion
        #region Menus and Views
        private string[] _mainAdminMenu = new string[] { "Tables", "Settings" };
        //private string[] _mainAdminMenu = new string[] { "Tables", "Settings" };
        private string[] _mainUserMenu = new string[] { "New Order", " My Orders"};
        private string[] _tablesMenu = new string[] { "Products", "Product categories", "Orders" , "Customers" };
        private ProductsViewModel _products = new ProductsViewModel();
        private ProductCategoriesViewModel _productCategories = new ProductCategoriesViewModel();
        private UserTransactionsViewModel _userTransactions = new UserTransactionsViewModel();
        private TransactionsViewModel _transactions = new TransactionsViewModel();

        private string[] _settingsMenu = new string[] { "Database" };
        private UsersViewModel _users = new UsersViewModel();
        private SetupConnectionViewModel _setupConnection = new SetupConnectionViewModel();
        #endregion
        #region Change Menu
        public string[] MainMenu
        {
            get
            {
                return UserLogin.LoginedAdmin ? _mainAdminMenu : _mainUserMenu;
            }
        }

        private string[] _currentMenu;
        public string[] CurrentMenu
        {
            get { return _currentMenu; }
            set { SetProperty(ref _currentMenu, value); }
        }

        private ICommand _switchMenuCommand;
        public ICommand SwitchMenuCommand
        {
            get
            {
                if (_switchMenuCommand == null) _switchMenuCommand = new RelayCommand(new Action<object>(SwitchMenu));
                return _switchMenuCommand;
            }
            set { SetProperty(ref _switchMenuCommand, value); }
        }

        private void SwitchMenu(object parameter)
        {
            string destination = (string)parameter;
            log.Debug(string.Format("Switch menu to: {0}", destination));

            if (UserLogin.LoginedAdmin)
            {
                switch (destination)
                {
                    case "Tables":
                        CurrentMenu = _tablesMenu;
                        break;
               
                    case "Settings":
                        CurrentMenu = _settingsMenu;
                        break;

                    default:
                        CurrentMenu = null;
                        break;
                }
                CurrentViewModel = null;
            } else{
                switch (destination)
                {
                   
                    case "Orders":

                        log.Debug(string.Format("Switch to list orders: {0}", destination));
                        ListTransactionWindow LTW = new ListTransactionWindow();
                        LTW.ShowDialog();
                        break;

                    case "New Order":

                        log.Debug("New order window");
                        NewTransactionWindow NUW = new NewTransactionWindow();
                        NUW.ShowDialog();
                       
                        CloseWindow();
                        
                     break;

                 
                }
                CurrentViewModel = null;

            }
        }
        #endregion
        #region Change View
        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set { SetProperty(ref _currentViewModel, value); }
        }
        private ICommand _switchViewCommand;
        public ICommand SwitchViewCommand
        {
            get
            {
                if (_switchViewCommand == null) _switchViewCommand = new RelayCommand(new Action<object>(Navigate));
                return _switchViewCommand;
            }
            set { SetProperty(ref _switchViewCommand, value); }
        }
        private void Navigate(object parameter)
        {
            string destination = (string)parameter;
            log.Debug(string.Format("Navigate to: {0}", destination));
            switch (destination)
            {
                case "Products":
                    CurrentViewModel = _products;
                    ((RelayCommand)_products.RefreshListCommand).CheckAndExecute(_products);
                    break;
                case "Product categories":
                    CurrentViewModel = _productCategories;
                    ((RelayCommand)_productCategories.RefreshListCommand).CheckAndExecute(_productCategories);
                    break;
                case "Customers":
                    CurrentViewModel = _users;
                    ((RelayCommand)_users.RefreshListCommand).CheckAndExecute(_users);
                    break;
                case "Orders":
                    CurrentViewModel = _transactions;
                    ((RelayCommand)_transactions.RefreshListCommand).CheckAndExecute(_transactions);
                    break;
                case "Database":
                    CurrentViewModel = _setupConnection;
                    break;
                default:
                    CurrentViewModel = null;
                    break;
            }
        }
        #endregion
        #region Close Main window
        private Window _mainWindow;
        public Window MainWindow
        {
            get { return _mainWindow; }
            set { SetProperty(ref _mainWindow, value); }
        }
        private ICommand _closeMainWindowCommand;
        public ICommand CloseMainWindowCommand
        {
            get
            {
                if (_closeMainWindowCommand == null) _closeMainWindowCommand = new RelayCommand(CloseWindow);
                return _closeMainWindowCommand;
            }
            set { SetProperty(ref _closeMainWindowCommand, value); }
        }
        private void CloseWindow(object parameter = null)
        {
            log.Debug("Close main window.");
            MainWindow?.Close();
        }
        #endregion
    }
}
