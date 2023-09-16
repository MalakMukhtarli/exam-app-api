using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamApp.Core.Entities.Models;

public class Lesson : CommonEntity
{
    public Lesson()
    {
        LessonGrades = new HashSet<LessonGrade>();
    }

    [Required] [StringLength(3)] public string Code { get; set; }
    [Required] [StringLength(30)]         
    [Column(TypeName = "nvarchar(30)")]
    public string Name { get; set; }
    
    public virtual ICollection<LessonGrade> LessonGrades { get; set; }
}