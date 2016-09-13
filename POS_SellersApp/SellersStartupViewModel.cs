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
            User = new User();
            SwitchViews = new ActionCommand(p => OnSwitchViews("catalog"));
            MessengerUser.Default.Register<User>(this, (user) =>
            {
                User.FirstName = user.FirstName;
                User.Id = user.Id;
                User.LastName = user.LastName;
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

                //CurrentDateText();
                //DispatcherTimerSetup();

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

       public ActionCommand SwitchViews { get; private set; }

        SellersMainWindowViewModel sellersVm = new SellersMainWindowViewModel();

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
                        CurrentView = sellersVm;
                        MessengerUserLogged.Default.Send(User);

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


        //private string _currentTime, _currentDate;

        //private void DispatcherTimerSetup()
        //{
        //    DispatcherTimer dispatcherTimer = new DispatcherTimer();
        //    dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        //    dispatcherTimer.Tick += new EventHandler(CurrentTimeText);
        //    dispatcherTimer.Start();
        //}

        //private void CurrentDateText()
        //{
        //    CurrentDate = DateTime.Now.ToString("g");
        //}

        //private void CurrentTimeText(object sender, EventArgs e)
        //{
        //    CurrentTime = DateTime.Now.ToString("HH:mm");
        //}

        //public string CurrentTime
        //{
        //    get { return _currentTime; }
        //    set
        //    {
        //        if (_currentTime != null)
        //            _currentTime = value;

        //        RaisePropertyChanged("CurrentTime");
        //    }
        //}

        //public string CurrentDate
        //{
        //    get { return _currentDate; }
        //    set
        //    {
        //        if (_currentDate != value)
        //            _currentDate = value;
        //        RaisePropertyChanged("CurrentDate");
        //    }
        //}
        #endregion
    }
}
