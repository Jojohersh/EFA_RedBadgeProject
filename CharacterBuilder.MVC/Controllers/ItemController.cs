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
                return View(model);
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

        [Authorize]
        public async Task<IActionResult> Update([FromRoute]int id)
        {
            var itemToUpdate = await _itemService.GetItemByIdAsync(id);
            if (itemToUpdate is null)
                return RedirectToAction(nameof(Index));
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null || itemToUpdate.CreatorId != currentUser.Id)
            {
                ViewData["ErrorMsg"] = "Unauthorized edit attempt. You shall not pass.";
                return RedirectToAction(nameof(Index));
            }
            var itemEditModel = new ItemEdit {
                Id = itemToUpdate.Id,
                Name = itemToUpdate.Name,
                Description = itemToUpdate.Description,
                CreatorId = itemToUpdate.CreatorId
            };
            return View(itemEditModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(ItemEdit model)
        {
            var updatedSuccess = await _itemService.UpdateItemAsync(model);
            if (!updatedSuccess)
                ViewData["ErrorMsg"] = "could not update item";

            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> ConfirmDelete([FromRoute] int id)
        {
            var itemToDelete = await _itemService.GetItemByIdAsync(id);
            if (itemToDelete is null)
            {
                ViewData["ErrorMsg"] = "Invalid item request";
                return RedirectToAction(nameof(Index));
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null || itemToDelete.CreatorId != currentUser.Id)
            {
                ViewData["ErrorMsg"] = "Unauthorized delete attempt. You shall not pass.";
                return RedirectToAction(nameof(Index)); 
            }
            return View(itemToDelete);
        }
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteSuccess = await _itemService.DeleteItemAsync(id);
            if (!deleteSuccess)
            {
                ViewData["ErrorMsg"] = "Item could not be deleted";
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