using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Data.Entities
{
    public class CharacterInventorySlotEntity
    {
        public int Id {get; set;}
        public int CharacterId {get; set;}
        public virtual CharacterEntity Character {get; set;} =null!;
        // inventory slot only store an item OR a weapon
        public virtual ItemEntity? Item {get; set;}=null;
        public int ItemCount {get; set;}=1;
        public virtual WeaponEntity? Weapon {get; set;}=null;
    }
}