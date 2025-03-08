using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    public class FotoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FotoController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int fotoId, int groepsreisId)
        {
            // Retrieve the Foto entity from the database
            var foto = await _unitOfWork.FotoRepository.GetByIdAsync(fotoId);

            if (foto == null)
            {
                return NotFound();
            }

            // Construct the file path
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string photoPath = Path.Combine(wwwRootPath, "img", "bestemming", foto.Naam);

            // Remove the Foto entity from the database
            _unitOfWork.FotoRepository.Delete(foto);
            await _unitOfWork.SaveChangesAsync();

            // Delete the file from the server
            if (System.IO.File.Exists(photoPath))
            {
                System.IO.File.Delete(photoPath);
            }

            // Redirect to the Edit action for the current Groepsreis
            return RedirectToAction("Edit", "Groepsreis", new { id = groepsreisId });
        }
    }
}
