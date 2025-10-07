using Company.DAL.Entites;
using Dashboard.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<User> userManager , RoleManager<IdentityRole>roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index(string searchInp)
        {
            List<UserDto> users = _userManager.Users.Where(R => (searchInp.IsNullOrEmpty() || R.UserName.ToLower().Contains(searchInp.ToLower()))).Select( C => new UserDto() { Id = C.Id, FName = C.FName, LName = C.LName,UserName = C.UserName,Roles = _userManager.GetRolesAsync(C).GetAwaiter().GetResult()}).ToList();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            ViewData["id"] = id;
            return View(viewName, new UserDetailsDto() { UserName = user.UserName,FName = user.FName,LName=user.LName,Roles = await _userManager.GetRolesAsync(user) });
        }
        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {
            if (id is null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            var userDto = new UserToUpdateDto()
            {
                UserName = user.UserName,
                FName = user.FName,
                LName = user.LName,
            };
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                userDto.UserRoleDtos.Add(new UserRoleDto
                {
                    Role = item.Name,
                    IsUserInRole = userRoles.Contains(item.Name)
                });
            }
            return View(userDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string id, UserToUpdateDto model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            user.UserName = model.UserName;
            user.FName = model.FName;
            user.LName = model.LName;
            var existingUserRoles = await _userManager.GetRolesAsync(user);


                foreach (var item in model.UserRoleDtos)
                {
                    if (!await _roleManager.RoleExistsAsync(item.Role))
                        continue;

                    var isCurrentlyInRole = existingUserRoles.Contains(item.Role);

                    if (item.IsUserInRole && !isCurrentlyInRole)
                        await _userManager.AddToRoleAsync(user, item.Role);
                    else if (!item.IsUserInRole && isCurrentlyInRole)
                        await _userManager.RemoveFromRoleAsync(user, item.Role);
                }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToAction("Index");
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            ViewData["id"] = id;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserDetailsDto model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }
    }
}
