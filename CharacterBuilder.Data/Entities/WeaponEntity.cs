using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CharacterBuilder.Data.Entities
{
    public class WeaponEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }="New Weapon";
        public string LowAttackRange { get; set; }=null!;
        public string? HighAttackRange { get; set; }
        public string? LowThrownRange {get; set;}
        public string? HighThrownRange {get; set;}
        public string AttackingStat { get; set; }=null!;
        public string TargetStat {get; set;}=null!;
        public bool IsTwoHanded {get; set;}=false;
        public virtual IdentityUser<int> CreatedBy {get; set;}

    }
}