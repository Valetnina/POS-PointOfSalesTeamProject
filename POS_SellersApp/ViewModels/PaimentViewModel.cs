using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS_SellersApp.ViewModels
{
    public class PaimentViewModel : ViewModel
    {
        private decimal balanceDue;
        private bool IsCash;
        public decimal BalanceDue
        {
            get
            {
                return balanceDue;
            }
            set
            {
                balanceDue = value;
                RaisePropertyChanged("BalanceDue");
            }
        }
       
        public PaimentViewModel(decimal balance)
        {
            this.BalanceDue = balance;
            Done = new ActionCommand(p => OnDoneCommand());

            //Handle command on buttons
            AddAmount = new ActionCommand(p => OnAddAmount(p.ToString()), p=> IsCash);
            PayCash = new ActionCommand(p => OnPayCash());
            Amount = "0";
        }

        private void OnPayCash()
        {
            IsCash = true;
        }


        public ActionCommand Done { get; private set; }

        public ActionCommand AddAmount { get; private set; }
        public ActionCommand PayCash { get; private set; }

        private void OnDoneCommand()
        {
            MessengerDone.Default.Send("Done");
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
                    return BalanceDue.ToString();
                }
                else
                {
                    return (BalanceDue - decimal.Parse(Amount)) + "";
                }

            }
        }

       
    }
}
