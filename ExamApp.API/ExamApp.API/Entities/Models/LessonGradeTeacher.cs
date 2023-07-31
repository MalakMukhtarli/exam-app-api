namespace ExamApp.API.Entities.Models;

public class LessonGradeTeacher : BaseEntity, ISoftDeletedEntity
{
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public int LessonGradeId { get; set; }
    public LessonGrade LessonGrade { get; set; }
    public bool Deleted { get; set; }
}