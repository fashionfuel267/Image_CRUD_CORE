using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class librarycontext:DbContext
    {
        public librarycontext(DbContextOptions<librarycontext> options) : base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
    }
}
