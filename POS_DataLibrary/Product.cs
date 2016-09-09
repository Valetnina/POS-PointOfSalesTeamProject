using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_DataLibrary
{
   public class Product
    {
        public string UPCCode { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public ProductCategory CategoryName { get; set; }
        public Image Picture { get; set; }
    }
}
