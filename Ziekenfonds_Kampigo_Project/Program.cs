using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Data.UnitOfWork;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Models;
using Ziekenfonds_Kampigo_Project.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<KampigoDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Mapper Service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Swagger Service
builder.Services.AddSwaggerGen();

// Identity Service
builder.Services.AddIdentity<CustomUser, IdentityRole>().AddEntityFrameworkStores<KampigoDbContext>().AddDefaultTokenProviders();

// Identity routing configuration service
// ALWAYS AFTER ADDIDENTITY
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// Razor pages Service
builder.Services.AddRazorPages();

// E-mail Service
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Controller with Views Service
builder.Services.AddControllersWithViews();

// Service for UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
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

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
