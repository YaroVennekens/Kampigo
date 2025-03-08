using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Ziekenfonds_Kampigo_Project.ViewModels.Monitor;
using Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class OpleidingController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<CustomUser> _userManager;

        public OpleidingController(IUnitOfWork unitOfWork, UserManager<CustomUser> userManager)
        {
            _uow = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Beheren()
        {
            var opleidingen = await _uow.OpleidingRepository.GetAllWithUsersAsync();
            var currentDate = DateTime.Now;

            var viewModel = opleidingen
                .Select(o => new OpleidingViewModel
                {
                    Id = o.Id,
                    Naam = o.Naam,
                    Beschrijving = o.Beschrijving,
                    BeginDatum = o.BeginDatum,
                    EindDatum = o.EindDatum,
                    Locatie = o.Locatie,
                    OpleidingVereist = o.OpleidingVereist,
                    IsVoltooid = o.EindDatum < currentDate,
                    IsUitgebreid = o.OpleidingVereist.HasValue,
                    IsBezig = o.BeginDatum <= currentDate && o.EindDatum >= currentDate,
                })
                .OrderBy(o => o.EindDatum)
                .ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> MijnOpleidingen()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var currentDate = DateTime.Now;
            var opleidingen = await _uow.OpleidingRepository.GetOpleidingenByUserIdAsync(user.Id);
            var userId = user.Id;

            var mijnOpleidingen = opleidingen
                .Select(o => new OpleidingViewModel
                {
                    Id = o.Id,
                    Naam = o.Naam,
                    Beschrijving = o.Beschrijving,
                    BeginDatum = o.BeginDatum,
                    EindDatum = o.EindDatum,
                    Locatie = o.Locatie,
                    OpleidingVereist = o.OpleidingVereist,
                    IsVoltooid = o.EindDatum < currentDate,
                    IsUitgebreid = o.OpleidingVereist.HasValue,
                    IsBezig = o.BeginDatum <= currentDate && o.EindDatum >= currentDate,
                    // OpleidingPersonen is nooit null hier aangezien de opleidingen zijn gefilterd op gebruiker
                    IsGeaccepteerd = o.OpleidingPersonen!.Any(op => op.GebruikerId == userId && op.OpleidingId == o.Id && op.IsGeaccepteerd)
                })
                .ToList();

            return View(mijnOpleidingen);
        }

        public async Task<IActionResult> OverzichtOpleidingen()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var currentDate = DateTime.Now;
            var aangeradenOpleidingen = await _uow.OpleidingRepository.GetRecommendedOpleidingenAsync(user.Id, currentDate);
            var andereOpleidingen = await _uow.OpleidingRepository.GetOtherOpleidingenAsync(user.Id, currentDate);

            var viewModel = new OverzichtOpleidingenListViewModel
            {
                AangeradenOpleidingen = aangeradenOpleidingen
                    .Select(o => MapToViewModel(o, user, currentDate, true))
                    .ToList(),
                AndereOpleidingen = andereOpleidingen
                    .Select(o => MapToViewModel(o, user, currentDate, false))
                    .Where(o => !aangeradenOpleidingen.Any(ao => ao.Id == o.Id))
                    .ToList()
            };

            return View(viewModel);
        }

        // Functie om voor beide lijsten dezelfde viewmodel te gebruiken
        private static OverzichtOpleidingenViewModel MapToViewModel(Opleiding o, CustomUser user, DateTime currentDate, bool isAangeraden)
        {
            return new OverzichtOpleidingenViewModel
            {
                Id = o.Id,
                Naam = o.Naam,
                Beschrijving = o.Beschrijving,
                BeginDatum = o.BeginDatum,
                EindDatum = o.EindDatum,
                // OpleidingPersonen kan null zijn als er geen gebruikers zijn ingeschreven
                AantalPlaatsen = o.AantalPlaatsen - (o.OpleidingPersonen?.Count ?? 0),
                OpleidingVereist = o.OpleidingVereist,
                Locatie = o.Locatie,
                IsAangeraden = isAangeraden,
                IsBezig = o.BeginDatum <= currentDate && o.EindDatum >= currentDate,
                // Als de opleiding een vereiste opleiding heeft, check of de gebruiker deze heeft gevolgd
                IsUitgebreid = o.OpleidingVereist.HasValue && !(user.OpleidingPersonen?.Any(uo => uo.OpleidingId == o.OpleidingVereist.Value && uo.Opleiding?.EindDatum < currentDate) ?? false),
                IsVoltooid = o.EindDatum < currentDate,
                IsBijnaVol = o.AantalPlaatsen <= 5,
                VereisteOpleidingNaam = o.VoorwaardeOpleiding != null ? o.VoorwaardeOpleiding.Naam : "geen naam gevonden"
            };
        }

        [HttpPost]
        public async Task<IActionResult> Inschrijven(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var opleiding = await _uow.OpleidingRepository.GetWithUsersByIdAsync(id);
            if (opleiding == null)
            {
                return NotFound();
            }

            if (opleiding.OpleidingPersonen?.Any(op => op.GebruikerId == user.Id) == true)
            {
                return BadRequest("U bent al ingeschreven voor deze opleiding.");
            }

            if (opleiding.OpleidingVereist.HasValue)
            {
                var vereisteOpleiding = await _uow.OpleidingRepository.GetWithUsersByIdAsync(opleiding.OpleidingVereist.Value);
                // Als de vereiste opleiding niet bestaat of de gebruiker deze niet heeft gevolgd, geef een melding
                if (vereisteOpleiding == null || !(vereisteOpleiding.OpleidingPersonen?.Any(op => op.GebruikerId == user.Id && op.Opleiding?.EindDatum < DateTime.Now) ?? false))
                {
                    return BadRequest("U hebt nog niet alle vereiste opleidingen gevolgd.");
                }
            }

            if (opleiding.AantalPlaatsen - opleiding.OpleidingPersonen?.Count <= 0)
            {
                return BadRequest("Geen plaatsen meer vrij.");
            }

            var opleidingPersoon = new OpleidingPersoon
            {
                OpleidingId = opleiding.Id,
                Opleiding = opleiding,
                GebruikerId = user.Id,
                Gebruiker = user
            };

            await _uow.OpleidingPersoonRepository.AddAsync(opleidingPersoon);

            opleiding.AantalPlaatsen -= 1;
            _uow.OpleidingRepository.Update(opleiding);

            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(MijnOpleidingen));
        }

        ///////////////////////
        // BEHEERDERSSCHERM //
        //////////////////////

        public async Task<IActionResult> Create()
        {
            var ViewModel = new OpleidingCreateViewModel
            {
                // Get all opleidingen
                Opleidingen = (await _uow.OpleidingRepository.GetAllAsync()).ToList(),
                Naam = "",
                Beschrijving = "",
                Locatie = "",
            };

            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OpleidingCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate the Opleidingen list if the model state is invalid
                model.Opleidingen = (await _uow.OpleidingRepository.GetAllAsync()).ToList();
                TempData["error"] = "Er is iets misgegaan. Controleer de invoer en probeer het opnieuw.";
                return View(model);
            }

            // Als de opleiding vereist is voor zichzelf, geef een foutmelding
            if (model.OpleidingVereist == model.Id)
            {
                ModelState.AddModelError(nameof(model.OpleidingVereist), "Een opleiding kan niet vereist zijn voor zichzelf.");
                model.Opleidingen = (await _uow.OpleidingRepository.GetAllAsync()).ToList();
                TempData["error"] = "Er is iets misgegaan. Controleer de invoer en probeer het opnieuw.";
                return View(model);
            }

            var opleiding = new Opleiding
            {
                Naam = model.Naam,
                Beschrijving = model.Beschrijving,
                BeginDatum = model.BeginDatum,
                EindDatum = model.EindDatum,
                Locatie = model.Locatie,
                AantalPlaatsen = model.AantalPlaatsen,
                OpleidingVereist = model.OpleidingVereist
            };

            await _uow.OpleidingRepository.AddAsync(opleiding);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Opleiding is succesvol aangemaakt.";
            return RedirectToAction(nameof(Beheren));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Opleiding? opleiding = await _uow.OpleidingRepository.GetOpleidingWithUsersByIdAsync(id);

            if (opleiding == null)
            {
                TempData["error"] = "Opleiding niet gevonden in de database.";
                return RedirectToAction("Beheren");
            }

            if (opleiding.OpleidingPersonen?.Count != 0)
            {
                Console.WriteLine("Boop!");
                TempData["error"] = "Opleiding kan niet verwijderd worden omdat er nog gebruikers ingeschreven zijn.";
                return RedirectToAction("Beheren");
            }

            _uow.OpleidingRepository.Delete(opleiding);
            await _uow.SaveChangesAsync();
            TempData["success"] = "Opleiding is succesvol verwijderd.";
            return RedirectToAction("Beheren");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var opleiding = await _uow.OpleidingRepository.GetByIdAsync(id);
            if (opleiding == null)
            {
                return NotFound();
            }

            var viewModel = new OpleidingEditViewModel
            {
                Id = opleiding.Id,
                Naam = opleiding.Naam,
                Beschrijving = opleiding.Beschrijving,
                BeginDatum = opleiding.BeginDatum,
                EindDatum = opleiding.EindDatum,
                Locatie = opleiding.Locatie,
                AantalPlaatsen = opleiding.AantalPlaatsen,
                OpleidingVereist = opleiding.OpleidingVereist,
                Opleidingen = (await _uow.OpleidingRepository.GetAllAsync()).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OpleidingEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate the Opleidingen list if the model state is invalid
                model.Opleidingen = (await _uow.OpleidingRepository.GetAllAsync()).ToList();
                TempData["error"] = "Er is iets misgegaan. Controleer de invoer en probeer het opnieuw.";
                return View(model);
            }

            var opleiding = await _uow.OpleidingRepository.GetByIdAsync(model.Id);
            if (opleiding == null)
            {
                TempData["error"] = "Opleiding niet gevonden in de database. Laad de pagina opnieuw.";
                return NotFound();
            }

            // Als de opleiding vereist is voor zichzelf, geef een foutmelding
            if (model.OpleidingVereist == model.Id)
            {
                ModelState.AddModelError(nameof(model.OpleidingVereist), "Een opleiding kan niet vereist zijn voor zichzelf.");
                model.Opleidingen = (await _uow.OpleidingRepository.GetAllAsync()).ToList();
                TempData["error"] = "Er is iets misgegaan. Controleer de invoer en probeer het opnieuw.";
                return View(model);
            }

            // Als de opleiding een vooropleiding heeft die zelf de huidige opleiding heeft als vooropleiding, geef een foutmelding
            var opleidingVereist = await _uow.OpleidingRepository.GetByIdAsync(model.OpleidingVereist);
            if (opleidingVereist != null && opleidingVereist.OpleidingVereist == model.Id)
            {
                ModelState.AddModelError(nameof(model.OpleidingVereist), "De geselecteerde vooropleiding heeft de huidige opleiding als vooropleiding ingesteld.");
                model.Opleidingen = (await _uow.OpleidingRepository.GetAllAsync()).ToList();
                TempData["error"] = "Er is iets misgegaan. Controleer de invoer en probeer het opnieuw.";
                return View(model);
            }

            opleiding.Naam = model.Naam;
            opleiding.Beschrijving = model.Beschrijving;
            opleiding.BeginDatum = model.BeginDatum;
            opleiding.EindDatum = model.EindDatum;
            opleiding.Locatie = model.Locatie;
            opleiding.AantalPlaatsen = model.AantalPlaatsen;
            opleiding.OpleidingVereist = model.OpleidingVereist;

            _uow.OpleidingRepository.Update(opleiding);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Opleiding is succesvol bijgewerkt.";
            return RedirectToAction(nameof(Beheren));
        }

        [Route("Opleiding/{opleidingId}/Monitors")]
        public async Task<IActionResult> Monitors(int opleidingId)
        {
            var opleiding = await _uow.OpleidingRepository.GetOpleidingWithUsersByIdAsync(opleidingId);
            if (opleiding == null)
            {
                return NotFound();
            }

            var ingeschrevenMonitors = await _uow.OpleidingRepository.GetIngeschrevenMonitorsAsync(opleidingId);
            var geaccepteerdeMonitors = await _uow.OpleidingRepository.GetGeaccepteerdeMonitorsAsync(opleidingId);
            var nietIngeschrevenMonitors = await _uow.CustomUserRepository.GetMonitorsNotInOpleidingAsync(opleidingId);

            var viewModel = new OpleidingMetMonitorsViewModel
            {
                Id = opleiding.Id,
                Naam = opleiding.Naam!,
                AantalPlaatsen = opleiding.AantalPlaatsen,
                IngeschrevenMonitors = ingeschrevenMonitors
                    .Select(m => new MonitorBijOpleidingViewModel
                    {
                        Id = m.Id,
                        Naam = $"{m.Voornaam} {m.Naam}",
                        IsGeaccepteerd = false
                    })
                    .ToList(),
                GeaccepteerdeMonitors = geaccepteerdeMonitors
                    .Select(m => new MonitorBijOpleidingViewModel
                    {
                        Id = m.Id,
                        Naam = $"{m.Voornaam} {m.Naam}",
                        IsGeaccepteerd = true
                    })
                    .ToList(),
                NietIngeschrevenMonitors = nietIngeschrevenMonitors
                    .Select(m => new MonitorBijOpleidingViewModel
                    {
                        Id = m.Id,
                        Naam = $"{m.Voornaam} {m.Naam}"
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [Route("Opleiding/{opleidingId}/Monitors/Accept")]
        [HttpPost]
        public async Task<IActionResult> Accept(int opleidingId, string monitorId)
        {
            var opleiding = await _uow.OpleidingRepository.GetWithUsersByIdAsync(opleidingId);
            if (opleiding == null)
            {
                return NotFound();
            }

            if (opleiding.AantalPlaatsen <= 0)
            {
                ModelState.AddModelError(nameof(opleiding), "De opleiding heeft geen plaatsen meer vrij.");
                TempData["error"] = "Er is iets misgegaan. Controleer de invoer en probeer het opnieuw.";
                return RedirectToAction(nameof(Monitors), new { opleidingId });
            }

            // OpleidingPersonen kan null zijn als er geen gebruikers zijn ingeschreven
            var monitor = opleiding.OpleidingPersonen?.FirstOrDefault(op => op.GebruikerId == monitorId);
            if (monitor == null)
            {
                return NotFound();
            }

            monitor.IsGeaccepteerd = true;
            opleiding.AantalPlaatsen--;

            _uow.OpleidingPersoonRepository.Update(monitor);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Monitor is geaccepteerd.";
            return RedirectToAction(nameof(Monitors), new { opleidingId });
        }

        [Route("Opleiding/{opleidingId}/Monitors/Remove")]
        [HttpPost]
        public async Task<IActionResult> Remove(int opleidingId, string monitorId)
        {
            var opleiding = await _uow.OpleidingRepository.GetWithUsersByIdAsync(opleidingId);
            if (opleiding == null)
            {
                return NotFound();
            }

            var monitor = opleiding.OpleidingPersonen?.FirstOrDefault(op => op.GebruikerId == monitorId);
            if (monitor == null)
            {
                return NotFound();
            }

            opleiding.AantalPlaatsen++;

            _uow.OpleidingPersoonRepository.Delete(monitor);
            await _uow.SaveChangesAsync();

            TempData["error"] = "Monitor is uitgeschreven.";
            return RedirectToAction(nameof(Monitors), new { opleidingId });
        }

        [Route("Opleiding/{opleidingId}/Monitors/Add")]
        [HttpPost]
        public async Task<IActionResult> Add(int opleidingId, string monitorId)
        {
            var opleiding = await _uow.OpleidingRepository.GetWithUsersByIdAsync(opleidingId);
            if (opleiding == null)
            {
                return NotFound();
            }

            if (opleiding.AantalPlaatsen <= 0)
            {
                ModelState.AddModelError(nameof(opleiding), "De opleiding heeft geen plaatsen meer vrij.");
                TempData["error"] = "Er is iets misgegaan. Controleer de invoer en probeer het opnieuw.";
                return RedirectToAction(nameof(Monitors), new { opleidingId });
            }


            var monitor = await _userManager.FindByIdAsync(monitorId);
            if (monitor == null)
            {
                return NotFound();
            }

            var opleidingPersoon = new OpleidingPersoon
            {
                OpleidingId = opleiding.Id,
                Opleiding = opleiding,
                GebruikerId = monitor.Id,
                Gebruiker = monitor,
                IsGeaccepteerd = true
            };

            opleiding.AantalPlaatsen--;

            await _uow.OpleidingPersoonRepository.AddAsync(opleidingPersoon);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Monitor is ingeschreven.";
            return RedirectToAction(nameof(Monitors), new { opleidingId });
        }
    }
}
