using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public List<User> getUserByUserName(string userName)
        {
            List<User> users = new List<User>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM USER WHERE UserName = @UserName", conn);
            cmd.Parameters.AddWithValue("@UserName", userName);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string password = reader.GetString(reader.GetOrdinal("Password"));
                        users.Add(new User{ UserName = userName, Password = password });
                    }
                }
            }
            return users;
        }

    }
}
