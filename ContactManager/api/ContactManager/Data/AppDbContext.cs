using Microsoft.EntityFrameworkCore;

namespace ContactManager.Data
{
    public class AppDbContext : DbContext
    {
        public IConfiguration _configuration { get; set; }
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }       
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DatabaseConnection"));
        }

        public DbSet<Client> Clients { get; set; }
    }
}
