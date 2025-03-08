using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utility;
using Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker;

namespace Ziekenfonds_Kampigo_Project.Controllers;

public class GebruikerController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<CustomUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public GebruikerController(IUnitOfWork unitofwork, UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _uow = unitofwork;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [Authorize(Roles = SD.Role_Beheerder)]
    public async Task<IActionResult> Index()
    {
        var gebruikers = await _uow.CustomUserRepository.GetAllAsync();
        var model = new GebruikerListViewmodel
        {
            Gebruikers = gebruikers
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = SD.Role_Beheerder)]
    public async Task<IActionResult> Delete(string id)
    {
        CustomUser? gebruiker = await _uow.CustomUserRepository.GetByIdAsync(id);

        if (gebruiker == null)
        {
            return NotFound($"De gebruiker met ID {id} werd niet gevonden.");
        }

        _uow.CustomUserRepository.Delete(gebruiker);
        await _uow.SaveChangesAsync();
        TempData["success"] = "Gebruiker is succesvol verwijderd.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = SD.Role_Beheerder)]
    public async Task<IActionResult> EditGebruiker(string id)
    {
        var gebruiker = await _uow.CustomUserRepository.GetByIdAsync(id);

        if (gebruiker == null)
        {
            return NotFound($"De gebruiker met ID {id} werd niet gevonden.");
        }

        var viewModel = new GebruikerViewmodel
        {
            Id = gebruiker.Id,
            Naam = gebruiker.Naam,
            Voornaam = gebruiker.Voornaam,
            Email = gebruiker.Email!,
            GeboorteDatum = gebruiker.GeboorteDatum,
         
            TelefoonNummer = gebruiker.TelefoonNummer,
            IsActief = gebruiker.IsActief,
            Gemeente = gebruiker.Gemeente,
            Straat = gebruiker.Straat,
            Geboortedatum =  gebruiker.GeboorteDatum,
            Postcode = gebruiker.Postcode,
            Huisnummer = gebruiker.Huisnummer,
            IsMonitor = gebruiker.IsMonitor,
            Role = (await _userManager.GetRolesAsync(gebruiker)).FirstOrDefault() ?? string.Empty
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = SD.Role_Beheerder)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditGebruiker(GebruikerViewmodel viewModel)
    {
        
        var gebruiker = await _uow.CustomUserRepository.GetByIdAsync(viewModel.Id);

        if (gebruiker == null)
        {
            return NotFound($"De gebruiker met ID {viewModel.Id} werd niet gevonden.");
        }

        gebruiker.Gemeente = viewModel.Gemeente;
        gebruiker.Straat = viewModel.Straat;
        gebruiker.Huisnummer = viewModel.Huisnummer;
        gebruiker.Postcode = viewModel.Postcode;
        gebruiker.GeboorteDatum = viewModel.GeboorteDatum;
        
        gebruiker.Naam = viewModel.Naam;
        gebruiker.Voornaam = viewModel.Voornaam;
        gebruiker.Email = viewModel.Email;
        gebruiker.UserName = viewModel.Email;
        gebruiker.NormalizedEmail = viewModel.Email.ToUpper();
        gebruiker.NormalizedUserName = viewModel.Email.ToUpper();
        gebruiker.IsActief = viewModel.IsActief;
        gebruiker.IsMonitor = viewModel.IsMonitor;

        _uow.CustomUserRepository.Update(gebruiker);

        var currentRoles = await _userManager.GetRolesAsync(gebruiker);
        await _userManager.RemoveFromRolesAsync(gebruiker, currentRoles);

        if (!string.IsNullOrEmpty(viewModel.Role))
        {
            if (!await _roleManager.RoleExistsAsync(viewModel.Role))
            {
                return BadRequest($"De rol '{viewModel.Role}' bestaat niet.");
            }

            var roleResult = await _userManager.AddToRoleAsync(gebruiker, viewModel.Role);
            if (!roleResult.Succeeded)
            {
                TempData["error"] = "Er is iets misgegaan.";
                return View();
            }
        }

        await _uow.SaveChangesAsync();
        TempData["success"] = "Gebruiker is succesvol bewerkt.";
        return RedirectToAction("Index");
    }
}