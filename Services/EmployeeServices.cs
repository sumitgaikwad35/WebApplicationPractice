using WebApplicationPractice.Models;

namespace WebApplicationPractice.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly List<Employee> employee = new List<Employee>
        {
            new() { Id = 1, Name = "Sumit", Email = "sumit@1234", Phone = 9697988854 ,Salary = 15000 , DeptName = "Trainee"},
            new() { Id = 2, Name = "Amit", Email = "amit@1234", Phone = 9697988854 ,Salary = 35000 , DeptName = "HelpDesk" },
            new() { Id = 3, Name = "Pranit", Email = "pranit@1234", Phone = 9697988854 ,Salary = 75000 , DeptName = "HR"},
            new() { Id = 4, Name = "Sujit", Email = "sujit@1234", Phone = 8697988854 ,Salary = 45000 , DeptName = "IT"},
        };

        public List<Employee> GetAllEmployees()
        {
            return employee;
        }

        public Employee? GetEmployeeById(int id)
        {
            return employee.FirstOrDefault(x => x.Id == id);
        }

        public Employee AddEmployee(Employee emp)
        {
            emp.Id = employee.Any() ? employee.Max(s => s.Id)+1 : 0;
            employee.Add(emp);
            return emp;
        }

        public bool UpdateEmployee(int id, Employee emp)
        {
            var existing = employee.FirstOrDefault(s => s.Id == id);
            if (existing == null) return false;

            existing.Name = emp.Name;
            existing.Email = emp.Email;
            existing.Phone = emp.Phone;
            return true;
        }

        public bool DeleteEmployee(int id)
        {
            var existing = employee.FirstOrDefault(s => s.Id == id);
            if (existing == null) return false;

            employee.Remove(existing);
            return true;
        }
    }
}