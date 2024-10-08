using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Identity.Client;
using Product_CURD_API.Model;

namespace Product_CURD_API.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions dbContextOptions) : base (dbContextOptions)
        {
            
        }
        public DbSet<Product> Products { get; set; }

    }
    
}
