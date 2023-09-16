using ExamApp.Core.Entities.Models;
using ExamApp.Data.Persistence;

namespace ExamApp.Data.Repository.Concrete;

public class GradeRepository : RepositoryBase<Grade>, IGradeRepository
{
    public GradeRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
}