using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.BLL.Models;
using Company.DAL.Entites;
using Company.PL.Dtos;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Company.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ITwilioService _twilioService;

        public AuthController(UserManager<User>userManager,SignInManager<User>signInManager,IEmailSender emailSender,ITwilioService twilioService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _twilioService = twilioService;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterDto registerDto)
        {
            if(ModelState.IsValid)
            {
                var userByEmail = _userManager.FindByEmailAsync(registerDto.Email);
                
                if(userByEmail is not null)
                {

                    var user = new User()
                    {
                        Email = registerDto.Email,
                        FName = registerDto.FirstName,
                        LName = registerDto.LastName,
                        UserName = registerDto.UserName
                    };
                    var result = await _userManager.CreateAsync(user, registerDto.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));
                    ModelState.AddModelError(string.Empty, string.Join(",", result.Errors.Select(E => E.Description)));
                }
                ModelState.AddModelError(string.Empty, "this user name is already use for another account");
            }
            return View(registerDto);
        }
        [HttpGet]
        public IActionResult SignIn()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LogInDto logInDto)
        {
            if (ModelState.IsValid)
            {
               var user = await _userManager.FindByEmailAsync(logInDto.Email);
                if(user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, logInDto.Password);
                    if(flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, logInDto.Password, logInDto.RememberME,true);
                        if (result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "Your Account is Locked");
                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index),"Home");
                      
                    }
                }
            }
            ModelState.AddModelError("", "InValid login");
            return View(logInDto);
        }
        [HttpGet] public async Task<IActionResult> Logout() {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordDto dto)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user is not null) {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action(nameof(ResetPassword),"Auth",new { email = user.Email , token },Request.Scheme);
                    bool IsSent = await _emailSender.SendEmail(new EmailMessage(dto.Email,"Forget Password Mail",url));
                    if (!IsSent)
                        ModelState.AddModelError("", "Email could not be sent. Please try again.");
                    return RedirectToAction(nameof(CheckYourInbox),"Auth");

                }
                ModelState.AddModelError(string.Empty, "there is no account With this email");
            }
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordSms(ForgetPasswordDto dto)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user is not null) {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action(nameof(ResetPassword),"Auth",new { email = user.Email , token },Request.Scheme);
                    var sms = new Sms()
                    {
                        To = user.PhoneNumber,
                        Body = url
                    };
                    await _twilioService.SendSms(sms);
                    return RedirectToAction(nameof(CheckYourPhone),"Auth");

                }
                ModelState.AddModelError(string.Empty, "there is no account With this email");
            }
            return View(dto);
        }
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        public IActionResult CheckYourPhone()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string ?? "";
                var token = TempData["token"] as string ?? "";
                var user  = await _userManager.FindByEmailAsync(email);
                if(user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
                    if(result.Succeeded)
                        return RedirectToAction(nameof(SignIn), "Auth");
                }
            }
            return View(dto);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(SignIn));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {

                return RedirectToAction("Index", "Home");
            }


            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = info.Principal.Claims
                  .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                    Email = email,
                    EmailConfirmed = true,
                    FName = info.Principal.FindFirstValue("given_name")
                };
                await _userManager.CreateAsync(user);
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");

        }
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Auth");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
    }
}
