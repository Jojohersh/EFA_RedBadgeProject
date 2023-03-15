using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Item;
using CharacterBuilder.Data;
using Microsoft.AspNetCore.Identity;
using CharacterBuilder.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CharacterBuilder.Services.Item
{
    public class ItemService : IItemService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ItemService(UserManager<IdentityUser<int>> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _dbContext = context;
        }
        public async Task<bool> CreateItemAsync(ItemCreate model, int creatorId)
        {
            if (model is null)
                return false;
                
            var itemCreator = await _userManager.FindByIdAsync(creatorId.ToString());

            if (itemCreator is null)
                return false;

            var newItem = new ItemEntity {
                Name = model.Name,
                Description = model.Description,
                CreatedBy = itemCreator
            };

            _dbContext.Items.Add(newItem);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var foundItem = await _dbContext.Items.FindAsync(id);
            if (foundItem is null)
                return false;
            _dbContext.Remove(foundItem);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<ItemListItem>> GetAllItemsAsync()
        {
            return await _dbContext.Items
                        .Select(item => new ItemListItem {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description
                        }).ToListAsync();
        }

        public async Task<ItemDetail> GetItemByIdAsync(int id)
        {
            var foundItem = await _dbContext.Items.Include(i => i.CreatedBy).FirstOrDefaultAsync(i => i.Id == id);
            if (foundItem is null)
                return null;
            
            return new ItemDetail {
                Id = foundItem.Id,
                Name = foundItem.Name,
                Description = foundItem.Description,
                CreatedBy = foundItem?.CreatedBy?.UserName
            };
        }

        public async Task<bool> UpdateItemAsync(ItemEdit model)
        {
            if (model is null)
                return false;

            var foundItem = await _dbContext.Items.FindAsync(model.Id);
            if (foundItem is null)
                return false;
            
            foundItem.Name = model.Name;
            foundItem.Description = model.Description;
            
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}