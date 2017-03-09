using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Load_sql_server
{

    class Program
    {
        static void queryToLocalServer()
        {
            String connStr = "Data Source=DMITRIY;Initial Catalog=Bookstore;Integrated Security=SSPI"; // Trusted connection
            String queryString = "select @@VERSION";
            String queryString2 = "Declare @date datetime = GETDATE(); select @date";
  
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int counter = 0;
            int countIterations = 5;
            while (counter != countIterations)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(queryString, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) 
                        {
                            try
                            {
                                while (reader.Read()) // reader становится на место первой строки в результате
                                {
                                    Console.WriteLine(reader[0]); // выбираем данные из первого столбца 
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                  

                }
                ++counter;
            }
          
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Console.WriteLine(time);
        }

        static void queryAzureServer()
        {
            String connStr = "some connection string"; 
            String queryString = "select @@VERSION";
            String queryString2 = "Declare @date datetime = GETDATE(); select @date";
            int counter = 0;
            int countIterations = 100;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connected successfully");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); 
                }

                while (counter != countIterations)
                {
                    using (SqlCommand command = new SqlCommand(queryString, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) 
                        {
                            try
                            {
                                while (reader.Read()) 
                                {
                                    Console.WriteLine(reader[0]);  
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                    ++counter;
                }
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Console.WriteLine(time);

        }
        static void Main(string[] args)
        {
            queryToLocalServer();
            //queryAzureServer();
            Console.ReadKey();
        }
    }
}
