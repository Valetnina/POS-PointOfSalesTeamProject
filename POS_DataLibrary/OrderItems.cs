using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_DataLibrary
{
    public class OrderItems : INotifyPropertyChanged
    {
        private int orderId;
        public int OrderId
        {
            get
            {
                return orderId;
            }

            set
            {
                orderId = value;
                RaisePropertyChanged("OrderId");
            }
        }
        private int quantity;
        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                RaisePropertyChanged("Quantity");
                RaisePropertyChanged("Total");
            }
        }

        public double Tax
        {
            get
            {
                return tax;
            }

            set
            {
                tax = value;
                RaisePropertyChanged("Tax");
            }
        }
        public decimal Total
        {
            get
            {
                return Quantity * Price;
            }
        }
        private double tax;

        private string uPCCode;

        public string UPCCode
        {
            get { return uPCCode; }
            set
            {
                uPCCode = value;
                RaisePropertyChanged("UPCCode");
            }
        }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChanged("Price");
                RaisePropertyChanged("Total");
            }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string categoryName;

        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                RaisePropertyChanged("CategoryName");
            }
        }
        private int categoryId;

        public int CategoryId
        {
            get { return categoryId; }
            set
            {
                categoryId = value;
                RaisePropertyChanged("CategoryId");
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
        public override string ToString()
        {
            return Name;
        }
    }
}
