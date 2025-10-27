using System.ComponentModel.DataAnnotations;

namespace WebApplicationPractice.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        [EmailAddress(ErrorMessage = "NOT A VALID EMAIL")]
        public string? Email { get; set; }

        [Range(9000000000, 9999999999, ErrorMessage = "Only Maharashtra numbers are required")]
        public double Phone { get; set; }

        public long Salary {  get; set; }   
        public string? DeptName { get; set; }
    }
}