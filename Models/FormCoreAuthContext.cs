using Microsoft.EntityFrameworkCore;

namespace FormAuthCore.Models
{
    public class FormCoreAuthContext:DbContext
    {
        public FormCoreAuthContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<User> AuthUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginRequest>().HasNoKey();
            


        }
    }
}
