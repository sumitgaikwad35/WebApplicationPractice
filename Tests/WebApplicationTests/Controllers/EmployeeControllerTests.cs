using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using System.Text.Json;
using WebApplicationPractice.Controllers;
using WebApplicationPractice.Models;
using WebApplicationPractice.Services;

namespace WebApplicationTests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeServices> _mockService;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly Mock<ILogger<EmployeeController>> _mockLogger;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockService = new Mock<IEmployeeServices>();
            _mockCache = new Mock<IDistributedCache>();
            _mockLogger = new Mock<ILogger<EmployeeController>>();

            _controller = new EmployeeController(
                _mockService.Object,
                _mockLogger.Object,
                _mockCache.Object
            );
        }

        [Fact]
        public void GetAllEmployee_ShouldReturnFromService_WhenCacheIsEmpty()
        {
            _mockCache.Setup(c => c.Get("employees:all")).Returns((byte[])null);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "A" },
                new Employee { Id = 2, Name = "B" }
            };

            _mockService.Setup(s => s.GetAllEmployees()).Returns(employees);

            var result = _controller.GetAllEmployee();

            var ok = result.Result as OkObjectResult;
            Assert.NotNull(ok);

            var returned = ok.Value as List<Employee>;
            Assert.Equal(2, returned.Count);

            _mockCache.Verify(c => c.Set(
                "employees:all",
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>()
            ), Times.Once);
        }

        [Fact]
        public void GetAllEmployee_ShouldReturnFromCache_WhenCacheExists()
        {
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "CachedUser" }
            };

            var bytes = JsonSerializer.SerializeToUtf8Bytes(employees);

            _mockCache.Setup(c => c.Get("employees:all")).Returns(bytes);

            var result = _controller.GetAllEmployee();

            var ok = result.Result as OkObjectResult;
            Assert.NotNull(ok);

            var returned = ok.Value as List<Employee>;
            Assert.Single(returned);
        }

        [Fact]
        public void GetEmployeeById_ShouldReturnFromCache_WhenCached()
        {
            var emp = new Employee { Id = 1, Name = "CacheUser" };
            var bytes = JsonSerializer.SerializeToUtf8Bytes(emp);

            _mockCache.Setup(c => c.Get("employee:1")).Returns(bytes);

            var result = _controller.GetEmployeeById(1);

            var ok = result.Result as OkObjectResult;
            Assert.NotNull(ok);

            var returned = ok.Value as Employee;
            Assert.Equal("CacheUser", returned.Name);
        }

        [Fact]
        public void GetEmployeeById_ShouldReturnNotFound_WhenMissing()
        {
            _mockCache.Setup(c => c.Get("employee:999")).Returns((byte[])null);
            _mockService.Setup(s => s.GetEmployeeById(999)).Returns((Employee)null);

            var result = _controller.GetEmployeeById(999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetEmployeeById_ShouldReturnFromService_WhenNotCached()
        {
            _mockCache.Setup(c => c.Get("employee:1")).Returns((byte[])null);

            var emp = new Employee { Id = 1, Name = "ServiceUser" };

            _mockService.Setup(s => s.GetEmployeeById(1)).Returns(emp);

            var result = _controller.GetEmployeeById(1);

            var ok = result.Result as OkObjectResult;
            Assert.NotNull(ok);

            var returned = ok.Value as Employee;
            Assert.Equal("ServiceUser", returned.Name);

            _mockCache.Verify(c => c.Set(
                "employee:1",
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>()
            ), Times.Once);
        }

        [Fact]
        public void DeleteEmployee_ShouldReturnOk_WhenDeleted()
        {
            _mockService.Setup(s => s.DeleteEmployee(1)).Returns(true);

            var result = _controller.DeleteEmployeeById(1);

            Assert.IsType<OkResult>(result);

            _mockCache.Verify(c => c.Remove("employee:1"), Times.Once);
            _mockCache.Verify(c => c.Remove("employees:all"), Times.Once);
        }

        [Fact]
        public void DeleteEmployee_ShouldReturnBadRequest_WhenNotFound()
        {
            _mockService.Setup(s => s.DeleteEmployee(999)).Returns(false);

            var result = _controller.DeleteEmployeeById(999);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateEmployee_ShouldReturnOk_WhenUpdated()
        {
            var updatedEmp = new Employee { Name = "UpdatedUser" };

            _mockService.Setup(s => s.UpdateEmployee(1, updatedEmp)).Returns(true);

            var result = _controller.UpdateEmployeeById(1, updatedEmp);

            Assert.IsType<OkResult>(result);

            _mockCache.Verify(c => c.Remove("employee:1"), Times.Once);
            _mockCache.Verify(c => c.Remove("employees:all"), Times.Once);
        }

        [Fact]
        public void UpdateEmployee_ShouldReturnBadRequest_WhenNotFound()
        {
            var updatedEmp = new Employee { Name = "UpdatedUser" };

            _mockService.Setup(s => s.UpdateEmployee(999, updatedEmp)).Returns(false);

            var result = _controller.UpdateEmployeeById(999, updatedEmp);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddEmployee_ShouldReturnOk_AndClearCache()
        {
            var newEmp = new Employee { Name = "NewUser" };

            _mockService.Setup(s => s.AddEmployee(newEmp)).Returns(newEmp);

            var result = _controller.AddEmployee(newEmp);

            var ok = result as OkObjectResult;
            Assert.NotNull(ok);

            var returned = ok.Value as Employee;
            Assert.Equal("NewUser", returned.Name);

            _mockCache.Verify(c => c.Remove("employees:all"), Times.Once);
        }
    }
}