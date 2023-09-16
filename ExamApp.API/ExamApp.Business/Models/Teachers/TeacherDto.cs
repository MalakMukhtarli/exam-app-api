using ExamApp.Business.Models.LessonGrades;

namespace ExamApp.Business.Models.Teachers;

public class TeacherDto : BaseDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<LessonGradeDto> LessonGrades { get; set; }
}