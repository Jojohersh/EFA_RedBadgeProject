using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.Character
{
    public class CharacterCreate
    {
        [Required]
        public string Name { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? Age { get; set; }

        public int? MindScore {get; set;}
        public int? BodyScore {get; set;}
        public int? ResilienceScore {get; set;}
        public int? SoulScore {get; set;}
        public int? MovementScore {get; set;}
        public string WeaponProficiencies {get; set;}="";
    }
}