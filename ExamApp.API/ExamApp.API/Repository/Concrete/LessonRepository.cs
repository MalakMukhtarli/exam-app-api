using ExamApp.API.Entities.Models;
using ExamApp.API.Persistence;

namespace ExamApp.API.Repository.Concrete;

public class LessonRepository : RepositoryBase<Lesson>, ILessonRepository
{
    public LessonRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
}