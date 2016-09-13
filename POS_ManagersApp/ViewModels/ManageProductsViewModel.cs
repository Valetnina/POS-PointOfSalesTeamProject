using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_ViewsLibrary;
using System.Collections.ObjectModel;
using POS_DataLibrary;
using System.Windows.Forms;
using System.IO;


namespace POS_ManagersApp.ViewModels
{
    
    
    class ManageProductsViewModel:ViewModel
    {
        private ObservableCollection<Product> productsList = new ObservableCollection<Product>();

        private ObservableCollection<ProductCategory> categoryList = new ObservableCollection<ProductCategory>();

        public ObservableCollection<ProductCategory> CategoryList
        {
            get { return categoryList; }
            set { categoryList = value; 
                RaisePropertyChanged("CategoryList"); }
        }

        private Database db = new Database();

       public ObservableCollection<Product> ProductsList
        {
            get { return productsList; }
            set { productsList = value; 
                RaisePropertyChanged("ProductsList"); }
        }


       public ManageProductsViewModel()
       {

           ProductsList = db.getAllProducts();

           CategoryList = db.getAllCategories();
           
           AddProduct = new ActionCommand(p => OnAddProduct());

           UpdateProduct = new ActionCommand(p => OnUpdateProduct());

           SelectImage = new ActionCommand(p => OnSelectImage());

           ClearForm = new ActionCommand(p => OnClearForm());
       }

              
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


        private ProductCategory selectedCategory;
        public ProductCategory SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                RaisePropertyChanged("SelectedCategory");
            }
        }


        public ActionCommand ClearForm { get; set; }

        private void OnClearForm()
        {
            UPCCode = "";
            SelectedCategory = null;
            ProductName = "";
            Price = 0;
            Path = ""; 
        }



        public ActionCommand AddProduct { get; set; }

        private void OnAddProduct()
        {
            string upcCode = UPCCode;
            int id = SelectedCategory.Id;
            string productName = ProductName;
            decimal price = Price;


            Product p = new Product() { UPCCode = UPCCode, CategoryId = id, Name = ProductName, Price = Price };

            db.addProduct(p, Path);            
        }

        private Product selectedProduct;

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; 
                RaisePropertyChanged("SelectedProduct");
                UPCCode = SelectedProduct.UPCCode;
                SelectedCategory = CategoryList.First(p => p.CategoryName == SelectedProduct.CategoryName);
                ProductName = SelectedProduct.Name;
                Price = SelectedProduct.Price;
            }
        }


        public ActionCommand UpdateProduct { get; set; }
        
        int currentProductId = 0;
        private void OnUpdateProduct()
        {
            if (currentProductId == 0)
            {
                //MessageBox.Show("Please select an item for update", "Invalid action", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            string upcCode = UPCCode;
            int id = SelectedCategory.Id;
            string productName = ProductName;
            decimal price = Price;
            Product p = new Product() { UPCCode = UPCCode, CategoryId = id, Name = ProductName, Price = Price};

            db.updateProduct(p);  

        }

        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; RaisePropertyChanged("Path"); }
        }

        public ActionCommand SelectImage { get; set; }

        private void OnSelectImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Png files (*.png)|*.png|Jpg files (*.jpg)|*.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Path = ofd.FileName;
            }

        }


    }
}
