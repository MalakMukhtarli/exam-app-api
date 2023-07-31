namespace ExamApp.API.Entities;

public interface ISoftDeletedEntity
{
    bool Deleted { get; set; }
}