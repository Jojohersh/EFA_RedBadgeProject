using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.CharacterInventorySlot
{
    public class InventorySlotListItem
    {
        public int Id {get; set;}

        public string ItemName {get; set;}
        public string? ItemDescription {get; set;}
        public int ItemCount {get; set;}

        public bool IsWeapon {get; set;}
        public string? WeaponLowRange {get; set;}
        public string? WeaponHighRange {get; set;}
        public string? WeaponLowThrownRange {get; set;}
        public string? WeaponHighThrownRange { get; set; }
        public string? WeaponAttackStat {get; set;}
        public string? WeaponTargetStat {get; set;}
        public bool? WeaponIsTwoHanded {get; set;}
    }
}