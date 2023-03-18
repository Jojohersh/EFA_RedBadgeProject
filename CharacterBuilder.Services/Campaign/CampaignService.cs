using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Data.Entities;
using CharacterBuilder.Models.Campaign;
using CharacterBuilder.Models.Character;
using CharacterBuilder.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CharacterBuilder.Models.CampaignPlayer;

namespace CharacterBuilder.Services.Campaign
{
    public class CampaignService : ICampaignService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public CampaignService(UserManager<IdentityUser<int>> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateCampaignAsync(CampaignCreate model, int gameMasterId)
        {
            var existingUser = await _userManager.FindByIdAsync(gameMasterId.ToString());
            if (existingUser is null)
                return false;

            var newCampaign = new CampaignEntity {
                Name = model.Name,
                Description = model.Description,
                GameMasterId = gameMasterId,
                GameMaster = existingUser,
                CreatedUTC = DateTimeOffset.Now
            };

            _dbContext.Campaigns.Add(newCampaign);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCampaignAsync(int id)
        {
            var existingCampaign = await _dbContext.Campaigns.FindAsync(id);
            if (existingCampaign is null)
                return false;
            
            _dbContext.Campaigns.Remove(existingCampaign);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<CampaignListItem>> GetAllCampaignsByPlayerIdAsync(int id)
        {
            List<CampaignListItem> campaigns = await _dbContext.CampaignPlayers
                .Include(cp => cp.Campaign)
                .ThenInclude(c => c.GameMaster)
                .Where(cp => cp.PlayerId == id)
                .Select( c => new CampaignListItem {
                    Id = c.Campaign.Id,
                    Name = c.Campaign.Name,
                    GameMasterName = c.Campaign.GameMaster.UserName
                }).ToListAsync();
                
            List<CampaignListItem> campaignsAsGM = await _dbContext.Campaigns
                .Include(c => c.GameMaster)
                .Where(c => c.GameMasterId == id)
                .Select(c => new CampaignListItem{
                    Id = c.Id,
                    Name = c.Name,
                    GameMasterName = c.GameMaster.UserName
                }).ToListAsync();

            return campaigns.Concat(campaignsAsGM).ToList();
        }

        public async Task<CampaignDetail> GetCampaignByIdAsync(int id)
        {
            var existingCampaign = await _dbContext.Campaigns
                .Include(c=>c.GameMaster)
                .Include(c => c.Players)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (existingCampaign is null)
                return null;

            return new CampaignDetail {
                Id = existingCampaign.Id,
                Name = existingCampaign.Name,
                GameMasterName = existingCampaign.GameMaster.UserName,
                Description = existingCampaign.Description,
                CreatedUTC = existingCampaign.CreatedUTC,
                Players = await _dbContext.CampaignPlayers
                    .Include(cp => cp.Player)
                    .Where(cp => cp.CampaignId == existingCampaign.Id)
                    .Select(cp => new CampaignPlayerListItem {
                        Id=id,
                        UserName = cp.Player.UserName
                    })
                    .ToListAsync(),
                Characters = await GetAllCharactersByCampaignIdAsync(existingCampaign.Id)
            };
        }
        public async Task<List<CharacterListItem>> GetAllCharactersByCampaignIdAsync(int id) {
            var characters = await _dbContext.Characters
                    .Include(c => c.Owner)
                    .Where(c => c.CampaignId == id)
                    .Select(x => new CharacterListItem {
                        Id = x.Id,
                        OwnerName = (x.Owner != null)? x.Owner.UserName : "unclaimed",
                        Name = x.Name,
                        Level = x.Level
                    })
                    .ToListAsync();
            return characters;
        }

        public async Task<bool> UpdateCampaignAsync(CampaignEdit model)
        {
            var existingCampaign = await _dbContext.Campaigns.FindAsync(model.Id);
            if (existingCampaign is null)
                return false;
            existingCampaign.Name = model.Name;
            existingCampaign.Description = model.Description;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}