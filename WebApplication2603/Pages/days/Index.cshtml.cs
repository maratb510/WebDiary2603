using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;
using System.Linq;

namespace WebApplication2603.Pages.days
{
    
    public class IndexModel : PageModel
    {
        public List<dayinfo> Days = new List<dayinfo>() { };
        public List<habit> habits = new List<habit>() { };
        Dictionary<int, int> completedhabits_key_dayid= new Dictionary<int, int>();
        public List<habit_value> habits_values= new List<habit_value>() { };
        public Dictionary<string, List<string>> habitsAtDates=new Dictionary<string, List<string>>() { };
        public Dictionary<string, int> dictionary_completed_habits;
        public void OnGet()
        {
            Dictionary<int, int> completedhabits_key_dayid = new Dictionary<int, int>();
            
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
                    StoreHabitsValues(connection);

                }
            }
            catch (Exception)
            {

                throw;
            }
            getprogress();
             void getprogress( )
            {

                dictionary_completed_habits = new Dictionary<string, int>() { };
                
                   
                    foreach (var item in habitsAtDates)
                    {
                        int completed = 0;
                        foreach (var value in item.Value)
                            {
                                if (value != "")
                                {
                                    completed++;
                                }
                            }
                        try
                        {
                            dictionary_completed_habits.Add((item.Key).ToString(), completed);
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    
                }
                    
            }

            void StoreHabitsValues(SqlConnection connection)
            {
                List<string> habits_;
                Dictionary<string, string> habitsAtNames;
                foreach (var item in Days)
                {
                    habits_ = new List<string>() { };
                    habitsAtNames = new Dictionary<string, string>();
                    var date = item.Date;
                    String sql = "SELECT * FROM HabitsValue WHERE habit_date=@date";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@date", date);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {



                            while (reader.Read())

                            {
                                habit_value new_value = new habit_value();
                                new_value.id = reader.GetInt32(0);
                                new_value.name = reader.GetString(3);
                                try
                                {
                                    new_value.value = (reader.GetString(1));
                                }
                                catch (Exception)
                                {
                                    new_value.value = "";
                                }

                                habits_values.Add(new_value);
                                habits_.Add(new_value.value);


                            }

                        }


                    }
                    try
                    {
                        habitsAtDates.Add(item.Date, habits_);
                    }
                    catch (Exception)
                    {


                    }

                }
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
        public int getcompletedhabits(string date)
        {
            int return_value;
            try
            {
               return_value= dictionary_completed_habits[date];
            }
            catch (Exception)
            {
                return_value=0;
                
            }
            return return_value;
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
