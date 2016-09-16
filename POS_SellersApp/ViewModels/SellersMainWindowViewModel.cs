using POS_DataLibrary;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Drawing.Printing;
using System.Drawing;
using POS_SellersApp.Views;
using System.Windows.Input;

namespace POS_SellersApp.ViewModels
{

    public class SellersMainWindowViewModel : ViewModel
    {
        private const double TAX = 0.15;
        private Database db;

        public SellersMainWindowViewModel()
        {
            try
            {
                db = new Database();
            }
            catch (Exception e)
            {
                MessageBox.Show("Fatal Error: Unable to connect to database", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                throw e;
            }

            //Register for messages from different viewModels

            MessengerPoduct.Default.Register<Product>(this, (product) =>
            {
                ReceiveMessage(product);
            });
            MessengerDone.Default.Register<String>(this, message =>
                 {
                     RecivedDoneMessage(message);
                 });
            SendLogoutMessage = new ActionCommand(p => OnSendLogoutMessage("login"));
            //Initialize comopents with some values
            OrderItems = new ObservableCollection<OrderItems>();

            //Register CollectionChangedEvent for the OrderItems
            OrderItems.CollectionChanged += ContentCollectionChanged;

            //Initialize commands
            RemoveItem = new ActionCommand(p => OnRemoveItem());
            CancelOrder = new ActionCommand(p => OnCancelOrder());
            SwitchViews = new ActionCommand((p) => OnSwitchViews(p.ToString()));
            PrintReceipt = new ActionCommand(p => OnPrintReceipt());
            SendOrder = new ActionCommand(p => OnSendOrder());
            DecreaseQuantity = new ActionCommand(p => OnDecreaseQuantity());
            currentView = ProductsCatalogViewModel;
            try
            {
                OrderNo = string.Format("Order# {0}", (db.getLastOrderNo() + 1).ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't fetch the dat afrom the database", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AmountTendered = "0";

        }

        private void OnSendOrder()
        {
            if (CurrentView != pvm && OrderItems.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("You cannot send an order that has not been paied. Do you want to go to paiement page?", "Send Order", MessageBoxButton.OKCancel, MessageBoxImage.Hand);
                if (result == MessageBoxResult.OK)
                {
                    CurrentView = pvm;
                }
            }
            else if (OrderItems.Count == 0)
            {
                MessageBox.Show("You cannot send an empty order");
            }
            else
            {
                MessageBox.Show("Register paiement and click Done. The order will be registered and sent to the kitchen");
            }
        }

        private void OnDecreaseQuantity()
        {
            if (SelectedOrderItem == null)
            {
                MessageBox.Show("Select item to decrease quantity");
                return;
            }
            //Check if the product was already selected
            if (OrderItems.Any(p => p.UPCCode == SelectedOrderItem.UPCCode))
            {
                if (OrderItems.First(p => p.UPCCode == SelectedOrderItem.UPCCode).Quantity <= 1)
                {
                    if (SelectedOrderItem != null)
                    {
                        MessageBoxResult result = MessageBox.Show("You are about to delete an item. Do you want to continue?", "Delete Item", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            OrderItems.Remove(SelectedOrderItem);
                        };
                    }
                }
                else
                {
                    OrderItems.First(p => p.UPCCode == SelectedOrderItem.UPCCode).Quantity -= 1;
                    RaisePropertyChanged("OrderSubTotal");
                    RaisePropertyChanged("OrderTax");
                    RaisePropertyChanged("BalanceDue");
                }

            }
        }


        public string AmountTendered { get; set; }
        private void RecivedDoneMessage(string message)
        {
            switch (message)
            {
                case "Register":

                    Order order = new Order { Date = DateTime.Now, StoreNo = "OV001", UserId = UserLoggedIn.Id, OrderAmount = OrderSubTotal, Tax = OrderTax };
                    AmountTendered = pvm.Amount;

                    try
                    {
                        MessageBoxResult result = MessageBox.Show("Order has been sent to processing. Do you want to print the receipt?", "Order Processed", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            OnPrintReceipt();
                        }
                        if (OrderItems.Count > 0)
                        {
                            int lastOrderId = db.saveOrderAndOrderItems(order, OrderItems.ToList());
                            OrderNo = string.Format("Order# {0}", lastOrderId.ToString());
                            CurrentView = ProductsCatalogViewModel;
                            OrderItems.Clear();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error inserting data to the database");
                        throw ex;
                    }
                    break;
                case "Back":
                default:
                    OnSwitchViews("catalog");
                    break;
            }
        }

        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("OrderSubTotal");
            RaisePropertyChanged("OrderTax");
            RaisePropertyChanged("BalanceDue");
            RaisePropertyChanged("SelectedOrderItem");
        }
        private ObservableCollection<OrderItems> orderItems;

        public ObservableCollection<OrderItems> OrderItems
        {
            get { return orderItems; }
            set
            {
                orderItems = value;
                RaisePropertyChanged("OrderItems");
            }
        }



        private OrderItems selectedOrderItem;
        public OrderItems SelectedOrderItem
        {
            get
            {
                return selectedOrderItem;
            }

            set
            {
                selectedOrderItem = value;
                RaisePropertyChanged("SelectedOrderItem");
            }
        }

        private User userLoggedIn;
        public User UserLoggedIn
        {
            get
            {
                return userLoggedIn;
            }

            set
            {
                userLoggedIn = value;

                RaisePropertyChanged("UserLoggedIn");
            }
        }
        public decimal OrderSubTotal
        {
            get
            {
                decimal value = orderItems.Sum(od => od.Total);
                if (value == 0)
                {
                    return 0;
                }
                else
                {
                    return value;
                }

            }
        }
        public decimal OrderTax
        {
            get
            {

                return OrderSubTotal * (decimal)TAX;
            }

        }
        public decimal BalanceDue
        {
            get
            {
                return OrderSubTotal + OrderTax;
            }

        }
        private string orderNo;
        public string OrderNo
        {
            get
            {
                return orderNo;
            }

            set
            {
                orderNo = value;
                RaisePropertyChanged("OrderNo");
            }
        }

        readonly static private ProductsCatalogViewModel ProductsCatalogViewModel = new ProductsCatalogViewModel();


        private ViewModel currentView;

        public ViewModel CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }
        public string DisplayedImagePath
        {
            get { return "/POS_SellersApp;component/Logo_Small.png"; }
        }
        private string payed;

        public string Payed
        {
            get { return payed; }
            set
            {
                payed = value;
                RaisePropertyChanged("Paied");
            }
        }

        #region Commands

        //Declaring the Commands
        public ActionCommand SendLogoutMessage { get; private set; }
        public ActionCommand RemoveItem { get; private set; }
        public ActionCommand CancelOrder { get; private set; }
        public ActionCommand PrintReceipt { get; private set; }
        public ActionCommand SendOrder { get; set; }
        public ActionCommand SwitchViews { get; private set; }
        public ActionCommand DecreaseQuantity { get; set; }
        private void ReceiveMessage(Product product)
        {
            //Check if the product was already selected
            if (OrderItems.Any(p => p.UPCCode == product.UPCCode))
            {
                OrderItems.First(p => p.UPCCode == product.UPCCode).Quantity += 1;
                RaisePropertyChanged("OrderSubTotal");
                RaisePropertyChanged("OrderTax");
                RaisePropertyChanged("BalanceDue");
            }
            else
            {
                OrderItems.Add(new POS_DataLibrary.OrderItems
                {
                    UPCCode = product.UPCCode,
                    CategoryName = product.CategoryName.ToString(),
                    CategoryId = product.CategoryId,
                    Quantity = 1,
                    Price = product.Price,
                    Name = product.Name
                });
            }
        }
        private void OnRemoveItem()
        {
            if (SelectedOrderItem != null)
            {
                MessageBoxResult result = MessageBox.Show("You are about to delete an item. Do you want to continue?", "Delete Item", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    OrderItems.Remove(SelectedOrderItem);
                };
            }
        }
        private void OnCancelOrder()
        {
            if (OrderItems.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("You are about to delete current order. Do you want to continue?", "Delete Order", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    OrderItems.Clear();
                };
            }
        }
        private void OnSendLogoutMessage(string v)
        {
            MessengerLogout.Default.Send(v);
        }
        readonly static PaimentViewModel pvm = new PaimentViewModel();

        private void OnSwitchViews(string destination)
        {
            switch (destination)
            {
                case "pay":
                    if (OrderItems.Count < 1)
                    {
                        MessageBox.Show("You cannot pay an empty order", "Paiement Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    pvm.Balance = BalanceDue.ToString("#.##");
                    pvm.Amount = "0";
                    pvm.IsCash = false;
                    CurrentView = pvm;
                    break;

                case "catalog":
                default:
                    CurrentView = ProductsCatalogViewModel;
                    break;
            }

        }
        #endregion

        #region PrintReceipt


        private void OnPrintReceipt()
        {
            if (AmountTendered == "0")
            {
                MessageBox.Show("The order is not payed yet. You cannot print it!", "Print", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            try
            {
                var doc = new PrintDocument();
                doc.PrintPage += new PrintPageEventHandler(CreateReceipt);
                doc.Print();
            }
            catch (Exception)
            {
                MessageBox.Show("Could not locate an available printing device", "Printing error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //this prints the reciept
            Graphics graphic = e.Graphics;
            Font font = new Font("Courier New", 12); //must use a mono spaced font as the spaces need to line up

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 40;

            graphic.DrawString(" Olya&Valya", new Font("Courier New", 18), new SolidBrush(Color.Black), startX, startY);
            string top = "Item Name".PadRight(15) + "Price".PadRight(10) + "Qty".PadRight(5) + "Total".PadRight(5);
            graphic.DrawString(top, font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent

            decimal totalprice = OrderSubTotal;

            foreach (var item in OrderItems)
            {
                //create the string to print on the reciept
                string productDescription = item.Name;
                string productQty = item.Quantity.ToString();
                string productPrice = item.Price.ToString("#.##");
                string productTotal = item.Total.ToString("#.##");

                string productLine = productDescription.PadRight(20) + productPrice.PadRight(10) + productQty.PadRight(5) + productTotal.PadRight(5);
                graphic.DrawString(productLine, font, new SolidBrush(Color.Black), startX, startY + offset);

                offset = offset + (int)fontHeight + 5; //make the spacing consistent
            }
            //TODO: Pass the paiement information
            string paiement = AmountTendered;
            decimal change = totalprice + OrderTax - decimal.Parse(paiement);

            //when we have draw all of the items add the total

            offset = offset + 20; //make some room so that the total stands out.

            graphic.DrawString("Total before tax ".PadRight(30) + String.Format("{0:c}", totalprice), new Font("Courier New", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Tax ".PadRight(30) + String.Format("{0:c}", OrderTax), new Font("Courier New", 9, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("Total to pay ".PadRight(30) + String.Format("{0:c}", (totalprice + OrderTax)), new Font("Courier New", 12, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Payed ".PadRight(30) + String.Format("{0:c}", paiement), font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("CHANGE ".PadRight(30) + String.Format("{0:c}", change), font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("     Thank-you and Enjoy you Meal,", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("       please come back soon!", font, new SolidBrush(Color.Black), startX, startY + offset);

        }
        #endregion



    }

}