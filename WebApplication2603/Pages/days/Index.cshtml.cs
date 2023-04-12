using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;

namespace WebApplication2603.Pages.days
{
    
    public class IndexModel : PageModel
    {
        public List<dayinfo> Days = new List<dayinfo>() { };
        public List<habit> habits = new List<habit>() { };
        public void OnGet()
        {
            Console.WriteLine("ongetindex");
            try
            {


                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=diary;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    StoreDays(connection);
                }
            }
            catch (Exception)
            {

                throw;
            }
            try
            {


                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=diary;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    StoreHabits(connection);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void StoreDays(SqlConnection connection)
        {
            String sql = "SELECT * FROM daysTable";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dayinfo new_day = new dayinfo();
                        new_day.id = reader.GetInt32(2);
                        new_day.Date = (reader.GetString(0));
                        new_day.Note = reader.GetString(1);

                        Days.Add(new_day);

                    }
                }


            }
        }

        private void StoreHabits(SqlConnection connection)
        {
            String sql = "SELECT * FROM Habits";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        habit newhabit = new habit();
                        newhabit.id = reader.GetInt32(2);
                        newhabit.name = reader.GetString(1);
                        newhabit.type = reader.GetString(0);
                        habits.Add(newhabit);

                    }
                }


            }
        }

    }
    public class dayinfo
    {
        public int id { get; set; }
        public string Date { get; set; }
        public string Note { get; set; } 

    }
    public class habit
    {
        public int id;
        public string name;
        public string type;
    }
}
