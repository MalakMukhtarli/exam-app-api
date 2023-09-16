using ExamApp.Business.Models.Pupils;
using ExamApp.Core.Entities.Models;

namespace ExamApp.Business.Services.Pupils;

public interface IPupilService
{
    Task<List<PupilDto>> GetAll();
    Task<int> Create(SavePupilRequest request);
    Task<List<PupilGrade>> GetByGradeId(int gradeId);
    Task<PupilDto> GetById(int pupilId);
    Task<int> Update(int pupilId, UpdatePupilRequest request);
    Task Delete(int pupilId);
}