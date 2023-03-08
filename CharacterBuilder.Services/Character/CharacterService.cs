using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Data;
using CharacterBuilder.Data.Entities;
using CharacterBuilder.Models.Character;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CharacterBuilder.Services.Character
{
    public class CharacterService : ICharacterService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public CharacterService(UserManager<IdentityUser<int>> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateCharacterAsync(CharacterCreate model, int ownerId)
        {
            var existingUser = await _userManager.FindByIdAsync(ownerId.ToString());
            if (existingUser is null)
                return false;
            var newCharacter = new CharacterEntity {
                OwnerId = existingUser.Id,
                Name = model.Name,
                Age = model.Age,
                Height = model.Height,
                Weight = model.Weight,
                MindScore = model.MindScore,
                BodyScore = model.BodyScore,
                ResilienceScore = model.ResilienceScore,
                SoulScore = model.SoulScore,
                MovementScore = model.MovementScore,
                WeaponProficiencies = model.WeaponProficiencies
            };
            _dbContext.Characters.Add(newCharacter);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<CharacterListItem>> GetAllCharactersByOwnerId(int Id)
        {
            var characters = await _dbContext.Characters
                .Include(c => c.Owner)
                .Where(c => c.OwnerId == Id)
                .Select(c => new CharacterListItem {
                    Id = c.Id,
                    OwnerName = c.Owner.UserName,
                    Name = c.Name,
                    Level = c.Level
                })
                .ToListAsync();
            return characters;
        }

        public async Task<CharacterDetail> GetCharacterById(int Id)
        {
            var character = await _dbContext.Characters
                .Include(c => c.Owner)
                .Include(c => c.Campaign)
                .Where(c => c.Id == Id)
                .Select(c => new CharacterDetail {
                    Id = c.Id,
                    OwnerName = (c.Owner != null)? c.Owner.UserName : "No current owner",
                    CampaignId = c.CampaignId,
                    CampaignName = (c.Campaign != null)? c.Campaign.Name : "Not in a campaign",
                    Name = c.Name,
                    Age = c.Age,
                    Height = c.Height,
                    Weight = c.Weight,
                    Level = c.Level,
                    MindScore = c.MindScore,
                    BodyScore = c.BodyScore,
                    ResilienceScore = c.ResilienceScore,
                    SoulScore = c.SoulScore,
                    MovementScore = c.MovementScore,
                    CurrentHp = c.CurrentHp,
                    CurrentTalentPoints = c.CurrentTalentPoints,
                    WeaponProficiencies = c.WeaponProficiencies,
                    //todo: Items = _inventoryService.GetItemsByCharacterId(Id),
                    //todo: Weapons = _inventoryService.GetWeaponsByCharacterId(Id)
                })
                .SingleAsync();
                return character;
        }
        public async Task<bool> UpdateCharacterAsync(CharacterEdit model)
        {
            var existingCharacter = await _dbContext.Characters.FindAsync(model.Id);
            if (existingCharacter is null)
                return false;
            existingCharacter.Name = model.Name;
            existingCharacter.Height = model.Height;
            existingCharacter.Weight = model.Weight;
            existingCharacter.Age = model.Age;
            existingCharacter.Level = model.Level;
            existingCharacter.MindScore = model.MindScore;
            existingCharacter.BodyScore = model.BodyScore;
            existingCharacter.ResilienceScore = model.ResilienceScore;
            existingCharacter.SoulScore = model.SoulScore;
            existingCharacter.MovementScore = model.MovementScore;
            existingCharacter.CurrentHp = model.CurrentHp;
            existingCharacter.CurrentTalentPoints = model.CurrentTalentPoints;
            existingCharacter.CurrentMovementPoints = model.CurrentMovementPoints;
            existingCharacter.WeaponProficiencies = model.WeaponProficiencies;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ClaimCharacterOwnershipAsync(int characterId, int newOwnerId)
        {
            var character = await _dbContext.Characters.FindAsync(characterId);
            if (character is null || character.OwnerId != null)
                return false;
            character.OwnerId = newOwnerId;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveCharacterOwnershipAsync(int characterId)
        {
            var character = await _dbContext.Characters.FindAsync(characterId);
            if (character is null || character.OwnerId is null || character.CampaignId is null)
                return false;
            character.OwnerId = null;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddCharacterToCampaignAsync(int characterId, int campaignId)
        {
            var character = await _dbContext.Characters.FindAsync(characterId);
            if (character is null || character.CampaignId != null)
                return false;
            
            character.CampaignId = campaignId;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveCharacterFromCampaignAsync(int characterId)
        {
            var character = await _dbContext.Characters.FindAsync(characterId);
            if (character is null || character.CampaignId is null || character.OwnerId is null)
                return false;
            character.CampaignId = null;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCharacterAsync(int CharacterId)
        {
            var existingCharacter = await _dbContext.Characters.FindAsync(CharacterId);
            if (existingCharacter is null)
                return false;
            _dbContext.Characters.Remove(existingCharacter);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}