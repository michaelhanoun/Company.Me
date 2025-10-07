using Company.BLL.Interfaces;
using Company.BLL.Models;
using Company.DAL.Entites;
using Dashboard.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
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
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, logInDto.Password);
                    if (flag&&await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, logInDto.Password, logInDto.RememberME, true);
                        if (result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "Your Account is Locked");
                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                    }
                }
            }
            ModelState.AddModelError("", "InValid login");
            return View(logInDto);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action(nameof(ResetPassword), "Auth", new { email = user.Email, token }, Request.Scheme);
                    bool IsSent = await _emailSender.SendEmail(new EmailMessage(dto.Email, "Forget Password Mail", url));
                    if (!IsSent)
                        ModelState.AddModelError("", "Email could not be sent. Please try again.");
                    return RedirectToAction(nameof(CheckYourInbox), "Auth");

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
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
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
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn), "Auth");
                }
            }
            return View(dto);
        }
    }
}
