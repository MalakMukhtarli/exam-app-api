namespace ExamApp.API.Models.Lessons;

public class SaveLessonRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public List<int> GradeIds { get; set; }
}