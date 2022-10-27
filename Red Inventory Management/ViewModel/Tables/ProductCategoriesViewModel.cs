using Inventory_Management.Model;
using EntityLayer;
using BusinessLayer;
using Inventory_Management.Views;

namespace Inventory_Management.ViewModel
{
    public class ProductCategoriesViewModel : TableModel<ProductCategoryEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProductCategoriesViewModel()
        {
            ItemName = "category";
            TableName = "Product categories";
        }

        protected override void DeleteItem(object parameter)
        {
            log.Debug("Delete " + ItemName + " button");

            string name = SelectedItem.Category;
            if (ManageProducts.DeleteProductCategory(SelectedItem))
            {
                RefreshList(parameter);
            }
            else
            {
                log.Error("Delete product category error: This category is set to one or more product");
            }
        }

        protected override void EditItem(object parameter)
        {
            log.Debug("Edit " + ItemName + " button");

            ProductCategoryEntity Item = new ProductCategoryEntity();
            EntityCloner.CloneProperties<ProductCategoryEntity>(SelectedItem, Item);
            EditProductCategoryViewModel EPVM = new EditProductCategoryViewModel(Item, false, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void NewItem(object parameter)
        {
            log.Debug("New " + ItemName + " button");

            ProductCategoryEntity Item = new ProductCategoryEntity();
            EditProductCategoryViewModel EPVM = new EditProductCategoryViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh " + ItemName + " list");

            List = ManageProducts.ListProductCategories();
        }
    }
}
