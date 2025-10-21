using CMCS_Web_App.Models;
using Microsoft.EntityFrameworkCore;


namespace CMCS_Web_App.Data
{
    public class AppDbContext: DbContext
    {
        

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Manager> Managers { get; set; }
    }
}

