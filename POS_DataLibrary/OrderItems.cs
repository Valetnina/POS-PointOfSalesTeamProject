using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_DataLibrary
{
    class OrderItems
    {
        public Order OrderId { get; set; }
        public Product ProductDetails { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }        
        public Order Tax { get; set; }
    }
}
