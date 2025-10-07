using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Marks> Marks { get; set; }

}