namespace ExamApp.API.Models.Exams;

public class SaveExamRequest
{
    public DateTime ExamDate { get; set; }
    public int LessonId { get; set; }
    public int GradeId { get; set; }
}