using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_DataLibrary
{
   public class Product: INotifyPropertyChanged
    {
        private string uPCCode;

        public string UPCCode
        {
            get { return uPCCode; }
            set { uPCCode = value;
            RaisePropertyChanged("UPCCode");
            }
        }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value;
            RaisePropertyChanged("Price");
            }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value;
            RaisePropertyChanged("Name");
            }
        }

        private ProductCategory categoryName;

        public ProductCategory CategoryName
        {
            get { return categoryName; }
            set { categoryName = value;
            RaisePropertyChanged("CategoryName");
            }
        }
        private Image picture;

        public Image Picture
        {
            get { return picture; }
            set { picture = value;
            RaisePropertyChanged("Picture");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        } 
    }
}
