using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Item;

namespace CharacterBuilder.Services.Item
{
    public interface IItemService
    {
        Task<bool> CreateItemAsync(ItemCreate model, int creatorId);
        Task<List<ItemListItem>> GetAllItemsAsync();
        Task<ItemDetail> GetItemByIdAsync(int id);
        Task<bool> UpdateItemAsync(ItemEdit model);
        Task<bool> DeleteItemAsync(int id);
    }
}