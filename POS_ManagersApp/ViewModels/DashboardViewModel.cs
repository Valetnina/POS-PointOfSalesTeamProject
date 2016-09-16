using iTextSharp.text;
using iTextSharp.text.pdf;
using POS_DataLibrary;
using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS_ManagersApp.ViewModels
{

    class DashboardViewModel : ViewModel
    {
        private Database db;
        //Property to bind to the Pie Chart DataSource
        public ObservableCollection<Sales> SalesChartPerSeller
        {
            get
            {
                return GetPieChartData();
            }
        }
        //Method to populate the Pie Chart 

        private ObservableCollection<Sales> GetPieChartData()
        {
            ObservableCollection<Sales> allSalesList = new ObservableCollection<Sales>();
            try
            {
                allSalesList = db.getSalesPerSeller(DateTime.Parse(selectedMonth).Month, DateTime.Parse(selectedMonth).Year);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Unbale to fetch data from the database", "Database Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                throw (ex);
            }
            return allSalesList;
        }
        public ObservableCollection<SalesChartData> salesChartData
        {
            get
            {
                return getSalesChartData();
            }
        }
        //Property to bind to the dtagrid ItemSource Top Items
        public ObservableCollection<Sales> AllSales
        {
            get
            {
                return GetAllSales();
            }
        }
        //Method to populate the data grid
        private ObservableCollection<Sales> GetAllSales()
        {
            ObservableCollection<Sales> allSalesList = new ObservableCollection<Sales>();
            try
            {
                allSalesList = db.getSales(DateTime.Parse(selectedMonth).Month, DateTime.Parse(selectedMonth).Year);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Unbale to fetch data from the database", "Database Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                throw (ex);
            }
            return allSalesList;
        }
        //Property to bind to the dtagrid ItemSource Top Items
        public ObservableCollection<Sales> TopItems
        {
            get
            {
                return GetTopItems();
            }
        }
        //Method to populate the data grid
        private ObservableCollection<Sales> GetTopItems()
        {
            ObservableCollection<Sales> topItemsList = new ObservableCollection<Sales>();
            try
            {
                topItemsList = db.getTopItemsPerMonth(DateTime.Parse(selectedMonth).Month, DateTime.Parse(selectedMonth).Year);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Unbale to fetch data from the database", "Database Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                throw (ex);
            }
            return topItemsList;
        }
        public ObservableCollection<ColumnChartData> ColumnChartSales
        {
            get
            {
                return GetColumnChartData();
            }
        }

        public ObservableCollection<string> CalendarMonths { get; set; }
        public DashboardViewModel()
        {
            //Initialize
            try
            {
                db = new Database();
            }
            catch (Exception e)
            {
                MessageBox.Show("Fatal Error: Unable to connect to database", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                throw e;
            }

            CalendarMonths = new ObservableCollection<string>();
            for (int i = 0; i < 3; i++)
            {
                CalendarMonths.Add(DateTime.Now.AddMonths(-i).ToString("Y"));
            }

            SelectedMonth = CalendarMonths[0];
            //Register commands
            ExportToPDF = new ActionCommand(p => OnExportToPDF());
            SendEmail = new ActionCommand(p => OnSendEmail());



        }
        // Method to  fetch data from the database for the Column chart
        private ObservableCollection<ColumnChartData> GetColumnChartData()
        {
            ObservableCollection<ColumnChartData> chart = new ObservableCollection<ColumnChartData>();
            ObservableCollection<ProductCategory> categoriesList = new ObservableCollection<ProductCategory>();
            try
            {
                categoriesList = db.getAllCategories();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Unbale to fetch data from the database", "Database Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                throw (ex);
            }


            foreach (var item in categoriesList)
            {
                chart.Add(new ColumnChartData { Name = item.CategoryName, TotalSales = db.getAllSalesByItem(item.CategoryName, DateTime.Parse(selectedMonth).Month, DateTime.Parse(selectedMonth).Year) });
            }
            return chart;
        }
        // Method to  populate the source of  the Pie chart

        private ObservableCollection<SalesChartData> getSalesChartData()
        {
            ObservableCollection<SalesChartData> chart = new ObservableCollection<SalesChartData>();

            foreach (var item in SalesChartPerSeller)
            {
                chart.Add(new SalesChartData { SellerChart = item.Seller, SalesPerSeller = item.ItemTotal });
            }
            return chart;
        }
        private object selectedItem = null;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
            }
        }
        private string selectedMonth;

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                RaisePropertyChanged("SelectedMonth");
                RaisePropertyChanged("SalesChartPerSeller");
                RaisePropertyChanged("TotalSalesPerMonth");
                RaisePropertyChanged("ColumnChartSales");
                RaisePropertyChanged("AllSales");
                RaisePropertyChanged("TopItems");
                RaisePropertyChanged("salesChartData");

            }
        }
        public decimal TotalSalesPerMonth
        {
            get
            {
                return ColumnChartSales.Sum(od => od.TotalSales);
            }
        }

        public ActionCommand ExportToPDF { get; private set; }
        public ActionCommand SendEmail { get; private set; }
        private Document document;
        protected void OnExportToPDF()
        {
            try
            {
                int noOfColumns = 0, noOfRows = 0;
                noOfColumns = 4;
                noOfRows = AllSales.Count;

                float HeaderTextSize = 8;
                float ReportNameSize = 10;
                float ReportTextSize = 8;
                float ApplicationNameSize = 7;

                // Creates a PDF document

                document = null;
                document = new Document(PageSize.A4, 0, 0, 15, 5);

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(2);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase("Total Month Sales", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_LEFT;

                // Creates a phrase to show the current date at the right hand side of the header.
                Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                // Creates a PdfPCell which accepts the date phrase as a parameter.
                PdfPCell clDate = new PdfPCell(phDate);
                // Sets the Horizontal Alignment of the PdfPCell to right.
                clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                // Sets the border of the cell to zero.
                clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                // Creates a phrase which holds the file name.
                Phrase phHeader = new Phrase("Sales for " + SelectedMonth, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                mainTable.AddCell(clHeader);

                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.

                mainTable.AddCell(new Phrase("Seller", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)));
                mainTable.AddCell(new Phrase("Product", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)));
                mainTable.AddCell(new Phrase("Qty", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)));
                mainTable.AddCell(new Phrase("Total", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)));

                // Reads the gridview rows and adds them to the mainTable
                foreach (var item in AllSales)
                {

                    {
                        mainTable.AddCell(new Phrase(item.Seller, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL)));
                        mainTable.AddCell(new Phrase(item.OrderItems.Name, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL)));
                        mainTable.AddCell(new Phrase(item.OrderItems.Quantity.ToString(), FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL)));
                        mainTable.AddCell(new Phrase(item.ItemTotal.ToString(), FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL)));

                    }
                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }


                // Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                PdfWriter wri = PdfWriter.GetInstance(document, new System.IO.FileStream("SalesReport.pdf", FileMode.Create));

                document.Open();//Open Document to write


                Paragraph paragraph = new Paragraph("data Exported From DataGridview!");

                document.Add(mainTable);
                //doc.Add(t1);
                document.Close(); //Close document
                //Ope the pdf file just created
                System.Diagnostics.Process.Start(@"SalesReport.pdf");
            }
            catch
            {
                MessageBox.Show("The Pdf could not be created or it could not be opened", "Error PDF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OnSendEmail()
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "mailto:tinursu@gmail.com?subject=SalesReport&body=Sales";
                proc.Start();
            }
            catch
            {
                MessageBox.Show("The Mail Client could not be opened. Check your computer settings", "Error Send Email", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    public class SalesChartData
    {
        public string SellerChart { get; set; }
        public decimal SalesPerSeller { get; set; }
    }
    public class ColumnChartData
    {
        public string Name { get; set; }
        public decimal TotalSales { get; set; }
    }
}
