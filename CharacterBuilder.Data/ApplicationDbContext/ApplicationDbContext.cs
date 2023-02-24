﻿using System.Collections.Immutable;
using CharacterBuilder.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CharacterBuilder.MVC.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CharacterEntity> Characters {get; set;}
    public DbSet<CampaignEntity> Campaigns {get; set;}
    public DbSet<CampaignPlayer> CampaignPlayers {get; set;}
    public DbSet<ItemEntity> Items {get; set;}
    public DbSet<WeaponEntity> Weapons {get; set;}
    public DbSet<CharacterInventorySlotEntity> InventorySlots {get; set;}

}
