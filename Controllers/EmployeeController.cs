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

        public EmployeeController(IEmployeeServices employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public ActionResult<List<Employee>> GetAllEmployee()
        {
            
            return Ok(_employeeService.GetAllEmployees());
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
            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null)
                return NotFound("Employee not found");

            return Ok(emp);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployeeById(int id)
        {
            var emp = _employeeService.DeleteEmployee(id);
            if(emp == false)
            {
                return BadRequest("Employee not found");
            }
            
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeById(int id, Employee e)
        {
            var emp = _employeeService.UpdateEmployee(id, e);   
            if(emp == false)
            {
                return BadRequest("Employee Not Found");
            }
      
            return Ok();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee newEmployee)
        {
            var emp = _employeeService.AddEmployee(newEmployee);
            return Ok(emp);
        }
    }
}