using Microsoft.AspNetCore.Mvc;
using MongoDB.Repository.IRepository;
using MongoDB.Repository.Models;
using MongoDBWebSample.Models;

namespace MongoDBWebSample.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _empRepo;

        public EmployeeController(IEmployeeRepository empRepo)
        {
            _empRepo = empRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetEmployees()
        {
            var employees = _empRepo.Gets();
            return Json(employees);
        }

        [HttpPost]
        public string SaveEmp(EmployeeViewModel employee)
        {
            Employee entity = new Employee()
            {
                Id = employee.Id,
                Name = employee.Name,
                CardNumber = employee.CardNumber,
                Salary = employee.Salary
            };

            if (employee.File != null && employee.File.Length > 0)
            {
                using var ms = new MemoryStream();
                employee.File.CopyTo(ms);
                entity.Photo = ms.ToArray();
            }

            var emp = _empRepo.Save(entity);
            return emp.Id.Trim() != "" ? "Saved" : "Save Fail";
        }

        public JsonResult DeleteEmp(string empId)
        {
            var msg = _empRepo.Delete(empId);
            return Json(msg);
        }
    }
}