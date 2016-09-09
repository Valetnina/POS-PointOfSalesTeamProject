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
                RaisePropertyChanged("User");
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
            Messenger.Default.Register<string>(this, (message)=>
            {

                OnSwitchViews(message);

            });
            currentView = loginView;
            SendLogoutMessage = new ActionCommand(p => OnSendLogoutMessage("login"));
        }

       private void OnSendLogoutMessage(string p)
       {
           Messenger.Default.Send("login");
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
       public ActionCommand SendLogoutMessage { get; private set; }

        

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
