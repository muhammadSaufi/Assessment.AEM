using Microsoft.EntityFrameworkCore;

namespace Assessment.AEM.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=Assessment;Trusted_Connection=True;");
        //    //optionsBuilder.UseSqlServer(@"Data Source = (LocalDb)\MSSQLLocalDB; Initial Catalog = Assessment; Integrated Security = SSPI;");
        //}

        public DbSet<Platform> Platform { get; set; }
        public DbSet<Well> Well { get; set; }


    }
}
