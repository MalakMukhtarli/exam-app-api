using ExamApp.Core.Entities;
using ExamApp.Core.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Data.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<Grade> Grades { get; set; }
    public virtual DbSet<Lesson> Lessons { get; set; }
    public virtual DbSet<LessonGradeTeacher> LessonGradeTeachers { get; set; }
    public virtual DbSet<LessonGradeTeacher> LessonGrades { get; set; }
    public virtual DbSet<Pupil> Pupils { get; set; }
    public virtual DbSet<PupilExam> PupilExams { get; set; }
    public virtual DbSet<PupilGrade> PupilGrades { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<CommonEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow.AddHours(4);
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<Grade>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<Lesson>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<LessonGrade>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<LessonGradeTeacher>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<Pupil>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<PupilExam>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<PupilGrade>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<Teacher>().HasQueryFilter(x => !x.Deleted);
        
        base.OnModelCreating(modelBuilder);
    }
}