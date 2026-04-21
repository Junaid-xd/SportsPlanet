using Microsoft.Data.SqlClient;
using SportsPlanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPlanet.Services
{
    public class DbService
    {

        private string connectionString = "Data Source=JUNAIDs\\SQLEXPRESS;Initial Catalog=ead_mids;Integrated Security=True;Encrypt=False";


        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT id, product_name, price, quantity, img_path FROM products";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id = (int)reader["id"],
                        ProductName = reader["product_name"].ToString(),
                        Price = (decimal)reader["price"],
                        Quantity = (int)reader["quantity"],
                        ImgPath = reader["img_path"] == DBNull.Value ? null : reader["img_path"].ToString()
                    });
                }
            }

            return products;
        }

        public bool AddNewUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO users (name, email, password, role, created_at)
                    VALUES (@Name, @Email, @Password, @Role, @CreatedAt);
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0? true:false; 
                }

            }
        }

        public User? FindByEmail(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM users WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                Role = reader["role"].ToString(),
                                CreatedAt = (long)reader["created_at"]
                            };
                        }
                    }
                }
            }

            return null; // user not found
        }

    }
}
