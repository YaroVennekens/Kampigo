using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Ziekenfonds_Kampigo_Project.Models;
using Ziekenfonds_Kampigo_Project.ViewModels.Activiteit;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;
using Ziekenfonds_Kampigo_Project.ViewModels.Review;
namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;

        public HomeController(IUnitOfWork unitofwork)
        {
            _uow = unitofwork;
        }

        public async Task<ActionResult> Index()
        {
            var groepsreizen = await _uow.GroepsreisRepository.GetAllWithActivities();
            GroepsreisListViewModel model = new();

            // Haal de reviews op
            var reviews = await _uow.ReviewRepository.GetAllWithNames();

            // Voeg de reviews toe aan het model
            model.Reviews = reviews.Select(r => new ReviewViewModel
            {
                GroepsreisNaam = r.Bestemming.Naam,
                Score = r.Score,
                Tekst = r.Tekst,
                BestemmingId = r.BestemmingId
            }).ToList();

            foreach (var groepsreis in groepsreizen)
            {
                GroepsreisViewModel groepsreisViewModel = new()
                {
                    Id = groepsreis.Id,
                    Naam = groepsreis.Bestemming?.Naam ?? "Groepsreis zonder naam",
                    BeginDatum = groepsreis.Begindatum,
                    EindDatum = groepsreis.Einddatum,
                    Prijs = groepsreis.Prijs,
                    Activiteiten = new ActiviteitListViewModel
                    {
                        Activiteiten = groepsreis.Programmas!
                            .Select(p => p.Activiteit)
                            .Select(a => new ActiviteitViewModel { Naam = a!.Naam })
                            .ToList()
                    }
                };

                if (groepsreisViewModel.BeginDatum >= DateTime.Now)
                    model.Groepsreizen.Add(groepsreisViewModel);
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
