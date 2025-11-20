using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace CMCS_Web_App.Data
{
    public class AppDbContext: DbContext
    {
        

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<HR> HR { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(u =>
            {
                u.HasKey(x => x.UserId);
                u.Property(x => x.Email).IsRequired().HasMaxLength(200);
                u.Property(x => x.PasswordHash).IsRequired();
                u.Property(x => x.Role).IsRequired().HasMaxLength(50);
                u.Property(x => x.FirstName).HasMaxLength(100);
                u.Property(x => x.LastName).HasMaxLength(100);
            });

            // ===============================
            // USER SEEDING (TEST ACCOUNTS)
            // ===============================

            builder.Entity<User>().HasData(

                // -----------------------------
                // LECTURERS (3 accounts)
                // -----------------------------
                new User
                {
                    UserId = 1,
                    Email = "Lofentse13@CMCSLEC.com",
                    FirstName = "Lofentse",
                    LastName = "Moagi",
                    Role = "Lecturer",
                    PasswordHash = PasswordHasher.Hash("0628")
                },
                new User
                {
                    UserId = 2,
                    Email = "Karabo28@CMCSLEC.com",
                    FirstName = "Karabo",
                    LastName = "Kgoebane",
                    Role = "Lecturer",
                    PasswordHash = PasswordHasher.Hash("1113")
                },
                new User
                {
                    UserId = 3,
                    Email = "Claudia06@CMCSLEC.com",
                    FirstName = "Claudia",
                    LastName = "Brander",
                    Role = "Lecturer",
                    PasswordHash = PasswordHasher.Hash("0731")
                },

                // -----------------------------
                // COORDINATOR
                // -----------------------------
                new User
                {
                    UserId = 4,
                    Email = "Co-ordinator@CoordCMCS.com",
                    FirstName = "Steph",
                    LastName = "Curry",
                    Role = "Coordinator",
                    PasswordHash = PasswordHasher.Hash("4639")
                },

                // -----------------------------
                // MANAGER
                // -----------------------------
                new User
                {
                    UserId = 5,
                    Email = "Manager@ManCMCS.com",
                    FirstName = "Ethan",
                    LastName = "Hunt",
                    Role = "Manager",
                    PasswordHash = PasswordHasher.Hash("1243")
                },

                // -----------------------------
                // HR
                // -----------------------------
                new User
                {
                    UserId = 6,
                    Email = "HR@ResourcesCMCS.com",
                    FirstName = "Shrek",
                    LastName = "Fiona",
                    Role = "HR",
                    PasswordHash = PasswordHasher.Hash("0432")
                }
            );
        }

        // ===========================
        // PASSWORD HASHER
        // ===========================
        public static class PasswordHasher
        {
            public static string Hash(string input)
            {
                using var sha = System.Security.Cryptography.SHA256.Create();
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToHexString(hash);
            }
        }

    }
}

