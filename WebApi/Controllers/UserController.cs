using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApi.Model;
namespace WebApi.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("addUser")]
        public JsonResult addUser(User e)
        {
            string query = @"
                    insert into [dbo].[UserInfo] values
                    ('" + e.UserName + @"','"
                        + e.Pass + @"')
                     ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connection");
            using (SqlConnection conn = new SqlConnection(sqlDataSource)) 
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn)) 
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    conn.Close();
                }
            }
            return new JsonResult("User Added");
        }

        [HttpGet("GetUser/{Id}")]
        public JsonResult GetUser(int Id)
        {
            string query = @"
                         select * from [dbo].[UserInfo]
                         where Id = " + Id + @"
                         ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connection");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource)) 
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut("editUser/{Id}")]
        public JsonResult editUser(int Id, User e)
        {
            string query = @"
        UPDATE [dbo].[UserInfo] SET
        UserName = @UserName,
        Pass = @Pass
        WHERE Id = @Id
    ";

            string sqlDataSource = _configuration.GetConnectionString("Connection");

            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", e.UserName);
                    cmd.Parameters.AddWithValue("@Pass", e.Pass);
                    cmd.Parameters.AddWithValue("@Id", Id);

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            return new JsonResult("User Updated Successfully");
        }


        [HttpDelete("DeleteUser/{Id}")]
        public JsonResult deleteUser(int Id) 
        {
            string query = @"
                       delete from [dbo].[UserInfo]
                       where Id = " + Id + @"
                       ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connection");
            SqlDataReader reader;
            using(SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open(); 
                using (SqlCommand cmd = new SqlCommand(query,conn))
                {
                    {
                        reader = cmd.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                        conn.Close();
                    } 
                }
                return new JsonResult("User Deleted");
            }
        }
    }
}
