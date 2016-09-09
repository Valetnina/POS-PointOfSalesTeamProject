using POS_DataLibrary;
using POS_PointOfSales.ViewModels;
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
        private int dom;

        public int Dom
        {
            get { return dom; }
            set { dom = value;
            SetProperty(ref dom, value);
            }
        }

        private string userName;
        public string UserName { get { return userName; }
            set {
                SetProperty(ref userName, value);
            } }
        public SellersMainWindowViewModel()
        {
            Messenger.Default.Register<User>(this, (user) =>
            {
                User = user;
            });

            SwitchViews = new ActionCommand(p=> OnSwitchViews("catalog"));
            OrderNo = 1;
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

        private int orderNo;
        public int OrderNo
        {
            get
            {
                return orderNo;
            }

            set
            {
                orderNo = value;
                SetProperty(ref orderNo, value);
            }
        }
    }
    }
