// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Ziekenfonds_Kampigo_Project.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        public IndexModel(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefoonnummer")]
            [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
            public string Telefoonnummer { get; set; }

            [Required(ErrorMessage = "Voornaam is verplicht.")]
            [Display(Name = "Voornaam")] public string Voornaam { get; set; }

            [Required(ErrorMessage = "Naam is verplicht.")]
            [Display(Name = "Naam")] public string Naam { get; set; }

            [Required(ErrorMessage = "Straat is verplicht.")]
            [Display(Name = "Straat")] public string Straat { get; set; }

            [Required(ErrorMessage = "Huisnummer is verplicht.")]
            [Display(Name = "Huisnnummer")] public string Huisnummer { get; set; }

            [Required(ErrorMessage = "Postcode is verplicht.")]
            [Display(Name = "Postcode")] public string Postcode { get; set; }

            [Required(ErrorMessage = "Gemeente is verplicht.")]
            [Display(Name = "Gemeente")] public string Gemeente { get; set; }

            [Required(ErrorMessage = "Geboortedatum is verplicht.")]
            [Display(Name = "Geboortedatum")]
            [DataType(DataType.Date)]
            public DateTime Geboortedatum { get; set; }
        }

        private async Task LoadAsync(CustomUser user)
        {
            Username = user.UserName;
            Input = new InputModel
            {
                Telefoonnummer = user.TelefoonNummer,
                Voornaam = user.Voornaam,
                Naam = user.Naam,
                Straat = user.Straat,
                Huisnummer = user.Huisnummer,
                Postcode = user.Postcode,
                Gemeente = user.Gemeente,
                Geboortedatum = user.GeboorteDatum
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Kan gebruiker met ID '  {_userManager.GetUserId(User)}  ' niet laden.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Kan gebruiker met ID '  {_userManager.GetUserId(User)}  ' niet laden.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }



            user.TelefoonNummer = Input.Telefoonnummer;
            user.Voornaam = Input.Voornaam;
            user.Naam = Input.Naam;
            user.Straat = Input.Straat;
            user.Huisnummer = Input.Huisnummer;
            user.Postcode = Input.Postcode;
            user.Gemeente = Input.Gemeente;
            user.GeboorteDatum = Input.Geboortedatum;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                TempData["error"] = "Er liep iets mis. Probeer opnieuw.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["success"] = "Account is aangepast.";
            return RedirectToAction("Index", "Kind");
        }
    }
}
