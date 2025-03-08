using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;
using Ziekenfonds_Kampigo_Project.ViewModels.Review;
using DataAccess.Data.UnitOfWork;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<CustomUser> _userManager;

        public ReviewController(IUnitOfWork unitOfWork, UserManager<CustomUser> userManager)
        {
            _uow = unitOfWork;
            _userManager = userManager;
        }

        // Actie voor het ophalen van groepsreizen voor de index-pagina (zoals eerder)

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isMonitor = await _userManager.IsInRoleAsync(user, "MONITOR");
            var isHoofdmonitor = await _userManager.IsInRoleAsync(user, "HOOFDMONITOR");


            if (isMonitor || isHoofdmonitor)
            {
                var groepsreizen = await _uow.MonitorRepository.GetMonitoredGroepsreizenForUserAsync(user.Id);

                // Haal groepsreizen van de kinderen van de gebruiker op
                var groepsreizenKinderen = await _uow.DeelnemerRepository.GetRegisteredDeelnemersForUserAsync(user.Id);

                // Combineer de groepsreizen van monitor en kinderen door ze naar hetzelfde type te transformeren
                var gecombineerdeGroepsreizen = groepsreizen
                    .Select(gr => new { Groepsreis = gr.Groepsreis }) // Zet de reizen om naar hetzelfde type als de kinderen
                    .Union(groepsreizenKinderen
                        .Select(gr => new { Groepsreis = gr.Groepsreis })) // Zet de reizen van kinderen ook om naar hetzelfde type
                    .GroupBy(gr => gr.Groepsreis.Id) // Groep op basis van de Groepsreis.Id
                    .Select(grp => grp.First()) // Selecteer alleen de eerste van elke groep (verwijdert duplicaten)
                    .ToList();

                if (!gecombineerdeGroepsreizen.Any())
                {
                    return View(new List<GroepsreisViewModel>());
                }

                // Filter de groepsreizen waarvoor de gebruiker al een review heeft geschreven
                var groepsreizenZonderReview = new List<GroepsreisViewModel>();

                foreach (var groepsreis in gecombineerdeGroepsreizen)
                {
                    // Controleer of de gebruiker al een review heeft voor deze groepsreis
                    var existingReview = await _uow.ReviewRepository.GetReviewsByUserAndGroepsreisAsync(user.Id, groepsreis.Groepsreis.Id);

                    // Voeg alleen de groepsreis toe als er nog geen review is
                    if (existingReview == null && !groepsreizenZonderReview.Any(gr => gr.Id == groepsreis.Groepsreis.Id))
                    {
                        groepsreizenZonderReview.Add(new GroepsreisViewModel
                        {
                            Id = groepsreis.Groepsreis.Id,
                            Naam = groepsreis.Groepsreis.Bestemming.Naam,
                            BeginDatum = groepsreis.Groepsreis.Begindatum,
                            EindDatum = groepsreis.Groepsreis.Einddatum
                        });
                    }
                }
                var gesorteerdeGroepsreizen = groepsreizenZonderReview
                .OrderBy(gr => gr.EindDatum) // Daarna sorteren op EindDatum
                .ToList();

                return View(gesorteerdeGroepsreizen);
            }
            else
            {
                var groepsreizen = await _uow.DeelnemerRepository.GetRegisteredDeelnemersForUserAsync(user.Id);

                if (groepsreizen == null || !groepsreizen.Any())
                {
                    return View(new List<GroepsreisViewModel>());
                }

                // Filter de groepsreizen waarvoor de gebruiker al een review heeft geschreven
                var groepsreizenZonderReview = new List<GroepsreisViewModel>();

                foreach (var groepsreis in groepsreizen)
                {
                    // Controleer of de gebruiker al een review heeft voor deze groepsreis
                    var existingReview = await _uow.ReviewRepository.GetReviewsByUserAndGroepsreisAsync(user.Id, groepsreis.Groepsreis.Id);


                    // Voeg alleen de groepsreis toe als er nog geen review is en als de groepsreis nog niet in de lijst staat
                    if (existingReview == null && !groepsreizenZonderReview.Any(gr => gr.Id == groepsreis.Groepsreis.Id))
                    {
                        groepsreizenZonderReview.Add(new GroepsreisViewModel
                        {
                            Id = groepsreis.Groepsreis.Id,
                            Naam = groepsreis.Groepsreis.Bestemming.Naam,
                            BeginDatum = groepsreis.Groepsreis.Begindatum,
                            EindDatum = groepsreis.Groepsreis.Einddatum
                        });
                    }
                }

                var gesorteerdeGroepsreizen = groepsreizenZonderReview
                .OrderBy(gr => gr.EindDatum) // Daarna sorteren op EindDatum
                .ToList();

                return View(gesorteerdeGroepsreizen);
            }
         

        }


        // Actie voor het weergeven van de pagina om een review te schrijven voor een specifieke groepsreis

        public async Task<IActionResult> ReviewSchrijven(int groepsreisId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            
            var groepsreis = await _uow.GroepsreisRepository.GetWithDetailsById(groepsreisId);

            if (groepsreis == null)
            {
                return NotFound();
            }

            var model = new ReviewViewModel
            {
                GroepsreisId = groepsreisId,
                BestemmingId = groepsreis.Bestemming.Id,
                GroepsreisNaam = groepsreis.Bestemming.Naam
            };

            return View(model);
        }

        // Actie voor het opslaan van de review (POST-aanroep)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SchrijfReview(ReviewViewModel model)
        {
           

            
            // Check of de modelgegevens geldig zijn
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var review = new Review
                {
                    PersoonId = user.Id,
                    BestemmingId = model.BestemmingId,
                    Score = model.Score,
                    Tekst = model.Tekst
                };

                await _uow.ReviewRepository.AddReviewAsync(review);

                
                await _uow.CompleteAsync();

                TempData["success"] = "Je review is succesvol opgeslagen!";

                return RedirectToAction("Index"); 
            }

            TempData["error"] = "Er is iets misgegaan bij het opslaan van je review.";  

            return RedirectToAction("ReviewScrijven");

        }

    }
}
