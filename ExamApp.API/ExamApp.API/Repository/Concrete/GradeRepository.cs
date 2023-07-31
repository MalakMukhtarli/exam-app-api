using ExamApp.API.Entities.Models;
using ExamApp.API.Persistence;

namespace ExamApp.API.Repository.Concrete;

public class GradeRepository : RepositoryBase<Grade>, IGradeRepository
{
    public GradeRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
}