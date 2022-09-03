using Microsoft.Data.SqlClient;
using aspnet_crud.Models;

namespace aspnet_crud.Repository
{
    public class RoleRepository
    {
        public List<Role> SelectRoles(int? id = null)
        {
            List<Role> roles = new();
            string query = "SELECT id, description FROM role";

            if (id != null) query += $" WHERE id = {id}";

            using (SqlConnection conn = new(Connection.ConnectionString))
            {
                conn.Open();
                
                using (SqlCommand cmd = new())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = query;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Role role = new()
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1)
                        };

                        roles.Add(role);
                    }

                    return roles;
                }
            }
        }
    }
}