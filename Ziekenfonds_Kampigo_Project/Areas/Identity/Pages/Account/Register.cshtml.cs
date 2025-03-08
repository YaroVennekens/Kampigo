// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Models;
using Utility;

namespace Ziekenfonds_Kampigo_Project.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<CustomUser> _userStore;
        private readonly IUserEmailStore<CustomUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<CustomUser> userManager,
            IUserStore<CustomUser> userStore,
            SignInManager<CustomUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<CustomUser>)GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [EmailAddress]
            [Display(Name = "Email")]
            [Required(ErrorMessage = "Email is verplicht.")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Wachtwoord")]
            [Required(ErrorMessage = "Wachtwoord is verplicht.")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Bevestig wachtwoord")]
            [Compare("Password", ErrorMessage = "Het wachtwoord en de bevestiging komen niet overeen.")]
            public string ConfirmPassword { get; set; }
            
            [Required(ErrorMessage = "Voornaam is verplicht.")]
            [StringLength(100, ErrorMessage = "Voornaam kan maximaal 100 karakters bevatten.")]
            [PersonalData]
            public string Voornaam { get; set; }
            
            
            [Required(ErrorMessage = "Naam is verplicht.")]
            [StringLength(100, ErrorMessage = "Naam kan maximaal 100 karakters bevatten.")]
            [PersonalData]
            public required string Naam { get; set; }
            
            // Huisnummer with maximum 10 character validation, is required and is marked as personalData
            [Required(ErrorMessage = "Huisnummer is verplicht.")]
            [StringLength(10, ErrorMessage = "Huisnummer kan maximaal 10 karakters bevatten.")]
            [PersonalData]
            public required string Huisnummer { get; set; }

            // Gemeente with maximum 50 character validation, is required and is marked as personalData
            [Required(ErrorMessage = "Gemeente is verplicht.")]
            [StringLength(100, ErrorMessage = "Gemeente kan maximaal 100 karakters bevatten.")]
            [PersonalData]
            public required string Gemeente { get; set; }

            // Postcode with maximum 10 character validation, is required and is marked as personalData
            [Required(ErrorMessage = "Postcode is verplicht.")]
            [StringLength(10, ErrorMessage = "Postcode kan maximaal 10 karakters bevatten.")]
            [PersonalData]
            public required string Postcode { get; set; }
            
            [Required(ErrorMessage = "Straat is verplicht.")]
            [StringLength(100, ErrorMessage = "Straat kan maximaal 100 karakters bevatten.")]
            [PersonalData]
            public required string Straat { get; set; }
            
            [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
            [StringLength(20, ErrorMessage = "Telefoon kan maximaal 20 karakters bevatten.")]
            [PersonalData]
            public required string TelefoonNummer { get; set; }
            
            
            [Required(ErrorMessage = "Geboortedatum is verplicht.")]
            [MaxDate(ErrorMessage = "Geboortedatum kan niet in de toekomst liggen.")]
            [PersonalData]
            public DateTime GeboorteDatum { get; set; } 
          
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            // if role gebruiker doesn't exist then create all the roles.
            if (!_roleManager.RoleExistsAsync(SD.Role_Deelnemer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Deelnemer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Monitor)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Hoofdmonitor)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Verantwoordelijke)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Beheerder)).GetAwaiter().GetResult();
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

      public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
    returnUrl ??= Url.Content("~/");
    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    
    if (ModelState.IsValid)
    {
        var user = CreateUser();

        
        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

       
        user.Voornaam = Input.Voornaam;
        user.Naam = Input.Naam;
        user.GeboorteDatum = Input.GeboorteDatum;   
        user.Huisnummer = Input.Huisnummer;
        user.Gemeente = Input.Gemeente;
        user.Postcode = Input.Postcode;
        user.Straat = Input.Straat;
        user.TelefoonNummer = Input.TelefoonNummer;
        user.IsActief = true;
        user.IsMonitor = false;

        
        var result = await _userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
                    TempData["success"] = "Account succesvol geregistreerd.";

                    _logger.LogInformation("User created a new account with password.");

            // Get user with an admin role
            var usersInAdminRole = await _userManager.GetUsersInRoleAsync(SD.Role_Beheerder);

            // If no user found with admin role, then the new user will get an admin role
            if (usersInAdminRole == null || !usersInAdminRole.Any())
            {
                // Assign all roles to Admin role
                await _userManager.AddToRoleAsync(user, SD.Role_Beheerder);
                _logger.LogInformation("Assigned Admin role to the user as no admins exist.");
            }
            else
            {
                // Otherwise, assign the Gebruiker role to user
                await _userManager.AddToRoleAsync(user, SD.Role_Deelnemer);
                _logger.LogInformation("Assigned default user role to the user.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code, returnUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Bevestig uw email",
                $"Bevestig aub uw account door <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier te klikken</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    // If we got this far, something failed, redisplay form
    return Page();
}

        private CustomUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<CustomUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Kan geen instantie maken van '{nameof(CustomUser)}'. " +
                    $"Ensure that '{nameof(CustomUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<CustomUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("De UI heeft een userstore nodig.");
            }
            return (IUserEmailStore<CustomUser>)_userStore;
        }
    }
}
