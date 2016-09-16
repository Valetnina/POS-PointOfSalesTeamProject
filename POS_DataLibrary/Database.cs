using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS_DataLibrary
{
    public class Database
    {
        const string CONN_STRING = "Data Source=posabbott.database.windows.net;Initial Catalog=POS_DB;Integrated Security=False;User ID=posadmin;Password=Pictor12;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection conn;
        public Product product;

        public Database()
        {
            conn = new SqlConnection(CONN_STRING);
            conn.Open();
        }
        
        public ObservableCollection<Sales> getSalesPerSeller(int month, int year)
        {
            ObservableCollection<Sales> salesList = new ObservableCollection<Sales>();

            {
                SqlCommand cmd = new SqlCommand("SELECT UserId, FirstName, LastName, SUM(Price * Quantity) as TotalSAles FROM Orders, OrderItems, Users  where Orders.OrderId = OrderItems.OrderId and Orders.UserId= Users.Id  and  Month(Date) = @Month and Year(Date)=@Year GROUP BY UserId, FirstName, LastName", conn);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string seller = reader.GetString(reader.GetOrdinal("FirstName")) + " " + reader.GetString(reader.GetOrdinal("LastName"));
                            decimal sales = reader.GetDecimal(reader.GetOrdinal("TotalSAles"));

                            salesList.Add(new Sales() { Seller = seller, ItemTotal = sales });
                        }
                    }
                }
            }

            return salesList;

        }

        public decimal getAllSalesByItem(string categoryName, int month, int year)
        {
            decimal sales = 0;
            SqlCommand cmd = new SqlCommand("SELECT CategoryName, SUM(Price * Quantity) as TotalSales FROM Orders, OrderItems, ProductsCategory where Orders.OrderId = OrderItems.OrderId and OrderItems.ProductCategoryId= ProductsCategory.Id and CategoryName = @CategoryName and  Month(Date) = @Month and Year(Date)=@Year GROUP BY CategoryName", conn);
            cmd.Parameters.AddWithValue("@categoryName", categoryName);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sales = reader.GetDecimal(reader.GetOrdinal("TotalSales"));
                    }
                }
            }
            return sales;
        }

        public ObservableCollection<Product> getAllProducts()
        {
            ObservableCollection<Product> productsList = new ObservableCollection<Product>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Product, ProductsCategory Where ProductCategoryId = Id", conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string uPCCode = reader.GetString(reader.GetOrdinal("UPCCode"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));
                        decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));
                        string productCategory = reader.GetString(reader.GetOrdinal("CategoryName"));

                        byte[] imgBytes = (byte[])reader.GetSqlBinary(reader.GetOrdinal("Picture"));
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                        Bitmap picture = (Bitmap)tc.ConvertFrom(imgBytes);
                        productsList.Add(new Product { UPCCode = uPCCode, Name = name, Price = price, CategoryName = productCategory, Picture = picture });
                    }
                }
            }
            return productsList;
        }

        public ObservableCollection<ProductCategory> getAllCategories()
        {
            ObservableCollection<ProductCategory> categoryList = new ObservableCollection<ProductCategory>();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT Id, CategoryName FROM ProductsCategory", conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string categoryName = reader.GetString(reader.GetOrdinal("CategoryName"));




                        categoryList.Add(new ProductCategory { Id = id, CategoryName = categoryName });
                    }
                }
            }
            return categoryList;
        }

        public ObservableCollection<Product> GetProductsByCategory(String category)
        {
            ObservableCollection<Product> productsList = new ObservableCollection<Product>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Product, ProductsCategory Where ProductCategoryId = Id and categoryName = @categoryName", conn);
            cmd.Parameters.AddWithValue("@categoryName", category);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string uPCCode = reader.GetString(reader.GetOrdinal("UPCCode"));
                        string name = reader.GetString(reader.GetOrdinal("Name"));
                        decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));
                        string productCategory = reader.GetString(reader.GetOrdinal("CategoryName"));
                        int productCategoryId = reader.GetInt32(reader.GetOrdinal("ProductCategoryId"));

                        byte[] imgBytes = (byte[])reader.GetSqlBinary(reader.GetOrdinal("Picture"));
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                        Bitmap picture = (Bitmap)tc.ConvertFrom(imgBytes);
                        productsList.Add(new Product { Picture = picture, UPCCode = uPCCode, Name = name, Price = price, CategoryName = productCategory, CategoryId = productCategoryId });
                    }
                }
            }
            return productsList;
        }

        public User getUserByUserName(string userName, string password)
        {

            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[USERS] WHERE UserName = @UserName and Password=@Password", conn);
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@Password", password);


            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string userId = reader.GetString(reader.GetOrdinal("Id"));
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        bool isManager = reader.GetBoolean(reader.GetOrdinal("IsManager"));

                        return new User { UserName = userName, Password = password, Id = userId, FirstName = firstName, LastName = lastName, IsManager = isManager };
                    }
                }
            }
            return null;
        }
        public int saveOrderAndOrderItems(Order order, List<OrderItems> orderItems)
        {

            int modified;
            SqlCommand command = conn.CreateCommand();
            using (SqlTransaction transaction = conn.BeginTransaction())
            {

                // Must assign both transaction object and connection 
                // to Command object for a pending local transaction
                command.Connection = conn;
                command.Transaction = transaction;

                try
                {
                    command.CommandText =
                        "Insert into Orders (Date, StoreNo, UserId, OrderAmount, Tax)  OUTPUT Inserted.OrderId VALUES (@Date, @StoreNo, @UserId, @OrderAmount, @OrderTax)";
                    command.Parameters.AddWithValue("@Date", order.Date);
                    command.Parameters.AddWithValue("@StoreNo", order.StoreNo);
                    command.Parameters.AddWithValue("@UserId", order.UserId);
                    command.Parameters.AddWithValue("@OrderAmount", order.OrderAmount);
                    command.Parameters.AddWithValue("@OrderTax", order.Tax);
                    modified = (int)command.ExecuteScalar();

                    foreach (var item in orderItems)
                    {
                        command.CommandText =
                                "Insert into OrderItems(OrderId, UPCCode, Quantity, ProductCategoryId, ProductName, Price, Tax) VALUES (@OrderIdF, @UPCCode, @Quantity, @ProductCategoryId, @ProductName, @Price, @Tax)";
                        command.Parameters.AddWithValue("@OrderIdF", modified);
                        command.Parameters.AddWithValue("@UPCCode", item.UPCCode);
                        command.Parameters.AddWithValue("@Quantity", item.Quantity);
                        command.Parameters.AddWithValue("@ProductCategoryId", item.CategoryId);
                        command.Parameters.AddWithValue("@ProductName", item.Name);
                        command.Parameters.AddWithValue("@Price", item.Price);
                        command.Parameters.AddWithValue("@Tax", item.Tax);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                    // Attempt to commit the transaction.
                    if (transaction.Connection != null) //Detecting zombie transaction
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred 
                        // on the server that would cause the rollback to fail, such as 
                        // a closed connection.
                        throw ex2;
                    }
                }
            }
            return modified;
        }
        public ObservableCollection<Sales> getSales(int month, int year)
        {
            ObservableCollection<Sales> salesList = new ObservableCollection<Sales>();
            SqlCommand cmd = new SqlCommand("SELECT UPCCode, FirstName, LastName, ProductName,Quantity, Price, SUM(Price * Quantity) as TotalSales FROM Orders, OrderItems, Users  where Orders.OrderId = OrderItems.OrderId and Orders.UserId= Users.Id and Month(Date) = @Month and Year(Date) = @Year GROUP BY ProductName, UPCCode, UserId, Quantity, Price,FirstName, LastName", conn);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //TODO
                        string upcCode = reader.GetString(reader.GetOrdinal("UpcCode"));
                        string seller = reader.GetString(reader.GetOrdinal("FirstName")) + " " + reader.GetString(reader.GetOrdinal("LastName"));
                        string productName = reader.GetString(reader.GetOrdinal("ProductName"));
                        int qty = reader.GetInt32(reader.GetOrdinal("Quantity"));
                        decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));
                        decimal sales = reader.GetDecimal(reader.GetOrdinal("TotalSales"));

                        salesList.Add(new Sales() { Seller = seller, OrderItems = new OrderItems() { UPCCode = upcCode, Name = productName, Quantity = qty }, ItemTotal = sales });
                    }
                }
            }
            return salesList;
        }
        public ObservableCollection<Sales> getTopItemsPerMonth(int month, int year)
        {
            ObservableCollection<Sales> salesList = new ObservableCollection<Sales>();
            SqlCommand cmd = new SqlCommand("SELECT Top 5 UPCCode, ProductName,Quantity, Price, sum(Quantity) as Sum FROM Orders, OrderItems where Orders.OrderId = OrderItems.OrderId and Month(Date) = @Month and Year(Date) = @Year GROUP BY UPCCode, ProductName, Quantity, Price Order by sum(Quantity) desc", conn);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //TODO
                        string upcCode = reader.GetString(reader.GetOrdinal("UPCCode"));
                        string productName = reader.GetString(reader.GetOrdinal("ProductName"));
                        int Sum = reader.GetInt32(reader.GetOrdinal("Sum"));
                        //int qty = reader.GetInt32(reader.GetOrdinal("Quantity"));
                        decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));
                        decimal sales = Sum * price;

                        salesList.Add(new Sales() { OrderItems = new OrderItems() { UPCCode = upcCode, Name = productName, Quantity = Sum }, ItemTotal = sales });
                    }
                }
            }
            return salesList;
        }


        public void addProduct(Product p, string path)
        {
            byte[] rawData = File.ReadAllBytes(path);
            SqlCommand cmd = new SqlCommand("INSERT INTO Product (UPCCode, ProductCategoryId, Name, Price, Picture) VALUES (@UPCCode, @ProductCategoryId, @Name, @Price, @Picture)", conn);


            cmd.Parameters.AddWithValue("@UPCCode", p.UPCCode);
            cmd.Parameters.AddWithValue("@ProductCategoryId", p.CategoryId);
            cmd.Parameters.AddWithValue("@Name", p.Name);
            cmd.Parameters.AddWithValue("@Price", p.Price);
            cmd.Parameters.AddWithValue("@Picture", rawData);


            cmd.ExecuteNonQuery();

        }

        public void updateProduct(Product p, string path)
        {
            byte[] rawData = File.ReadAllBytes(path);
            SqlCommand cmd = new SqlCommand("UPDATE Product SET  Name = @Name, Price = @Price, Picture=@Picture WHERE UPCCode = @UPCCode", conn);

            cmd.Parameters.AddWithValue("@UPCCode", p.UPCCode);
            cmd.Parameters.AddWithValue("@Name", p.Name);
            cmd.Parameters.AddWithValue("@Price", p.Price);
            cmd.Parameters.AddWithValue("@Picture", rawData);


            cmd.ExecuteNonQuery();

        }

        public int getLastOrderNo()
        {
            int orderId = 0;
            SqlCommand cmd = new SqlCommand("SELECT top 1 OrderId FROM Orders order By date desc", conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //TODO
                        orderId = reader.GetInt32(reader.GetOrdinal("orderId"));

                    }
                }

            }
            return orderId;
        }

        public void updateProductWithoutImage(Product p)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Product SET  Name = @Name, Price = @Price WHERE UPCCode = @UPCCode", conn);

            cmd.Parameters.AddWithValue("@UPCCode", p.UPCCode);
            cmd.Parameters.AddWithValue("@Name", p.Name);
            cmd.Parameters.AddWithValue("@Price", p.Price);
            cmd.ExecuteNonQuery();
        }
    }
}
