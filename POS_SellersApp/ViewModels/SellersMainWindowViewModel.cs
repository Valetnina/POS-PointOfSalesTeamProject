using POS_DataLibrary;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;

namespace POS_SellersApp.ViewModels
{

    public class SellersMainWindowViewModel : ViewModel

    {
        private const double TAX = 0.15;

        private Database db;
        private ObservableCollection<Int32> discountButtons;
        public ObservableCollection<Int32> DiscountButtons
        {
            get
            {
                return discountButtons;
            }

            set
            {
                discountButtons = value;
                RaisePropertyChanged("DiscountButtons");
            }
        }
        public SellersMainWindowViewModel()
        {
            db = new Database();
            //Register for messages from differnet viewModels
            MessengerUser.Default.Register<User>(this, (user) =>
            {
                User = user;

            });
            MessengerPoduct.Default.Register<Product>(this, (product) =>
            {
                ReceiveMessage(product);
            });
            MessengerDone.Default.Register<String>(this, message => 
                RecivedDoneMessage()
            );
            SendLogoutMessage = new ActionCommand(p => OnSendLogoutMessage("login"));
            User = user;
            //Initialize comopents with some values
            OrderItems = new ObservableCollection<OrderItems>();
            DiscountButtons = new ObservableCollection<Int32>();
            for (int i = 0; i <= 20; i = i + 5)
            {
                DiscountButtons.Add(i);
            }
            DiscountButtons.Add(50);
            DiscountButtons.Add(100);
            SelectedDiscount = 0;
            //Register CollectionChangedEvent for the OrderItems
            OrderItems.CollectionChanged += ContentCollectionChanged;

            //Initialize commands
            RemoveItem = new ActionCommand(p => OnRemoveItem());
            CancelOrder = new ActionCommand(p => OnCancelOrder());
            SwitchViews = new ActionCommand((p) => OnSwitchViews(p.ToString()));
            currentView = ProductsCatalogViewModel;
            OrderNo = "1";
            CurrentDateText();
            DispatcherTimerSetup();
            //Set the totals to zero
            //  OrderSubTotal = 0;
        }

        private void RecivedDoneMessage()
        {
            //try
            //{
            //    db.saveOrderAndOrderItems(order, orderItems);
            //}catch(Exception ex)
            //{

            //}
            //currentView = ProductsCatalogViewModel;
        }

        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("OrderSubTotal");
            RaisePropertyChanged("OrderTax");
            RaisePropertyChanged("BalanceDue");
        }
        private ObservableCollection<OrderItems> orderItems;

        public ObservableCollection<OrderItems> OrderItems
        {
            get { return orderItems; }
            set
            {
                orderItems = value;
                RaisePropertyChanged("OrderItems");
            }
        }

        private void ReceiveMessage(Product product)
        {
            //Check if the product was already selected
            if (OrderItems.Any(p => p.UPCCode == product.UPCCode))
            {
                OrderItems.First(p => p.UPCCode == product.UPCCode).Quantity +=1;
            }
            else
            {
                OrderItems.Add(new POS_DataLibrary.OrderItems
                {
                    UPCCode = product.UPCCode,
                    CategoryName = product.CategoryName.ToString(),
                    Quantity = 1,
                    Price = product.Price,
                    Name = product.Name
                });
            }
        }
        private Int32 selectedDiscount;
        public Int32 SelectedDiscount { 
            get
            {
                return selectedDiscount;
            }

            set
            {
                selectedDiscount = value;
                RaisePropertyChanged("SelectedDiscount");
                RaisePropertyChanged("Discount");
            }
        }
        public double Discount
        {
            get
            {

                return SelectedDiscount;
            }
        }
        private OrderItems selectedOrderItem;
        public OrderItems SelectedOrderItem
        {
            get
            {
                return selectedOrderItem;
            }

            set
            {
                selectedOrderItem = value;
                RaisePropertyChanged("SelectedOrderItem");
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
                    // Messenger.Default.Unregister(this);
                    return user.UserName;
                }
                return "NOT SET";
            }
        }
        public decimal OrderSubTotal
        {
            get
            {
                decimal value = orderItems.Sum(od => od.Total);
                if (value == 0)
                {
                    return 0;
                }
                else
                {
                    return value;
                }

            }
        }
        public decimal OrderTax
        {
            get
            {

                return OrderSubTotal * (decimal)TAX;
            }

        }
        public decimal BalanceDue
        {
            get
            {

                return (OrderSubTotal + OrderTax);
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

        private ProductsCatalogViewModel ProductsCatalogViewModel = new ProductsCatalogViewModel();

    //    private PaimentViewModel payiementViewModel = new PaimentViewModel();


        private ViewModel currentView;

        public ViewModel CurrentView
        {
            get { return currentView; }
            set {
                currentView = value;
                RaisePropertyChanged("CurrentView"); }
        }
        #region Commands
        private void OnSendLogoutMessage(string v)
        {
            MessengerLogout.Default.Send(v);
        }

        public ActionCommand SendLogoutMessage { get; private set; }
        public ActionCommand RemoveItem { get; private set; }
        public ActionCommand CancelOrder { get; private set; }

        private void OnRemoveItem()
        {
            if (SelectedOrderItem != null)
            {
                MessageBoxResult result = MessageBox.Show("You are about to delete an item. Do you want to continue?", "Delete Item", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    OrderItems.Remove(SelectedOrderItem);
                };
            }
        }
        private void OnCancelOrder()
        {
            if (OrderItems.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("You are about to delete current order. Do you want to continue?", "Delete Order", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    OrderItems.Clear();
                };
            }
        }
        public ActionCommand SwitchViews { get; private set; }


        private void OnSwitchViews(string destination)
        {
            switch (destination)
            {
                case "pay":
                    CurrentView =  new PaimentViewModel(BalanceDue);
                    break;

                case "catalog":
                default:
                    CurrentView = ProductsCatalogViewModel;
                    break;
            }

        }
        #endregion


        #region DateTime Members
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
        #endregion
    }
}
