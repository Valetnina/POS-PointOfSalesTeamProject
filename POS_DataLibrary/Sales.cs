using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_DataLibrary
{
    public class Sales
    {
        private string seller;

        public string Seller
        {
            get { return seller; }
            set { seller = value; }
        }

        private OrderItems orderItems;

        public OrderItems OrderItems
        {
            get { return orderItems; }
            set { orderItems = value; }
        }
        private decimal itemTotal;

        public decimal ItemTotal
        {
            get { return itemTotal; }
            set { itemTotal = value; }
        }

    }
}
