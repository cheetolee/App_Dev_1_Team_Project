using System;
using EntityLayer;
using BusinessLayer;
using Inventory_Management.Model;
using Inventory_Management.Views;

namespace Inventory_Management.ViewModel
{
    public class TransactionsViewModel : TableModel<TransactionHeadListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       
        public TransactionsViewModel()
        {
            ItemName = "transaction";
            TableName ="transactions";
        }
     

        protected override void DeleteItem(object parameter)
        {
            log.Debug("Delete " + ItemName + " button");

            string date = SelectedItem.Head.Date.ToString("d");
            string Username = SelectedItem.User.Username;
            int id = SelectedItem.Head.Id;
            if (ManageTransactions.RemoveTransaction(SelectedItem.Head))
            {
                RefreshList(parameter);
                log.Info("Transaction deleted:"+ string.Format("Id: {0}\nDate: {1}\nPartner name: {2}",id,date, Username));
            }
            else
            {
                log.Error("Delete transaction error: Unknown reason.");
            }
        }

        protected override void EditItem(object parameter)
        {
            log.Debug("Edit " + ItemName + " button");

            TransactionHeadListEntity Item = new TransactionHeadListEntity();
            EntityCloner.CloneProperties<TransactionHeadListEntity>(SelectedItem, Item);
            EditTransactionViewModel ETVM = new EditTransactionViewModel(Item, false,ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = ETVM };
            EIV.ShowDialog();
            if (ETVM.SaveEdit)
            {
                Item = ETVM.Item;
                log.Info("Transaction saved:"+ string.Format("Id: {0}\nDate: {1}\nPartner name: {2}", Item.Head.Id, Item.Head.Date.ToString("d"), Item.User.Username));
                RefreshList(parameter);
                foreach (var t in List)
                    if (Item.Head.Id == t.Head.Id)
                        SelectedItem = t;
            }
        }

        protected override void NewItem(object parameter)
        {
            log.Debug("New " + ItemName + " button");

            TransactionHeadListEntity Item = new TransactionHeadListEntity();
            Item.Head = new TransactionHeadEntity();
            Item.Head.Date = DateTime.Now.Date;
            EditTransactionViewModel ETVM = new EditTransactionViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = ETVM };
            EIV.ShowDialog();
            if (ETVM.SaveEdit)
            {
                Item = ETVM.Item;
                log.Info("Transaction added:"+ string.Format("Id: {0}\nDate: {1}\nPartner name: {2}", Item.Head.Id, Item.Head.Date.ToString("d"), Item.User.Username));
                RefreshList(parameter);
                foreach (var t in List)
                    if (Item.Head.Id == t.Head.Id)
                        SelectedItem = t;
            }
        }

        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh " + ItemName + " list");

            List = ManageTransactions.ListHead();
        }
    }
}
