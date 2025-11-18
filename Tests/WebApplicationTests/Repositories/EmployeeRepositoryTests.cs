using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplicationPractice.Data;
using WebApplicationPractice.Models;
using WebApplicationPractice.Repositories;
using Xunit;

namespace WebApplicationPractice.Tests.Repositories
{
    public class EmployeeRepositoryTests
    {
        private EmployeeRepository _employeeRepository;
        private AppDbContext _appDbContext;
        private void CreateNewDatabase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
            _appDbContext = new AppDbContext(options);
            _employeeRepository = new EmployeeRepository(_appDbContext);
        }

        [Fact]
        public void GetAll_ShouldReturnAllEmployee_WhenEmployeesExist()
        {
            CreateNewDatabase();

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Datta", Email = "datta@example.com", Salary = 50000 },
                new Employee { Id = 2, Name = "Sunny", Email = "sunny@example.com", Salary = 60000 },
                new Employee { Id = 3, Name = "Raj", Email = "raj@example.com", Salary = 70000 }
            };
            _appDbContext.AddRange(employees);
            _appDbContext.SaveChanges();

            var result = _employeeRepository.GetAll();
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(employees, result);

        }

        [Fact]
        public void GetById_ShouldReturnEmployee_WhenExists()
        {
            CreateNewDatabase();

            var employee = new Employee { Id = 1, Name = "Datta", Email = "datta@example.com", Salary = 50000 };

            _appDbContext.Add(employee);
            _appDbContext.SaveChanges();

            var result = _employeeRepository.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(1 , result.Id);
            Assert.Equal("Datta" , result.Name);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenNotFound()
        {
            CreateNewDatabase();

            var result = _employeeRepository.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public void Add_ShouldAddEmployeeSuccessfully()
        {
            CreateNewDatabase();
            var employee = new Employee { Id = 1, Name = "Datta", Email = "datta@example.com", Salary = 50000 };
  
            var result = _employeeRepository.Add(employee);

            Assert.NotNull(result);
            Assert.Equal(1 , result.Id);
            Assert.Equal(result, employee);
        }

        [Fact]
        public void Update_ShouldReturnTrue_WhenEmployeeExists()
        {
            CreateNewDatabase();
            var employee = new Employee { Id = 1, Name = "Datta", Email = "datta@example.com", Salary = 50000 };

            _appDbContext.Add(employee);
            _appDbContext.SaveChanges();

            var updatedEmployee = new Employee { Id = 1, Name = "Datta Updated" };

            var result = _employeeRepository.Update(updatedEmployee);
            Assert.True(result);
            Assert.Equal(1, updatedEmployee.Id);
            var updEmp = _employeeRepository.GetById(1);
            Assert.Equal(updatedEmployee.Name, updEmp.Name);            
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenEmployeeExists()
        {
            CreateNewDatabase();

            var emp = new Employee { Id = 1, Name = "Datta" , Email = "datta@example.com", Salary = 50000 };
            _appDbContext.Employees.Add(emp);
            _appDbContext.SaveChanges();

            var result = _employeeRepository.Delete(1);

            Assert.True(result);
            Assert.Empty(_employeeRepository.GetAll());
        }

        [Fact]
        public void Delete_ShouldReturnFalse_WhenEmployeeDoesNotExist()
        {
            CreateNewDatabase();

            var result = _employeeRepository.Delete(1);

            Assert.False(result);
        }
    }
}
