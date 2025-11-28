using Microsoft.EntityFrameworkCore;

namespace UserRoleAPi.Models
{
    public class UserRoleDbContext : DbContext
    {
        public UserRoleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<RoleUser> roleusers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=userroles;user=root;password=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleUser>()
                .HasKey(ru => new { ru.UserId, ru.RoleId });

            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.User)
                .WithMany(u => u.RoleUsers)
                .HasForeignKey(ru => ru.UserId);

            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.Role)
                .WithMany(r => r.RoleUsers)
                .HasForeignKey(ru => ru.RoleId);
        }
    }
}
