using POS_DataLibrary;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;
using System;
using System.Windows;
using System.Windows.Threading;

namespace POS_SellersApp.ViewModels
{
   public class SellersMainWindowViewModel : ViewModel
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
                SetProperty(ref user, value);
                UserName = user.UserName;
            }
        }
        private int dom;

        public int Dom
        {
            get { return dom; }
            set { dom = value;
            SetProperty(ref dom, value);
            }
        }

        private string userName;
        public string UserName { get { return userName; }
            set {
                SetProperty(ref userName, value);
            } }
        public SellersMainWindowViewModel()
        {
            Messenger.Default.Register<User>(this, (user) =>
            {
                User = user;
            });

            SwitchViews = new ActionCommand(p=> OnSwitchViews("catalog"));
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
            set { SetProperty(ref currentView, value); }
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
                SetProperty(ref orderNo, value);
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

                    SetProperty(ref _currentTime, value);
                }
            }

            public string CurrentDate
            {
                get { return _currentDate; }
                set
                {
                    if (_currentDate != value)
                        _currentDate = value;

                    SetProperty(ref _currentDate, value);
                }
            }

            

    }
    }
