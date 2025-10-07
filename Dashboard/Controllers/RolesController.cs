using Company.BLL.Specification.Class;
using Company.DAL.Entites;
using Dashboard.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index(string searchInp)
        {
            List<RoleDto> roles = _roleManager.Roles.Where(R=>(searchInp.IsNullOrEmpty() || R.Name.ToLower().Contains(searchInp.ToLower()))).Select(C=>new RoleDto() {Name = C.Name,Id = C.Id }).ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleToCreateDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var roleName = model.Name.Trim();
            var existingRole = await _roleManager.FindByNameAsync(roleName);

            if (existingRole == null)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                if (!result.Errors.Any())
                    ModelState.AddModelError("", "Cannot add the role.");
            }
            else
            {
                ModelState.AddModelError("", "Role already exists.");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            ViewData["id"] = id;
            return View(viewName, new RoleToCreateDto(){Name = role.Name });
        }
        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {
            return await Details(id, "Update");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string id, RoleToCreateDto model)
        {
            if (!ModelState.IsValid) return View(model);
            var role = await _roleManager.FindByIdAsync(id);
           if(role is  null) return NotFound();
           role.Name = model.Name;
            var result =await _roleManager.UpdateAsync(role);
           if(result.Succeeded)
                return RedirectToAction("Index");
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToCreateDto model)
        {
            if (!ModelState.IsValid) return View(model);
            var role =await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

    }
}
