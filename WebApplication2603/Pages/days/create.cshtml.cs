using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplication2603.Pages.days
{
    
    public class createModel : PageModel
    {
        public dayinfo newday=new dayinfo();
        public List<habit_value> habits_list;
        public void OnGet()
        {
            Console.WriteLine("onget_CREATE DAY");
        }
        public void OnPost()
        {
            habits_list = new List<habit_value>();
            newday.Date =( Request.Form["date"]);
            newday.Note = Request.Form["note"];
            try
            {
               
                string connectionstring = "Data Source=.\\sqlexpress;Initial Catalog=diary;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection=new SqlConnection(connectionstring))
                {
                    InsertNewValues(connection);

                    List<int> habits_index = new List<int>() { };
                    List<string> names_strings = new List<string>();
                    Store_ids_names(connection, habits_index, names_strings);

                    AddHAbitValues(connection, habits_index, names_strings);

                }
            }
            catch
            {

            }
           
            Response.Redirect(("/days/index"));

            void InsertNewValues(SqlConnection connection)
            {
                connection.Open();
                String sql = "INSERT INTO daysTable" +
                    "(daydate,Note) VALUES" +
                    "(@date,@note);"

                    ;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@date", newday.Date);
                    command.Parameters.AddWithValue("@note", newday.Note);

                    command.ExecuteNonQuery();
                }
            }
        }

        private static void Store_ids_names(SqlConnection connection, List<int> habits_index, List<string> names_strings)
        {
            string sql_habits_get_ids = "SELECT * FROM Habits;";
            using (SqlCommand command = new SqlCommand(sql_habits_get_ids, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int habit_index = reader.GetInt32(2);
                        string habit_name = reader.GetString(1);
                        habits_index.Add(habit_index);
                        names_strings.Add(habit_name);
                    }
                }


            }
        }

        private void AddHAbitValues(SqlConnection connection, List<int> habits_index, List<string> names_strings)
        {
            string sql_habitsValue_createion = "INSERT INTO HabitsValue" +
                                    "(HabitID,habit_date,habit_name) VALUES" +
                                    "(@habit_id,@date,@habit_name);";
            for (int i = 0; i < habits_index.Count; i++)
            {
                int id = habits_index[i];
                string name = names_strings[i];
                Console.WriteLine("item: " + id);
                using (SqlCommand command = new SqlCommand(sql_habitsValue_createion, connection))
                {

                    Console.WriteLine("habit value :" + newday.Date + " id=" + id);

                    command.Parameters.AddWithValue("@date", newday.Date);
                    command.Parameters.AddWithValue("@habit_id", id);
                    command.Parameters.AddWithValue("@habit_name", name);
                    command.ExecuteNonQuery();



                }
            }
        }
    }
}
