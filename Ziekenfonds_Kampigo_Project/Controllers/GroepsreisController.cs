using AutoMapper;
using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Ziekenfonds_Kampigo_Project.ViewModels.Activiteit;
using Ziekenfonds_Kampigo_Project.ViewModels.Bestemming;
using Ziekenfonds_Kampigo_Project.ViewModels.Foto;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;
using Ziekenfonds_Kampigo_Project.ViewModels.Monitor;
using Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM;
using Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker;
using Microsoft.AspNetCore.Identity;
using Utility;
using Monitor = Models.Monitor;
using Ziekenfonds_Kampigo_Project.ViewModels.Deelnemer;
using Ziekenfonds_Kampigo_Project.ViewModels.Kind;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Org.BouncyCastle.Bcpg;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class GroepsreisController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public GroepsreisController(IUnitOfWork unitofwork,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            UserManager<CustomUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender)
        {
            _uow = unitofwork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> Create()
        {
            var ViewModel = new CreateGroepsreisViewModel
            {
                NieuweBestemmingBeschrijving = "",
                NieuweBestemmingNaam = "",
            };

            // Load bestemmingen dropdown with name and code
            var Bestemmingen = await _uow.BestemmingRepository.GetAllAsync();
            ViewModel.Bestemmingen = Bestemmingen.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = $"{b.Naam} - {b.Code}"
            }).ToList();

            // Load activiteiten dropdown with name
            var Activiteiten = await _uow.ActiviteitRepository.GetAllAsync();
            ViewModel.Activiteiten = Activiteiten.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Naam
            }).ToList();

            return View(ViewModel);
        }

        // POST: Groepsreis/Create
        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> Create(CreateGroepsreisViewModel model, List<IFormFile> photos)
        {
            // custom validation for dates
            if (model.Einddatum < model.Begindatum)
            {
                ModelState.AddModelError("Einddatum", "Einddatum mag niet eerder zijn dan Begindatum.");
            }

            // custom validation for price
            if (model.Prijs <= 0)
            {
                ModelState.AddModelError("Prijs", "Prijs is verplicht en moet groter zijn dan 0.");
            }

            // if create bestemming is checked we do do custom validation for bestemming fields
            if (model.IsCreatingNewBestemming)
            {
                // WARNING: this cant be done in viewmodel because there needs to be a check if create bestemming is selected or not.

                // validation for bestemming naam
                if (string.IsNullOrEmpty(model.NieuweBestemmingNaam))
                {
                    ModelState.AddModelError("NieuweBestemmingNaam", "De naam van de bestemming is verplicht.");
                }

                // validation for beschrijving
                if (string.IsNullOrEmpty(model.NieuweBestemmingBeschrijving))
                {
                    ModelState.AddModelError("NieuweBestemmingBeschrijving", "De beschrijving van de bestemming is verplicht.");
                }

                //// Validate ages
                //if (!model.NieuweBestemmingMinLeeftijd.HasValue)
                //{
                //    ModelState.AddModelError("NieuweBestemmingMinLeeftijd", "De minimum leeftijd is verplicht.");
                //}

                //if (!model.NieuweBestemmingMaxLeeftijd.HasValue)
                //{
                //    ModelState.AddModelError("NieuweBestemmingMaxLeeftijd", "De maximum leeftijd is verplicht.");
                //}

                if (model.NieuweBestemmingMinLeeftijd < 6 || model.NieuweBestemmingMinLeeftijd > 21)
                {
                    ModelState.AddModelError("NieuweBestemmingMinLeeftijd", "De minimum leeftijd moet tussen 6 en 21 liggen.");
                }

                if (model.NieuweBestemmingMaxLeeftijd < 6 || model.NieuweBestemmingMaxLeeftijd > 21)
                {
                    ModelState.AddModelError("NieuweBestemmingMaxLeeftijd", "De maximum leeftijd moet tussen 6 en 21 liggen.");
                }

                // valiation where minimum leeftijd cant be higher than maxium leeftijd
                if (/*model.NieuweBestemmingMinLeeftijd.HasValue &&
                    model.NieuweBestemmingMaxLeeftijd.HasValue &&*/
                    model.NieuweBestemmingMinLeeftijd > model.NieuweBestemmingMaxLeeftijd)
                {
                    ModelState.AddModelError("NieuweBestemmingMinLeeftijd", "De minimum leeftijd mag niet groter zijn dan de maximumleeftijd.");
                }
            }
            else
            {
                // If not creating a new Bestemming, validate BestemmingId. Meaning a bestemming must be required with a groepsreis.
                if (!model.BestemmingId.HasValue || model.BestemmingId == 0)
                {
                    ModelState.AddModelError("BestemmingId", "U moet een geldige bestemming selecteren.");
                }
            }

            // if validation fails show the errors and keep the checkbox state when page reloads
            if (!ModelState.IsValid)
            {
                // Persist the state of the checkbox only if the user had previously checked it
                if (model.BestemmingId == null)
                {
                    // Only set the checkbox if the user didn't select an existing Bestemming
                    model.IsCreatingNewBestemming = model.IsCreatingNewBestemming || false;
                }
                model = await GetCreateGroepsreisViewModel(model); // Repopulate dropdowns
                TempData["error"] = "Er is iets misgegaan.";
                return View(model);
            }

            // Create new groepsreis
            Bestemming? bestemming = null;

            if (model.IsCreatingNewBestemming)
            {
                bestemming = new Bestemming
                {
                    Naam = model.NieuweBestemmingNaam,
                    Beschrijving = model.NieuweBestemmingBeschrijving,
                    MaxLeeftijd = model.NieuweBestemmingMaxLeeftijd,
                    MinLeeftijd = model.NieuweBestemmingMinLeeftijd,
                    Code = Guid.NewGuid().ToString(),
                    Fotos = []
                };

                // Handle file upload for new Bestemming if they are provided
                if (photos != null && photos.Count > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string photoFolderPath = Path.Combine(wwwRootPath, "img", "bestemming");

                    if (!Directory.Exists(photoFolderPath))
                    {
                        Directory.CreateDirectory(photoFolderPath);
                    }

                    foreach (var photo in photos)
                    {
                        if (photo.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                            string fullPath = Path.Combine(photoFolderPath, fileName);

                            using (var fileStream = new FileStream(fullPath, FileMode.Create))
                            {
                                await photo.CopyToAsync(fileStream);
                            }

                            bestemming.Fotos.Add(new Foto
                            {
                                Naam = fileName,
                                Bestemming = bestemming
                            });
                        }
                    }
                }

                // Save the new Bestemming to the database
                await _uow.BestemmingRepository.AddAsync(bestemming);
                await _uow.SaveChangesAsync();
            }
            else // if a  exisisting bestemming is selected then connect this to groepsreis
            {
                bestemming = await _uow.BestemmingRepository.GetByIdAsync(model.BestemmingId);

                if (bestemming == null)
                {
                    ModelState.AddModelError("BestemmingId", "De geselecteerde bestemming bestaat niet.");
                    model = await GetCreateGroepsreisViewModel(model);
                    TempData["error"] = "Er is iets misgegaan.";
                    return View(model);
                }
            }

            // Proceed with creating the Groepsreis
            var groepsreis = new Groepsreis
            {
                Begindatum = model.Begindatum,
                Einddatum = model.Einddatum,
                Prijs = model.Prijs,
                BestemmingId = bestemming.Id,
                Programmas = []
            };

            // Create Programma for each selected activiteit
            foreach (var activiteitId in model.ActiviteitenIds)
            {
                var programma = new Programma
                {
                    GroepsreisId = groepsreis.Id,
                    ActiviteitId = activiteitId,
                };
                groepsreis.Programmas.Add(programma);
            }

            // Save the Groepsreis to the database
            await _uow.GroepsreisRepository.AddAsync(groepsreis);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Groepsreis is succesvol aangemaakt.";

            return RedirectToAction(nameof(Index));
        }

        //Get groepsreis with all their deelnemers if any
        [HttpGet]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> Deelnemers(int id)
        {
            var groepsreis = await _uow.GroepsreisRepository.GetWithBestemmingDeelnemersAndTheirOuderByIdAsync(id);

            if (groepsreis == null)
            {
                TempData["error"] = "Groepsreis niet gevonden in de database.";
                return View();
            }

            if (groepsreis.Deelnemers == null)
            {
                TempData["error"] = "Geen deelnemers teruggevonden.";
                return View();
            }

            // Step 2: Map the fetched data to the GroepsreisDeelnemersWithOuderViewModel
            var groepsreisViewModel = new GroepsreisDeelnemersWithOuderViewModel
            {
                // Mapping Groepsreis
                Groepsreis = new GroepsreisViewModel
                {
                    Id = groepsreis.Id,
                    Naam = groepsreis.Bestemming!.Naam,
                    BeginDatum = groepsreis.Begindatum,
                    EindDatum = groepsreis.Einddatum,
                    Prijs = groepsreis.Prijs,
                },
                // Mapping Bestemming
                Bestemming = new BestemmingDetailViewModel
                {
                    Naam = groepsreis.Bestemming!.Naam,
                    Beschrijving = groepsreis.Bestemming!.Beschrijving,
                    Code = groepsreis.Bestemming!.Code,
                    MinLeeftijd = groepsreis.Bestemming.MinLeeftijd,
                    MaxLeeftijd = groepsreis.Bestemming.MaxLeeftijd
                },

                // Mapping Deelnemers
                Deelnemers = groepsreis.Deelnemers!.Select(d => new DeelnemerViewModel
                {
                    Kind = new KindViewModel
                    {
                        // Map Kind details
                        Id = d.Kind.Id,
                        Naam = d.Kind.Naam,
                        Voornaam = d.Kind.Voornaam,
                        Geboortedatum = d.Kind.Geboortedatum,
                        Allergieen = d.Kind.Allergieen,
                        Medicatie = d.Kind.Medicatie,

                        // Map Ouder details (Gebruiker)
                        Ouder = new GebruikerViewmodel
                        {
                            Id = d.Kind.Gebruiker!.Id, 
                            Voornaam = d.Kind.Gebruiker.Naam,
                            Naam = d.Kind.Gebruiker.Naam,
                            Email = d.Kind.Gebruiker.Email!,
                            TelefoonNummer = d.Kind.Gebruiker.TelefoonNummer,
                            Postcode = d.Kind.Gebruiker.Postcode,
                            Gemeente = d.Kind.Gebruiker.Gemeente,
                            GeboorteDatum = d.Kind.Gebruiker.GeboorteDatum,
                        }
                    },
                    Opmerking = d.Opmerking! 
                }).ToList()
            };
            return View(groepsreisViewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> DeleteDeelnemer(int groepsreisId, int deelnemerId)
        {
            // Get the deelnemer for the given groepsreis and user
            var deelnemer = await _uow.DeelnemerRepository.GetAsync(d => d.GroepsreisDetailsId == groepsreisId && d.KindId == deelnemerId);

            if (deelnemer == null)
            {
                // If deelnemer is not found, show an error message
                TempData["error"] = "Deelnemer niet gevonden in de database.";
                return Redirect($"/Groepsreis/Deelnemers/{groepsreisId}");
            }

            // Delete deelnemer and save changes to the database
            _uow.DeelnemerRepository.Delete(deelnemer);
            await _uow.SaveChangesAsync();

            // Provide feedback to the user
            TempData["success"] = "Deelnemer is succesvol verwijderd.";

            // Redirect to the same page to refresh the list of deelnemers
            return Redirect($"/Groepsreis/Deelnemers/{groepsreisId}");
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Groepsreis? groepsreis = await _uow.GroepsreisRepository.GetByIdAsync(id);

            if (groepsreis == null)
            {
                TempData["error"] = "Groepsreis niet gevonden in de database.";
                return View();
            }

            await _uow.GroepsreisRepository.DeleteGroepsreisWithMonitorsAndOnkostenAsync(groepsreis);
            await _uow.SaveChangesAsync();
            TempData["success"] = "Groepsreis is succesvol verwijderd.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var groepsreis = await _uow.GroepsreisRepository.GetWithDetailsById(id);
            if (groepsreis == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var groepsreisDetailViewModel = new GroepsreisDetailViewModel
            {
                UserId = userId,
                Id = id,
                Naam = groepsreis.Bestemming?.Naam ?? "Groepsreis zonder naam",
                Beschrijving = groepsreis.Bestemming!.Beschrijving,
                BeginDatum = groepsreis.Begindatum,
                EindDatum = groepsreis.Einddatum,
                MinLeeftijd = groepsreis.Bestemming.MinLeeftijd,
                MaxLeeftijd = groepsreis.Bestemming.MaxLeeftijd,
                Prijs = groepsreis.Prijs,
                Activiteiten = groepsreis.Programmas?
                    .Select(p => new ActiviteitDetailViewModel
                    {
                        Naam = p.Activiteit!.Naam,
                        Beschrijving = p.Activiteit.Beschrijving
                    }).ToList(),
                Fotos = groepsreis.Bestemming.Fotos?
                    .Select(f => new FotoViewModel { Naam = f.Naam }).ToList()
            };

            return View(groepsreisDetailViewModel);
        }
       
        [HttpGet]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> Edit(int id)
        {
            var groepsreis = await _uow.GroepsreisRepository.GetWithDetailsById(id);

            if (groepsreis == null)
            {
                return NotFound();
            }

            // Map the Groepsreis to the EditGroepsreisViewModel
            var viewModel = new EditGroepsreisViewModel
            {
                Id = groepsreis.Id,
                Begindatum = groepsreis.Begindatum,
                Einddatum = groepsreis.Einddatum,
                Prijs = groepsreis.Prijs,
                Bestemming = new EditBestemmingViewModel
                {
                    Naam = groepsreis.Bestemming!.Naam,
                    Beschrijving = groepsreis.Bestemming.Beschrijving,
                    MinLeeftijd = groepsreis.Bestemming.MinLeeftijd,
                    MaxLeeftijd = groepsreis.Bestemming.MaxLeeftijd,
                    Fotos = groepsreis.Bestemming.Fotos?.Select(photo => new FotoViewModel
                    {
                        Id = photo.Id,
                        Naam = photo.Naam
                    }).ToList()
                },
                Activiteiten = (await _uow.ActiviteitRepository.GetAllAsync())
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Naam
                    }).ToList(),
                ActiviteitenIds = groepsreis.Programmas?.Select(p => p.ActiviteitId).ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> Edit(EditGroepsreisViewModel model, List<IFormFile> photos)
        {
            // custom validation for dates
            if (model.Einddatum < model.Begindatum)
            {
                ModelState.AddModelError("Einddatum", "Einddatum mag niet eerder zijn dan Begindatum.");
            }
            // custom validation for ages
            if (model.Bestemming.MaxLeeftijd < model.Bestemming.MinLeeftijd)
            {
                ModelState.AddModelError("Bestemming.MaxLeeftijd", "Maximale leeftijd mag niet lager zijn dan de minimale leeftijd.");
            }

            // repopulate activities when form fails.
            if (!ModelState.IsValid)
            {
                model.Activiteiten = (await _uow.ActiviteitRepository.GetAllAsync())
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Naam
                    }).ToList();
                TempData["error"] = "Er is iets misgegaan.";
                return View(model);
            }

            // Retrieve the existing Groepsreis from the database
            var groepsreis = await _uow.GroepsreisRepository.GetWithDetailsById(model.Id);

            if (groepsreis == null)
            {
                TempData["error"] = "Groepsreis niet gevonden in de database.";
                return View();
            }

            // Update groepsreis
            groepsreis.Begindatum = model.Begindatum;
            groepsreis.Einddatum = model.Einddatum;
            groepsreis.Prijs = model.Prijs;

            // Update Bestemming
            groepsreis.Bestemming!.Naam = model.Bestemming.Naam;
            groepsreis.Bestemming.Beschrijving = model.Bestemming.Beschrijving;
            groepsreis.Bestemming.MinLeeftijd = model.Bestemming.MinLeeftijd;
            groepsreis.Bestemming.MaxLeeftijd = model.Bestemming.MaxLeeftijd;

            // Photo uploads related to the Bestemming
            if (photos != null && photos.Count > 0)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string photoFolderPath = Path.Combine(wwwRootPath, "img", "bestemming");

                if (!Directory.Exists(photoFolderPath))
                {
                    Directory.CreateDirectory(photoFolderPath);
                }

                foreach (var photo in photos)
                {
                    if (photo.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                        string fullPath = Path.Combine(photoFolderPath, fileName);

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await photo.CopyToAsync(fileStream);
                        }

                        // Associate photo with the Bestemming
                        var newFoto = new Foto
                        {
                            Naam = fileName,
                            Bestemming = groepsreis.Bestemming
                        };

                        // Add the new Foto to the Bestemming
                        groepsreis.Bestemming.Fotos?.Add(newFoto);
                    }
                }
            }

            // Add new activities and remove activities depending of what user selected
            var existingActiviteitIds = groepsreis.Programmas?.Select(p => p.ActiviteitId).ToList();
            existingActiviteitIds ??= [];

            groepsreis.Programmas?.RemoveAll(p => !model.ActiviteitenIds!.Contains(p.ActiviteitId));

            // Add new Programmas for any ActiviteitenIds not already linked
            foreach (var activiteitId in model.ActiviteitenIds!.Where(id => !existingActiviteitIds.Contains(id)))
            {
                groepsreis.Programmas?.Add(new Programma
                {
                    GroepsreisId = groepsreis.Id,
                    ActiviteitId = activiteitId
                });
            }

            // Save changes to the database
            _uow.GroepsreisRepository.Update(groepsreis);
            await _uow.SaveChangesAsync();

            TempData["success"] = "Groepsreis is succesvol bewerkt.";

            // Redirect to the Index page after successful update
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetBestemmingDetails(int id)
        {
            // map maybe

            Bestemming? bestemming = await _uow.BestemmingRepository.GetByIdAsync(id);

            if (bestemming == null)
            {
                TempData["error"] = "Bestemming niet gevonden in de database.";
                return View();
            }

           
            return Json(new
            {
                naam = bestemming.Naam,
                beschrijving = bestemming.Beschrijving,
                minLeeftijd = bestemming.MinLeeftijd,
                maxLeeftijd = bestemming.MaxLeeftijd
            });
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> Index()
        {
            var reizen = await _uow.GroepsreisRepository.GetAllWithActivities();
            
            var model = new GroepsreisBestemmingActiviteitenListViewModel
            {
                Groepsreizen = _mapper.Map<List<GroepsreisBestemmingActiviteitenViewModel>>(reizen)
            };

            return View(model);
        }
           
        
        
        [HttpGet]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> Monitors(int id)
        {
            // get monitor role
            var monitorUsers = await _userManager.GetUsersInRoleAsync("Monitor");

            // Fetch all users that have the role opleidingen with their followed opleidingen
            var allMonitors = await _userManager.Users
                .Where(u => monitorUsers.Contains(u)) 
                .Include(u => u.OpleidingPersonen!)  
                    .ThenInclude(op => op.Opleiding) 
                .ToListAsync();

            if (allMonitors == null)
            {
                TempData["error"] = "Er zijn geen monitoren gevonden in de database.";
                return View();
            }

            // Fetch groepsreis on id with it's information and monitoren information
            var groepsreis = await _uow.GroepsreisRepository.GetWithDetailsAndMonitorenDetailsByIdAsync(id);

            // Fetch monitors already assigned to that Groepsreis
            var assignedMonitors = await _uow.MonitorRepository.GetMonitorsForGroepsreisAsync(id);

            // Get the list of user IDs already assigned as monitors
            var assignedMonitorIds = assignedMonitors.Select(m => m.GebruikerId).ToList();

            // filter out monitors that are already assigned to the groepsreis (no duplication)
            var unassignedMonitors = allMonitors.Where(u => !assignedMonitorIds.Contains(u.Id)).ToList();

            // Map the unassigned monitors to the view model with their Opleidingen
            var allUsersWithOpleidingen = unassignedMonitors.Select(u => new GebruikerWithOpleidingenViewModel
            {
                Gebruiker = new GebruikerViewmodel
                {
                    Id = u.Id,
                    Voornaam = u.Voornaam,
                    Naam = u.Naam,
                    Email = u.Email ?? "",
                    IsActief = u.IsActief,
                    IsMonitor = u.IsMonitor,
                    TelefoonNummer = u.TelefoonNummer,
                    Postcode = u.Postcode,
                    Gemeente = u.Gemeente,
                    GeboorteDatum = u.GeboorteDatum,
                },
                Opleidingen = u.OpleidingPersonen?.Select(op => new OpleidingViewModel
                {
                    Naam = op.Opleiding!.Naam,
                    Beschrijving = op.Opleiding.Beschrijving,
                    Locatie = op.Opleiding.Locatie,
                }).ToList() ?? []
            }).ToList();

            // Map the assigned monitors to the view model
            var monitorenForGroepsreis = groepsreis.Monitors?.Select(m => new MonitorViewModel
            {
                IsToegewezen = m.IsToegewezen,
                IsHoofdmonitor = m.IsHoofdMonitor,
                Gebruiker = new GebruikerViewmodel
                {
                    Id = m.Gebruiker!.Id,
                    Voornaam = m.Gebruiker.Voornaam,
                    Naam = m.Gebruiker.Naam,
                    Email = m.Gebruiker.Email ?? "",
                    TelefoonNummer = m.Gebruiker.TelefoonNummer,
                    Postcode = m.Gebruiker.Postcode,
                    Gemeente = m.Gebruiker.Gemeente,
                    GeboorteDatum = m.Gebruiker.GeboorteDatum,
                    IsMonitor = m.Gebruiker.IsMonitor
                },
                Opleidingen = m.Gebruiker.OpleidingPersonen?.Select(op => new OpleidingViewModel
                {
                    Naam = op.Opleiding!.Naam,
                    Beschrijving = op.Opleiding.Beschrijving,
                    Locatie = op.Opleiding.Locatie,
                }).ToList() ?? []
            }).ToList() ?? [];

            // Map GroepsreisDetailsWithMonitors view model
            var groepsreisViewModel = new GroepsreisDetailsWithMonitors
            {
                Id = groepsreis.Id,
                Begindatum = groepsreis.Begindatum,
                Einddatum = groepsreis.Einddatum,
                Prijs = groepsreis.Prijs,
                Bestemming = new BestemmingDetailViewModel
                {
                    Naam = groepsreis.Bestemming!.Naam,
                    Beschrijving = groepsreis.Bestemming.Beschrijving,
                    Code = groepsreis.Bestemming.Code,
                    MinLeeftijd = groepsreis.Bestemming.MinLeeftijd,
                    MaxLeeftijd = groepsreis.Bestemming.MaxLeeftijd
                },
                Monitoren = monitorenForGroepsreis,
                AllUsers = allUsersWithOpleidingen //All unassigned monitors
            };

            return View(groepsreisViewModel);
        }

        // Helper method to check if a monitor has overlapping dates with the new groepsreis
        private static bool MonitorIsAssignedToConflictingGroepsreis(CustomUser monitor, DateTime groepsreisStartDate, DateTime groepsreisEndDate)
        {
            // Get any existing groepsreis the monitor is assigned to
            var existingGroepsreizen = monitor.Monitors!
                .Where(m => m.Groepsreis != null)
                .Select(m => m.Groepsreis)
                .ToList();

            // Check for overlap with any existing groepsreis the monitor is assigned to
            foreach (var existingGroepsreis in existingGroepsreizen)
            {
                if (existingGroepsreis?.Begindatum < groepsreisEndDate && existingGroepsreis.Einddatum > groepsreisStartDate)
                {
                    // If there is an overlap, the monitor is unavailable
                    return true;
                }
            }

            // Monitor is available if there is no conflict
            return false;
        }

        // assign monitors or hoofdmonitor to groepsreis
        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> AssignMonitor(int groepsreisId, string userId, bool isHoofdMonitor)
        {
            // fetch the groepsreis on id
            var groepsreis = await _uow.GroepsreisRepository.GetByIdAsync(groepsreisId);

            // fetch current user
            var user = await _userManager.FindByIdAsync(userId);

            // fetch monitor to see if it's assigned to groepsreis or not
            var monitor = await _uow.MonitorRepository.GetAsync(m => m.GroepsreisDetailsId == groepsreisId && m.GebruikerId == userId);

            // Email subject and message
            string subject = isHoofdMonitor ? "Je bent als Hoofdmonitor toegewezen" : "Je bent als Monitor toegewezen";
            string message = $"<h1>Je bent geaccepteerd als {(isHoofdMonitor ? "Hoofdmonitor" : "Monitor")} voor de groepsreis.</h1>" +
                             "<p>Bedankt voor je inzet! We wensen je een fijne tijd bij de groepsreis.</p>";

            // If monitor is not found, create a new one
            if (monitor == null)
            {
                monitor = new Monitor
                {
                    GroepsreisDetailsId = groepsreisId,
                    GebruikerId = userId,
                    IsToegewezen = true,
                    IsHoofdMonitor = isHoofdMonitor
                };

                // Add the new monitor to database
                await _uow.MonitorRepository.AddAsync(monitor);
                TempData["success"] = isHoofdMonitor ? "Nieuwe hoofdmonitor toegewezen." : "Nieuwe monitor toegewezen.";
            }
            else
            {
                // If monitor exists, update the existing monitor
                monitor.IsToegewezen = true;
                monitor.IsHoofdMonitor = isHoofdMonitor;
                _uow.MonitorRepository.Update(monitor);
                TempData["success"] = isHoofdMonitor ? "Hoofdmonitor toegewezen." : "Monitor toegewezen.";
            }

            // save database
            await _uow.SaveChangesAsync();

            // Send the email to the assigned monitor
            await _emailSender.SendEmailAsync(user?.Email!, subject, message);

            return Redirect($"/Groepsreis/Monitors/{groepsreisId}");
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}")]
        public async Task<IActionResult> DeleteMonitor(int groepsreisId, string userId)
        {
            // Get the monitor for the given groepsreis and user
            var monitor = await _uow.MonitorRepository.GetAsync(m => m.GroepsreisDetailsId == groepsreisId && m.GebruikerId == userId);

            if (monitor == null)
            {
                // If monitor is not found show error.
                TempData["error"] = "Monitor not found for this groepsreis.";
                return Redirect($"/Groepsreis/Monitors/{groepsreisId}");
            }

            // Delete monitor and save database
            _uow.MonitorRepository.Delete(monitor);
            await _uow.SaveChangesAsync();

            TempData["success"] = (monitor.IsHoofdMonitor ? "Hoofdmonitor" : "Monitor") + " is succesvol verwijderd van groepsreis.";


            // Redirect to the same page to refresh the list of monitors
            return Redirect($"/Groepsreis/Monitors/{groepsreisId}");
        }

        // Helper method to populate the CreateGroepsreisViewModel
        private async Task<CreateGroepsreisViewModel> GetCreateGroepsreisViewModel(CreateGroepsreisViewModel existingModel)
        {
            var bestemmingen = await _uow.BestemmingRepository.GetAllAsync();
            var activiteiten = await _uow.ActiviteitRepository.GetAllAsync();

            existingModel ??= new CreateGroepsreisViewModel
                {
                    NieuweBestemmingBeschrijving = "",
                    NieuweBestemmingNaam = "",
                };

            existingModel.Bestemmingen = bestemmingen.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = $"{b.Naam} - {b.Code}"
            }).ToList();

            existingModel.Activiteiten = activiteiten.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Naam
            }).ToList();

            return existingModel;
        }
    }
}