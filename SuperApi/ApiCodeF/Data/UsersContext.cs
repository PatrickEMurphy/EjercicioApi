using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiCodeF.Data
{
    public class UsersContext : IdentityDbContext
    {
        public UsersContext() { }
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole {Name = "Basic", NormalizedName = "BASIC" },
                new IdentityRole {Name = "Admin", NormalizedName = "ADMIN" }
            };

            // Usuarios
            var users = new List<IdentityUser>
            {
                new IdentityUser { UserName = "basic@ejercicio4.com"   , NormalizedUserName = "BASIC@EJERCICIO4.COM"  , Email = "basic@ejercicio4.com"  , NormalizedEmail = "BASIC@EJERCICIO4.COM" },
                new IdentityUser { UserName = "admin@ejercicio4.com"   , NormalizedUserName = "ADMIN@EJERCICIO4.COM"  , Email = "admin@ejercicio4.com"  , NormalizedEmail = "ADMIN@EJERCICIO4.COM" }
            };
            var passwordHasher = new PasswordHasher<IdentityUser>();

            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "Basic123!");
            users[1].PasswordHash = passwordHasher.HashPassword(users[1], "Admin123!");

            // Seeder de UserRoles
            var userRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> { UserId = users[0].Id, RoleId = roles[0].Id },
                new IdentityUserRole<string> { UserId = users[1].Id, RoleId = roles[1].Id }
            };

            // Incluir datos
            modelBuilder.Entity<IdentityRole>().HasData(roles);
            modelBuilder.Entity<IdentityUser>().HasData(users);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }
    }
}
