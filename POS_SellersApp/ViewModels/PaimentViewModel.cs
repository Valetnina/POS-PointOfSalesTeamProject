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
        }

        public ActionCommand Done { get; private set; }

        private void OnDoneCommand()
        {
            MessengerDone.Default.Send("Done");
        }
    }
}
