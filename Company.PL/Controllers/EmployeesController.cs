using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Specification.Class;
using Company.DAL.Entites;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;
        public EmployeesController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchInp)
        {
            return View(await _unitOfWork.Repository<Employee>().GetAllDataWithSpec(new EmployeesSpecification(searchInp)));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _mapper.Map<Employee>(model);
                    if (model.Image is not null)
                        employee.ImageName =await DocumentSettings.Upload(model.Image, "images");
                    await _unitOfWork.Repository<Employee>().AddAsync(employee);

                    int count = await _unitOfWork.Complete();

                    if (count > 0)

                        return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, e.Message);
                    else
                        ModelState.AddModelError(string.Empty, "An Error Has Occurred during Adding the Employee");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var employee = await _unitOfWork.Repository<Employee>().GetDataWithSpec(new EmployeesSpecification(id.Value));
            if (employee is null) return NotFound();
                ViewData["id"] = id.Value;
            if (viewName == "Delete" || viewName == "Update")
            { 
                TempData["ImageName"] = employee.ImageName;
            }
            return View(viewName, _mapper.Map<EmployeeDto>(employee));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            return await Details(id, "Update");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int id, EmployeeDto model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                if (model.Image is not null)
                    employee.ImageName = await DocumentSettings.Upload(model.Image, "images");
                else
                    employee.ImageName = TempData["ImageName"] as string;
                _unitOfWork.Repository<Employee>().Update(employee);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    if(model.Image is not null)
                    {
                        DocumentSettings.Delete(TempData["ImageName"] as string??"not found", "images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, e.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Employee");
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
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeDto model)
        {
            try
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.Repository<Employee>().Delete(employee);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.Delete(TempData["ImageName"] as string??"not found", "images");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, e.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Deleting the Employee");
            }
                return View(model);
        }
    }
}
