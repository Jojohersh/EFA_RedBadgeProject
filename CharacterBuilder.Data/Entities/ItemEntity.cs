using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CharacterBuilder.Data.Entities
{
    public class ItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }="New Item";
        public string Description { get; set; }="";
        public virtual IdentityUser<int> CreatedBy {get; set;}
    }
}