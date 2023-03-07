using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.Character
{
    public class CharacterEdit
    {
        public int Id {get; set;}
        public int CampaignId {get; set;}
        public int OwnerId {get; set;}
        public string Name {get; set;}
        public string? Height {get; set;}
        public string? Weight {get; set;}
        public string? Age {get; set;}

        public int Level {get; set;}
        public int MindScore {get; set;}
        public int BodyScore {get; set;}
        public int ResilienceScore {get; set;}
        public int SoulScore {get; set;}
        public int MovementScore {get; set;}

        public int CurrentHp {get; set;}
        public int CurrentTalentPoints {get; set;}
        public int CurrentMovementPoints {get; set;}
        public string WeaponProficiencies {get; set;}
    }
}