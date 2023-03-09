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
                return View("Error!");
                
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
            return RedirectToAction(nameof(Index));
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}