using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Weapon;
using CharacterBuilder.Services.Weapon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CharacterBuilder.MVC.Controllers
{
    public class WeaponController : Controller
    {
        private readonly ILogger<WeaponController> _logger;
        private readonly IWeaponService _weaponService;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public WeaponController(ILogger<WeaponController> logger, IWeaponService weaponService, UserManager<IdentityUser<int>> userManager)
        {
            _logger = logger;
            _weaponService = weaponService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var allWeapons = await _weaponService.GetAllWeaponsAsync();
            return View(allWeapons);
        }
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(WeaponCreate model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
            {
                ViewData["ErrorMsg"] = "invalid user";
                return View(model);
            }
            var weaponCreated = await _weaponService.CreateWeaponAsync(model,currentUser.Id);
            if (!weaponCreated)
            {
                ViewData["ErrorMsg"] = "Could not create weapon";
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var weaponToUpdate = await _weaponService.GetWeaponByIdAsync(id);
            if (weaponToUpdate is null)
                return RedirectToAction(nameof(Index));
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null || weaponToUpdate.CreatorId != currentUser.Id)
            {
                ViewData["ErrorMsg"] = "Unauthorized edit attempt. You shall not pass.";
                return RedirectToAction(nameof(Index));
            }
            var weaponEditModel = new WeaponEdit {
                Id = weaponToUpdate.Id,
                Name = weaponToUpdate.Name,
                LowAttackRange = weaponToUpdate.LowAttackRange,
                HighAttackRange = weaponToUpdate.HighAttackRange,
                LowThrownRange = weaponToUpdate.LowThrownRange,
                HighThrownRange = weaponToUpdate.HighThrownRange,
                AttackingStat = weaponToUpdate.AttackingStat,
                TargetStat = weaponToUpdate.TargetStat,
                IsTwoHanded = weaponToUpdate.IsTwoHanded,
                CreatorId = weaponToUpdate.CreatorId
            };
            return View(weaponEditModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(WeaponEdit model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
                return RedirectToAction(nameof(Index));
            var updatedSuccess = await _weaponService.UpdateWeaponAsync(model,currentUser.Id);
            if (!updatedSuccess)
                ViewData["ErrorMsg"] = "Could not update weapon";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> ConfirmDelete([FromRoute]int id)
        {
            var weaponToDelete = await _weaponService.GetWeaponByIdAsync(id);
            if (weaponToDelete is null)
            {
                ViewData["ErrorMsg"] = "Invalid weapon request";
                return RedirectToAction(nameof(Index));
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null || weaponToDelete.CreatorId != currentUser.Id)
            {
                ViewData["ErrorMsg"] = "Unauthorized delete attempt. You shall not pass.";
                return RedirectToAction(nameof(Index));
            }
            return View(weaponToDelete);
        }
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var deleteSuccess = await _weaponService.DeleteWeaponAsync(id);
            if (!deleteSuccess)
            {
                ViewData["ErrorMsg"] = "Weapon could not be deleted";
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