using ExamApp.API.Models.PupilExams;

namespace ExamApp.API.Models.Exams;

public class ExamDto : BaseDto
{
    public DateTime ExamDate { get; set; }
    public string LessonCode { get; set; }
    public List<PupilExamDto> PupilExams { get; set; }
}