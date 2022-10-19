using System.Windows;
using Inventory_Management.ViewModel;

namespace Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoginViewModel context = new LoginViewModel();
            context.LoginWindow = this;
            this.DataContext = context;
        }
    }
}
