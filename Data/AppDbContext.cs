using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Marks> Marks { get; set; }
    public DbSet<Courses> Courses { get; set; }
    public DbSet<Enrollments> Enrollments { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<EnrollmentMarkView> EnrollmentMarkView { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Mark EnrollmentMarkView as keyless
        modelBuilder.Entity<EnrollmentMarkView>().HasNoKey();
    }
}



