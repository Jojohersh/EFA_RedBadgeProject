using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CharacterBuilder.MVC.Data;
using CharacterBuilder.Services.Item;
using CharacterBuilder.Services.Campaign;
using CharacterBuilder.Services.CampaignPlayer;
using CharacterBuilder.Services.Character;
using CharacterBuilder.Services.Weapon;
using CharacterBuilder.Services.InventorySlots;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser<int>>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options => {
    //Cookie settings
    options.Cookie.HttpOnly = true;
    //options.Cookie.Expiration

    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<ICampaignPlayerService, CampaignPlayerService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IWeaponService, WeaponService>();
builder.Services.AddScoped<IInventorySlotsService, InventorySlotService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
