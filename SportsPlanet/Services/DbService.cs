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


    }
}
