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
    public class ProductsCatalogViewModel : ViewModel
    {
        private Database db;
        private ObservableCollection<Product> catalogCollection;

        public ObservableCollection<Product> CatalogCollection
        {
            get { return catalogCollection; }
            set
            {
                catalogCollection = value;
                RaisePropertyChanged("CatalogCollection");
            }
        }

        public ActionCommand SwitchViews { get; private set; }
        public ActionCommand AddToOrder { get; private set; }



        public ProductsCatalogViewModel()
        {
            try
            {
                db = new Database();
            }
            catch (Exception e)
            {
                MessageBox.Show("fatal Error: Unable to connect to database", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                throw e;
            }

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

            EnabledMeals = false;
            EnabledDrinks = true;
            EnabledDeserts = true;
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
            try
            {
                switch (destination)
                {
                    case "Meals":
                        CatalogCollection = db.GetProductsByCategory("Meals");
                        Category = "Meals";
                        EnabledMeals = false;
                        EnabledDrinks = true;
                        EnabledDeserts = true;
                        break;
                    case "Drinks":
                        CatalogCollection = db.GetProductsByCategory("Drinks");
                        Category = "Drinks";
                        EnabledMeals = true;
                        EnabledDrinks = false;
                        EnabledDeserts = true;
                        break;
                    case "Desserts":
                    default:
                        CatalogCollection = db.GetProductsByCategory("Desserts");
                        Category = "Desserts";
                        EnabledMeals = true;
                        EnabledDrinks = true;
                        EnabledDeserts = false;
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Could not fetch product categories feom the database", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool enabledMeals;

        public bool EnabledMeals
        {
            get { return enabledMeals; }
            set
            {
                enabledMeals = value;
                RaisePropertyChanged("EnabledMeals");
            }
        }

        private bool enabledDrinks;

        public bool EnabledDrinks
        {
            get { return enabledDrinks; }
            set
            {
                enabledDrinks = value;
                RaisePropertyChanged("EnabledDrinks");
            }
        }


        private bool enabledDeserts;

        public bool EnabledDeserts
        {
            get { return enabledDeserts; }
            set
            {
                enabledDeserts = value;
                RaisePropertyChanged("EnabledDeserts");
            }
        }

        private void OnAddToOrder(Product product)
        {
            if (product != null)
            {
                MessengerPoduct.Default.Send(product);

            }
        }

    }
}
