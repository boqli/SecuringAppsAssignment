using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using WebApplication1.Models;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.Utility;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IStudentTeacherService _studentTeacherService;
        private readonly StudentTeacherViewModel data = new StudentTeacherViewModel();

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger, 
            IStudentTeacherService studentTeacherService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _studentTeacherService = studentTeacherService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Address { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public string GeneratePassword()
        {
            int length = 10;
            const string valid = "abcdefghijklmnopqrstuvwxyz1234567890!@$%&*_ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@$%&*_"; 
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            string password = GeneratePassword();

            var pubpriKeys = Encryption.GenerateAsymmetricKeys();
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, Address= Input.Address, PrivateKey = pubpriKeys.PrivateKey,PublicKey = pubpriKeys.PublicKey };
                var result = await _userManager.CreateAsync(user, password);
                var ip = HttpContext.Connection.RemoteIpAddress;
                if (result.Succeeded)
                {
                    
                    _logger.LogInformation("-->" + User.Identity.Name + " has successfully created a new account with a password from this Ip address " + ip);


                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //StudentTeacher table
                    data.teacherEmail = User.Identity.Name;
                    data.studentEmail = Input.Email;
                    _studentTeacherService.AddMember(data);

                    /*
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    */

                    // Send email to new user
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Account Information", "projecttest119@gmail.com"));
                    message.To.Add(new MailboxAddress("New Account", Input.Email));
                    message.Subject = "Your new school account is set up!";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Your school information\n\n" +
                        "Full name: "+Input.FirstName + " " + Input.LastName + "\n" +
                        "Address: "+Input.Address + "\n" +
                        "Password: " + password+
                        "\n\nDO NOT SHARE YOUR PASSWORD"
                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("projecttest119@gmail.com", "visualstudio");
                        client.Send(message);
                        client.Disconnect(true);
                    }

                    
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //Do not sign in the newly created user
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    
                }
                
                _logger.LogError("-->" + User.Identity.Name + " could not create an account from this Ip address " + ip);

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
