using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.CharacterInventorySlot
{
    public class WeaponSlotListItem
    {
        public int SlotId {get; set;}
        public string Name {get; set;}
        public string LowAttackRange { get; set; }=null!;
        public string? HighAttackRange { get; set; }
        public string? LowThrownRange {get; set;}
        public string? HighThrownRange {get; set;}
        public string AttackingStat {get; set;}
        public string TargetStat {get; set;}
        public bool IsTwoHanded {get; set;}
        public int ItemCount {get; set;}
    }
}