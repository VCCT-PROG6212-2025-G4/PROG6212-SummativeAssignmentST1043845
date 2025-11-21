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

            // Lecturer
            builder.Entity<Lecturer>(l =>
            {
                l.HasKey(x => x.LecturerId);
                l.Property(x => x.Email).IsRequired().HasMaxLength(200);
                l.Property(x => x.FirstName).HasMaxLength(100);
                l.Property(x => x.LastName).HasMaxLength(100);
                l.Property(x => x.Department).HasMaxLength(100);
                l.HasMany(x => x.Claims).WithOne(c => c.Lecturer).HasForeignKey(c => c.LecturerId);
            });

            // Claim
            builder.Entity<Claim>(c =>
            {
                c.HasKey(x => x.ClaimId);
                c.Property(x => x.HoursWorked).IsRequired();
                c.Property(x => x.HourlyRate).HasColumnType("decimal(10,2)");
                c.Property(x => x.DateSubmitted).HasColumnType("datetime");
            });

            // ===============================
            // USER SEEDING (TEST ACCOUNTS)
            // ===============================
            builder.Entity<User>().HasData(
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
                new User
                {
                    UserId = 4,
                    Email = "Co-ordinator@CoordCMCS.com",
                    FirstName = "Steph",
                    LastName = "Curry",
                    Role = "Coordinator",
                    PasswordHash = PasswordHasher.Hash("4639")
                },
                new User
                {
                    UserId = 5,
                    Email = "Manager@ManCMCS.com",
                    FirstName = "Ethan",
                    LastName = "Hunt",
                    Role = "Manager",
                    PasswordHash = PasswordHasher.Hash("1243")
                },
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

            // ===============================
            // LECTURER SEEDING (must match User emails)
            // ===============================
            builder.Entity<Lecturer>().HasData(
                new Lecturer
                {
                    LecturerId = 1,
                    Email = "Lofentse13@CMCSLEC.com",
                    FirstName = "Lofentse",
                    LastName = "Moagi",
                    Department = "Mathematics",
                    RatePerHour = 200.00m
                },
                new Lecturer
                {
                    LecturerId = 2,
                    Email = "Karabo28@CMCSLEC.com",
                    FirstName = "Karabo",
                    LastName = "Kgoebane",
                    Department = "Computer Science",
                    RatePerHour = 220.00m
                },
                new Lecturer
                {
                    LecturerId = 3,
                    Email = "Claudia06@CMCSLEC.com",
                    FirstName = "Claudia",
                    LastName = "Brander",
                    Department = "Physics",
                    RatePerHour = 210.00m
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