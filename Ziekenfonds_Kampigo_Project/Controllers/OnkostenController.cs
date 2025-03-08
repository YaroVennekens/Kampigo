using Microsoft.AspNetCore.Mvc;
using Models;
using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Utility;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;

namespace Ziekenfonds_Kampigo_Project.Controllers;


[Authorize(Roles = $"{SD.Role_Verantwoordelijke}, {SD.Role_Beheerder}, {SD.Role_Hoofdmonitor}")]
public class OnkostenController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public OnkostenController(IUnitOfWork unitofwork,  IWebHostEnvironment webHostEnvironment)
    {
        _uow = unitofwork;
        _webHostEnvironment = webHostEnvironment;
    }
    [HttpGet("OnkostOverview")]
    public async Task<IActionResult> Onkosten(int id)
    {
           
        var groepsreis = await _uow.GroepsreisRepository.GetWithOnkostenById(id);

        if (groepsreis == null)
        {
            TempData["error"] = "Groepsreis niet gevonden.";
            return RedirectToAction("Index"); 
        }
       
            
        var model = new OnkostenViewModel
        {
            Groepsreis = groepsreis, 
            GroepsreisId = groepsreis.Id,
              
   
            Onkosten = groepsreis.Onkosten?.Select(o => new OnkostenViewModel
            {
                Id = o.Id,
                Omschrijving = o.Omschrijving,
                Bedag = o.Bedrag,
                DatumOnkost = o.Datum,
                Titel = o.Titel,
                    
                     
            }).ToList() ?? new List<OnkostenViewModel>(),
        };

        return View(model);
    }
    [HttpGet("DetailOnkosten/{id}")]
   
    public async Task<IActionResult> DetailOnkost(int id)
    {
        var onkost = await _uow.OnkostenRepository.GetByIdAsync(id);

        if (onkost == null)
        {
            return NotFound($"The onkost with ID {id} was not found.");
        }

        var viewModel = new OnkostenViewModel()
        {
            Id = onkost.Id,
            Titel = onkost.Titel,
            Bedag = onkost.Bedrag,
            Foto = onkost.Foto,
            Omschrijving = onkost.Omschrijving,
           };

        return View(viewModel);
    }


    [HttpPost("Add")]
    public async Task<IActionResult> AddOnkosten(int groepsreisId,int onkostId, string description, string photourl, float amount, DateTime date,
        string title, List<IFormFile> photos)
    {
        var groepsreis = await _uow.GroepsreisRepository.GetWithOnkostenById(groepsreisId);

        if (groepsreis == null)
        {
            TempData["error"] = $"Groepsreis niet gevonden: {groepsreisId}.";
            return RedirectToAction("Index", "Groepsreis");
        }

        Onkosten? onkost;
        
         onkost = new Onkosten
        {
            Id = onkostId,
            GroepsreisId = groepsreisId,
            Omschrijving = description,
            Bedrag = amount,
            Datum = date,
            Titel = title,
            Foto = photourl
            
        };
         
        if (photos != null && photos.Count > 0)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string photoFolderPath = Path.Combine(wwwRootPath, "img", "onkost");

            if (!Directory.Exists(photoFolderPath))
            {
                Directory.CreateDirectory(photoFolderPath);
            }

            // Assign the Groepsreis property to the onkost object
            onkost.Groepsreis = groepsreis;

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

                    onkost.Foto = fileName;
                    
                   
                }
            }
        }


        await _uow.OnkostenRepository.AddAsync(onkost);
        await _uow.SaveChangesAsync();

        TempData["success"] = "Onkosten succesvol toegevoegd!" + onkost.Foto;
      
        return RedirectToAction("Onkosten", "Onkosten", new { id = groepsreisId });
    }

    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Onkosten>> DeleteOnkost(int id)
    {
        Onkosten? onkost = await _uow.OnkostenRepository.GetByIdAsync(id);

        if (onkost == null)
        {
            TempData["error"] = "Onkost niet gevonden in de database.";
            return RedirectToAction("Index", "Groepsreis"); 
        }

    
         _uow.OnkostenRepository.Delete(onkost);
        await _uow.SaveChangesAsync();
        
        TempData["success"] = "Onkost is succesvol verwijderd.";
        return RedirectToAction("Onkosten", "Onkosten", new { id = onkost.GroepsreisId });
    }
}