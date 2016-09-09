using POS_DataLibrary;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;
using System.ComponentModel;
using System.Windows;

namespace POS_SellersApp.ViewModels
{
    public class SellersMainWindowViewModel : ViewModel,INotifyPropertyChanged 
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
                RaisePropertyChanged2("User");
                RaisePropertyChanged2("UserName");
            }
        }
        public string UserName
        {
            get
            {
                if (user != null)
                {
                    return user.UserName;
                }
                return "NOT SET";
            }
        }
        
        public SellersMainWindowViewModel()
        {
            Messenger.Default.Register<User>(this, (user) =>
            {
                User = user;
            });

            SwitchViews = new ActionCommand(p=> OnSwitchViews("catalog"));
            OrderNo = 1;
            currentView = ProductsCatalogViewModel;
        }

        private ProductsCatalogViewModel ProductsCatalogViewModel = new ProductsCatalogViewModel();        

        private PaimentViewModel payiementViewModel = new PaimentViewModel();


        private ViewModel currentView;

        public ViewModel CurrentView
        {
            get { return currentView; }
            set { RaisePropertyChanged2("CurrentView"); }
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
                RaisePropertyChanged2("OrderNo");
            }
        }
        public new event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged2(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        } 
    }
    }
