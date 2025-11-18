using WebApplicationPractice.Models;
using WebApplicationPractice.Repositories;

namespace WebApplicationPractice.Services
{
    public class EmployeeDbServices : IEmployeeServices
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeDbServices(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public List<Employee> GetAllEmployees()
        {
            return _repository.GetAll();
        }

        public Employee GetEmployeeById(int id)
        {
            return _repository.GetById(id);
        }

        public Employee AddEmployee(Employee employee)
        {
            return _repository.Add(employee);
        }

        public bool UpdateEmployee(int id, Employee updated)
        {
            updated.Id = id;
            return _repository.Update(updated);
        }

        public bool DeleteEmployee(int id)
        {
            return _repository.Delete(id);
        }
    }
}