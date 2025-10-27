using Microsoft.EntityFrameworkCore;
using WebApplicationPractice.Models;
using Microsoft.Extensions.Logging;

namespace WebApplicationPractice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        // Define DbSets for your entities
        public DbSet<Employee> Employees { get; set; }
    }
}
