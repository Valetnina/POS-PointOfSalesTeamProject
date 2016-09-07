using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_DataLibrary;

namespace POS_SellersApp.ViewModels
{
  public  class ProductsCatalogViewModel:ViewModel
    {
        public ObservableCollection<Product> catalogCollection { get; set; }
        public ActionCommand<Object> PopulateWithProducts { get; private set; }

        public ProductsCatalogViewModel()
        {
            catalogCollection = new ObservableCollection<Product>();
            catalogCollection.Add(new Product { Name = "beer"});
            catalogCollection.Add(new Product { Name = "IceCream" });
            PopulateWithProducts = new ActionCommand<Object>(OnPopulateWithProducts);

        }

        private void OnPopulateWithProducts(object obj)
        {
            throw new NotImplementedException();
        }

        private void OnPopulateWithProducts()
        {
            throw new NotImplementedException();
        }


    }
}
