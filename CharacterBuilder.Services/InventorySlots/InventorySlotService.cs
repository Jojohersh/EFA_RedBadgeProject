using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Data.Entities;
using CharacterBuilder.Models.CharacterInventorySlot;
using CharacterBuilder.Data;
using CharacterBuilder.Services.Item;
using CharacterBuilder.Services.Weapon;
using Microsoft.EntityFrameworkCore;
using CharacterBuilder.Models.Item;

namespace CharacterBuilder.Services.InventorySlots
{
    public class InventorySlotService : IInventorySlotsService
    {
        private readonly ApplicationDbContext _dbContext;

        public InventorySlotService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateInventorySlotAsync(InventorySlotCreate model)
        {
            if (model.ItemId is null && model.WeaponId is null)
                return false;
            if (model.ItemId is not null && model.WeaponId is not null)
                return false;
            if (model.ItemId is not null)
            {
                var itemSearched = await _dbContext.Items.FindAsync(model.ItemId);
                if (itemSearched is null)
                    return false;
                var characterSearched = await _dbContext.Characters.FindAsync(model.CharacterId);
                if (characterSearched is null)
                    return false;
                var newInventoryItem = new CharacterInventorySlotEntity {
                    Character = characterSearched,
                    Item = itemSearched,
                    Weapon = null,
                    ItemCount = model.ItemCount
                };
                _dbContext.InventorySlots.Add(newInventoryItem);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                var weaponSearched = await _dbContext.Weapons.FindAsync(model.WeaponId);
                if (weaponSearched is null)
                    return false;
                var characterSearched = await _dbContext.Characters.FindAsync(model.CharacterId);
                if (characterSearched is null)
                    return false;
                var newInventoryItem = new CharacterInventorySlotEntity {
                    Character = characterSearched,
                    Weapon = weaponSearched,
                    Item = null,
                    ItemCount = model.ItemCount
                };
                _dbContext.InventorySlots.Add(newInventoryItem);
                return await _dbContext.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<ItemSlotListItem>> GetAllItemSlotsAsync(int characterId)
        {
            return await _dbContext.InventorySlots
                .Include(slot => slot.Character)
                .Include(slot => slot.Item)
                .Where(slot => slot.Character.Id == characterId && slot.Weapon == null)
                .Select(slot => new ItemSlotListItem {
                    Name = slot.Item.Name,
                    Description = slot.Item.Description,
                    ItemCount = slot.ItemCount
                })
                .ToListAsync();
        }

        public async Task<List<WeaponSlotListItem>> GetAllWeaponSlotsAsync(int characterId)
        {
            return await _dbContext.InventorySlots
                .Include(slot => slot.Character)
                .Include(slot => slot.Weapon)
                .Where(slot => slot.Character.Id == characterId && slot.Item == null)
                .Select(slot => new WeaponSlotListItem {
                    SlotId = slot.Id,
                    Name = slot.Weapon.Name,
                    ItemCount = slot.ItemCount,
                    LowAttackRange = slot.Weapon.LowAttackRange,
                    HighAttackRange = slot.Weapon.HighAttackRange,
                    LowThrownRange = slot.Weapon.LowThrownRange,
                    HighThrownRange = slot.Weapon.HighThrownRange,
                    AttackingStat = slot.Weapon.AttackingStat,
                    TargetStat = slot.Weapon.TargetStat,
                    IsTwoHanded = slot.Weapon.IsTwoHanded
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateInventorySlotAsync(InventorySlotEdit model)
        {
            var slotToUpdate = await _dbContext.InventorySlots.FindAsync(model.Id);
            if (slotToUpdate is null || model.ItemCount < 0)
                return false;
                
            slotToUpdate.ItemCount = model.ItemCount;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        
        public async Task<bool> DeleteInventorySlotAsync(int slotId)
        {
            var slotToDelete = await _dbContext.InventorySlots.FindAsync(slotId);
            if (slotToDelete is null)
                return false;

            _dbContext.InventorySlots.Remove(slotToDelete);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}