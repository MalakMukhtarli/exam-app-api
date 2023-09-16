namespace ExamApp.Business.Models.LessonGrades;

public class LessonGradeDto : BaseDto
{
    public string Lesson { get; set; }
    public int Grade { get; set; }
}