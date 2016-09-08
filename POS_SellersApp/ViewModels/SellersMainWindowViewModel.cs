using POS_ViewsLibrary;

namespace POS_SellersApp.ViewModels
{
   public class SellersMainWindowViewModel : ViewModel
    {

        public SellersMainWindowViewModel()
        {
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
