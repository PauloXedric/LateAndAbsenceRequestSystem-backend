using DLARS.Entities;
using DLARS.Models;
using DLARS.Models.Identity;
using DLARS.Views;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DLARS.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RequestEntity> Request { get; set; }
        public DbSet<StatusEntity> Status { get; set; }
        public DbSet<TeacherEntity> Teacher { get; set; }
        public DbSet<SubjectsEntity> Subject { get; set; }
        public DbSet<TeacherSubjectEntity> TeacherSubject { get; set; }



        public DbSet<TeacherAssignedSubjectsModelView> TeacherAssignedSubjects { get; set; }


        public AppDbContext(DbContextOptions options) : base(options) 
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TeacherAssignedSubjectsModelView>()
               .HasNoKey()
               .ToView("TeacherAssignedSubjects");

            base.OnModelCreating(modelBuilder);
        }

    }
}
