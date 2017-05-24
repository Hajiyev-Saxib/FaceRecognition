using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.IO;
namespace WebCam
{
    class DataBaseConections
    {
        static string oradb = "DATA SOURCE=localhost:1521/xe;PASSWORD=1111;PERSIST SECURITY INFO=True;USER ID=FACECONTROLL";//DATA SOURCE=localhost:1521/xe;PASSWORD=1111;PERSIST SECURITY INFO=True;USER ID=FACECONTROLL
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
                
              
            return true;
           
        }
        static public void ConnectClose()
        {
            connection.Close();
        }
        static bool PersoneAdd(Person person)
        {
             OracleCommand command = new OracleCommand();
             command.Connection = connection;
             command.CommandText = @"INSERT INTO PERSONS VALUES ("+person.Id+"," +person.Surname+","+person.Name+","+person.Old+")";
             command.ExecuteNonQuery();

             command.CommandText = @"INSERT INTO IMAGES VALUES (@imagespers, @persons_idpers)";
             command.Parameters.Add("@imagepers", OracleType.Blob, 50);
             command.Parameters.Add("@persons_idpers", OracleType.Number);

             byte[] imageData;
            
             for (int i = 0; i < 5; i++)
             {
                 using (System.IO.FileStream fs = new System.IO.FileStream(person.Images[i], FileMode.Open))
                 {
                     imageData = new byte[fs.Length];
                     fs.Read(imageData, 0, imageData.Length);

                 }
                 command.Parameters["@imagepers"].Value = person.Id;

                 command.Parameters["@persons_idpers"].Value = imageData;
                 command.ExecuteNonQuery();
             }

 
            return true;
        }
        static bool PersoneDelete(String Name,String Surname)
        {
            OracleCommand command = new OracleCommand();
            command.Connection = connection;
            command.CommandText = @"select id from persons where Name='"+Name+"' and surname='"+Surname+"'";
            OracleDataReader reader = command.ExecuteReader();
            int id = 0;
            bool flag = false;
            while (reader.Read())
            {
                 id = reader.GetInt32(0);
                 flag = true;
               
            }
            if (flag == true)
            {
                command.CommandText = @"delete from persons where id=" + id;
                command.ExecuteNonQuery();

                command.CommandText = @"delete from images where id=" + id;
                command.ExecuteNonQuery();
            }
            return flag;
        }

    }
}