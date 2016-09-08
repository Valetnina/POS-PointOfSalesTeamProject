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

namespace POS_SellersApp
{
   public class SellersStartupViewModel: ViewModel
    {
        private User user;

        public User User
        {
            get { return user; }
            set
            {
                user = value;
                if (loginView.User != null)
                {
                    MessageBox.Show("Hello");
                    SetProperty(ref user, value);
                    OnSwitchViews("catalog");
                }
            }
        }

       private LoginViewModel loginView = new LoginViewModel();
       
       private SellersMainWindowViewModel sellersMainView = new SellersMainWindowViewModel();
       public SellersStartupViewModel()
       {
           SwitchViews = new ActionCommand(p => OnSwitchViews("catalog"));
           CurrentView = loginView;
          // User = loginView.User;
          // if (User != null)
           //{
           //    OnSwitchViews("catalog");
           //}
       }

       private ViewModel currentView;

       public ViewModel CurrentView
       {
           get { return currentView; }
           set {SetProperty(ref currentView, value); }
       }

       public ActionCommand SwitchViews { get; private set; }

       private void OnSwitchViews(string destination)
       {
           MessageBox.Show("Hello");
           if (!loginView.IsAuthenticated)
           {
               return;
           }
           switch (destination)
           {
               case "catalog":
                   if (loginView.User.IsManager)
                   {
                       MessageBox.Show("You don't have acces from this location") ;
                       return;
                   }
                   CurrentView = sellersMainView;
                   break;
               case "login":
               default:
                  CurrentView = loginView;
                   break;
           }

       }
    }
}
