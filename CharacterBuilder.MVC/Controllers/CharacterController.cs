using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Services.Character;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using CharacterBuilder.Models.Character;

namespace CharacterBuilder.MVC.Controllers
{
    [Authorize]
    public class CharacterController : Controller
    {
        private readonly ILogger<CharacterController> _logger;
        private readonly ICharacterService _characterService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public CharacterController(ICharacterService characterService, ILogger<CharacterController> logger, UserManager<IdentityUser<int>> userManager)
        {
            _logger = logger;
            _characterService = characterService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            int userId = 0;
            var validId = int.TryParse( _userManager.GetUserId(currentUser), out userId);
            
            if (!validId)
            {
                ViewBag["ErrorMsg"] = "Invalid user ID";
                return RedirectToAction(nameof(Error));
            }
                
            var characters = await _characterService.GetAllCharactersByOwnerId(userId);
            return View(characters);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CharacterCreate model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMsg"] = "Invalid data entered";
                return View(model);
            }
            
            var currentUser = this.User;
            var userId = 0;
            var validId = int.TryParse(_userManager.GetUserId(currentUser), out userId);
            if (!validId)
            {
                ViewData["ErrorMsg"] = "Invalid user ID. Character not created.";
                return View(model);
            }
            var characterCreated = await _characterService.CreateCharacterAsync(model, userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail([FromRoute]int id)
        {
            var character = await _characterService.GetCharacterById(id);
            if (character is null)
            {
                ViewData["ErrorMsg"] = "Invalid character ID";
                return RedirectToAction(nameof(Index));
            }
            return View(character);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CharacterDetail model) {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMsg"] = "Invalid inputs";
                return await Detail(model.Id);
            }
            var existingCharacter = await _characterService.GetCharacterById(model.Id);
            var currentUser = await _userManager.GetUserAsync(User);
            if (existingCharacter is null)
            {
                ViewData["ErrorMsg"] = "Character does not exist";
                return RedirectToAction(nameof(Index));
            } 
            // if(existingCharacter.OwnerName != currentUser.UserName)
            // {
            //     ViewData["ErrorMsg"] = "Unauthorized access of character";
            //     return RedirectToAction(nameof(Index));
            // }
            var updateModel = new CharacterEdit {
                Id = model.Id,
                CampaignId = model.CampaignId,
                OwnerId = currentUser.Id,
                Name = model.Name,
                Height = model.Height,
                Weight = model.Weight,
                Age = model.Age,
                Level = model.Level,
                MindScore = model.MindScore,
                BodyScore = model.BodyScore,
                ResilienceScore = model.ResilienceScore,
                SoulScore = model.SoulScore,
                MovementScore = model.MovementScore,
                CurrentHp = model.CurrentHp,
                CurrentTalentPoints = model.CurrentTalentPoints,
                CurrentMovementPoints = model.CurrentMovementPoints,
                WeaponProficiencies = model.WeaponProficiencies
            };
            var successfullyUpdated = await _characterService.UpdateCharacterAsync(updateModel);
            if (!successfullyUpdated)
                ViewData["ErrorMsg"] = "Character not updated";
            return RedirectToAction(nameof(Detail),new {id = updateModel.Id});
        }

        public async Task<IActionResult> ConfirmDelete([FromRoute]int id)
        {
            var character = await _characterService.GetCharacterById(id);
            return View(character);
        }
        
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteSuccess = await _characterService.DeleteCharacterAsync(id);
            if (!deleteSuccess)
            {
                ViewData["ErrorMsg"] = "Character could not be deleted";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}