namespace ExamApp.API.Entities;

public abstract class CommonEntity : BaseEntity, 
                            ISoftDeletedEntity, IActiveEntity, 
                            ICreatedDateEntity, IUpdatedDateEntity, 
                            ICreatedByEntity, IUpdatedByEntity
{
    public bool Deleted { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? CreatedUserId { get; set; }
    public int? UpdatedUserId { get; set; }
}