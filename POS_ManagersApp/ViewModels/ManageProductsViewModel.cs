using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_ViewsLibrary;
using System.Collections.ObjectModel;
using POS_DataLibrary;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Windows;
using Microsoft.Win32;


namespace POS_ManagersApp.ViewModels
{
    class ManageProductsViewModel : ViewModel
    {
        //Declare properties and fields
        private Database db;
        private bool IsValid;
        private ObservableCollection<Product> productsList = new ObservableCollection<Product>();
        private ObservableCollection<Product> topItems = new ObservableCollection<Product>();

        private ObservableCollection<ProductCategory> categoryList = new ObservableCollection<ProductCategory>();

        public ObservableCollection<ProductCategory> CategoryList
        {
            get { return categoryList; }
            set
            {
                categoryList = value;
                RaisePropertyChanged("CategoryList");
            }
        }

        public ObservableCollection<Product> ProductsList
        {

            get { return db.getAllProducts(); }
        }

        public ManageProductsViewModel()
        {
            //Initialize
            db = new Database();

            CategoryList = db.getAllCategories();
            //Register Commands
            AddProduct = new ActionCommand(p => OnAddProduct(), p => CanAddProduct());
            UpdateProduct = new ActionCommand(p => OnUpdateProduct());
            SelectImage = new ActionCommand(p => OnSelectImage());
            ClearForm = new ActionCommand(p => OnClearForm());
            //Populate by default the combobox Item Source
            productsList = db.getAllProducts();

        }

        private bool CanAddProduct()
        {
            return true;
            //return !String.IsNullOrWhiteSpace(UPCCode) &&
            //             !String.IsNullOrWhiteSpace(ProductName) &&
            //             Price != 0 && !String.IsNullOrWhiteSpace(Path);
        }

        private string upcCode;

        [Required]
        [StringLength(5)]
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
        [Required]
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                RaisePropertyChanged("Category");
            }
        }

        private string productName;
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                RaisePropertyChanged("ProductName");
            }
        }

        private  decimal price;
        [Required]
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChanged("Price");
            }
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
            SelectedProduct = null;
        }

        private bool ValidateInput()
        {
            if (UPCCode == null || UPCCode.Length != 5)
            {
                MessageBox.Show("UPCCode must be 5 characters long", "Error inserting data", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            string upcCode = UPCCode;
            if (SelectedCategory == null)
            {
                MessageBox.Show("You mut choose a Category", "Error inserting data", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            int id = SelectedCategory.Id;
            if (ProductName == null || ProductName.Length < 2 || ProductName.Length > 50)
            {
                MessageBox.Show("Product Name must contain between 2 and 50 characters", "Error inserting data", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            string productName = ProductName;
            if (Price <= 0)
            {
                MessageBox.Show("Price cannot be less than zero", "Error inserting data", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            decimal price = Price;
            if (Path == null)
            {
                MessageBox.Show("You must choose a valid path of an image", "Error inserting data", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            return true;
        }

        public ActionCommand AddProduct { get; set; }

        private void OnAddProduct()
        {
            if (!ValidateInput())
            {
                return;
            }
            Product p = new Product() { UPCCode = UPCCode, CategoryId = SelectedCategory.Id, Name = ProductName, Price = Price };
            try
            {
                db.addProduct(p, Path);
                MessageBox.Show("Product was added succesfully", "Add product", MessageBoxButton.OK, MessageBoxImage.Information);
                OnClearForm();
                RaisePropertyChanged("ProductsList");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not insert record in the database", "Insert error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw (ex);
            }
        }

        private Product selectedProduct;

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                if (value != null)
                {
                    selectedProduct = value;
                    RaisePropertyChanged("SelectedProduct");
                    UPCCode = SelectedProduct.UPCCode;
                    SelectedCategory = CategoryList.First(p => p.CategoryName == SelectedProduct.CategoryName);
                    ProductName = SelectedProduct.Name;
                    Price = SelectedProduct.Price;
                    Path = "If you want to update image than browse...";
                }
                else
                {
                    selectedProduct = value;
                    RaisePropertyChanged("SelectedProduct");
                }


            }
        }


        public ActionCommand UpdateProduct { get; set; }

        private void OnUpdateProduct()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select an item for update", "Invalid action", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            if (!ValidateInput())
            {
                return;
            }
            string upcCode = UPCCode;
            int id = SelectedCategory.Id;
            string productName = ProductName;
            decimal price = Price;
            Product p = new Product() { UPCCode = UPCCode, CategoryId = id, Name = ProductName, Price = Price };
            try
            {
                if (Path == "If you want to update image than browse...")
                {
                    db.updateProductWithoutImage(p);

                }
                else
                {
                    db.updateProduct(p, Path);
                    
                }
                MessageBox.Show("Product was updated succesfully", "Update product", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnClearForm();
                RaisePropertyChanged("ProductsList");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Could not update record.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
            catch (IOException ex)
            {
                MessageBox.Show("Could not update record. Probably the Image path it nis not a valid one", "IO Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
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
            if (ofd.ShowDialog() == true)
            {
                Path = ofd.FileName;
            }
        }


    }
}
