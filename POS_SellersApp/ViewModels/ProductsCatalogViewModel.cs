using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_DataLibrary;
using System.Windows;

namespace POS_SellersApp.ViewModels
{
  public  class ProductsCatalogViewModel:ViewModel
    {
        private Database db;
        public ObservableCollection<Product> catalogCollection { get; set; }

        public ActionCommand PopulateWithProducts { get; private set; }

        public ProductsCatalogViewModel()
        {
            db = new Database();
            catalogCollection = db.getAllProducts();
            PopulateWithProducts = new ActionCommand(OnPopulateWithProducts);

           // MessageBox.Show("Call catalog");
          //  db.saveProduct(new Product());

        }

        private void OnPopulateWithProducts(object obj)
        {
            throw new NotImplementedException();
        }

        private void OnPopulateWithProducts()
        {
            throw new NotImplementedException();
        }


    }
}
