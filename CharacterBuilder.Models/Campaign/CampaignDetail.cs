using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Character;

namespace CharacterBuilder.Models.Campaign
{
    public class CampaignDetail
    {
        public int Id {get; set;}
        public string GameMasterName {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public List<CharacterListItem> Characters {get; set;}
        public DateTimeOffset CreatedUTC {get; set;}
    }
}