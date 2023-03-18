using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Character;

namespace CharacterBuilder.Services.Character
{
    public interface ICharacterService
    {
        Task<bool> CreateCharacterAsync(CharacterCreate model, int ownerId);
        Task<List<CharacterListItem>> GetAllCharactersByOwnerId(int Id);
        Task<List<CharacterListItem>> GetAllUnusedCharactersByOwnerId(int Id);
        Task<CharacterDetail> GetCharacterById(int Id);
        Task<bool> UpdateCharacterAsync(CharacterEdit model);
        Task<bool> AddCharacterToCampaignAsync(int characterId, int campaignId);
        Task<bool> RemoveCharacterFromCampaignAsync(int characterId);
        Task<bool> RemoveCharacterOwnershipAsync(int CharacterId);
        Task<bool> ClaimCharacterOwnershipAsync(int CharacterId, int OwnerId);
        Task<bool> DeleteCharacterAsync(int CharacterId);
    }
}