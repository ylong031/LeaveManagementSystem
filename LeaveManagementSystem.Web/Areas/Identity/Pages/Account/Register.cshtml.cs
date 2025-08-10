// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


using LeaveManagementSystem.Application.Services.LeaveAllocations;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace LeaveManagementSystem.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILeaveAllocationsService _leaveAllocationsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RegisterModel(
            ILeaveAllocationsService leaveAllocationsService,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment hostEnvironment)
        {
            this._leaveAllocationsService = leaveAllocationsService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /*
       you will get null reference when you call Input.Email,Input.Password etc in OnGetAsync 
       if Input is not initialized.You are basically calling null.Email,null.Password
       */
        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();


        public string[] RoleNames { get; set; }
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
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Date Of Birth")]
            public DateOnly DateOfBirth { get; set; }

            [Required]
            public string RoleName { get; set; }

        }

        /* 
        var roles =: Declares a new variable named roles.
        await roleManager.Roles: Asynchronously accesses the list of roles from the database.
        .Select(q => q.Name): From each role object, take only the role's name (e.g., "Employee", "Supervisor").
        q => q.Name is a lambda expression: for each q in the collection, select q.Name.
        .Where(q => q != "Administrator"): Filters out the role named "Administrator". Only roles with a name not equal to "Administrator" are kept.
        .ToArrayAsync(): Converts the filtered list of names into an array, asynchronously.
        */
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var roles = await _roleManager.Roles
                .Select(q => q.Name)
                .Where(q => q != "Administrator")
                .ToArrayAsync();

            RoleNames = roles;

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();//create an instance of application user

                //setting username
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                //setting email
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

    
                /*         
                This ensures that the user object is fully populated with all the necessary data 
                — including fields like FirstName that the database expects to be non - null.*/

                user.DateOfBirth = Input.DateOfBirth;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                //create the user and inserts the registration data into the database.
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (Input.RoleName == Roles.Supervisor)
                    {
                        await _userManager.AddToRolesAsync(user, [Roles.Supervisor, Roles.Employee]);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Employee);

                    }

                    /*           
                    So basically, after we create an account,
                    we use the generated code and user ID to create a callback URL.
                    We send this link to the user by email.
                    When the user clicks it,
                    the system uses the information in the link to verify and confirm their email address.*/

                    var userId = await _userManager.GetUserIdAsync(user);
                    await _leaveAllocationsService.AllocateLeave(userId);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //grab the email template from the wwwroot folder
                    //go to wwwroot/templates/email_layout
                    var emailTemplatePath = Path.Combine(_hostEnvironment.WebRootPath, "templates", "email_layout.html");
                    
                    var template = await System.IO.File.ReadAllTextAsync(emailTemplatePath);

                    //replace the placeholders in the template with actual values
                    var messageBody = template
                        .Replace("{UserName}", $"{Input.FirstName}{Input.LastName}")
                        .Replace("{MessageContent}",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                    //"confirm your email" is the subject of the email
                    //adding the message body to the email
                    //and sending the email
                    await _emailSender.SendEmailAsync(Input.Email,"Confirm your email",messageBody);

                    /* If confirmation is required:  
                     Redirect to confirmation page*/
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        /*Otherwise:
                        Sign in and redirect*/
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // IMPORTANT:
            // When registration fails (e.g., invalid email or weak password),
            // we redisplay the form using return Page().
            // However, non-form-bound properties like RoleNames (used for dropdowns or radio buttons)
            // are not preserved across postbacks because they are not part of the form model.(not bound with asp-for)
            // To prevent a NullReferenceException in the Razor page,
            // we must reload the roles before returning the page

            var roles = await _roleManager.Roles
                .Select(q => q.Name)
                .Where(q => q != "Administrator")
                .ToArrayAsync();

            RoleNames = roles;

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                //create an instance of application user
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
