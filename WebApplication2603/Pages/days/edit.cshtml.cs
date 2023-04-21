using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace WebApplication2603.Pages.days
{
    public class habit_value
    {
        public int id;
        public string name;
        public string value;
    }
    
    public class editModel : PageModel
    {
        public List<string> checkedids = new List<string>() { };
        public List<habit_value> habits_value_list = new List<habit_value>();
        public List<habit_value> backup;
        public int id;
        public dayinfo new_day = new dayinfo();
        public Dictionary<int, string> habit_types = new Dictionary<int, string>() { };
        public void OnGet()
        {
            string id = Request.Query["id"];
            
            Dictionary<string,string> habits= new Dictionary<string,string>();
            try
            {
                UpdatePageDayValues(id);
            }
            catch (Exception)
            {

                throw;
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=diary;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                    UpdatePageHabitsValues(connection);
            }
            catch (Exception)
            {

                throw;
            }

            void UpdatePageDayValues(string id)
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=diary;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM daysTable WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())

                            {

                                new_day.id = reader.GetInt32(2);
                                new_day.Date = reader.GetString(0);
                                new_day.Note = reader.GetString(1);

                                Console.WriteLine(new_day.Note);

                            }
                        }


                    }
                }
            }

            void UpdatePageHabitsValues(SqlConnection connection)
            {
                
                connection.Open();
                String sql = "SELECT * FROM HabitsValue WHERE habit_date=@date";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@date", new_day.Date);
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

                            habits_value_list.Add(new_value);



                        }

                    }


                }
                String sql_habit_types = "SELECT * FROM Habits";
                using (SqlCommand command = new SqlCommand(sql_habit_types, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            habit newhabit = new habit();
                            newhabit.id = reader.GetInt32(2);
                            newhabit.name = reader.GetString(1);
                            newhabit.type = reader.GetString(0);
                            habit_types.Add(newhabit.id, newhabit.type);

                        }
                    }


                }
            }
        }

        public dayinfo dayinfo = new dayinfo();
        public string checked_ids;
        public void OnPost()
        {
            Console.WriteLine(" checked"+Request.Form["AreChecked"]);

            //checkedids = Request.Form["AreChecked"];
            string id = Request.Query["id"];
            checked_ids= ","+Request.Form["AreChecked"]+",";
            
            dayinfo.id = int.Parse(id);
            dayinfo.Date =( Request.Form["date"]);
            
            dayinfo.Note = Request.Form["note"];

            

                try
            {
                
                    string connectionstring = "Data Source=.\\sqlexpress;Initial Catalog=diary;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    UpdateDayValues(connection);

                    UpdateHabitsValues(connection);

                }

            }
            catch (Exception)
            {

                throw;
            }

            
            Response.Redirect(("/days/index"));

            void UpdateDayValues(SqlConnection connection)
            {
                String sql =
                    "UPDATE daysTable " +
                    "SET Note=@note,daydate=@date " +
                    "WHERE ID=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", dayinfo.id);
                    command.Parameters.AddWithValue("@date", dayinfo.Date);
                    command.Parameters.AddWithValue("@note", dayinfo.Note);

                    command.ExecuteNonQuery();
                }
                String sql_getHabits = "SELECT * FROM HabitsValue WHERE habit_date=@date";
                using (SqlCommand command = new SqlCommand(sql_getHabits, connection))
                {

                    command.Parameters.AddWithValue("@date", Request.Form["date"].ToString());
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

                            habits_value_list.Add(new_value);



                        }

                    }


                }
            }
            
        }

        private void UpdateHabitsValues(SqlConnection connection)
        {
            string sql_habit_update = "UPDATE HabitsValue " +
                                        "SET habit_value=@habit_value " +
                                        "WHERE HabitID=@id AND habit_date=@date";
            for (int i = 0; i < habits_value_list.Count; i++)
            {
                int habit_id = habits_value_list[i].id;
                
                string form_name = "value" + i;
                string value = Request.Form[form_name].ToString();
                if (value.Contains("inputcheckbox"))
                {
                    var check_id = value[value.Length-1].ToString();
                    if (checked_ids.Contains((","+ check_id+",")))
                    {
                        value= "on";

                    }
                    else
                    {
                        value = "";
                    }
                }
                //Request.Form[form_name];
                
                
                if (value==null)
                {
                    value = "";
                    
                }
                else
                {
                    
                }
                
                using (SqlCommand command = new SqlCommand(sql_habit_update, connection))
                {
                    command.Parameters.AddWithValue("@id", habit_id);
                    command.Parameters.AddWithValue("@date", dayinfo.Date);
                    command.Parameters.AddWithValue("@habit_value", value);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
