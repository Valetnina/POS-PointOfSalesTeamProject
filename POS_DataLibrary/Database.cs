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
        public Database()
        {
            conn = new SqlConnection(CONN_STRING);
            conn.Open();
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
                        decimal  price = reader.GetDecimal(reader.GetOrdinal("Price"));
                        string productCategory = reader.GetString(reader.GetOrdinal("CategoryName"));
                        byte[] imgBytes = (byte[])reader.GetSqlBinary(reader.GetOrdinal("Picture"));
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                       Bitmap picture = (Bitmap)tc.ConvertFrom(imgBytes);



                        productsList.Add(new Product {UPCCode=uPCCode, Name=name, Price=price, CategoryName= productCategory, Picture = picture  });
                    }
                }
            }
            return productsList;
        }

        public ObservableCollection<ProductCategory> getCategories()
        {
            ObservableCollection<ProductCategory> categoriesCollection = new ObservableCollection<ProductCategory>();
            categoriesCollection.Add(new ProductCategory {CategoryName = "Meals" });
            categoriesCollection.Add(new ProductCategory { CategoryName = "Drinks" });
            categoriesCollection.Add(new ProductCategory { CategoryName = "Desserts" });
            //ToDO; Implemenny SQl

            return categoriesCollection;
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

                        byte[] imgBytes = (byte[])reader.GetSqlBinary(reader.GetOrdinal("Picture"));
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                        Bitmap picture = (Bitmap)tc.ConvertFrom(imgBytes);
                        productsList.Add(new Product { Picture=picture, UPCCode = uPCCode, Name = name, Price = price, CategoryName =  productCategory});
                    }
                }
            }
            return productsList;
        }

        public User getUserByUserName(string userName, string password)
        {
            
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[USERS] WHERE UserName = @UserName and Password=@Password", conn);
            cmd.Parameters.AddWithValue("@UserName",userName);
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

                        return new User { UserName = userName, Password = password, Id = userId, FirstName = firstName, LastName = lastName };
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
                                "Insert into OrderItems(OrderId, UPCCode, Quantity, Discount, ProductCategoryId, ProductName, Price, Tax) VALUES (@OrderIdF, @UPCCode, @Quantity, @Discount, @ProductCategoryId, @ProductName, @Price, @Tax)";
                        command.Parameters.AddWithValue("@OrderIdF", modified);
                        command.Parameters.AddWithValue("@UPCCode", item.UPCCode);
                        command.Parameters.AddWithValue("@Quantity", item.Quantity);
                        command.Parameters.AddWithValue("@Discount", item.Discount);
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
        
        public void saveProduct(Product product)
        {
 byte[] rawData = File.ReadAllBytes(@"C:\Users\Valentina\Documents\POS-PointOfSalesTeamProject\Images\Drinks\Fanta.jpg");

          ////  SqlCommand cmd = new SqlCommand("INSERT INTO Product (UPCCode, ProductCategoryId, Name, Price, Picture) VALUES (@UPCCode, @ProductCategoryId, @Name, @Price, @Picture)", conn);

          //  SqlCommand cmd = new SqlCommand("Update Product set  Picture = @Picture where UPCCode = @UPCCode", conn);
          //  cmd.Parameters.AddWithValue("@UPCCode", "DRK03");
          //  //cmd.Parameters.AddWithValue("@ProductCategoryId", 2);
          //  //cmd.Parameters.AddWithValue("@Name", "Sprite");
          //  //cmd.Parameters.AddWithValue("@Price", 1.5);
          //  //cmd.Parameters.AddWithValue("@Picture", rawData);


          //  //cmd.ExecuteScalar();
          //  cmd.ExecuteNonQuery();

        }
            }
        }
