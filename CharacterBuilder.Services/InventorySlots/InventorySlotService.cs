using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Data.Entities;
using CharacterBuilder.Models.CharacterInventorySlot;
using CharacterBuilder.MVC.Data;
using CharacterBuilder.Services.Item;
using CharacterBuilder.Services.Weapon;
using Microsoft.EntityFrameworkCore;

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
                    ItemCount = 1
                };
                _dbContext.InventorySlots.Add(newInventoryItem);
                return await _dbContext.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<InventorySlotListItem>> GetAllItemSlotsAsync(int characterId)
        {
            return await _dbContext.InventorySlots
                .Include(slot => slot.Character)
                .Include(slot => slot.Item)
                .Where(slot => slot.Character.Id == characterId && slot.Weapon == null)
                .Select(slot => new InventorySlotListItem {
                    IsWeapon = false,
                    ItemName = slot.Item.Name,
                    ItemDescription = slot.Item.Description,
                    ItemCount = slot.ItemCount
                })
                .ToListAsync();
        }

        public async Task<List<InventorySlotListItem>> GetAllWeaponSlotsAsync(int characterId)
        {
            return await _dbContext.InventorySlots
                .Include(slot => slot.Character)
                .Include(slot => slot.Weapon)
                .Where(slot => slot.Character.Id == characterId && slot.Item == null)
                .Select(slot => new InventorySlotListItem {
                    IsWeapon = true,
                    ItemName = slot.Weapon.Name,
                    ItemCount = slot.ItemCount,
                    WeaponLowRange = slot.Weapon.LowAttackRange,
                    WeaponHighRange = slot.Weapon.HighAttackRange,
                    WeaponLowThrownRange = slot.Weapon.LowThrownRange,
                    WeaponHighThrownRange = slot.Weapon.HighThrownRange,
                    WeaponAttackStat = slot.Weapon.AttackingStat,
                    WeaponTargetStat = slot.Weapon.TargetStat
                })
                .ToListAsync();
        }

        public Task<bool> UpdateInventorySlotAsync(InventorySlotEdit model)
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> DeleteInventorySlotAsync(int slotId)
        {
            throw new NotImplementedException();
        }
    }
}