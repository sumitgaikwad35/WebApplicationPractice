using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationPractice.Models;

namespace WebApplicationPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        public static List<Employee> employee = new List<Employee>
        {
            new Employee { Id = 1, Name = "Sumit", Email = "sumit@1234", Phone = 9697988854 },
            new Employee { Id = 2, Name = "Amit", Email = "amit@1234", Phone = 9697988854 },
            new Employee { Id = 3, Name = "Pranit", Email = "pranit@1234", Phone = 9697988854 },
            new Employee { Id = 4, Name = "Sujit", Email = "sujit@1234", Phone = 9697988854 },
        };

        [HttpGet]
        public ActionResult<List<Employee>> GetAllEmployee()
        {
            return Ok(employee);
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            var emp = employee.FirstOrDefault(n => n.Id == id);
            return Ok(emp);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployeeById(int id)
        {
            var emp = employee.FirstOrDefault(n => n.Id == id);
            if(emp == null)
            {
                return BadRequest("Employee not found");
            }
            employee.Remove(emp);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeById(int id, Employee e)
        {
            var emp = employee.FirstOrDefault(s  => s.Id == id);
            if(emp != null)
            {
                emp.Id = id;
                emp.Name = e.Name;
                emp.Email = e.Email;
                emp.Phone = e.Phone;
            }
            else
            {
                return BadRequest("Employee Not Found");
            }
                return Ok();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee newEmployee)
        {
            newEmployee.Id = employee.Any() ? employee.Max(e => e.Id) + 1 : 1;
            employee.Add(newEmployee);
            return Ok(employee);
        }
    }
}