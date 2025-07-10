using VertexTestApi.Models;

namespace VertexTestApi.Interfaces
{
    public interface IDatabaseRepository
    {
        Task<List<Item>> GetItemsAsync();
        Task AddItemAsync(Item item);
    }
}