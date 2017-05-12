using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
namespace WebCam
{
    class DataBaseConections
    {
        static string oradb = "Data Source=localhost:1521;Persist Security Info=True;" +
           "User ID=myUserID;Password=1111;Unicode=True";
        static OracleConnection connection;
        static public bool ConnectOpen()
        {
            string connectionString = oradb;
            try
            {
                connection.ConnectionString = connectionString;
                connection.Open();
            }
            catch( Exception e)
            {
                Console.WriteLine("very bad");
                return false;

            }
                
                Console.WriteLine("State: {0}", connection.State);
                Console.WriteLine("ConnectionString: {0}",
                                  connection.ConnectionString);
            return true;
           
        }
        static public void ConnectClose()
        {
            connection.Close();
        }
        static bool PersoneAdd(String Name,String[] inputImage)
        {

            return true;
        }
        static bool PersoneDelete(String Name)
        {
            return true;
        }

    }
}