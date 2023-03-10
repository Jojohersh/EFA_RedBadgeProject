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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}