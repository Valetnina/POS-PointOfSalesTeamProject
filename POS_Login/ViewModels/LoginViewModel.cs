using POS_DataLibrary;
using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Remoting.Contexts;
using System.ComponentModel;
using System.Windows.Threading;
using System.Data.SqlClient;

namespace POS_PointOfSales.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        //Declare  fields
        private Database db;
        private User user;

        //Properties
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }

        private DateTime CurrentTime { get; set; }
        private string userName;
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }
        private string password;

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }
        public string DisplayedImagePath
        {
            get { return "/POS-PointOfSales;component/Logo_Big.png"; }
        }

        public LoginViewModel()
        {
            User = new User();
            //TODO handle Exceptions

            try
            {
                db = new Database();
            }
            catch (Exception e)
            {
                MessageBox.Show("fatal Error: Unable to connect to database", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                throw e;
            }
            Login = new ActionCommand(p => OnLogin(UserName, Password),
                p => CanLogin);
        }
        //Declare Commands
        public ActionCommand Login { get; set; }
        

        
        //Method to validate when the login command can execute
        public bool CanLogin
        {
            get
            {
                return !String.IsNullOrWhiteSpace(UserName) &&
                        !String.IsNullOrWhiteSpace(Password);
            }
        }
        private void OnLogin(string userN, string pass)
        {
            if (userN != null)
            {
                try
                {
                    var user = db.getUserByUserName(userN, pass);
                    if (user != null)
                    {
                        User = user;
                        MessengerUser.Default.Send(User);
                        UserName = null;
                        Password = null;

                    }
                    else
                    {
                        MessageBox.Show("Could not authenticate user " + UserName);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Unable to fetch records from database", "Database Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    throw ex;
                }
            }
            else
            {
                MessageBox.Show("You didn't provide an username");

            }
        }

        

    }
}
