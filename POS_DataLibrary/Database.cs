using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
        public User getUserByUserName(string userName, string password)
        {
            
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[USER] WHERE UserName = @UserName and Password=@Password", conn);
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

    }
}
