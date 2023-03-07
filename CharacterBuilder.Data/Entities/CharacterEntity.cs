using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CharacterBuilder.Data.Entities
{
    public class CharacterEntity
    {
        [Key]
        public int Id { get; set; }
        public int? OwnerId { get; set; }
        public virtual IdentityUser<int>? Owner {get; set;}
        public int? CampaignId {get; set;}
        public virtual CampaignEntity? Campaign {get; set;}

        public string Name {get; set;}="";
        public string? Height {get; set;}
        public string? Weight {get; set;}
        public string? Age {get; set;}

        public int Level {get; set;} = 1;
        public int MindScore {get; set;} = 1;
        public int BodyScore {get; set;} = 1;
        public int ResilienceScore  {get; set;} = 1;
        public int SoulScore {get; set;} = 1;
        public int MovementScore {get; set;} = 2;

        public int CurrentHp {get; set;} = 3;
        public int MaxHp {get {
            return (ResilienceScore > 0)? 0 : ResilienceScore * 3;
        }}
        public int CurrentTalentPoints { get; set; } = 1;
        public int CurrentMovementPoints {get; set;} = 2;
        public string WeaponProficiencies {get; set;} = "";
        public virtual List<CharacterInventorySlotEntity> Inventory {get; set;}=new();
    }
}