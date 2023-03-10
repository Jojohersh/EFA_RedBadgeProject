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

        public int MindScore {get; set;}=1;
        public int BodyScore {get; set;}=1;
        public int ResilienceScore {get; set;}=1;
        public int SoulScore {get; set;}=1;
        public int MovementScore {get; set;}=2;
        public string? WeaponProficiencies {get; set;}="";
    }
}