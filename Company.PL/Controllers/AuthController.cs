using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using Company.DAL.Entites;
using Company.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User>userManager,SignInManager<User>signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            return View(logInDto);
        }
        [HttpGet] public async Task<IActionResult> Logout() {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
    }
}
