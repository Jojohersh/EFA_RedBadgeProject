using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Item;
using CharacterBuilder.Services.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CharacterBuilder.MVC.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IItemService _itemService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public ItemController(ILogger<ItemController> logger, IItemService itemService, UserManager<IdentityUser<int>> userManager)
        {
            _logger = logger;
            _itemService = itemService;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ItemCreate model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMsg"] = "Invalid data to create item";
                return RedirectToAction(nameof(Index));
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
                return RedirectToAction(nameof(Index));
            
            var createSuccess = await _itemService.CreateItemAsync(model,currentUser.Id);
            if (!createSuccess)
            {
                ViewData["ErrorMsg"] = "Could not create item";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var allItems = await _itemService.GetAllItemsAsync();
            return View(allItems);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}