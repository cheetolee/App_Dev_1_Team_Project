using EntityLayer;
using Inventory_Management.Model;
using BusinessLayer;

namespace Inventory_Management.ViewModel
{
    public class EditProductCategoryViewModel : EditItemModel<ProductCategoryEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EditProductCategoryViewModel(ProductCategoryEntity item, bool newRecord, string itemName) : base(item, newRecord, itemName) { }

        protected override bool Save(object parameter)
        {
            log.Debug("Save " + ItemName);

            bool result = false;
            if (string.IsNullOrWhiteSpace(Item.Category))
            {
                log.Error(" product category error Please fill the category field.");
            }
            else
            {
                if (NewRecord)
                {
                    result = ManageProducts.NewProductCategory(Item);
                }
                else
                {
                    result = ManageProducts.ModifyProductCategory(Item);
                }
                if (!result)
                    log.Error(" product category error Category name already exist.");
            }
            return result;
        }
    }
}
