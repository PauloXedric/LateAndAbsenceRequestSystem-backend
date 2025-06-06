using DLARS.Entities;
using DLARS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<RequestEntity> Request { get; set; }
        public DbSet<StatusEntity> Status { get; set; }
        public DbSet<TeacherEntity> Teacher { get; set; }
        public DbSet<SubjectsEntity> Subject { get; set; }
        public DbSet<TeacherSubjectsEntity> TeacherSubject { get; set; }



        public AppDbContext(DbContextOptions options) : base(options) 
        {

        }
        

    }
}
