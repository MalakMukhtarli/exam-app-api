namespace ExamApp.API.Entities.Models;

public class Exam : CommonEntity
{
    public Exam()
    {
        PupilExams = new HashSet<PupilExam>();
    }
    
    public DateTime ExamDate { get; set; }
    public int LessonGradeId { get; set; }
    public virtual LessonGrade LessonGrade { get; set; }
    
    public virtual ICollection<PupilExam> PupilExams { get; set; }
}