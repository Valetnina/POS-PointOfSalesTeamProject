using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_ViewsLibrary;
using System.Collections.ObjectModel;
using POS_DataLibrary;


namespace POS_ManagersApp.ViewModels
{
    
    
    class ManageProductsViewModel:ViewModel
    {
        private ObservableCollection<Product> productsList = new ObservableCollection<Product>();

        private Database db;

       public ObservableCollection<Product> ProductsList
        {
            get { return productsList; }
            set { productsList = value; 
                RaisePropertyChanged("ProductsList"); }
        }


       //public ManageProductsViewModel()
       //{
       //    productsList = db.getAllProducts();
       //}


        private string upcCode;
        public string UPCCode
        {
            get { return upcCode; }
            set
            {
                upcCode = value;
                RaisePropertyChanged("UPCCode");
            }
        }

        private string category;

        public string Category
        {
            get { return category; }
            set { category = value; 
                RaisePropertyChanged("Category"); }
        }

        private string productName;

        public string ProductName
        {
            get { return productName; }
            set { productName = value; 
                RaisePropertyChanged("ProductName"); }
        }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value; 
                RaisePropertyChanged("Price"); }
        }


    }
}
