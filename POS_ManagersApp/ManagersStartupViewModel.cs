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
using POS_ManagersApp.ViewModels;

namespace POS_ManagersApp

{
   public class ManagersStartupViewModel: ViewModel
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

        private LoginViewModel loginViewManager = new LoginViewModel();


        public ManagersStartupViewModel()
        {
            SwitchViews = new ActionCommand(p => OnSwitchViews("dashboard"));
            Exit = new ActionCommand(p => OnExit());
            MessengerUser.Default.Register<User>(this, (user) =>
            {
                User = user;
                OnSwitchViews("dashboard");

            });
            MessengerLogout.Default.Register<string>(this, (message) =>
            {
                OnSwitchViews(message);

            });
            currentView = loginViewManager;

            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, args) =>
            {
                CurrentTime = DateTime.Now.ToLongTimeString();
            };
            timer.Start();
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

       readonly static ManagersMainWindowViewModel managersVm = new ManagersMainWindowViewModel();

        private void OnSwitchViews(string destination)
       {
           if (User == null)
           {
               return;
           }
           switch (destination)
           {
               case "dashboard":
                   if (!User.IsManager)
                   {
                       MessageBox.Show("You don't have acces from this location") ;
                       return;
                   }
                    try
                    {
                        managersVm.UserLoggedIn = User;
                        CurrentView = managersVm;

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error. Could not navigate to next page");
                       throw (ex);
                    }

                   break;
               case "login":
               default:
                  CurrentView = loginViewManager;
                   break;
           }

       }
        #region CurrentTime

        private string currentTime;

        public DispatcherTimer timer;

        public string CurrentTime
        {
            get
            {
                return this.currentTime;
            }
            set
            {
                if (currentTime == value)
                    return;
                currentTime = value;
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

        public ActionCommand Exit { get; set; }

        private void OnExit()
        {
            MessageBoxResult result = MessageBox.Show("You are about to close the application.Are you sure?", "Close Application", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

    }
}
