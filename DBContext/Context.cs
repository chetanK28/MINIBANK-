using Microsoft.EntityFrameworkCore;
using MINIBANK.Models;

namespace MINIBANK.DBContext
{
    public class Context : DbContext
    {
        private readonly IConfiguration _configuration;

        // Default constructor for migrations and scaffolding
        public Context()
        {
        }

        // Constructor with dependency injection
        public Context(DbContextOptions<Context> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        // DbSets for tables
        public DbSet<Users> Users { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Configure the database connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("MyDatabase");
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 39)));
            }
        }

        // Configure entity relationships and properties
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Users configuration
            modelBuilder.Entity<Users>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Users>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // User --> BankAccount relationship
            modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.User)
                .WithMany(u => u.BankAccount)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Transactions configuration
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.RelatedAccount)
                .WithMany()
                .HasForeignKey(t => t.RelatedAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Store TransactionType as string in the database
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Type)
                .HasConversion<string>();

            // Configure decimal precision for Amount (if needed)
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2); // Total 18 digits, 2 decimal places

            // Additional configurations can go here...
        }
    }
}
