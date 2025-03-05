using Microsoft.EntityFrameworkCore;
using ReportTA.Models;

namespace ReportTA.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<TaxRequest> TaxRequests { get; set; }
    }
}
