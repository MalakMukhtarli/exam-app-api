using ExamApp.API.Models.Exams;
using ExamApp.API.Models.PupilExams;

namespace ExamApp.API.Services.Exams;

public interface IExamService
{
    Task<List<ExamDto>> GetAll();
    Task<int> Create(SaveExamRequest request);
    Task<ExamDto> GetById(int examId);
    Task<int> Update(int examId, UpdateExamRequest request);
    Task<int> UpdatePupilExam(UpdatePupilExamRequest request);
    Task Delete(int examId);
}