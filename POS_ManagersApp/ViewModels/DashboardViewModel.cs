using iTextSharp.text;
using iTextSharp.text.pdf;
using POS_DataLibrary;
using POS_ViewsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
  

    class DashboardViewModel:ViewModel
    {
        private Database db;
        public ObservableCollection<Sales> SalesChartPerSeller { get; private set; }
        public ObservableCollection<SalesChartData> salesChartData { get; private set; }

        public ObservableCollection<Sales> TodaySales { get; private set; }
        public ObservableCollection<Sales> TopItems { get; set; }

        private ObservableCollection<AreaData> Areas { get; set; }




        public DashboardViewModel()
        {
            db = new Database();
            SalesChartPerSeller = db.getSalesPerSeller();
            TodaySales = db.getTodaySales();
            TopItems = db.getTopItemsPerMonth();
            ExportToPDF = new ActionCommand(p => OnExportToPDF());
            SendEmail = new ActionCommand(p => OnSendEmail());
            salesChartData = new ObservableCollection<SalesChartData>();
            foreach (var item in SalesChartPerSeller)
            {
                salesChartData.Add(new SalesChartData { SellerChart = item.Seller, SalesPerSeller= item.ItemTotal});
            }
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
                // selected item has changed
                selectedItem = value;
            }
        }

        public ActionCommand ExportToPDF { get; private set; }
        public ActionCommand SendEmail { get; private set; }
        private Document document;
        protected void OnExportToPDF()
        {
            
            int noOfColumns = 0, noOfRows = 0;
            DataTable tbl = null;


            noOfColumns = 5;
            noOfRows = TodaySales.Count;


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
            Phrase phHeader = new Phrase("Contact List", FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
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

            foreach (var p in TodaySales[0].GetType().GetProperties().Where(p => p.GetGetMethod().GetParameters().Count() == 0))
            {

                Phrase ph = null;
                ph = new Phrase(p.GetValue(TodaySales[0], null).ToString(), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                mainTable.AddCell(ph);
            }
           
          
            // Reads the gridview rows and adds them to the mainTable


            foreach (var item in TodaySales)
            {

                foreach (var p in item.GetType().GetProperties().Where(p => p.GetGetMethod().GetParameters().Count() == 0))
                {
                    string s = p.GetValue(item, null).ToString();

                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                    mainTable.AddCell(ph);
                }
                // Tells the mainTable to complete the row even if any cell is left incomplete.
                mainTable.CompleteRow();
            }


           // Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(document, new System.IO.FileStream("Test.pdf", FileMode.Create));

            document.Open();//Open Document to write


            Paragraph paragraph = new Paragraph("data Exported From DataGridview!");

            document.Add(mainTable);
            //doc.Add(t1);
            document.Close(); //Close document
                         //
            MessageBox.Show("PDF Created!");
            
        }
        private void OnSendEmail()
        {
           
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream); //capture the object
            document.Open();
            document.Add(new Paragraph("First Paragraph"));
            document.Add(new Paragraph("Second Paragraph"));
            writer.CloseStream = false; //set the closestream property
            document.Close(); //close the document without closing the underlying stream
            memoryStream.Position = 0;
            
            MailMessage mm = new MailMessage("tinursu@gmail.com", "migalatii@gmail.com")
            {
                Subject = "subject",
                IsBodyHtml = true,
                Body = "body"
            };

            mm.Attachments.Add(new Attachment(memoryStream, "filename.pdf"));
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("tinursu@gmail.com", "*******")

            };

            smtp.Send(mm);
        }
        
    }
    public class SalesChartData
    {
        public string SellerChart { get; set; }
        public decimal SalesPerSeller { get; set; }
    }
    public class AreaData
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }
}
