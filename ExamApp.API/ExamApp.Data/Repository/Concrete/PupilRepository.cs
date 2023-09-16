using ExamApp.Core.Entities.Models;
using ExamApp.Data.Persistence;

namespace ExamApp.Data.Repository.Concrete;

public class PupilRepository : RepositoryBase<Pupil>, IPupilRepository
{
    public PupilRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
}