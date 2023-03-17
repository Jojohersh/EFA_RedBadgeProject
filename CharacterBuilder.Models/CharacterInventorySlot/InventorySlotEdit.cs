using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.CharacterInventorySlot
{
    public class InventorySlotEdit
    {
        [Required]
        public int Id {get; set;}
        [Required]
        public int CharacterId {get; set;}
        [Required]
        public int ItemCount {get; set;}
    }
}