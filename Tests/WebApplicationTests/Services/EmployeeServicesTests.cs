using WebApplicationPractice.Models;
using WebApplicationPractice.Services;
using Xunit;

namespace WebApplicationPractice.Tests.Services
{
    public class EmployeeServicesTests
    {
        private readonly IEmployeeServices _employeeService;

        public EmployeeServicesTests()
        {
            _employeeService = new EmployeeServices();
        }

        [Fact]
        public void GetAllEmployees_ShouldReturnAllEmployees()
        {
            var result = _employeeService.GetAllEmployees();

            Assert.NotNull(result);
            Assert.Equal(4, result.Count); 
        }

        [Fact]
        public void GetEmployeeById_ShouldReturnEmployee_WhenExists()
        {
            var result = _employeeService.GetEmployeeById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Sumit", result.Name);
        }

        [Fact]
        public void GetEmployeeById_ShouldReturnNull_WhenNotFound()
        {
            var result = _employeeService.GetEmployeeById(999);

            Assert.Null(result);
        }

        [Fact]
        public void AddEmployee_ShouldAddAndReturnEmployee()
        {
            var newEmployee = new Employee
            {
                Name = "Rohit",
                Email = "rohit@1234",
                Phone = 999888777,
                Salary = 50000,
                DeptName = "Finance"
            };

            var result = _employeeService.AddEmployee(newEmployee);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("Rohit", result.Name);

            var allEmployees = _employeeService.GetAllEmployees();
            Assert.Equal(5, allEmployees.Count); // 4 + 1 added
        }

        [Fact]
        public void UpdateEmployee_ShouldReturnTrue_WhenEmployeeExists()
        {
            var updatedEmployee = new Employee
            {
                Name = "Sumit Updated",
                Email = "sumitupdated@1234",
                Phone = 123456789
            };

            var result = _employeeService.UpdateEmployee(1, updatedEmployee);

            Assert.True(result);

            var employee = _employeeService.GetEmployeeById(1);
            Assert.Equal("Sumit Updated", employee.Name);
            Assert.Equal("sumitupdated@1234", employee.Email);
            Assert.Equal(123456789, employee.Phone);
        }

        [Fact]
        public void UpdateEmployee_ShouldReturnFalse_WhenEmployeeDoesNotExist()
        {
            var updatedEmployee = new Employee
            {
                Name = "Ghost",
                Email = "ghost@1234",
                Phone = 111111111
            };

            var result = _employeeService.UpdateEmployee(999, updatedEmployee);

            Assert.False(result);
        }

        [Fact]
        public void DeleteEmployee_ShouldReturnTrue_WhenEmployeeExists()
        {
            var result = _employeeService.DeleteEmployee(1);

            Assert.True(result);

            var employee = _employeeService.GetEmployeeById(1);
            Assert.Null(employee);

            var allEmployees = _employeeService.GetAllEmployees();
            Assert.Equal(3, allEmployees.Count); // 4 - 1 deleted
        }

        [Fact]
        public void DeleteEmployee_ShouldReturnFalse_WhenEmployeeDoesNotExist()
        {
            var result = _employeeService.DeleteEmployee(999);

            Assert.False(result);
        }
    }
}
