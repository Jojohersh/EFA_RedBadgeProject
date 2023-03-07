using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBuilder.Models.Weapon;

namespace CharacterBuilder.Services.Weapon
{
    public interface IWeaponService
    {
        Task<bool> CreateWeaponAsync(WeaponCreate model, int createdById);
        Task<List<WeaponListItem>> GetAllWeaponsAsync();
        Task<WeaponDetail> GetWeaponByIdAsync(int id);
        Task<bool> UpdateWeaponAsync(WeaponEdit model, int editorId);
        Task<bool> DeleteWeaponAsync(int id);
    }
}