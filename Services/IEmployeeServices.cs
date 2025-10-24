using WebApplicationPractice.Models;

namespace WebApplicationPractice.Services
{
    public interface IEmployeeServices
    {
        List<Employee> GetAllEmployees();
        Employee? GetEmployeeById(int id);
        Employee AddEmployee(Employee employee);    
        bool UpdateEmployee(int id, Employee employee);
        bool DeleteEmployee(int id);
    }
}