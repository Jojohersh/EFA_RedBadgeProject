using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.CampaignPlayer
{
    public class CampaignPlayerCreate
    {
        [Required]
        public int PlayerId {get; set;}
        [Required]
        public int CampaignId {get; set;}
    }
}