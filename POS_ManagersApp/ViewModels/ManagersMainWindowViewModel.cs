using POS_DataLibrary;
using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS_ManagersApp.ViewModels
{
public class ManagersMainWindowViewModel: ViewModel
    {
        private User userLoggedIn;
        public User UserLoggedIn
        {
            get { return userLoggedIn; }
            set { userLoggedIn = value;
            RaisePropertyChanged("User");
            }
        }

        public ManagersMainWindowViewModel()
        {
            SwitchViews = new ActionCommand((p) => OnSwitchViews(p.ToString()));
            SendLogoutMessage = new ActionCommand(p => OnSendLogoutMessage("login"));
            Exit = new ActionCommand(p => OnExit());
            CurrentView = dash;
            EnabledDashboard = false;
            EnabledManageProducts = true;
        }

        private void OnExit()
        {
            MessageBoxResult result = MessageBox.Show("You are about to close the application. Do you want to continue", "Close Application", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private bool enabledDashboard;

        public bool EnabledDashboard
        {
            get { return enabledDashboard; }
            set { enabledDashboard = value; }
        }

        private bool enabledManageProducts;

        public bool EnabledManageProducts
        {
            get { return enabledManageProducts; }
            set { enabledManageProducts = value; }
        }

      readonly static   DashboardViewModel dash = new DashboardViewModel();
      readonly static ManageProductsViewModel manage = new ManageProductsViewModel();
             private void OnSwitchViews(string destination)
        {
         

            switch (destination)
            {
                case "dashboard":
                    CurrentView = dash;
                    EnabledDashboard = false;
                    EnabledManageProducts = true;
                    break;

                case "manage":
                default:
                    CurrentView = manage;
                    EnabledManageProducts = false;
                    EnabledDashboard = true;
                    break;
            }

        }
             private void OnSendLogoutMessage(string v)
             {
                 MessengerLogout.Default.Send(v);
             }

             public ActionCommand SendLogoutMessage { get; private set; }
        private ViewModel currentView;

        public ViewModel CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }
        public ActionCommand SwitchViews { get; private set; }

        public ActionCommand Exit { get; set; }
    }
}
