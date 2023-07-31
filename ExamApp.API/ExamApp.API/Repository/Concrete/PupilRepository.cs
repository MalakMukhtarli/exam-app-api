using ExamApp.API.Entities.Models;
using ExamApp.API.Persistence;

namespace ExamApp.API.Repository.Concrete;

public class PupilRepository : RepositoryBase<Pupil>, IPupilRepository
{
    public PupilRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
}