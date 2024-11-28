using Microsoft.EntityFrameworkCore;
using PLVT08_HSZF_2024251.Model;

namespace PLVT08_HSZF_2024251.Persistence.MsSql
{
    public class HouseholdContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Mail> Mails { get; set; }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }

        public HouseholdContext()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(e => e.Mails)
                .WithOne()
                .HasForeignKey(e => e.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Person>()
                .HasMany(e => e.FavoriteProducts)
                .WithMany()
                .UsingEntity<FavoriteProduct>();
            modelBuilder.Entity<Storage>()
                .HasMany(e => e.Products)
                .WithOne()
                .HasForeignKey(e => e.StoreInFridge)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=HouseHoldRegister;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}