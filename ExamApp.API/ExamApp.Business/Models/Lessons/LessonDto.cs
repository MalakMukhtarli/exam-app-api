namespace ExamApp.Business.Models.Lessons;

public class LessonDto : BaseDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public List<byte> Grades { get; set; }
}