using AutoMapper;
using DataAccess.Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utility;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;

namespace Ziekenfonds_Kampigo_Project.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<CustomUser> _userManager;

        public DashboardController(IUnitOfWork unitofwork, IMapper mapper, UserManager<CustomUser> userManager)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
            _userManager = userManager;
        }


        [Authorize(Roles = $"{SD.Role_Beheerder},{SD.Role_Verantwoordelijke}")]
        [HttpGet]
        public async Task<IActionResult> AankomendeReizen()
        {
            var upcomingGroepsreizen = await _unitOfWork.GroepsreisRepository.GetUpcomingGroepsreizenAsync();

            var viewModel = upcomingGroepsreizen.Select(g => new UpcomingGroepsreizenOverviewViewModel
            {
                BestemmingNaam = g.Bestemming!.Naam,
                BeginDatum = g.Begindatum,
                EindDatum = g.Einddatum,
                AantalDeelnemers = g.Deelnemers?.Count ?? 0,
                AantalMonitors = g.Monitors?.Count ?? 0,
                HoofdmonitorNamen = g.Monitors?
                                    .Where(m => m.IsHoofdMonitor)
                                    .Select(m => m.Gebruiker!.Voornaam)
                                    .ToList() ?? []
            }).ToList();

            return View(viewModel);
        }

        [Authorize(Roles = $"{SD.Role_Deelnemer}, {SD.Role_Monitor}, {SD.Role_Hoofdmonitor}")]
        [HttpGet]
        public async Task<IActionResult> BeschikbareGroepsreizen()
        {
            var availableGroepsreizen = await _unitOfWork.GroepsreisRepository.GetAvailableGroepsreizenAsync();

            // map groepsreizen to viewmodel
            var model = new GroepsreisBestemmingActiviteitenListViewModel
            {
                Groepsreizen = _mapper.Map<List<GroepsreisBestemmingActiviteitenViewModel>>(availableGroepsreizen)
            };
            return View(model);
        }

        [Authorize(Roles = $"{SD.Role_Monitor}, {SD.Role_Hoofdmonitor}, {SD.Role_Deelnemer}")]
        [HttpGet]
        public async Task<IActionResult> RegisteredGroepsreizen()
        {
            var user = await _userManager.GetUserAsync(User);

            // Fetch registered groepsreizen for the current user where a child is registered
            var registeredDeelnemersFromDb = await _unitOfWork.DeelnemerRepository.GetRegisteredDeelnemersForUserAsync(user?.Id!);

            // Map the data to the ViewModel
            var viewModel = registeredDeelnemersFromDb.Select(d => new RegisteredGroepsreisViewModel
            {
                Id = d.Groepsreis!.Id,
                Naam = d.Groepsreis.Bestemming!.Naam,
                BeginDatum = d.Groepsreis.Begindatum,
                EindDatum = d.Groepsreis.Einddatum,
                Prijs = d.Groepsreis.Prijs,
                Opmerking = d.Opmerking,
                KindNaam = $"{d.Kind.Voornaam} {d.Kind.Naam}"
            }).ToList();

            return View(viewModel);
        }

        [Authorize(Roles = $"{SD.Role_Monitor}, {SD.Role_Hoofdmonitor}")]
        [HttpGet]
        public async Task<IActionResult> MonitorReizen()
        {
            var userId = _userManager.GetUserId(User);

            var monitoredGroepsreizen = await _unitOfWork.MonitorRepository.GetMonitoredGroepsreizenForUserAsync(userId!);

            // Map the data to the ViewModel
            var viewModel = monitoredGroepsreizen.Select(g => new MijnMonitoredGroepsreisViewModel
            {
                Id = g.Groepsreis!.Id,                             
                Naam = g.Groepsreis.Bestemming!.Naam,              
                BeginDatum = g.Groepsreis.Begindatum,                      
                EindDatum = g.Groepsreis.Einddatum,                          
                Prijs = g.Groepsreis.Prijs,                                 
                IsHoofdMonitor = g.IsHoofdMonitor,                          
                IsToegewezen = g.IsToegewezen
            }).ToList();

            return View(viewModel);
        }

    }
}
