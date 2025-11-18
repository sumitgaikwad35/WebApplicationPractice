using FluentValidation;
using WebApplicationPractice.Models;

namespace WebApplicationPractice.Validator
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.Email).NotEmpty().WithMessage("Email can not be empty").EmailAddress().WithMessage("Not a valid Email");
            RuleFor(e => e.Name).NotEmpty().WithMessage("Name can not be empty").Length(2, 20).WithMessage("Name length should be between 2 and 20");
            RuleFor(e => e.Salary).GreaterThan(0).WithMessage("Salary should more than 0");
            RuleFor(e => e.Phone).InclusiveBetween(1111111111, 9999999999).WithMessage("Phone Number should be 10 digits only");
        }
    }
}
