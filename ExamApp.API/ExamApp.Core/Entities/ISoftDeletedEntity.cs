namespace ExamApp.Core.Entities;

public interface ISoftDeletedEntity
{
    bool Deleted { get; set; }
}