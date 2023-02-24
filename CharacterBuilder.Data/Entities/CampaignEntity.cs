using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CharacterBuilder.Data.Entities
{
    public class CampaignEntity
    {
        [Key]
        public int Id {get; set;}
        public int GameMasterId {get; set;}
        public virtual IdentityUser<int> GameMaster {get; set;} = null!;
        public string Name {get; set;} = "New Campaign";
        public string? Description {get; set;}
        public DateTimeOffset CreatedUTC {get; set;}
        public List<CampaignPlayer> Players {get; set;} = new();
    }
}