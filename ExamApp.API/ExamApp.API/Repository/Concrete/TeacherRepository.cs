using ExamApp.API.Entities.Models;
using ExamApp.API.Persistence;

namespace ExamApp.API.Repository.Concrete;

public class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
    
}