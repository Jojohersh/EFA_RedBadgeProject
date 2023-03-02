using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Data.Entities;
using CharacterBuilder.Models.CampaignPlayer;
using CharacterBuilder.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CharacterBuilder.Services.CampaignPlayer
{
    public class CampaignPlayerService : ICampaignPlayerService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public CampaignPlayerService(UserManager<IdentityUser<int>> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<bool> AddPlayerToCampaignAsync(CampaignPlayerCreate model)
        {
            var user = await _userManager.FindByIdAsync(model.PlayerId.ToString());
            var campaign = await _dbContext.Campaigns.FindAsync(model.CampaignId);

            if (user == null || campaign == null)
                return false;
            
            CampaignPlayerEntity newCampaignPlayer = new CampaignPlayerEntity {
                Player = user,
                Campaign = campaign
            };
            _dbContext.CampaignPlayers.Add(newCampaignPlayer);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<CampaignPlayerListItem>> GetPlayersByCampaignIdAsync(int id)
        {
            var players = await _dbContext.CampaignPlayers
                .Include(cp => cp.Player)
                .Where(cp => cp.CampaignId == id)
                .Select(cp => new CampaignPlayerListItem {
                    Id = cp.Player.Id,
                    UserName = cp.Player.UserName
                })
                .ToListAsync();
            return players;
        }

        public async Task<bool> RemovePlayerFromCampaignAsync(int userId, int campaignId)
        {
            
            var campaignPlayer = await _dbContext.CampaignPlayers.FirstOrDefaultAsync(cp => cp.CampaignId == campaignId && cp.PlayerId == userId);
            if (campaignPlayer is null)
                return false;
            _dbContext.CampaignPlayers.Remove(campaignPlayer);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}