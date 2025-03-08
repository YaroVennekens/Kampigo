using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Ziekenfonds_Kampigo_Project.ViewModels.DeelnemersInfo;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class DeelnemersInfoController : Controller
    {

        private readonly IUnitOfWork _uow;
        private readonly UserManager<CustomUser> _userManager;

        public DeelnemersInfoController(IUnitOfWork unitofwork, UserManager<CustomUser> userManager)
        {
            _uow = unitofwork;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("DeelnemersInfo/DeelnemersLijst{id}")]
        public async Task<IActionResult> DeelnemersLijst(int id)
        {
            var groepsreis = await _uow.GroepsreisRepository.GetWithBestemmingDeelnemersAndTheirOuderByIdAsync(id);
            if (groepsreis == null)
            {
                return NotFound();
            }

            var deelnemersLijst = groepsreis.Deelnemers.Select(d => new DeelnemersInfoViewModel
            {
                Id = d.Id,
                Naam = d.Kind.Naam,
                Voornaam = d.Kind.Voornaam,
                Leeftijd = DateTime.Now.Year - d.Kind.Geboortedatum.Year,
                TelefoonnummerOuderVoogd = d.Kind.Gebruiker.TelefoonNummer,
                MedischeGegevens = d.Kind.Medicatie,
            }).ToList();

            return View(deelnemersLijst);
        }


        [HttpGet, Route("DeelnemersInfo/MonitorsLijst{id}")]
        public async Task<IActionResult> MonitorsLijst(int id)
        {
            var groepsreis = await _uow.GroepsreisRepository.GetWithDetailsAndMonitorenDetailsByIdAsync(id);
            if (groepsreis == null)
            {
                return NotFound();
            }

            var monitorsLijst = groepsreis.Monitors.Select(d => new MonitorsInfoViewModel
            {
                Id = d.Id,
                Naam = d.Gebruiker.Naam,
                Voornaam = d.Gebruiker.Voornaam,
                Leeftijd = DateTime.Now.Year - d.Gebruiker.GeboorteDatum.Year,
                Email = d.Gebruiker.Email,
                Telefoonnummer = d.Gebruiker.TelefoonNummer,
            }).ToList();

            return View(monitorsLijst);
        }





    }
}
