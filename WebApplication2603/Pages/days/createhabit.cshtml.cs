using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Xml;


namespace WebApplication2603.Pages.days
{
    public class createhabitModel : PageModel
    {
        public class newhabit
        {
            public string name;
            public string type;
        }
        public void OnGet()
        {
        }
        
        public void OnPost()
        {
           
            newhabit newhabit=new newhabit();
            newhabit.name = Request.Form["name"] ;
            newhabit.type = Request.Form["type"];
            Console.WriteLine(newhabit.type);
            Console.WriteLine(newhabit.name+" "+newhabit.type);
            try
            {
                string connectionstring = "Data Source=.\\sqlexpress;Initial Catalog=diary;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    String sql = "INSERT INTO Habits" +
                        "(habit_name,habit_type) VALUES" +
                        "(@name,@type);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@name", newhabit.name);
                        command.Parameters.AddWithValue("@type", newhabit.type);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
            
            Response.Redirect(("/days/index"));
        }
    }
}
   
