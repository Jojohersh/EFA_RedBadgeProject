using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.CharacterInventorySlot;
using CharacterBuilder.Services.InventorySlots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CharacterBuilder.MVC.Controllers
{
    public class InventorySlotController : Controller
    {
        private readonly ILogger<InventorySlotController> _logger;
        private readonly IInventorySlotsService _inventorySlotService;

        public InventorySlotController(ILogger<InventorySlotController> logger, IInventorySlotsService inventorySlotService)
        {
            _logger = logger;
            _inventorySlotService = inventorySlotService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(InventorySlotCreate model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMsg"] = "Invalid Model";
            }
            var slotCreateSuccess = await _inventorySlotService.CreateInventorySlotAsync(model);
            if (!slotCreateSuccess)
                ViewData["ErrorMsg"] = "Could not create inventory slot";

            return RedirectToAction("Detail", "Character", new {id=model.CharacterId});
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(InventorySlotEdit model)
        {
            if (ModelState.IsValid)
            {
                var updateSuccess = await _inventorySlotService.UpdateInventorySlotAsync(model);
                if (!updateSuccess)
                    ViewData["ErrorMsg"] = "Error updating inventory slot";
            }
            return RedirectToAction("Detail", "Character", new {id=model.CharacterId});
        }

        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id,int characterId)
        {
            var deleteSuccess = await _inventorySlotService.DeleteInventorySlotAsync(id);
            if (!deleteSuccess)
                ViewData["ErrorMsg"] = "Could not delete item slot";
            return RedirectToAction("Detail", "Character", new {id=characterId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}