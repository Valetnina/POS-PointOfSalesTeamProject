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
                SetProperty(ref userLoggedIn, value);
            }
        }

        private LoginViewModel loginView = new LoginViewModel();
       
      
       public SellersStartupViewModel()
       {
            SwitchViews = new ActionCommand(p => OnSwitchViews("catalog"));
            Messenger.Default.Register<User>(this, (user) =>
            {
                User = user;
                OnSwitchViews("catalog");
               
            });
            currentView = loginView;
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
                   CurrentView = new SellersMainWindowViewModel();
                   break;
               case "login":
               default:
                  CurrentView = loginView;
                   break;
           }

       }
    }
}
