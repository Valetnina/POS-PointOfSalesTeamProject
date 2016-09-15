using POS_SellersApp.ViewModels;
using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_PointOfSales.ViewModels;
using System.Windows;
using POS_DataLibrary;
using System.Runtime.Remoting.Contexts;
using System.Windows.Threading;

namespace POS_SellersApp
{
   public class SellersStartupViewModel: ViewModel
    {
        private User userLoggedIn;

        public User User
        {
            get { return userLoggedIn; }
            set
            {
                userLoggedIn = value;
                RaisePropertyChanged("User");
            }
        }

        private LoginViewModel loginView = new LoginViewModel();


        public SellersStartupViewModel()
        {
            SwitchViews = new ActionCommand(p => OnSwitchViews("catalog"));
            Exit = new ActionCommand(p => OnExit());
            MessengerUser.Default.Register<User>(this, (user) =>
            {
                User = user;
                OnSwitchViews("catalog");

            });
            MessengerLogout.Default.Register<string>(this, (message) =>
            {
                OnSwitchViews(message);

            });
            currentView = loginView;

            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, args) =>
            {
                CurrentTime = DateTime.Now.ToLongTimeString();
            };
            _timer.Start();
        }
        private ViewModel currentView;

       public ViewModel CurrentView
       {
           get { return currentView; }
           set{
               currentView = value;

               RaisePropertyChanged("CurrentView");
           }
          
       }
       private void OnExit()
       {
           MessageBoxResult result = MessageBox.Show("You are about to close the application", "Close Application", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
           if (result == MessageBoxResult.Yes)
           {
               Application.Current.Shutdown();
           }
       }

       public ActionCommand SwitchViews { get; private set; }

       readonly static SellersMainWindowViewModel sellersVm = new SellersMainWindowViewModel();

        private void OnSwitchViews(string destination)
       {
           if (User == null)
           {
               return;
           }
           switch (destination)
           {
               case "catalog":
                   if (User.IsManager)
                   {
                       MessageBox.Show("You don't have acces from this location") ;
                       return;
                   }
                    try
                    {
                        sellersVm.UserLoggedIn = User;
                        sellersVm.OrderItems.Clear();
                        CurrentView = sellersVm;

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error. Could not navigate to next page");
                       throw (ex);
                    }

                   break;
               case "login":
               default:
                  CurrentView = loginView;
                   break;
           }

       }
        #region CurrentTime

        private string _currentTime;

        public DispatcherTimer _timer;

        public string CurrentTime
        {
            get
            {
                return this._currentTime;
            }
            set
            {
                if (_currentTime == value)
                    return;
                _currentTime = value;
                RaisePropertyChanged("CurrentTime");
            }
        }
        #endregion

        public ActionCommand Exit { get; set; }
    }
}
