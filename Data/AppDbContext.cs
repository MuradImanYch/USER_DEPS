using Microsoft.EntityFrameworkCore;
using USER_DEPS.Entity;

namespace USER_DEPS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Personal> Personals => Set<Personal>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Surname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.FatherName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(x => x.User)
                    .WithOne(x => x.Personal)
                    .HasForeignKey<User>(x => x.PersonalId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Username)
                    .IsUnique();

                entity.HasIndex(x => x.PersonalId)
                    .IsUnique();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });
        }
    }
}
