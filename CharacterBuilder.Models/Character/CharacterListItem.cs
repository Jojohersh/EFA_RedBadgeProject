using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.Character
{
    public class CharacterListItem
    {
        public int Id {get; set;}
        public string OwnerName {get; set;}
        public string Name {get; set;}
        public int Level {get; set;}
    }
}