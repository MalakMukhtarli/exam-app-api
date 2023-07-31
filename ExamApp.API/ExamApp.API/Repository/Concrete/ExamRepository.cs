using ExamApp.API.Entities.Models;
using ExamApp.API.Persistence;

namespace ExamApp.API.Repository.Concrete;

public class ExamRepository : RepositoryBase<Exam>, IExamRepository
{
    public ExamRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
}