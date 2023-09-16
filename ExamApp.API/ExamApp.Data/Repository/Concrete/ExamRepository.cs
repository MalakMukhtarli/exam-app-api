using ExamApp.Core.Entities.Models;
using ExamApp.Data.Persistence;

namespace ExamApp.Data.Repository.Concrete;

public class ExamRepository : RepositoryBase<Exam>, IExamRepository
{
    public ExamRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
}