using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.Item
{
    public class ItemListItem
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public int CreatorId {get; set;}
    }
}