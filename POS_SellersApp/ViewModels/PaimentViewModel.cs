using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POS_SellersApp.ViewModels
{
    public class PaimentViewModel : ViewModel
    {
        private bool IsCash;
        private bool IsMessageReceived;
       
       private string balance;
       public string Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
                RaisePropertyChanged("Balance");
                //if(decimal.Parse(Balance) > 0 && IsMessageReceived)
                //{
                //    MessengerBalance.Default.Unregister(this);
                //}
            }
        }
        public PaimentViewModel()
        {

            Done = new ActionCommand(p => OnDoneCommand(p.ToString()), p=> CanExecuteDone());

            //Handle command on buttons
            AddAmount = new ActionCommand(p => OnAddAmount(p.ToString()), p=> IsCash);
            PayCash = new ActionCommand(p => OnPayCash());
            Amount = "0";
            Balance = "0";
        }

        private bool CanExecuteDone()
        {
            if ((decimal.Parse(Change) < 0))
            {
                MessageBox.Show("You cannot register paiement. The Amount tendered Shoud be greater than the Balance Due");
                return false;
            }
            else return true;
        }

        private void OnPayCash()
        {
            IsCash = true;
        }


        public ActionCommand Done { get; private set; }

        public ActionCommand AddAmount { get; private set; }
        public ActionCommand PayCash { get; private set; }

        private void OnDoneCommand(string message)
        {
            MessengerDone.Default.Send(message);
            MessengerDone.Default.Unregister(this);
        }

        private void OnAddAmount(string number)
        {
            
            switch(number){
                case "c":
                    Amount = "0";
                    break;
                default:
                    if (Amount == "0")
                    {
                        Amount = number;
                    }
                    else
                    {
                        Amount += number;
                    }
                    break;
            }
        }

        private string amount;
        public string Amount
        {
            get
            {
               return amount;
            }
            set
            {
                amount = value;
                RaisePropertyChanged("Amount");
                RaisePropertyChanged("Change");
            }
        }
      
        public string Change
        {
            get
            {
                if (decimal.Parse(Amount) == 0)
                {
                    return Balance;
                }
                else
                {
                    return (-decimal.Parse(Balance) + decimal.Parse(Amount)) + "";
                }

            }
        }
        //private ICommand _closeCommand;
        //public ICommand CloseCommand
        //{
        //    get
        //    {
        //        if (_closeCommand == null)
        //            _closeCommand = new ActionCommand(param => this.OnRequestClose());

        //        return _closeCommand;
        //    }
        //}

        //public event EventHandler RequestClose;

        //void OnRequestClose()
        //{
        //    EventHandler handler = this.RequestClose;
        //    if (handler != null)
        //        handler(this, EventArgs.Empty);
        //}

    }
}
