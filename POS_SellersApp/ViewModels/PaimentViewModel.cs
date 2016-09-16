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
        public bool IsCash;

        private string balance;
        private bool IsDotClicked;
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
            }
        }
        public PaimentViewModel()
        {

            Done = new ActionCommand(p => OnDoneCommand(p.ToString()));

            //Handle command on buttons
            AddAmount = new ActionCommand(p => OnAddAmount(p.ToString()), p => IsCash);
            PayCash = new ActionCommand(p => OnPayCash());
            PayCredit = new ActionCommand(p => OnPayCredit());
            PayGift = new ActionCommand(p => OnPayGift());

            //Initialize 
            Amount = "0";
            Balance = "0";
            CashEnabled = true;
            CardEnabled = true;
            GiftEnabled = true;
        }

        private bool cashEnabled;
        public bool CashEnabled
        {
            get { return cashEnabled; }
            set
            {
                cashEnabled = value;
                RaisePropertyChanged("CashEnabled");
            }
        }

        private bool cardEnabled;
        public bool CardEnabled
        {
            get { return cardEnabled; }
            set
            {
                cardEnabled = value;
                RaisePropertyChanged("CardEnabled");
            }
        }

        private bool giftEnabled;
        public bool GiftEnabled
        {
            get { return giftEnabled; }
            set
            {
                giftEnabled = value;
                RaisePropertyChanged("GiftEnabled");
            }
        }


        private void OnPayCash()
        {
            IsCash = true;
            CashEnabled = false;
            CardEnabled = true;
            GiftEnabled = true;
        }

        private void OnPayCredit()
        {
            MessageBoxResult result = MessageBox.Show("Wait for the payment and press OK", "Card Paiement", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                Amount = Balance;
                IsCash = false;
                CashEnabled = true;
                CardEnabled = false;
                GiftEnabled = true;
            }
        }

        private void OnPayGift()
        {
            MessageBoxResult result = MessageBox.Show("Scan the Gift card and press ok", "Card Paiement", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                Amount = Balance;
                IsCash = false;
                CashEnabled = true;
                CardEnabled = true;
                GiftEnabled = false;
            }
        }
        public ActionCommand Done { get; private set; }

        public ActionCommand AddAmount { get; private set; }
        public ActionCommand PayCash { get; private set; }

        public ActionCommand PayCredit { get; private set; }

        public ActionCommand PayGift { get; private set; }
        
        //Method to execute on Done Command
        private void OnDoneCommand(string message)
        {
            switch (message)
            {
                case "Register":
                    try
                    {
                        if (decimal.Parse(Change) < 0 || Amount == "0")
                        {
                            MessageBox.Show("you cannot commit transaction. Client paiement cannot be less than the Balance Due");
                        }
                        else MessengerDone.Default.Send(message);
                    }
                    catch
                    {
                        MessageBox.Show("Could not parse the change field", "Parsing error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                    break;
                case "Back":
                default:
                    MessengerDone.Default.Send(message);
                    break;

            }
            Balance = "0";
            Amount = "0";
            IsCash = false;
        }

        private void OnAddAmount(string number)
        {

            switch (number)
            {
                case "c":
                    Amount = "0";
                    break;
                case ".":
                    if (Amount == "0")
                    {
                        return;
                    }
                    if (!IsDotClicked)
                    {
                        if (Amount == "0")
                        {
                            Amount = number;
                        }
                        else
                        {
                            Amount += number;
                        }
                        IsDotClicked = true;
                    }
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
                decimal parsedAmount;
                if (decimal.TryParse(Amount, out parsedAmount))
                {
                    if (parsedAmount == 0)
                    {
                        return "-" + Balance;
                    }
                    else return (decimal.Parse(Amount)) - decimal.Parse(Balance) + "";
                }
                else return "0";
            }
        }
    }
}
