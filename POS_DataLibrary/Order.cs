using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_DataLibrary
{
    class Order
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public string StoreNo { get; set; }
        public User UserId { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal Tax { get; set; }
    }
}
