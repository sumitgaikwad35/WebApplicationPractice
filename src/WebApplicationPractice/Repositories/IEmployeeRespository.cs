using WebApplicationPractice.Models;

namespace WebApplicationPractice.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetById(int id);
        Employee Add(Employee employee);
        bool Update(Employee employee);
        bool Delete(int id);
    }
}
