using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplicationPractice.Models;
using WebApplicationPractice.Repositories;
using WebApplicationPractice.Services;
using Xunit;

namespace WebApplicationPractices.Tests.Services
{
    public class EmployeeDbServiceTests
    {
        private EmployeeDbServices _employeeService;
        private readonly Mock<IEmployeeRepository> _mockRepo;

        public EmployeeDbServiceTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
        }

        [Fact]
        public void GetAllEmployees_ShouldReturnAllEmployees()
        {
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Datta", Email = "datta@example.com", Salary = 50000 },
                new Employee { Id = 2, Name = "Sunny", Email = "sunny@example.com", Salary = 60000 },
                new Employee { Id = 3, Name = "Raj", Email = "raj@example.com", Salary = 70000 }
            };

            _mockRepo.Setup(r => r.GetAll()).Returns(employees);
            _employeeService = new EmployeeDbServices(_mockRepo.Object);

            var result = _employeeService.GetAllEmployees();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetEmployeeById_ShouldReturnEmployee_WhenExists()
        {
            var employee = new Employee { Id = 1, Name = "Datta", Email = "datta@example.com" };
            _mockRepo.Setup(r => r.GetById(1)).Returns(employee);
            _employeeService = new EmployeeDbServices(_mockRepo.Object);

            var result = _employeeService.GetEmployeeById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Datta", result.Name);
        }

        [Fact]
        public void GetEmployeeById_ShouldReturnNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetById(1)).Returns((Employee)null);
            _employeeService = new EmployeeDbServices(_mockRepo.Object);

            var result = _employeeService.GetEmployeeById(1);

            Assert.Null(result);
        }

        [Fact]
        public void AddEmployee_ShouldReturnEmployee_WhenAdded()
        {
            var emp = new Employee { Id = 1, Name = "Datta", Email = "datta@example.com" };
            _mockRepo.Setup(r => r.Add(emp)).Returns(emp);
            _employeeService = new EmployeeDbServices(_mockRepo.Object);

            var result = _employeeService.AddEmployee(emp);

            Assert.NotNull(result);
            Assert.Equal(emp, result);
            _mockRepo.Verify(r => r.Add(emp), Times.Once);
        }

        [Fact]
        public void UpdateEmployee_ShouldReturnTrue_WhenUpdatedSuccessfully()
        {
            var emp = new Employee { Id = 1, Name = "Datta" };
            _mockRepo.Setup(r => r.Update(It.IsAny<Employee>())).Returns(true);
            _employeeService = new EmployeeDbServices(_mockRepo.Object);

            var result = _employeeService.UpdateEmployee(1, emp);

            Assert.True(result);
            _mockRepo.Verify(r => r.Update(It.Is<Employee>(e => e.Id == 1)), Times.Once);
        }

        [Fact]
        public void DeleteEmployee_ShouldReturnTrue_WhenDeleted()
        {
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);
            _employeeService = new EmployeeDbServices(_mockRepo.Object);

            var result = _employeeService.DeleteEmployee(1);

            Assert.True(result);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void DeleteEmployee_ShouldReturnFalse_WhenNotFound()
        {
            _mockRepo.Setup(r => r.Delete(1)).Returns(false);
            _employeeService = new EmployeeDbServices(_mockRepo.Object);

            var result = _employeeService.DeleteEmployee(1);

            Assert.False(result);
        }
    }
}
