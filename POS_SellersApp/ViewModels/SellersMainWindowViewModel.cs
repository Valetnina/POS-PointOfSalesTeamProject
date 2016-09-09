using POS_DataLibrary;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace POS_SellersApp.ViewModels
{
    public class SellersMainWindowViewModel : ViewModel
    {
        private ObservableCollection<Product> productsInOrder;

        public ObservableCollection<Product> ProductsInOrder
        {
            get { return productsInOrder; }
            set { productsInOrder = value;
            RaisePropertyChanged("ProductsInOrder");
            }
        }
        private User user;
        public User User
        {
            get
            {
                return user;
            }

            set
            {
               // MessageBox.Show("User is set " + User.UserName);
                user = value;
                RaisePropertyChanged("User");
                RaisePropertyChanged("UserName");
            }
        }
        public string UserName
        {
            get
            {
                if (user != null)
                {
                    return user.UserName;
                }
                return "NOT SET";
            }
        }
        //private Product product;

        //public Product Product
        //{
        //    get { return product; }
        //    set { product = value; 
        //    RaisePropertyChanged("Product")}
        //}
        public SellersMainWindowViewModel()
        {
            //Register for messages from differnet viewModels
            Messenger.Default.Register<User>(this, (user) =>
            {
                MessageBox.Show(user.UserName + " received by Order");
                User = user;
                
            });
            //Messenger.Default.Register<Product>(this, (product) =>
            //{
            // //   MessageBox.Show(product.Name + " received by Order");
            //    ReceiveMessage(product);
            //});
            ProductsInOrder = new ObservableCollection<Product>();


            SwitchViews = new ActionCommand(p=> OnSwitchViews("catalog"));
            currentView = ProductsCatalogViewModel;
            OrderNo = "1";
            CurrentDateText();
            DispatcherTimerSetup();
        }

        private void ReceiveMessage(Product product)
        {
            MessageBox.Show(product.Name + " received by Order");
            ProductsInOrder.Add(product);
            MessageBox.Show("ProductsIn order" + ProductsInOrder.Count);
        }

       

        private ProductsCatalogViewModel ProductsCatalogViewModel = new ProductsCatalogViewModel();        

        private PaimentViewModel payiementViewModel = new PaimentViewModel();


        private ViewModel currentView;

        public ViewModel CurrentView
        {
            get { return currentView; }
            set { RaisePropertyChanged("CurrentView"); }
        }

        public ActionCommand SwitchViews { get; private set; }

       
        private void OnSwitchViews(string destination)
        {
            switch (destination)
            {
                case "pay":
                    CurrentView = payiementViewModel;
                    break;
                
                case "catalog":
                default:
                    CurrentView = ProductsCatalogViewModel;
                    break;
            }

        }

        private string orderNo;
        public string OrderNo
        {
            get
            {
                return orderNo;
            }

            set
            {
                orderNo = value;
                RaisePropertyChanged("OrderNo");
            }
        }
        

        private string _currentTime, _currentDate;
                      
            private void DispatcherTimerSetup()
            {
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                dispatcherTimer.Tick += new EventHandler(CurrentTimeText);
                dispatcherTimer.Start();
            }

            private void CurrentDateText()
            {
                CurrentDate = DateTime.Now.ToString("g");
            }

            private void CurrentTimeText(object sender, EventArgs e)
            {
                CurrentTime = DateTime.Now.ToString("HH:mm");
            }

            public string CurrentTime
            {
                get { return _currentTime; }
                set
                {
                    if (_currentTime != null)
                        _currentTime = value;

                    RaisePropertyChanged("CurrentTime");
                }
            }

            public string CurrentDate
            {
                get { return _currentDate; }
                set
                {
                    if (_currentDate != value)
                        _currentDate = value;
                    RaisePropertyChanged("CurrentDate");
                }
            }
    }
    }
