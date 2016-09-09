using POS_DataLibrary;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Threading;

namespace POS_SellersApp.ViewModels
{
    public class SellersMainWindowViewModel : ViewModel,INotifyPropertyChanged 
    {
        private User user;
        public User User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
                RaisePropertyChanged2("User");
                RaisePropertyChanged2("UserName");
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
        
        public SellersMainWindowViewModel()
        {
            Messenger.Default.Register<User>(this, (user) =>
            {
                User = user;
            });

            SwitchViews = new ActionCommand(p=> OnSwitchViews("catalog"));
            currentView = ProductsCatalogViewModel;
            OrderNo = "1";
            CurrentDateText();
            DispatcherTimerSetup();
        }

        private ProductsCatalogViewModel ProductsCatalogViewModel = new ProductsCatalogViewModel();        

        private PaimentViewModel payiementViewModel = new PaimentViewModel();


        private ViewModel currentView;

        public ViewModel CurrentView
        {
            get { return currentView; }
            set { RaisePropertyChanged2("CurrentView"); }
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
                RaisePropertyChanged2("OrderNo");
            }
        }
        public new event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged2(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
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

                    RaisePropertyChanged2("CurrentTime");
                }
            }

            public string CurrentDate
            {
                get { return _currentDate; }
                set
                {
                    if (_currentDate != value)
                        _currentDate = value;
                    RaisePropertyChanged2("CurrentDate");
                }
            }
    }
    }
