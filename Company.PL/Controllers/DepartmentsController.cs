using System.Threading.Tasks;
using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Specification.Class;
using Company.DAL.Entites;
using Company.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentsController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

                await _unitOfWork.Repository<Department>().AddAsync(department);

                int count = await _unitOfWork.Complete();

                if (count >0)
       
                    return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
