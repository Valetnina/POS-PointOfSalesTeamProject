using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_DataLibrary
{
    public class User
    {
        public string UserName {  get; set;}
        public string Password { get; set; }
        public string FirstName { get; set; }
        public int Id { get; set; }
        public string LastName { get; set; }
        public bool IsManager { get; set; }

    }
}
