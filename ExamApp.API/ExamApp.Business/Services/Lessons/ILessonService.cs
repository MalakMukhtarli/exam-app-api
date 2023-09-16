using ExamApp.Business.Models.Lessons;
using ExamApp.Core.Entities.Models;

namespace ExamApp.Business.Services.Lessons;

public interface ILessonService
{
    Task<List<LessonDto>> GetAll();
    Task<int> Create(SaveLessonRequest request);
    Task CheckById(int lessonId);
    Task<LessonGrade> CheckByGradeId(int lessonId, int gradeId);
    Task<LessonDto> GetById(int lessonId);
    Task<int> Update(int lessonId, UpdateLessonRequest request);
    Task Delete(int lessonId);
}