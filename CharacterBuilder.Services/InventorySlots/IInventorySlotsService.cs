using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.CharacterInventorySlot;

namespace CharacterBuilder.Services.InventorySlots
{
    public interface IInventorySlotsService
    {
        Task<bool> CreateInventorySlotAsync(InventorySlotCreate model);
        Task<List<ItemSlotListItem>> GetAllItemSlotsAsync(int characterId);
        Task<List<WeaponSlotListItem>> GetAllWeaponSlotsAsync(int characterId);
        Task<bool> UpdateInventorySlotAsync(InventorySlotEdit model);
        Task<bool> DeleteInventorySlotAsync(int slotId);
    }
}