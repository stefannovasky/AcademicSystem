using DAL.MapConfigs;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DAL
{
    public class AcademyContext : DbContext
    {
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }

        public AcademyContext() : base ()
        {

        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\stefa\OneDrive\Documentos\FILHADAPUTA.mdf;Integrated Security=True;Connect Timeout=30");
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
                entityType.GetProperties().Where(c => c.ClrType == typeof(string)).ToList().ForEach(p => p.SetIsUnicode(false));
            }
            modelBuilder.ApplyConfiguration(new UserMapConfig());
            modelBuilder.ApplyConfiguration(new SubjectMapConfig());
            modelBuilder.ApplyConfiguration(new StudentMapConfig());
            modelBuilder.ApplyConfiguration(new OwnerMapConfig());
            modelBuilder.ApplyConfiguration(new InstructorMapConfig());
            modelBuilder.ApplyConfiguration(new EvaluationMapConfig());
            modelBuilder.ApplyConfiguration(new CourseMapConfig());
            modelBuilder.ApplyConfiguration(new CoordinatorMapConfig());
            modelBuilder.ApplyConfiguration(new ClassMapConfig());
            modelBuilder.ApplyConfiguration(new AttendanceMapConfig());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubjectInstructor>()
                .HasKey(si => new { si.SubjectID, si.InstructorID });
            modelBuilder.Entity<SubjectInstructor>()
                .HasOne(si => si.Subject)
                .WithMany(s => s.Instructors)
                .HasForeignKey(si => si.SubjectID);
            modelBuilder.Entity<SubjectInstructor>()
                .HasOne(si => si.Instructor)
                .WithMany(i => i.Subjects)
                .HasForeignKey(si => si.InstructorID);
                
            modelBuilder.Entity<StudentClass>()
                .HasKey(sc => new { sc.StudentID, sc.ClassID });
            modelBuilder.Entity<StudentClass>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.Classes)
                .HasForeignKey(sc => sc.StudentID);
            modelBuilder.Entity<StudentClass>()
                .HasOne(sc => sc.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(sc => sc.ClassID);
                
            modelBuilder.Entity<CoordinatorClass>()
                .HasKey(cc => new { cc.CoordinatorID, cc.ClassID });
            modelBuilder.Entity<CoordinatorClass>()
                .HasOne(cc => cc.Coordinator)
                .WithMany(c => c.Classes)
                .HasForeignKey(cc => cc.CoordinatorID);
            modelBuilder.Entity<CoordinatorClass>()
                .HasOne(cc => cc.Class)
                .WithMany(c => c.Coordinators)
                .HasForeignKey(cc => cc.ClassID);

            modelBuilder.Entity<InstructorClass>()
                .HasKey(ic => new { ic.InstructorID, ic.ClassID });
            modelBuilder.Entity<InstructorClass>()
                .HasOne(ic => ic.Instructor)
                .WithMany(i => i.Classes)
                .HasForeignKey(ic => ic.InstructorID);
            modelBuilder.Entity<InstructorClass>()
                .HasOne(ic => ic.Class)
                .WithMany(c => c.Instructors)
                .HasForeignKey(ic => ic.ClassID);

            modelBuilder.Entity<OwnerCourse>()
                .HasKey(oc => new { oc.OwnerID, oc.CourseID });
            modelBuilder.Entity<OwnerCourse>()
                .HasOne(oc => oc.Owner)
                .WithMany(o => o.Courses)
                .HasForeignKey(oc => oc.OwnerID);
            modelBuilder.Entity<OwnerCourse>()
                .HasOne(oc => oc.Course)
                .WithMany(c => c.Owners)
                .HasForeignKey(oc => oc.CourseID);
        }


    }
}
