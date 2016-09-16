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
    public class ManagersMainWindowViewModel : ViewModel
    {
        private User userLoggedIn;
        public User UserLoggedIn
        {
            get { return userLoggedIn; }
            set
            {
                userLoggedIn = value;
                RaisePropertyChanged("User");
            }
        }

        public ManagersMainWindowViewModel()
        {
            SwitchViews = new ActionCommand((p) => OnSwitchViews(p.ToString()));
            SendLogoutMessage = new ActionCommand(p => OnSendLogoutMessage("login"));

            EnabledDashboard = true;
            EnabledManageProducts = true;
            OnSwitchViews("dashboard");
        }


        private bool enabledDashboard;

        public bool EnabledDashboard
        {
            get { return enabledDashboard; }
            set
            {
                enabledDashboard = value;
                RaisePropertyChanged("EnabledDashboard");
            }
        }

        private bool enabledManageProducts;

        public bool EnabledManageProducts
        {
            get { return enabledManageProducts; }
            set
            {
                enabledManageProducts = value;
                RaisePropertyChanged("EnabledManageProducts");
            }
        }

        readonly static DashboardViewModel dash = new DashboardViewModel();
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

    }
}
