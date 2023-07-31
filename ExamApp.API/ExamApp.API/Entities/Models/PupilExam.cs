namespace ExamApp.API.Entities.Models;

public class PupilExam : CommonEntity
{
    public int PupilId { get; set; }
    public Pupil Pupil { get; set; }
    public int ExamId { get; set; }
    public Exam Exam { get; set; }
    public byte? Mark { get; set; }
}