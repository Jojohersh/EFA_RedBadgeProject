using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.CharacterInventorySlot
{
    public class ItemSlotListItem
    {
        public int SlotId {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public int ItemCount {get; set;}
    }
}