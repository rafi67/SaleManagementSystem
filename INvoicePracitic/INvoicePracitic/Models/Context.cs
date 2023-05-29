using Microsoft.EntityFrameworkCore;

namespace INvoicePracitic.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) { }
        public DbSet<SaleMaster>SaleMasters { get; set; }
        public DbSet<SaleDetails> SaleDetails { get; set; }
    }
}
