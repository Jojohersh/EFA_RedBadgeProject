using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Campaign;
using CharacterBuilder.Models.CampaignPlayer;
using CharacterBuilder.Services.Campaign;
using CharacterBuilder.Services.CampaignPlayer;
using CharacterBuilder.Services.Character;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CharacterBuilder.MVC.Controllers
{
    [Authorize]
    public class CampaignController : Controller
    {
        private readonly ILogger<CampaignController> _logger;
        private readonly ICampaignService _campaignService;
        private readonly ICharacterService _characterService;
        private readonly ICampaignPlayerService _campaignPlayerService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public CampaignController(ILogger<CampaignController> logger, ICampaignService campaignService, ICharacterService characterService, ICampaignPlayerService campaignPlayerService, UserManager<IdentityUser<int>> userManager)
        {
            _logger = logger;
            _campaignService = campaignService;
            _characterService = characterService;
            _campaignPlayerService = campaignPlayerService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = 0;
            int.TryParse(_userManager.GetUserId(User), out userId);
            if (userId == 0)
                return RedirectToAction("Index","Home");
            var campaigns = await _campaignService.GetAllCampaignsByPlayerIdAsync(userId);
            return View(campaigns);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CampaignCreate model)
        {
            var userId = 0;
            int.TryParse(_userManager.GetUserId(User), out userId);
            if (userId == 0)
                return View(model);
            if (!ModelState.IsValid)
                return View(model);
            var campaignCreated = await _campaignService.CreateCampaignAsync(model, userId);
            if (!campaignCreated)
                return View(model);
            return RedirectToAction(nameof(Index), new {id=userId});
        }

        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign is null)
                return RedirectToAction(nameof(Index));

            return View(campaign);
        }
        public async Task<IActionResult> AddPlayer([FromRoute]int id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign is null)
                return RedirectToAction(nameof(Index));
            var model = new CampaignPlayerCreate {
                CampaignId = id
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromRoute]int id, string username)
        {
            var userToAdd = await _userManager.FindByEmailAsync(username);
            if (userToAdd is null)
                return RedirectToAction("Detail", new {id=id});
            
            var usersCampaigns = await _campaignPlayerService.GetPlayersByCampaignIdAsync(id);

            var userInCampaign = usersCampaigns.Where(c => c.UserName == userToAdd.UserName);
                if (userInCampaign.Count() > 0)
                    return RedirectToAction("Detail", new {id=id});
            
            var addedPlayer = await _campaignPlayerService
                .AddPlayerToCampaignAsync(new CampaignPlayerCreate 
                {
                    CampaignId = id,
                    PlayerId = userToAdd.Id
                });
            return RedirectToAction("Detail", new {id=id});
        }

        public async Task<IActionResult> RemovePlayer([FromRoute] int id, string username)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign is null)
                return RedirectToAction(nameof(Index));

            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return RedirectToAction(nameof(Index));

            var removeSuccess = await _campaignPlayerService.RemovePlayerFromCampaignAsync(user.Id,campaign.Id);
            
            return RedirectToAction("Detail", new {id=campaign.Id});
        }   

        public async Task<IActionResult> AddCharacter([FromRoute] int id)
        {
            var campaignId = id;
            var campaignExists = await _campaignService.GetCampaignByIdAsync(campaignId);
            if (campaignExists is null)
                return RedirectToAction(nameof(Index));

            var userId = 0;
            int.TryParse(_userManager.GetUserId(User), out userId);
            if (userId == 0)
                return RedirectToAction(nameof(Detail), new {id=id});

            var characters = await _characterService.GetAllUnusedCharactersByOwnerId(userId);
            ViewData["CampaignId"] = id;
            return View(characters);
        }
        [HttpPost]
        public async Task<IActionResult> AddCharacter(int characterId, [FromRoute]int id)
        {
            var updatedSuccess = await _characterService.AddCharacterToCampaignAsync(characterId, id);
            if (!updatedSuccess)
                return await AddCharacter(id);
            return RedirectToAction("Detail", new {id=id});
        }
        
        public async Task<IActionResult> RemoveCharacter([FromRoute]int id, int campaignId)
        {
            var currentUserId = 0;
            var validId = int.TryParse(_userManager.GetUserId(User), out currentUserId);
            if (!validId)
                return RedirectToAction(nameof(Index));

            var currentCharacter = await _characterService.GetCharacterById(id);
            if (currentCharacter is null || currentCharacter.OwnerId != currentUserId)
                return RedirectToAction(nameof(Index));

            var removedCampaignSuccess = await _characterService.RemoveCharacterFromCampaignAsync(id);

            return RedirectToAction("Detail", new {id=campaignId});
        }
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign is null)
                return RedirectToAction(nameof(Index));
            var userId = 0;
            var validId = int.TryParse(_userManager.GetUserId(User), out userId);
            if (!validId)
                return RedirectToAction(nameof(Index));
            var currentUser = await _userManager.FindByIdAsync(userId.ToString());
            if (currentUser is null || currentUser.UserName != campaign.GameMasterName)
                return RedirectToAction(nameof(Index));
            var campaignEdit = new CampaignEdit {
                Id = campaign.Id,
                Name = campaign.Name,
                Description = campaign.Description
            };
            
            return View(campaignEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CampaignEdit model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var updateSuccess = await _campaignService.UpdateCampaignAsync(model);
            if (!updateSuccess)
                return View(model);
            return RedirectToAction("Detail", new {id=model.Id});
        }
        
        public async Task<IActionResult> ConfirmDelete([FromRoute]int id)
        {
            var userId = 0;
            int.TryParse(_userManager.GetUserId(User), out userId);

            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if(campaign is null)
                return RedirectToAction(nameof(Index));
            return View(campaign);
        }
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var deletedSuccessfully = await _campaignService.DeleteCampaignAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}