using WebApplicationPractice.Data;
using WebApplicationPractice.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebApplicationPractice.Services
{
    public class EmployeeDbServices : IEmployeeServices
    {
        private readonly AppDbContext _context;

        public EmployeeDbServices(AppDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            return _context.Employees.Find(id);
        }

        public Employee AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public bool UpdateEmployee(int id, Employee updated)
        {
            var existing = _context.Employees.Find(id);
            if (existing == null)
                return false;

            existing.Name = updated.Name;
            existing.Email = updated.Email;
            existing.Phone = updated.Phone;
            existing.Salary = updated.Salary;
            existing.DeptName = updated.DeptName;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteEmployee(int id)
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
