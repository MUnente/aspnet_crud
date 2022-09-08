using WebAPI.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace WebAPI.Repository
{
    public class PersonRepository
    {
        public List<Person> SelectPeople(int? id = null, string? filterType = null, string? filterValue = null, int? page = null)
        {
            List<Person> people = new();
            string query = "SELECT * FROM person WHERE id = COALESCE(@id, id)";

            using (SqlConnection conn = new(Connection.ConnectionString))
            {
                SqlCommand cmd = new();

                cmd.Parameters.AddWithValue("@id", (object)id ?? DBNull.Value);

                if (!String.IsNullOrEmpty(filterType) && !String.IsNullOrEmpty(filterValue))
                {
                    query += $" AND {filterType} = @filterValue";
                    cmd.Parameters.AddWithValue("@filterValue", filterType == "id" ? Convert.ToInt32(filterValue) : filterValue.ToString());
                }

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Person person = new()
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Fullname = reader.GetString(2),
                        Fulldate = reader.GetString(3),
                        Active = reader.GetBoolean(4),
                        Country = reader.GetString(5),
                        Role = new RoleRepository().SelectRoles(reader.GetInt32(6)).FirstOrDefault()
                    };

                    people.Add(person);
                }

                return people;
            }
        }

        public Person InsertPerson(Person person)
        {
            string query = @"
                INSERT INTO person (
                    username, 
                    fullname, 
                    fulldate, 
                    active, 
                    country, 
                    roleId
                ) OUTPUT Inserted.* 
                VALUES (
                    @username, 
                    @fullname, 
                    @fulldate, 
                    @active, 
                    @country, 
                    @roleId
                )";

            using (SqlConnection conn = new(Connection.ConnectionString))
            {
                SqlCommand cmd = new(query, conn);

                cmd.Parameters.AddWithValue("@username", person.Username);
                cmd.Parameters.AddWithValue("@fullname", person.Fullname);
                cmd.Parameters.AddWithValue("@fulldate", person.Fulldate);
                cmd.Parameters.AddWithValue("@active", person.Active);
                cmd.Parameters.AddWithValue("@country", person.Country);
                cmd.Parameters.AddWithValue("@roleId", person.Role?.Id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    person.Id = reader.GetInt32(0);
                    person.Role = new RoleRepository().SelectRoles(person.Role?.Id).FirstOrDefault();
                }

                return person;
            }
        }

        public Person UpdatePerson(Person person)
        {
            string query = @"
                UPDATE person 
                SET username = @username, 
                    fullname = @fullname, 
                    fulldate = @fulldate, 
                    active = @active, 
                    country = @country, 
                    roleId = @roleId 
                OUTPUT inserted.*
                WHERE id = @id";

            using (SqlConnection conn = new(Connection.ConnectionString))
            {
                SqlCommand cmd = new(query, conn);

                cmd.Parameters.AddWithValue("@username", person.Username);
                cmd.Parameters.AddWithValue("@fullname", person.Fullname);
                cmd.Parameters.AddWithValue("@fulldate", person.Fulldate);
                cmd.Parameters.AddWithValue("@active", person.Active);
                cmd.Parameters.AddWithValue("@country", person.Country);
                cmd.Parameters.AddWithValue("@roleId", person.Role?.Id);
                cmd.Parameters.AddWithValue("@id", person.Id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    person.Id = reader.GetInt32(0);
                    person.Username = reader.GetString(1);
                    person.Fulldate = reader.GetString(2);
                    person.Fulldate = reader.GetString(3);
                    person.Active = reader.GetBoolean(4);
                    person.Country = reader.GetString(5);
                    person.Role = new RoleRepository().SelectRoles(reader.GetInt32(6)).FirstOrDefault();
                }

                return person;
            }
        }

        public bool DeletePerson(int id)
        {
            string query = "DELETE FROM person WHERE id = @id";

            using (SqlConnection conn = new(Connection.ConnectionString))
            {
                SqlCommand cmd = new(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0 ? true : false;
            }
        }
    }
}
