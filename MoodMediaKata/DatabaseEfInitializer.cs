using System.Data;
using Microsoft.EntityFrameworkCore;
using MoodMediaKata.Company;

namespace MoodMediaKata;

public class DatabaseEfInitializer(KataDbContext dbContext) : IDatabaseInitializer
{
    public void InitializeDatabase()
    {
        using var connection = dbContext.Database.GetDbConnection();

        // Check if the database exists
        if (connection.State == ConnectionState.Closed)
            connection.Open();

        if (!connection.GetSchema("tables").Rows.Cast<DataRow>().Any(
                row => row.Field<string>("table_name").Equals("Companies", StringComparison.OrdinalIgnoreCase)))
        {
            dbContext.Database.ExecuteSqlRaw(@"
                    CREATE SCHEMA kata_schema;

                    CREATE TABLE kata_schema.Company (
                        Id INT IDENTITY(1, 1) PRIMARY KEY,
                        Name NVARCHAR(255) NOT NULL,
                        Code NVARCHAR(50) NOT NULL UNIQUE,
                        Licensing INT NOT NULL
                    );

                    CREATE TABLE kata_schema.Location (
                        Id INT IDENTITY(1, 1) PRIMARY KEY,
                        Name NVARCHAR(255) NOT NULL,
                        Address NVARCHAR(MAX) NOT NULL,
                        ParentId INT NOT NULL,
                        FOREIGN KEY (ParentId) REFERENCES kata_schema.Company(Id)
                    );

                    CREATE TABLE kata_schema.Device (
                        Id INT IDENTITY(1, 1) PRIMARY KEY,
                        SerialNumber NVARCHAR(255) NOT NULL UNIQUE,
                        Type INT NOT NULL,
                        LocationId INT NOT NULL,
                        FOREIGN KEY (LocationId) REFERENCES kata_schema.Location(Id)
                    );

                    GRANT USAGE ON SCHEMA kata_schema TO public;
                    GRANT ALL ON SCHEMA kata_schema TO sa;
                ");
        }
    }
}

public class KataDbContext(DbContextOptions<KataDbContext> optionsBuilderOptions) : DbContext
{
    public DbSet<Company.Company> Companies { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Device> Devices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost;Database=kata_db;User Id=sa;Password=V3ryS3cr3t;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company.Company>().HasKey(c => c.Id);
        modelBuilder.Entity<Company.Company>().Property(c => c.Name).IsRequired();
        modelBuilder.Entity<Company.Company>().Property(c => c.Code).IsRequired();
        modelBuilder.Entity<Company.Company>().Property(c => c.Licensing).HasConversion(
            v => (int)Enum.Parse(typeof(Licensing), v),
            v => Enum.GetName(typeof(Licensing), v) ?? string.Empty);
        
        modelBuilder.Entity<Location>().HasKey(l => l.Id);
        modelBuilder.Entity<Location>().Property(l => l.Name).IsRequired();
        modelBuilder.Entity<Location>().Property(l => l.Address).IsRequired();

        modelBuilder.Entity<Device>().HasKey(d => d.Id);
        modelBuilder.Entity<Device>().Property(d => d.SerialNumber).IsRequired();
        modelBuilder.Entity<Device>().Property(c => c.Type).HasConversion(
            v => (int)Enum.Parse(typeof(DeviceType), v),
            v => Enum.GetName(typeof(DeviceType), v) ?? string.Empty);

        modelBuilder.Entity<Location>().HasOne<Company.Company>().WithMany().HasForeignKey("ParentId");
        modelBuilder.Entity<Device>().HasOne<Location>().WithMany().HasForeignKey("LocationId");
    }
}
