using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CharacterBuilder.Data.Entities
{
    public class CampaignPlayerEntity
    {
        public int Id { get; set; }
        public int PlayerId {get; set;}
        public virtual IdentityUser<int> Player {get; set;} = null!;
        public int CampaignId {get; set;}
        public virtual CampaignEntity Campaign {get; set;} = null!;
    }
}