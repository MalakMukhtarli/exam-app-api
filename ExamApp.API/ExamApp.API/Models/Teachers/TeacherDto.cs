using ExamApp.API.Models.LessonGrades;

namespace ExamApp.API.Models.Teachers;

public class TeacherDto : BaseDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<LessonGradeDto> LessonGrades { get; set; }
}