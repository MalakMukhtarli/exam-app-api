namespace ExamApp.Core.Entities.Models;

public class LessonGrade : BaseEntity, ISoftDeletedEntity
{
    public LessonGrade()
    {
        LessonGradeTeachers = new HashSet<LessonGradeTeacher>();
        Exams = new HashSet<Exam>();
    }
    
    public int LessonId { get; set; }
    public virtual Lesson Lesson { get; set; }
    public int GradeId { get; set; }
    public virtual Grade Grade { get; set; }
    public bool Deleted { get; set; }
    
    public virtual ICollection<Exam> Exams { get; set; }
    public virtual ICollection<LessonGradeTeacher> LessonGradeTeachers { get; set; }
}