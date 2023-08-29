using Microsoft.EntityFrameworkCore;
using ModelsDataflow;

namespace WebApiDataflow.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
