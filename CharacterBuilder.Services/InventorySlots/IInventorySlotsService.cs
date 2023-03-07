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
        Task<List<InventorySlotListItem>> GetAllItemSlotsAsync(int characterId);
        Task<List<InventorySlotListItem>> GetAllWeaponSlotsAsync(int characterId);
        Task<bool> UpdateInventorySlotAsync(InventorySlotEdit model);
        Task<bool> DeleteInventorySlotAsync(int slotId);
    }
}