using ExamApp.Business.Models.PupilExams;

namespace ExamApp.Business.Models.Exams;

public class ExamDto : BaseDto
{
    public DateTime ExamDate { get; set; }
    public string LessonCode { get; set; }
    public List<PupilExamDto> PupilExams { get; set; }
}