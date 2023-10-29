using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GroomerApi.Entities
{
    public class GroomerDbContext: DbContext
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=GroomerDb;Trusted_Connection=True;";
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired();

            modelBuilder.Entity<Animal>()
                .Property(a => a.Name)
                .IsRequired();

            modelBuilder.Entity<Animal>()
                .Property(a => a.Hair)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            //'add -migration Init' - dodaje pliki migracyjne 
            //'update-database' - aktualizuje bazę danych o ostatni plik migracyjny
        }
    }
}
