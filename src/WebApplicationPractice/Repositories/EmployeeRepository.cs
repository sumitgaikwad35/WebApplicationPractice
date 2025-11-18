using WebApplicationPractice.Data;
using WebApplicationPractice.Models;

namespace WebApplicationPractice.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.Find(id);
        }

        public Employee Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public bool Update(Employee employee)
        {
            var existing = _context.Employees.Find(employee.Id);
            if (existing == null)
                return false;

            existing.Name = employee.Name;
            existing.Email = employee.Email;
            existing.Phone = employee.Phone;
            existing.Salary = employee.Salary;
            existing.DeptName = employee.DeptName;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp == null)
                return false;

            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return true;
        }
    }
}
