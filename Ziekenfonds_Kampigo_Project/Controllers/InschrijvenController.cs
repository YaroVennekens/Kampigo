using AutoMapper;
using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Models;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;
using Ziekenfonds_Kampigo_Project.ViewModels.Inschrijving;
using Ziekenfonds_Kampigo_Project.ViewModels.Kind;
using Ziekenfonds_Kampigo_Project.ViewModels.Monitor;
using Monitor = Models.Monitor;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class InschrijvenController : Controller
    {

        private readonly IUnitOfWork _uow;
        private readonly UserManager<CustomUser> _userManager;

        public InschrijvenController(IUnitOfWork unitofwork, UserManager<CustomUser> userManager)
        {
            _uow = unitofwork;
            _userManager = userManager;

        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var kinderen = await _uow.KindRepository.GetAllKinderenOnGebruikerIdAsync(currentUser.Id);
            var groepsreis = await _uow.GroepsreisRepository.GetWithBestemmingDeelnemersAndTheirOuderByIdAsync(id);

            // Map the Groepsreis entity to the GroepsreisViewModel
            var groepsreisViewModel = new GroepsreisViewModel
            {
                Id = groepsreis.Id,
                Naam = "",
                BeginDatum = groepsreis.Begindatum,
                EindDatum = groepsreis.Einddatum,
                Prijs = groepsreis.Prijs
            };

            // Prepare the view model
            var viewModel = new InschrijvingViewModel
            {
                GroepsreisDetailsId = id,  
                KinderenOfUser = kinderen.Select(k => new SelectListItem
                {
                    Value = k.Id.ToString(),
                    Text = $"{k.Voornaam} {k.Naam}"
                }).ToList(),
                KindNamen = kinderen.Select(k => new KindNameViewModel
                {
                    Id = k.Id,
                    Naam = $"{k.Voornaam} {k.Naam}",
                    isIngeschreven = groepsreis!.Deelnemers!.Any(d => d.KindId == k.Id)
                }).ToList()
            };
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Index(InschrijvingViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var allKinderen = await _uow.KindRepository.GetAllKinderenOnGebruikerIdAsync(user.Id);

            if (model.SelectedKindIds == null || !model.SelectedKindIds.Any())
            {
                TempData["error"] = "U moet minstens één gezinslid selecteren.";
                model.KinderenOfUser = allKinderen.Select(k => new SelectListItem
                {
                    Value = k.Id.ToString(),
                    Text = $"{k.Voornaam} {k.Naam}"
                }).ToList();

                model.KindNamen = allKinderen.Select(k => new KindNameViewModel
                {
                    Id = k.Id,
                    Naam = $"{k.Voornaam} {k.Naam}"
                }).ToList();

                return View(model);
            }

            if (ModelState.IsValid)
            {
                // Retrieve all existing deelnemers for the selected groepsreis
                var existingDeelnemers = await _uow.DeelnemerRepository.FindAllAsync(d =>
                    d.GroepsreisDetailsId == model.GroepsreisDetailsId &&
                    model.SelectedKindIds.Contains(d.KindId));

                // Get the IDs of the already registered children
                var alreadyRegisteredKindIds = existingDeelnemers.Select(d => d.KindId).ToList();

                if (alreadyRegisteredKindIds.Any())
                {
                    // Populate KindNamen to ensure it has data for the lookup

                    model.KindNamen = allKinderen.Select(k => new KindNameViewModel
                    {
                        Id = k.Id,
                        Naam = $"{k.Voornaam} {k.Naam}"
                    }).ToList();

                    // Get the names of the already registered children from KindNamen
                    var alreadyRegisteredNames = model.KindNamen
                        .Where(k => alreadyRegisteredKindIds.Contains(k.Id))
                        .Select(k => k.Naam)
                        .ToList();

                    var namesString = string.Join(", ", alreadyRegisteredNames);

                    // Repopulate the 'KinderenOfUser' dropdown and return the view
                    model.KinderenOfUser = allKinderen.Select(k => new SelectListItem
                    {
                        Value = k.Id.ToString(),
                        Text = $"{k.Voornaam} {k.Naam}"
                    }).ToList();

                    TempData["error"] = $"De volgende kinderen zijn al ingeschreven voor deze groepsreis: {namesString}.";
                    return View(model);
                }

                // Add new deelnemers if no duplicates found
                foreach (var kindId in model.SelectedKindIds)
                {
                    var deelnemer = new Deelnemer
                    {
                        KindId = kindId,
                        GroepsreisDetailsId = model.GroepsreisDetailsId,
                        Opmerking = model.Opmerking
                    };

                    await _uow.DeelnemerRepository.AddAsync(deelnemer);
                }

                await _uow.SaveChangesAsync();

                TempData["success"] = "Succesvol ingeschreven aan deze groepsreis.";
                return RedirectToAction("Index", "Home");
            }

            // Populate KindNamen in case of general ModelState failure
            model.KinderenOfUser = allKinderen.Select(k => new SelectListItem
            {
                Value = k.Id.ToString(),
                Text = $"{k.Voornaam} {k.Naam}"
            }).ToList();

            model.KindNamen = allKinderen.Select(k => new KindNameViewModel
            {
                Id = k.Id,
                Naam = $"{k.Voornaam} {k.Naam}"
            }).ToList();


            TempData["error"] = "Er is iets misgegaan.";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignMonitor(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Check if the user is already assigned as a hoofdmonitor for this groepsreis
            var existingMonitor = await _uow.MonitorRepository.FindAsync(m =>
                m.GebruikerId == currentUser!.Id && m.GroepsreisDetailsId == id);

            if (existingMonitor != null)
            {
                TempData["error"] = "Je bent al aangemeld als monitor of hoofdmonitor voor deze groepsreis.";
                return Redirect($"/Groepsreis/Details/{id}");
            }

            var groepsReis = await _uow.GroepsreisRepository.GetWithDetailsById(id);
            if (groepsReis == null)
            {
                TempData["error"] = "Groepsreis niet gevonden.";
                return Redirect($"/Groepsreis/Details/{id}");
            }

            // Calculate the user's age
            var age = DateTime.Now.Year - currentUser!.GeboorteDatum.Year;
            if (currentUser.GeboorteDatum > DateTime.Now.AddYears(-age)) age--;

            // Check if the user meets the minimum age requirement
            if (age < groepsReis.Bestemming!.MaxLeeftijd)
            {
                TempData["error"] = $"Je moet minimaal {groepsReis.Bestemming.MaxLeeftijd} jaar oud zijn om je in te schrijven als monitor voor deze groepsreis.";
                return Redirect($"/Groepsreis/Details/{id}");
            }

            if (ModelState.IsValid)
            {
                // Create a new Monitor object
                var monitor = new Monitor
                {
                    GebruikerId = currentUser.Id,
                    GroepsreisDetailsId = id,
                    IsHoofdMonitor = false,
                    IsToegewezen = false
                };

                // Add the Monitor to the database
                await _uow.MonitorRepository.AddAsync(monitor);

                // Commit the changes to the database
                await _uow.SaveChangesAsync();

                TempData["success"] = "Succesvol kandidaat gesteld als monitor.";
                return Redirect($"/Groepsreis/Details/{id}");
            }

            // Log ModelState errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            TempData["error"] = "Er is iets misgegaan.";
            return Redirect($"/Groepsreis/Details/{id}");
        }

        [HttpPost]
        public async Task<IActionResult> AssignHoofdMonitor(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Check if the user is already assigned as a hoofdmonitor for this groepsreis
            var existingMonitor = await _uow.MonitorRepository.FindAsync(m =>
                m.GebruikerId == currentUser!.Id && m.GroepsreisDetailsId == id);

            if (existingMonitor != null)
            {
                TempData["error"] = "Je bent al aangemeld als monitor of hoofdmonitor voor deze groepsreis.";
                return Redirect($"/Groepsreis/Details/{id}");
            }

            var groepsReis = await _uow.GroepsreisRepository.GetWithDetailsById(id);
            if (groepsReis == null)
            {
                TempData["error"] = "Groepsreis niet gevonden.";
                return Redirect($"/Groepsreis/Details/{id}");
            }

            // Calculate the user's age
            var age = DateTime.Now.Year - currentUser!.GeboorteDatum.Year;
            if (currentUser.GeboorteDatum > DateTime.Now.AddYears(-age)) age--;

            // Check if the user meets the minimum age requirement
            if (age < groepsReis.Bestemming!.MaxLeeftijd)
            {
                TempData["error"] = $"Je moet minimaal {groepsReis.Bestemming.MaxLeeftijd} jaar oud zijn om je in te schrijven als hoofdmonitor voor deze groepsreis.";
                return Redirect($"/Groepsreis/Details/{id}");
            }

            if (ModelState.IsValid)
            {
                // Create a new Monitor object
                var monitor = new Monitor
                {
                    GebruikerId = currentUser.Id,
                    GroepsreisDetailsId = id,
                    IsHoofdMonitor = true,
                    IsToegewezen = false
                };

                // Add the Monitor to the database
                await _uow.MonitorRepository.AddAsync(monitor);

                // Commit the changes to the database
                await _uow.SaveChangesAsync();

                TempData["success"] = "Succesvol kandidaat gesteld als Hoofdmonitor.";
                return Redirect($"/Groepsreis/Details/{id}");
            }

            TempData["error"] = "Er is iets misgegaan.";
            return Redirect($"/Groepsreis/Details/{id}");
        }
    }
}
