using Microsoft.AspNetCore.Mvc;
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

        public EmployeeController(IEmployeeServices employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Employee>> GetAllEmployee()
        {
            _logger.LogInformation("Fetching all employees");
            var employees = _employeeService.GetAllEmployees();
            if (employees == null)
            {
                _logger.LogWarning("No employees found!");
            }
            return Ok(employees);
        }

        //[HttpGet("by-id")]
        //public ActionResult<Employee> GetEmployeeById([FromQuery] int id)
        //{
        //    var emp = _employeeService.GetEmployeeById(id);
        //    if (emp == null)
        //        return NotFound("Employee not found");

        //    return Ok(emp);
        //}
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            _logger.LogInformation($"Fetching Employee with Id {@id}");
            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null)
            {
                _logger.LogWarning($"Id {id} is not a employee");
                return NotFound("Employee not found");
            }

            _logger.LogInformation($"Employee Found with Id {@id}");
            return Ok(emp);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployeeById(int id)
        {
            _logger.LogInformation($"Fetching Employee with Id {@id}");
            var emp = _employeeService.DeleteEmployee(id);
            if(emp == false)
            {
                _logger.LogWarning($"Id {id} is not a employee");
                return BadRequest("Employee not found");
            }
            _logger.LogInformation($"Employee Found with Id {@id} and deleted");
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeById(int id, Employee e)
        {
            _logger.LogInformation($"Fetching Employee with Id {@id}");
            var emp = _employeeService.UpdateEmployee(id, e);   
            if(emp == false)
            {
                _logger.LogWarning($"Id {id} is not a employee");
                return BadRequest("Employee Not Found");
            }
            _logger.LogInformation($"Employee Found with Id {@id} and updated");
            return Ok();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee newEmployee)
        {
            var emp = _employeeService.AddEmployee(newEmployee);
            _logger.LogInformation("New Employee Added");
            return Ok(emp);
        }
    }
}