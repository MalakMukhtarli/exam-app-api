using ExamApp.Core.Entities.Models;
using ExamApp.Data.Persistence;

namespace ExamApp.Data.Repository.Concrete;

public class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
    
}