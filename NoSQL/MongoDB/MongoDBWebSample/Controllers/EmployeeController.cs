using Microsoft.AspNetCore.Mvc;
using MongoDB.Repository.IRepository;
using MongoDB.Repository.Models;

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

        public JsonResult SaveEmp(Employee employee)
        {
            var emp = _empRepo.Save(employee);
            return Json(emp);
        }

        public JsonResult DeleteEmp(string empId)
        {
            var msg = _empRepo.Delete(empId);
            return Json(msg);
        }
    }
}