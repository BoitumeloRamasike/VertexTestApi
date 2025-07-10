using MySql.Data.MySqlClient;
using System.Data;
using VertexTestApi.Interfaces;
using VertexTestApi.Models;

namespace VertexTestApi.Services
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString;

        public DatabaseRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MySQLConnection")
                ?? throw new InvalidOperationException("The database connection string is missing.");
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            var items = new List<Item>();

            try
            {
                await using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                //Stored procedure to retrieve items
                using var command = new MySqlCommand("sp_get_items", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Execute the command and read the results
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    items.Add(new Item
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Description = reader.GetString("description")
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting the items.", ex);
            }

            return items;
        }

        public async Task AddItemAsync(Item item)
        {
            try
            {
                await using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Stored procedure to add an item
                using var command = new MySqlCommand("sp_add_item", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new MySqlParameter("p_name", MySqlDbType.VarChar) { Value = item.Name });
                command.Parameters.Add(new MySqlParameter("p_description", MySqlDbType.VarChar) { Value = item.Description });

                await command.ExecuteNonQueryAsync();
               
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the item.", ex);
            }
        }
    }
}
