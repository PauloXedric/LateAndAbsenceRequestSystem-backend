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
        public DbSet<RequestHistoryEntity> RequestHistory { get; set; }
        public DbSet<ResultEntity> Result { get; set; }
        public DbSet<StatusEntity> Status { get; set; }
        public DbSet<SubjectEntity> Subject { get; set; }
        public DbSet<TeacherEntity> Teacher { get; set; }
        public DbSet<TeacherSubjectEntity> TeacherSubject { get; set; }
        


        public DbSet<TeacherAssignedSubjectsModelView> TeacherAssignedSubjects { get; set; }
        public DbSet<RequestResultHistoryModelView> RequestResultHistory { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) 
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);          
       
            modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Status)
            .HasConversion<string>();

            modelBuilder.Entity<TeacherAssignedSubjectsModelView>()
               .HasNoKey()
               .ToView("TeacherAssignedSubjects");

            modelBuilder.Entity<RequestResultHistoryModelView>()
               .HasNoKey()
               .ToView("RequestResultHistory");

        }

    }
}
