using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_SellersApp.ViewModels
{
    public class PaimentViewModel : ViewModel
    {
        public Decimal  BalanceDue;

        public PaimentViewModel(decimal balanceDue)
        {
            this.BalanceDue = balanceDue;
            Done = new ActionCommand(p => OnDoneCommand());
            AddAmount = new ActionCommand(p => OnAddAmount(p.ToString()));
        }

        public ActionCommand Done { get; private set; }

        public ActionCommand AddAmount { get; private set; }

        private void OnDoneCommand()
        {
            MessengerDone.Default.Send("Done");
        }

        private void OnAddAmount(string number)
        {
            Amount = number;
        }

        private string amount;

        public string Amount
        {
            get { return amount; }
            set { amount = value; RaisePropertyChanged("Amount"); }
        }

      
    }
}
