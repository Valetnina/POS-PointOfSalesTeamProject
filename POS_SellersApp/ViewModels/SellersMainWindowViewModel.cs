using POS_DataLibrary;
using POS_ViewsLibrary;
using System.Windows;

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
        private string userName;
        public string UserName { get { return userName; }
            set {
                SetProperty(ref userName, value);
                MessageBox.Show("" + userName);
            } }
        public SellersMainWindowViewModel()
        {
            Messenger.Default.Register<User>(this, (user) =>
            {
                User = user;
            });

            SwitchViews = new ActionCommand(p=> OnSwitchViews("pay"));

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
    }
    }
