using Microsoft.EntityFrameworkCore;
using WebApplicationPractice.Models;
namespace WebApplicationPractice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
