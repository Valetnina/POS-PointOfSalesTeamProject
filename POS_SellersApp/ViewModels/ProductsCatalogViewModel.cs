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
            Category = "Meals";
        }
        private string category;
        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                RaisePropertyChanged("Category");
            }
        }
        private void OnSwitchViews(string destination)
        {
            switch (destination)
            {
                case "Meals":
                    CatalogCollection = db.GetProductsByCategory("Meals");
                    Category = "Meals";
                    break;
                case "Drinks":
                    CatalogCollection = db.GetProductsByCategory("Drinks");
                    Category = "Drinks";
                    break;
                case "Desserts":
                
                default:
                    CatalogCollection = db.GetProductsByCategory("Desserts");
                    Category = "Desserts";

                    break;
            }

        }
        private void OnAddToOrder(Product product)
        {
            if(product != null)
            {
                MessengerPoduct.Default.Send(product);

            }
        }

    }
}
