using Microsoft.AspNetCore.Mvc;
using Models;
using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Ziekenfonds_Kampigo_Project.ViewModels.Kind;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class KindController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<CustomUser> _userManager;
        private CustomUser? _currentUser;

        public KindController(IUnitOfWork unitofwork, UserManager<CustomUser> userManager)
        {
            _unitOfWork = unitofwork;
            _userManager = userManager;
        }

        public IActionResult CreateKind()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateKind(Kind kind)
        {
            // Associate the Kind with the logged-in user
            kind.GebruikerId = _currentUser?.Id!;

            // Validate Geboortedatum
            var geboortedatum = kind.Geboortedatum;
            var today = DateTime.Today;
            var minDate = today.AddYears(-21);
            var maxDate = today.AddYears(-6);

            if (geboortedatum < minDate || geboortedatum > maxDate)
            {
                ModelState.AddModelError("Geboortedatum", "Leeftijd moet tussen 6 en 21 jaar zijn.");
            }

            try
            {
                await _unitOfWork.KindRepository.AddAsync(kind);
                await _unitOfWork.SaveChangesAsync();
                TempData["success"] = "Kind is succesvol aangemaakt.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Er is iets misgegaan. Error: {ex.Message}";
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kind>>> Index()
        {
         
            return View(await _unitOfWork.KindRepository.GetAllKinderenOnGebruikerIdAsync(_currentUser?.Id!));
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Retrieve the current user asynchronously and store it in _currentUser
            _currentUser = await _userManager.GetUserAsync(User);

            // Proceed with the action execution
            await base.OnActionExecutionAsync(context, next);
        }


        [HttpGet]
        public async Task<IActionResult> EditKind(int id)
        {
            Kind? kind = await _unitOfWork.KindRepository.GetByIdAsync(id);
            if (kind == null)
            {
                return NotFound();
            }

            var viewmodel = new KindViewModel
            {
                Allergieen = kind.Allergieen,
                Geboortedatum = kind.Geboortedatum,
                Id = kind.Id,
                Medicatie = kind.Medicatie,
                Naam = kind.Naam,
                Voornaam = kind.Voornaam
            };

            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditKind(KindViewModel viewModel)
        {
            var geboortedatum = viewModel.Geboortedatum;
            var today = DateTime.Today;
            var minDate = today.AddYears(-21);
            var maxDate = today.AddYears(-6);

            if (geboortedatum < minDate || geboortedatum > maxDate)
            {
                ModelState.AddModelError("Geboortedatum", "Leeftijd moet tussen 6 en 21 jaar zijn.");
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Er is iets misgegaan";
                return View(viewModel);

            }

            var kind = await _unitOfWork.KindRepository.GetByIdAsync(viewModel.Id);

            if (kind == null)
            {
                TempData["error"] = "Kind niet gevonden in de database.";
                return View(viewModel);
            }

            kind.Allergieen = viewModel.Allergieen;
            kind.Geboortedatum = viewModel.Geboortedatum;
            kind.Medicatie = viewModel.Medicatie;
            kind.Naam = viewModel.Naam;
            kind.Voornaam = viewModel.Voornaam;

            _unitOfWork.KindRepository.Update(kind);
            await _unitOfWork.SaveChangesAsync();
            TempData["success"] = "Kind is succesvol gewijzigd.";
            return RedirectToAction(nameof(Index));
        }

        // Add the DeleteKind action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Kind>> DeleteKind(int id)
        {
            Kind? kind = await _unitOfWork.KindRepository.GetByIdAsync(id);

            if (kind == null)
            {
                TempData["error"] = "Kind niet gevonden in de database.";
                return View();
            }

            _unitOfWork.KindRepository.Delete(kind);
            await _unitOfWork.SaveChangesAsync();
            TempData["success"] = "Kind is succesvol verwijderd.";
            return RedirectToAction("Index");
        }
    }
}