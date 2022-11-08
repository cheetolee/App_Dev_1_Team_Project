using System;
using System.Windows;
using Inventory_Management.ViewModel;
using Inventory_Management.Views.Login;

namespace Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        ApiResults ResultsData = new ApiResults();

        public LoginWindow()
        {
            InitializeComponent();
            LoginViewModel context = new LoginViewModel();
            context.LoginWindow = this;
            this.DataContext = context;
        }

        internal void SendArgumentsBack(ApiResults data)
        {
            ResultsData = data;
            if (data.ErrorFound)
            {
                MessageBox.Show(ResultsData.Error + Environment.NewLine + ResultsData.ErrorDescription + Environment.NewLine + ResultsData.ErrorReason);
            }
            else
            {
                //MessageBox.Show(ResultsData.Accesstoken + Environment.NewLine + ResultsData.GrantedScopes + Environment.NewLine + ResultsData.DeniedScopes);
            };
        }
    }
}
