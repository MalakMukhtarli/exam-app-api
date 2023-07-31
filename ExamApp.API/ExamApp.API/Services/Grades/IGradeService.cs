using ExamApp.API.Entities.Models;
using ExamApp.API.Models.Grades;

namespace ExamApp.API.Services.Grades;

public interface IGradeService
{
    Task<List<GradeDto>> GetAll();
    Task CheckById(int gradeId);
    Task<int> Create(byte grade);
    Task<GradeDto> GetById(int gradeId);
    Task<int> Update(int gradeId, UpdateGradeRequest request);
    Task Delete(int gradeId);
}