using Microsoft.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.Repository
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
                SqlCommand cmd = new(query, conn);

                conn.Open();

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