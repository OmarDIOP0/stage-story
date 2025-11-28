using Microsoft.EntityFrameworkCore;
using StageStory.Models;

namespace StageStory.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Enterprise>().ToTable("Enterprises");
            modelBuilder.Entity<Internship>().ToTable("Internships");
            modelBuilder.Entity<Notification>().ToTable("Notifications");

            //------ADMIN----------
            modelBuilder.Entity<Admin>()
                .HasKey(a => a.AdminId);


            //------STUDENT----------
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Internships)
                .WithOne(i => i.Student)
                .HasForeignKey(i => i.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Notifications)
                .WithOne(n => n.Student)
                .HasForeignKey(n => n.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            //------Enterprise----------    
            modelBuilder.Entity<Enterprise>()
            .HasKey(e => e.Id);

            modelBuilder.Entity<Enterprise>()
                .HasMany(e => e.Internships)
                .WithOne(i => i.Enterprise)
                .HasForeignKey(i => i.Id)
                .OnDelete(DeleteBehavior.Cascade);
            // ---------- INTERNSHIP ----------
            modelBuilder.Entity<Internship>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Internship>()
                .HasMany(i => i.Notifications)
                .WithOne(n => n.Internship)
                .HasForeignKey(n => n.InternshipId)
                .OnDelete(DeleteBehavior.SetNull);


            // ---------- NOTIFICATION ----------
            modelBuilder.Entity<Notification>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<Notification>()
                .Property(n => n.Status)
                .HasConversion<int>(); // Enum → int


            // ---------- ENUMS ----------
            modelBuilder.Entity<Internship>()
                .Property(i => i.SignatureType)
                .HasConversion<int>();

            modelBuilder.Entity<Student>()
                .Property(s => s.Profile)
                .HasConversion<int>();

        }
    }
}
