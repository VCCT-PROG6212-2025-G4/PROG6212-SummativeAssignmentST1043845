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
                    Email = "lecturer1@test.com",
                    FirstName = "Alice",
                    LastName = "Moyo",
                    Role = "Lecturer",
                    PasswordHash = PasswordHasher.Hash("password123")
                },
                new User
                {
                    UserId = 2,
                    Email = "lecturer2@test.com",
                    FirstName = "Brian",
                    LastName = "Zulu",
                    Role = "Lecturer",
                    PasswordHash = PasswordHasher.Hash("password123")
                },
                new User
                {
                    UserId = 3,
                    Email = "lecturer3@test.com",
                    FirstName = "Cindy",
                    LastName = "Nkosi",
                    Role = "Lecturer",
                    PasswordHash = PasswordHasher.Hash("password123")
                },

                // -----------------------------
                // COORDINATOR
                // -----------------------------
                new User
                {
                    UserId = 4,
                    Email = "coordinator@test.com",
                    FirstName = "David",
                    LastName = "Maseko",
                    Role = "Coordinator",
                    PasswordHash = PasswordHasher.Hash("password123")
                },

                // -----------------------------
                // MANAGER
                // -----------------------------
                new User
                {
                    UserId = 5,
                    Email = "manager@test.com",
                    FirstName = "Ethan",
                    LastName = "Smith",
                    Role = "Manager",
                    PasswordHash = PasswordHasher.Hash("password123")
                },

                // -----------------------------
                // HR
                // -----------------------------
                new User
                {
                    UserId = 6,
                    Email = "hr@test.com",
                    FirstName = "Fiona",
                    LastName = "Muller",
                    Role = "HR",
                    PasswordHash = PasswordHasher.Hash("password123")
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

