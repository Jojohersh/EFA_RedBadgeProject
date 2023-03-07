using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.CampaignPlayer;

namespace CharacterBuilder.Services.CampaignPlayer
{
    public interface ICampaignPlayerService
    {
        Task<bool> AddPlayerToCampaignAsync(CampaignPlayerCreate model);
        Task<bool> RemovePlayerFromCampaignAsync(int userId, int campaignId);
        Task<List<CampaignPlayerListItem>> GetPlayersByCampaignIdAsync(int id);
    }
}