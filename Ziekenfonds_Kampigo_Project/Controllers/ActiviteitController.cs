using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class ActiviteitController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        //private readonly UserManager<CustomUser> _userManager;
        //private CustomUser? _currentUser;

        public ActiviteitController(IUnitOfWork unitofwork /*, UserManager<CustomUser> userManager*/)
        {
            _unitOfWork = unitofwork;
            //_userManager = userManager;
        }

        public IActionResult CreateActiviteit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateActiviteit(Activiteit activiteit)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.ActiviteitRepository.AddAsync(activiteit);
                await _unitOfWork.SaveChangesAsync();
                TempData["success"] = "Activiteit is succesvol aangemaakt.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Er is iets misgegaan.";
            return View(activiteit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activiteit>>> Index()
        {
            return View(await _unitOfWork.ActiviteitRepository.GetAllAsync());
        }

        //public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    // Retrieve the current user asynchronously and store it in _currentUser
        //    _currentUser = await _userManager.GetUserAsync(User);

        //    // Proceed with the action execution
        //    await base.OnActionExecutionAsync(context, next);
        //}

        [HttpGet]
        public async Task<IActionResult> EditActiviteit(int id)
        {
            Activiteit? activiteit = await _unitOfWork.ActiviteitRepository.GetByIdAsync(id);

            if (activiteit == null)
            {
                TempData["error"] = "Activiteit niet gevonden.";
                return View();
            }

            return View(activiteit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActiviteit(Activiteit viewModel)
        {
           
            if (ModelState.IsValid)
            {
                Activiteit? activiteit = await _unitOfWork.ActiviteitRepository.GetByIdAsync(viewModel.Id);

                if (activiteit == null )
                {
                    TempData["error"] = "Activiteit niet gevonden.";
                    return View();
                }

                activiteit.Naam = viewModel.Naam;
                activiteit.Beschrijving = viewModel.Beschrijving;

                _unitOfWork.ActiviteitRepository.Update(activiteit);
                await _unitOfWork.SaveChangesAsync();
                TempData["success"] = "Activiteit is succesvol bewerkt.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Er is iets misgegaan.";
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Activiteit>> DeleteActiviteit(int id)
         {
             Activiteit? activiteit = await _unitOfWork.ActiviteitRepository.GetByIdAsync(id);

             if (activiteit == null )
             {
                TempData["error"] = "Activiteit niet gevonden.";
                return View();
             }

             _unitOfWork.ActiviteitRepository.Delete(activiteit);
             await _unitOfWork.SaveChangesAsync();
            TempData["success"] = "Activiteit is succesvol verwijderd.";
            return RedirectToAction("Index");
         }
    }
}
