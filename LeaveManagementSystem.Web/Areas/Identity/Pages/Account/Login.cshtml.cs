// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Humanizer;
using Azure;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Packaging.Signing;
using System.Security.Policy;
using System.Diagnostics.Metrics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using NuGet.Protocol.Plugins;
using System.Net.Sockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace LeaveManagementSystem.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        //bind model to the form
        //Input is the model for the form
        [BindProperty] 
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

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
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        //do these things before the page loads
        public async Task OnGetAsync(string returnUrl = null)
        {
            /* If there's an error message from a previous login attempt or redirect,
            it's added to the model state,
            so it can be displayed on the page.*/
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            //If no returnUrl is provided, default it to the homepage (~/).
            //empty out the claims principle
            returnUrl ??= Url.Content("~/");


            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            
            /*Loads external login providers(e.g., Google, Facebook)
            that will be shown as login options.*/
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        //form gets submitted
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //defaults to homepage if no return URL is given.
            returnUrl ??= Url.Content("~/");

            /*Load external providers again
            (some Razor Pages need this info even after a failed login attempt).*/
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            /*Checks if the email / password input is valid(e.g., not empty, correct format).*/
            if (ModelState.IsValid)
            {
               /* lockoutOnFailure: false means incorrect attempts won't lock the account 
                (can be changed to true if you want security lockout behavior).*/

                //_signInManager.PasswordSignInAsync to check the user’s credentials.
                //Input.Email
                //(model is bound to the form so we dont need to pass in from parameter)
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                /*Logs success and redirects the user to their intended page*/
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);//default is homepage
                }

                /*Redirects user to a 2FA page to complete login.*/
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                /*  If the account is locked(too many failed attempts), redirect to a lockout info page.*/
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                 //username & password incorrect
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

           /* If validation failed, show the form again(with validation errors)*/
            return Page();
        }
    }
}
/*what return Page(); does?
 * it does not run OnGetAsync() again
Razor Pages:
Keeps the form input values (e.g., Input.Email, Input.RememberMe)
Keeps the ModelState, including validation or login errors
Re-renders the original .cshtml page
Uses validation tags like <span asp-validation-for="..."> and
<div asp-validation-summary="All"> to show error messages*/