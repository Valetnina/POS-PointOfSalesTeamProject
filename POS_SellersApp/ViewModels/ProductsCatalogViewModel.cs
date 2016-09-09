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


        private ObservableCollection<Product> catalogCollection;

        public ObservableCollection<Product> CatalogCollection
        {
            get { return catalogCollection; }
            set { catalogCollection = value;
            RaisePropertyChanged("CatalogCollection");
            }
        }


        public ActionCommand SwitchViews { get; private set; }
        public ActionCommand AddToOrder { get; private set; }

        public ProductsCatalogViewModel()
        {
            db = new Database();
            CatalogCollection = db.GetProductsByCategory("Meals");
            SwitchViews = new ActionCommand((param) =>
            {
                OnSwitchViews(param.ToString());
            });
            AddToOrder = new ActionCommand((param) =>
            {
                OnAddToOrder(param as Product);
            });

          //  db.saveProduct(new Product());

        }

        private void OnSwitchViews(string destination)
        {
            switch (destination)
            {
                case "Meals":
                    CatalogCollection = db.GetProductsByCategory("Meals");
                    break;
                case "Drinks":
                    CatalogCollection = db.GetProductsByCategory("Drinks");
                    break;
                case "Desserts":
                
                default:
                    CatalogCollection = db.GetProductsByCategory("Desserts");
                    break;
            }

        }
        private void OnAddToOrder(Product product)
        {
         //   Messenger.Default.Send(product);
        }

    }
}
