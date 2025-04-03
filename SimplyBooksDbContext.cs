using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
        // Set the relationship between Authors and Books
        modelBuilder.Entity<Books>()
            .HasOne(b => b.AuthorId)
            .WithMany()
            .HasForeignKey("AuthorId") // This is the foreign key in the Books table
            .IsRequired(false); // Allow nulls if needed, otherwise set to true

    }
}
