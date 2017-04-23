using Microsoft.EntityFrameworkCore;
 
namespace Exam1.Models
{
    public class Exam1Context : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public Exam1Context(DbContextOptions<Exam1Context> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Connections> Connections { get; set; }
    }
}