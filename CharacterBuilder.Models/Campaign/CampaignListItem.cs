using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBuilder.Models.Campaign
{
    public class CampaignListItem
    {
        public int Id {get;set;}
        public string Name {get; set;}
        public string GameMasterName {get; set;}
        public int NumberOfPlayers {get; set;}
    }
}