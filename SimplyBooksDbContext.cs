using Microsoft.EntityFrameworkCore;
using SimplyBooksBE.Models;

    public class SimplyBooksDbContext : DbContext
{
    public SimplyBooksDbContext(DbContextOptions<SimplyBooksDbContext> options)
        : base(options)
    {
    }
    public DbSet<Authors> Authors { get; set; }
    public DbSet<Books> Books { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set Author UID TO AUTO GENERATED
        modelBuilder.Entity<Authors>()
            .Property(a => a.uid)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()"); // Use NEWID() for SQL Server to generate a new unique identifier

        modelBuilder.Entity<Authors>()
            .Property(a => a.firebaseKey)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()"); // Use NEWID() for SQL Server to generate a new unique identifier

        // Set Book UID TO AUTO GENERATED
        modelBuilder.Entity<Books>()
            .Property(b => b.uid)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()"); // Use NEWID() for SQL Server to generate a new unique identifier

        modelBuilder.Entity<Books>()
            .Property(b => b.firebaseKey)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()"); // Use NEWID() for SQL Server to generate a new unique identifier

        // Set the relationship between Authors and Books
        modelBuilder.Entity<Books>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId) // This is the foreign key in the Books table
            .IsRequired(false);

    }
}
