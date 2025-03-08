using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ziekenfonds_Kampigo_Project.API
{
    [Authorize("Hoofdmonitor")]
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ApiController(IUnitOfWork unitofwork)
        {
            _uow = unitofwork;
        }

        [HttpGet("groepsreizen")]
        public async Task<IActionResult> GetGroepsreizenBijMonitor(string userId)
        {
            var groepsreizen = await _uow.GroepsreisRepository.GetMonitoredGroepsreizenForUserAsync(userId);

            if (groepsreizen == null || !groepsreizen.Any())
            {
                return NotFound(new { message = "Geen groepsreizen gevonden voor deze monitor." });
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(groepsreizen, options);
        }

        [HttpGet("deelnemers")]
        public async Task<IActionResult> GetDeelnemersBijGroepsreis(int groepsreisId)
        {
            var groepsreisMetDeelnemers = await _uow.GroepsreisRepository.GetWithBestemmingDeelnemersAndTheirOuderByIdAsync(groepsreisId);

            if (groepsreisMetDeelnemers == null)
            {
                return NotFound(new { message = "Geen groepsreis teruggevonden. Probeer opnieuw." });
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(groepsreisMetDeelnemers, options);
        }

        [HttpGet("onkosten")]
        public async Task<IActionResult> GetOnkostenBijGroepsreis(int groepsreisId)
        {
            var groepsreis = await _uow.GroepsreisRepository.GetWithOnkostenById(groepsreisId);

            if (groepsreis == null)
            {
                return NotFound(new { message = "Geen groepsreis teruggevonden. Probeer opnieuw." });
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(groepsreis, options);
        }

        [HttpGet("onkosten/{id}")]
        public async Task<IActionResult> DetailOnkost(int id)
        {
            var onkost = await _uow.OnkostenRepository.GetByIdAsync(id);

            if (onkost == null)
            {
                return NotFound(new { message = "Geen onkosten teruggevonden. Probeer opnieuw." });
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(onkost, options);
        }

        [HttpPost("onkosten/add")]
        public async Task<IActionResult> AddOnkosten(Onkosten onkost, int groepsreisId)
        {
            var groepsreis = await _uow.GroepsreisRepository.GetWithOnkostenById(groepsreisId);

            if (groepsreis == null)
            {
                return NotFound(new { message = "Geen groepsreis teruggevonden. Probeer opnieuw." });
            }

            await _uow.OnkostenRepository.AddAsync(onkost);
            await _uow.SaveChangesAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(onkost, options);
        }

        [HttpDelete("onkosten/delete/{id}")]
        public async Task<IActionResult> DeleteOnkost(int id)
        {
            var onkost = await _uow.OnkostenRepository.GetByIdAsync(id);

            if (onkost == null)
            {
                return NotFound(new { message = "Onkost niet gevonden in de database." });
            }

            _uow.OnkostenRepository.Delete(onkost);
            await _uow.SaveChangesAsync();

            return Ok(new { message = "Onkost is succesvol verwijderd." });
        }
    }
}

