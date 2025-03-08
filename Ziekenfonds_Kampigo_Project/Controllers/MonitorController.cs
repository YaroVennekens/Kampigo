using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Ziekenfonds_Kampigo_Project.ViewModels.Monitor;
using Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;
using Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM;
using Ziekenfonds_Kampigo_Project.ViewModels.Deelnemer;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class MonitorController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<CustomUser> _userManager;

        public MonitorController(IUnitOfWork unitOfWork, UserManager<CustomUser> userManager)
        {
            _uow = unitOfWork;
            _userManager = userManager;
        }

        // GET: /Monitor/Index
        public async Task<IActionResult> Index()
        {
            var allUsers = await _uow.CustomUserRepository.GetAllAsync();

            // Initialize a list to hold filtered monitors
            var filteredMonitors = new List<GebruikerViewmodel>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Monitor") || roles.Contains("Hoofdmonitor"))
                {
                    // Ensure all required properties are set
                    filteredMonitors.Add(new GebruikerViewmodel
                    {
                        Id = user.Id,
                        Naam = user.Naam,
                        Voornaam = user.Voornaam,
                        Email = user.Email,
                        TelefoonNummer = user.TelefoonNummer, // Set TelefoonNummer
                        GeboorteDatum = user.GeboorteDatum,   // Set GeboorteDatum
                        Gemeente = user.Gemeente,             // Set Gemeente
                        Postcode = user.Postcode,             // Set Postcode
                        Role = roles.Contains("Hoofdmonitor") ? "Hoofdmonitor" : "Monitor",
                        IsMonitor = true,
                        IsActief = user.IsActief // Assuming you have this property
                    });
                }
            }

            // Sort Hoofdmonitoren first, then by Naam
           var sortedMonitors = filteredMonitors
                .OrderByDescending(m => m.Role == "Hoofdmonitor")
                .ThenBy(m => m.Naam)
                .ToList();


            var model = new MonitorListViewModel
            {
                Monitoren = sortedMonitors // This must be List<GebruikerViewmodel>
            };

            return View(model);
        }

        // POST: /Monitor/VerwijderMonitor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerwijderMonitor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Monitor met ID {id} niet gevonden.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Remove from roles if present
            if (roles.Contains("Hoofdmonitor"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Hoofdmonitor");
            }

            if (roles.Contains("Monitor"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Monitor");
            }

            // Add to 'Deelnemer' role if not already in it
            if (!roles.Contains("Deelnemer"))
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Deelnemer");
                if (!addToRoleResult.Succeeded)
                {
                    // Handle errors (e.g., log them)
                    TempData["error"] = "Fout bij het toevoegen aan de rol 'Deelnemer'.";
                    return RedirectToAction("Index");
                }
            }

            // Update the IsMonitor property
            user.IsMonitor = false;

            // Update the user in the repository
            _uow.CustomUserRepository.Update(user);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Gebruiker is succesvol verwijderd als monitor.";
            return RedirectToAction("Index");
        }

        // POST: /Monitor/VerwijderHoofdMonitor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerwijderHoofdMonitor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Monitor met ID {id} niet gevonden.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Remove 'Hoofdmonitor' role if present
            if (roles.Contains("Hoofdmonitor"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Hoofdmonitor");
            }

            // Add to 'Monitor' role if not already in it
            if (!roles.Contains("Monitor"))
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Monitor");
                if (!addToRoleResult.Succeeded)
                {
                    // Handle errors (e.g., log them)
                    TempData["error"] = "Fout bij het toevoegen aan de rol 'Monitor'.";
                    return RedirectToAction("Index");
                }
            }

            // Update the IsMonitor property
            user.IsMonitor = false;

            // Update the user in the repository
            _uow.CustomUserRepository.Update(user);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Gebruiker is succesvol verwijderd als hoofdmonitor.";
            return RedirectToAction("Index");
        }

        // POST: /Monitor/MaakHoofdMonitor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MaakHoofdMonitor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Monitor met ID {id} niet gevonden.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Remove 'Monitor' role if present
            if (roles.Contains("Monitor"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Monitor");
            }

            // Add 'Hoofdmonitor' role if not already in it
            if (!roles.Contains("Hoofdmonitor"))
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Hoofdmonitor");
                if (!addToRoleResult.Succeeded)
                {
                    // Handle errors (e.g., log them)
                    TempData["error"] = "Fout bij het toevoegen aan de rol 'Hoofdmonitor'.";
                    return RedirectToAction("Index");
                }
            }

            // Update the IsMonitor property
            user.IsMonitor = true;

            // Update the user in the repository
            _uow.CustomUserRepository.Update(user);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Gebruiker is succesvol aangemaakt als hoofdmonitor.";
            return RedirectToAction("Index");
        }

        // GET: /Monitor/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid user ID.");
            }

            // Fetch the monitor with related groepsreizen and opleidingen
            var monitor = await _uow.CustomUserRepository.GetMonitorWithGroepsreizenAsync(id);
            if (monitor == null)
            {
                return NotFound($"Monitor met ID {id} niet gevonden.");
            }

            var user = await _userManager.FindByIdAsync(id);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Geen Rol";

            // Construct the ViewModel
            var viewModel = new MonitorViewModel
            {
                Gebruiker = new GebruikerViewmodel
                {
                    Id = monitor.Id,
                    Naam = monitor.Naam,
                    Voornaam = monitor.Voornaam,
                    Email = monitor.Email,
                    TelefoonNummer = monitor.TelefoonNummer,
                    GeboorteDatum = monitor.GeboorteDatum,
                    Gemeente = monitor.Gemeente,
                    Postcode = monitor.Postcode,
                    Role = role,
                    IsMonitor = monitor.IsMonitor,
                    IsActief = monitor.IsActief
                },
                Groepsreizen = monitor.Monitors
                    .Select(gr => new GroepsreisViewModel
                    {
                        Id = gr.Groepsreis.Id,
                        Naam = gr.Groepsreis.Bestemming.Naam,
                        BeginDatum = gr.Groepsreis.Begindatum,
                        EindDatum = gr.Groepsreis.Einddatum
                    })
                    .ToList(),
                Opleidingen = monitor.OpleidingPersonen.Select(op => new OpleidingViewModel
                {
                    Naam = op.Opleiding.Naam,
                    BeginDatum = op.Opleiding.BeginDatum,
                    EindDatum = op.Opleiding.EindDatum,
                    Beschrijving = op.Opleiding.Beschrijving,
                    Locatie = op.Opleiding.Locatie // Add this line to set Locatie
                }).OrderBy(o => o.BeginDatum).ToList()

        };

            return View(viewModel);
        }

        // GET: /Monitor/GetDeelnemers
        // GET: /Monitor/DeelnemerMonitor
        [HttpGet]
        public async Task<IActionResult> DeelnemerMonitor()
        {
            var allUsers = await _uow.CustomUserRepository.GetAllAsync();

            // Filter deelnemers met een aparte lijst
            var deelnemers = new List<GebruikerViewmodel>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!user.IsMonitor && roles.Contains("Deelnemer"))
                {
                    deelnemers.Add(new GebruikerViewmodel
                    {
                        Id = user.Id,
                        Naam = user.Naam,
                        Voornaam = user.Voornaam,
                        Email = user.Email
                    });
                }
            }

            return View(deelnemers);
        }


        // POST: /Monitor/MaakMonitor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MaakMonitor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["error"] = "Ongeldig gebruikers-ID.";
                return RedirectToAction("DeelnemerMonitor");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["error"] = $"Deelnemer met ID {id} niet gevonden.";
                return RedirectToAction("Index");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Deelnemer"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Deelnemer");
            }

            // Add to 'Monitor' role if not already in it
            if (!roles.Contains("Monitor"))
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Monitor");
                if (!addToRoleResult.Succeeded)
                {
                    // Handle errors (e.g., log them)
                    TempData["error"] = "Fout bij het toevoegen aan de rol 'Monitor'.";
                    return RedirectToAction("Index");
                }
            }

            // Update the IsMonitor property
            user.IsMonitor = false;

            // Update the user in the repository
            _uow.CustomUserRepository.Update(user);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Gebruiker is succesvol aangemaakt als monitor";
            return RedirectToAction("Index");
        }


    }
}
