using EnterpriseMVVM.Windows;
using POS_ViewsLibrary;

namespace POS_SellersApp.ViewModels
{
    class SellersMainWindowViewModel : ViewModel
    {

        public SellersMainWindowViewModel()
        {
            SwitchViews = new ActionCommand<string>(OnSwitchViews);

        }

        private ProductsCatalogViewModel ProductsCatalogViewModel = new ProductsCatalogViewModel();

        private PaimentViewModel payiementViewModel = new PaimentViewModel();

        private ViewModel currentView;

        public ViewModel CurrentView
        {
            get { return currentView; }
            set { SetProperty(ref currentView, value); }
        }

        public ActionCommand<string> SwitchViews { get; private set; }

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
