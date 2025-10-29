using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using WebApplicationPractice.Models;
using WebApplicationPractice.Services;

namespace WebApplicationPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeService;
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;

        public EmployeeController(
            IEmployeeServices employeeService,
            ILogger<EmployeeController> logger,
            IDistributedCache cache)
        {
            _employeeService = employeeService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public ActionResult<List<Employee>> GetAllEmployee()
        {
            _logger.LogInformation("Fetching all employees");

            string cacheKey = "employees:all";
            var cachedData = _cache.Get(cacheKey);

            if (cachedData != null)
            {
                _logger.LogInformation("Cache hit: returning employees from Redis");
                var employees = JsonSerializer.Deserialize<List<Employee>>(cachedData);
                return Ok(employees);
            }

            _logger.LogInformation("Cache miss: fetching employees from service");
            var employeesFromService = _employeeService.GetAllEmployees();

            if (employeesFromService == null || !employeesFromService.Any())
            {
                _logger.LogWarning("No employees found!");
                return NotFound();
            }

            var serialized = JsonSerializer.SerializeToUtf8Bytes(employeesFromService);
            _cache.Set(cacheKey, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20)
            });

            return Ok(employeesFromService);
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            string cacheKey = $"employee:{id}";
            _logger.LogInformation($"Fetching Employee with Id {id}");

            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {
                _logger.LogInformation($"Cache hit for employee {id}");
                var emp = JsonSerializer.Deserialize<Employee>(cachedData);
                return Ok(emp);
            }

            var empFromService = _employeeService.GetEmployeeById(id);
            if (empFromService == null)
            {
                _logger.LogWarning($"Employee with Id {id} not found");
                return NotFound("Employee not found");
            }

            _logger.LogInformation($"Employee found, caching result for {id}");
            var serialized = JsonSerializer.SerializeToUtf8Bytes(empFromService);
            _cache.Set(cacheKey, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });

            return Ok(empFromService);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployeeById(int id)
        {
            _logger.LogInformation($"Deleting Employee with Id {id}");
            var deleted = _employeeService.DeleteEmployee(id);
            if (!deleted)
            {
                _logger.LogWarning($"Employee {id} not found");
                return BadRequest("Employee not found");
            }

            _cache.Remove($"employee:{id}");
            _cache.Remove("employees:all");

            _logger.LogInformation($"Employee {id} deleted and cache cleared");
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeById(int id, Employee e)
        {
            _logger.LogInformation($"Updating Employee with Id {id}");
            var updated = _employeeService.UpdateEmployee(id, e);
            if (!updated)
            {
                _logger.LogWarning($"Employee {id} not found");
                return BadRequest("Employee Not Found");
            }

            _cache.Remove($"employee:{id}");
            _cache.Remove("employees:all");

            _logger.LogInformation($"Employee {id} updated and cache cleared");
            return Ok();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee newEmployee)
        {
            var emp = _employeeService.AddEmployee(newEmployee);
            _logger.LogInformation("New Employee Added");
            _cache.Remove("employees:all");

            return Ok(emp);
        }
    }
}
