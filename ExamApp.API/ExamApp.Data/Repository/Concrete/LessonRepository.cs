using ExamApp.Core.Entities.Models;
using ExamApp.Data.Persistence;

namespace ExamApp.Data.Repository.Concrete;

public class LessonRepository : RepositoryBase<Lesson>, ILessonRepository
{
    public LessonRepository(AppDbContext databaseContext) : base(databaseContext)
    {
    }
}