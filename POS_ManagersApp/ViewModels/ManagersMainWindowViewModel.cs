using POS_DataLibrary;
using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            EnabledDashboard = true;
            EnabledManageProducts = true;
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

             private void OnSwitchViews(string destination)
        {
            DashboardViewModel dash = new DashboardViewModel();
            ManageProductsViewModel manage = new ManageProductsViewModel();

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
