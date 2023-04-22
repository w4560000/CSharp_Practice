using MongoDB.Driver;
using MongoDB.Repository.IRepository;
using MongoDB.Repository.Models;

namespace MongoDB.Repository.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        private IMongoCollection<Employee> _employeeTable;

        public EmployeeRepository()
        {
            _mongoClient = new MongoClient("mongodb://10.10.10.10:27017");
            _database = _mongoClient.GetDatabase("OfficeDB");
            _employeeTable = _database.GetCollection<Employee>("Employee");
        }

        public string Delete(string employeeId)
        {
            _employeeTable.DeleteOne(x => x.Id == employeeId);
            return "Delete";
        }

        public Employee Get(string employeeId)
        {
            return _employeeTable.Find(f => f.Id == employeeId).FirstOrDefault();
        }

        public List<Employee> Gets()
        {
            return _employeeTable.Find(FilterDefinition<Employee>.Empty).ToList();
        }

        public Employee Save(Employee employee)
        {
            var empObj = _employeeTable.Find(f => f.Id == employee.Id).FirstOrDefault();

            // 若沒上傳圖檔 則不更新圖檔
            if (employee.Photo.Length == 0)
                employee.Photo = empObj.Photo;

            if (empObj == null)
                _employeeTable.InsertOne(employee);
            else
                _employeeTable.ReplaceOne(r => r.Id == employee.Id, employee);

            return employee;
        }
    }
}