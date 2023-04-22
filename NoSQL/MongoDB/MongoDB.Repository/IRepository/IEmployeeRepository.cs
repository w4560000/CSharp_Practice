using MongoDB.Repository.Models;

namespace MongoDB.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        Employee Save(Employee employee);

        Employee Get(string employeeId);

        List<Employee> Gets();

        string Delete(string employeeId);
    }
}