namespace ExamApp.Core.Entities.Models;

public class PupilGrade : BaseEntity, ISoftDeletedEntity
{
    public int PupilId { get; set; }
    public Pupil Pupil { get; set; }
    public int GradeId { get; set; }
    public Grade Grade { get; set; }
    public bool Deleted { get; set; }
}