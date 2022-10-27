using EntityLayer;
using BusinessLayer;
using Inventory_Management.Model;

namespace Inventory_Management.ViewModel
{
    class UserTransactionsDetailsViewModel : ListModel<TransactionHeadListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private UserTransactionsDetailsViewModel() { }
        public UserTransactionsDetailsViewModel(string username)
            :this()
        {
            _username = username;
            RefreshList(null);
        }

        private string _username;

        private decimal _totalTransactions;
        public decimal TotalTransactions
        {
            get { return _totalTransactions; }
            set { SetProperty(ref _totalTransactions, value); }
        }

        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh list: User transaction summary details");

            List = ManageTransactions.ListHead(_username);
            TotalTransactions = 0;
            foreach (var record in List)
                TotalTransactions += record.ListVariable;
        }
    }
}
