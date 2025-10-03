using System.Threading.Tasks;
using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Specification.Class;
using Company.DAL.Entites;
using Company.PL.Dtos;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;

        public DepartmentsController(IUnitOfWork unitOfWork,IMapper mapper,IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchInp)
        {  
           return View(await _unitOfWork.Repository<Department>().GetAllDataWithSpec(new DepartmentsSpecification(searchInp)));
        }
        [HttpGet]
        public IActionResult Create() {
            return View();            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentDto model)
        {
            if (ModelState.IsValid)
            { 
                var department = _mapper.Map<Department>(model);

                try
                {
                    await _unitOfWork.Repository<Department>().AddAsync(department);
                    int count = await _unitOfWork.Complete();

                    if (count > 0)

                        return RedirectToAction("Index");
                }
                catch (Exception e) {
                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, e.Message);
                    else
                        ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Department");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id,string viewName = "Details") {
            if (id is null) return BadRequest();
            var department = await _unitOfWork.Repository<Department>().GetDataWithSpec(new DepartmentsSpecification(id.Value));
            if (department is null) return NotFound();
            ViewData["id"] = id.Value;
            return View(viewName,_mapper.Map<DepartmentDto>(department));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            return await Details(id, "Update");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute]int id ,DepartmentDto model)
        {
            if(!ModelState.IsValid)return View(model);
            var department = _mapper.Map<Department>(model);
            department.Id = id;
            try
            {
                _unitOfWork.Repository<Department>().Update(department);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, e.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Department");
            }
                return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentDto model)
        {
            var department = _mapper.Map<Department>(model);
            department.Id = id;
            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, e.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Deleting the Department");
            }
                return View(model);
        }
    }
}
