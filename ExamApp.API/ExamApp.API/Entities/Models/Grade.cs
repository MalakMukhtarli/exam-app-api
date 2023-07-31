using System.ComponentModel.DataAnnotations;

namespace ExamApp.API.Entities.Models;

public class Grade : CommonEntity
{
    public Grade()
    {
        PupilGrades = new HashSet<PupilGrade>();
        LessonGrades = new HashSet<LessonGrade>();
    }

    [Required] public byte Value { get; set; }
    
    public virtual ICollection<PupilGrade> PupilGrades { get; set; }
    public virtual ICollection<LessonGrade> LessonGrades { get; set; }

}