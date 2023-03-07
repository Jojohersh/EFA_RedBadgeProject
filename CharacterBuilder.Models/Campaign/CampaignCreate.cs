using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.Campaign
{
    public class CampaignCreate
    {
        [Required]
        [MaxLength(120, ErrorMessage = "Name can be a maximum of {1} characters")]
        public string Name {get; set;}
        public string? Description {get; set;}
    }
}