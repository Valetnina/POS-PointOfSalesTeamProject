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



                        productsList.Add(new Product {UPCCode=uPCCode, Name=name, Price=price, CategoryName=new ProductCategory {CategoryName = productCategory }, Picture = picture  });
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



                        productsList.Add(new Product { UPCCode = uPCCode, Name = name, Price = price, CategoryName = new ProductCategory { CategoryName = productCategory }, Picture = picture });
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
                    
                    return new User { UserName = userName, Password= password };
                }
            }
            return null;
        }
        public void saveProduct(Product product)
        {
            byte[] rawData = File.ReadAllBytes(@"C:\Users\Valentina\Documents\POS-PointOfSalesTeamProject\Images\Drinks\DrPepper.jpg");

          //  SqlCommand cmd = new SqlCommand("INSERT INTO Product (UPCCode, ProductCategoryId, Name, Price, Picture) VALUES (@UPCCode, @ProductCategoryId, @Name, @Price, @Picture)", conn);
            SqlCommand cmd = new SqlCommand("Update Product set  Picture = @Picture", conn);
            //cmd.Parameters.AddWithValue("@UPCCode", "DRK01");
            //cmd.Parameters.AddWithValue("@ProductCategoryId", 3);
            //cmd.Parameters.AddWithValue("@Name", "Test");
            //cmd.Parameters.AddWithValue("@Price", 1);
            cmd.Parameters.AddWithValue("@Picture", rawData);



                    cmd.ExecuteNonQuery();

                }
            }
        }
