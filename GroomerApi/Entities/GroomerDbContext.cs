using DevExpress.Utils.Serializing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GroomerApi.Entities
{
    public class GroomerDbContext : DbContext
    {
        public GroomerDbContext(DbContextOptions<GroomerDbContext> options)
            : base(options)
        { }
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=GroomerDb;Trusted_Connection=True;";
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(a => a.PostalCode)
                .IsRequired()
                .HasMaxLength(6);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Name)
                .IsRequired();

            modelBuilder.Entity<Animal>()
                .Property(a => a.Hair)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(a => a.Name)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            // trzeba zainstalować paczki Entity Framweork Core oraz SQLServer i Tools
            //'add-migration Init' - dodaje pliki migracyjne 
            //'update-database' - aktualizuje bazę danych o ostatni plik migracyjny
        }
    }
}
