using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Data.Entities;
using CharacterBuilder.Models.Weapon;
using CharacterBuilder.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CharacterBuilder.Services.Weapon
{
    public class WeaponService : IWeaponService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public WeaponService(ApplicationDbContext context, UserManager<IdentityUser<int>> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }

        public async Task<bool> CreateWeaponAsync(WeaponCreate model, int CreatedById)
        {
            var userExists = await _userManager.FindByIdAsync(CreatedById.ToString());
            if (userExists is null)
                return false;
            var newWeapon = new WeaponEntity
            {
                Name = model.Name,
                LowAttackRange = model.LowAttackRange,
                HighAttackRange = model.HighAttackRange,
                LowThrownRange = model.LowThrownRange,
                HighThrownRange = model.HighThrownRange,
                AttackingStat = model.AttackingStat,
                TargetStat = model.TargetStat,
                IsTwoHanded = model.IsTwoHanded,
                CreatedBy = userExists
            };
            _dbContext.Weapons.Add(newWeapon);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<WeaponListItem>> GetAllWeaponsAsync()
        {
            return await _dbContext.Weapons
                .Include(w => w.CreatedBy)
                .Select(w => new WeaponListItem
                {
                    Id = w.Id,
                    Name = w.Name,
                    LowAttackRange = w.LowAttackRange,
                    HighAttackRange = w.HighAttackRange,
                    LowThrownRange = w.LowThrownRange,
                    HighThrownRange = w.HighThrownRange,
                    AttackingStat = w.AttackingStat,
                    TargetStat = w.TargetStat,
                    IsTwoHanded = w.IsTwoHanded
                }).ToListAsync();
        }

        public async Task<WeaponDetail> GetWeaponByIdAsync(int id)
        {
            return await _dbContext.Weapons
                .Include(w => w.CreatedBy)
                .Select(w => new WeaponDetail
                {
                    Id = w.Id,
                    Name = w.Name,
                    LowAttackRange = w.LowAttackRange,
                    HighAttackRange = w.HighAttackRange,
                    LowThrownRange = w.LowThrownRange,
                    HighThrownRange = w.HighThrownRange,
                    AttackingStat = w.AttackingStat,
                    TargetStat = w.TargetStat,
                    IsTwoHanded = w.IsTwoHanded,
                    CreatorId = w.CreatedBy.Id,
                    CreatedByUserName = w.CreatedBy.UserName
                })
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<bool> UpdateWeaponAsync(WeaponEdit model, int editorId)
        {
            var existingWeapon = await _dbContext.Weapons
                .Include(w => w.CreatedBy)
                .FirstOrDefaultAsync(w => w.Id == model.Id);
            if (existingWeapon is null)
                return false;

            if (editorId != existingWeapon.CreatedBy.Id)
                return false;
                
            existingWeapon.Id = model.Id;
            existingWeapon.Name = model.Name;
            existingWeapon.LowAttackRange = model.LowAttackRange;
            existingWeapon.HighAttackRange = model.HighAttackRange;
            existingWeapon.LowThrownRange = model.LowThrownRange;
            existingWeapon.HighThrownRange = model.HighThrownRange;
            existingWeapon.AttackingStat = model.AttackingStat;
            existingWeapon.TargetStat = model.TargetStat;
            existingWeapon.IsTwoHanded = model.IsTwoHanded;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteWeaponAsync(int id)
        {
            var existingWeapon = await _dbContext.Weapons.FindAsync(id);
            if (existingWeapon is null)
                return false;
            _dbContext.Weapons.Remove(existingWeapon);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}