using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Campaign;

namespace CharacterBuilder.Services.Campaign
{
    public interface ICampaignService
    {
        Task<bool> CreateCampaignAsync(CampaignCreate model, int GameMasterId);
        Task<List<CampaignListItem>> GetAllCampaignsAsync();
        Task<CampaignDetail> GetCampaignById(int id);
        Task<bool> UpdateCampaignAsync(CampaignEdit model);
        Task<bool> DeleteCampaignAsync(int id);
    }
}