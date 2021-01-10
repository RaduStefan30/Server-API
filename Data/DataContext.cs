using Microsoft.EntityFrameworkCore;
using StudentApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignation> Assignations { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Attendance> AttendanceList { get; set; }
        public DbSet<Message> Messages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignation>()
                .HasKey(a => new{a.CourseId,a.UserId});

            modelBuilder.Entity<Message>()
                .HasOne(s=> s.Sender)
                .WithMany(m=>m.MessagesSent);
            
            modelBuilder.Entity<Message>()
                .HasOne(r=> r.Receiver)
                .WithMany(m=>m.MessagesReceived);
        }

    }
}
