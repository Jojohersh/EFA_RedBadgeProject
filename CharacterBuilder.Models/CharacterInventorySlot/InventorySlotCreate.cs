using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.CharacterInventorySlot
{
    public class InventorySlotCreate
    {
        [Required]
        public int CharacterId {get; set;}
        
        public int? ItemId {get; set;}
        public int? WeaponId {get; set;}
        public int ItemCount {get; set;}
    }
}