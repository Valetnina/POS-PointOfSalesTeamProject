using POS_DataLibrary;
using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS_PointOfSales.ViewModels
{
    class LoginViewModel:ViewModel
    {
        private Database db;
        private DateTime CurrentTime { get; set; }
        public ActionCommand<string> Login { get; set; }
        public LoginViewModel()
        {
            //TODO handle Exceptions
            db = new Database();
            Login = new ActionCommand<string>(OnLogin);
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
            SetProperty(ref userName, value);
            }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value;
            SetProperty(ref password, value);
            }
        }
        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value;
            SetProperty(ref firstName, value);
            }
        }
        private void OnLogin(string parameter)
        {
            List<User> usersList = db.getUserByUserName(UserName);
            foreach (User user in usersList)
            {
                if (Password == user.Password)
                {
                    MessageBox.Show("Authenticated succesfully user " + UserName);

                }
                else
                {
                    MessageBox.Show("Could not authenticate user " + UserName);
                }
            }
        }
/*
        private bool CanLogin()
        {
            return (string.IsNullOrWhiteSpace(UserName) && string.IsNullOrWhiteSpace(Password));
        }
      
        */



        
    }
}
