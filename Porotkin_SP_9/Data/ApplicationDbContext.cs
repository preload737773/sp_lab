using Microsoft.EntityFrameworkCore;
using Porotkin_SP_9.Models;

namespace Porotkin_SP_9.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<AccessRecord> AccessRecords{ get; set; }
    }
}
