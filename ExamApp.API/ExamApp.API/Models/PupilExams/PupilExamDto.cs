namespace ExamApp.API.Models.PupilExams;

public class PupilExamDto : BaseDto
{
    public int PupilNumber { get; set; }
    public byte? Mark { get; set; }
}