using ExamApp.Business.Exceptions;
using ExamApp.Business.Models.Grades;
using ExamApp.Core.Entities.Models;
using ExamApp.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Business.Services.Grades;

public class GradeManager : IGradeService
{
    private readonly IGradeRepository _gradeRepository;

    public GradeManager(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }

    public async Task<List<GradeDto>> GetAll()
    {
        var grades = await _gradeRepository.GetQuery().Where(x => x.Active)
            .Include(x => x.LessonGrades.Where(y=> !y.Deleted))
            .ThenInclude(x => x.Grade)
            .Select(x => new GradeDto{ Id = x.Id, Value = x.Value})
            .ToListAsync();
            
        return grades;
    }

    public async Task<int> Create(byte grade)
    {
        if (grade < 1 || grade > 11)
            throw new NotFoundException("0-dan kiçik və ya 11-dən böyük sinif ola bilməz");

        var isExist = await _gradeRepository.GetQuery().FirstOrDefaultAsync(x => x.Active && x.Value == grade);

        if (isExist is not null)
            throw new DuplicateConflictException("Bu sinif daha əvvəl yaradılıb");

        var newGrade = await _gradeRepository.AddAsync(new Grade { Value = grade });

        return newGrade.Id;
    }
    
    public async Task CheckById(int gradeId)
    {
        var grade = await _gradeRepository.GetQuery().AnyAsync(x => x.Active && x.Id == gradeId);

        if (!grade)
            throw new NotFoundException("Belə bir sinif tapılmadı");
    }
    
    public async Task<GradeDto> GetById(int gradeId)
    {
        var grade = await _gradeRepository.GetQuery().Where(x => x.Active && x.Id == gradeId)
            .Select(x => new GradeDto { Id = x.Id, Value = x.Value }).FirstOrDefaultAsync();

        if (grade is null)
            throw new NotFoundException("Belə bir sinif tapılmadı");

        return grade;
    }
    
    public async Task<int> Update(int gradeId, UpdateGradeRequest request)
    {
        var grade = await _gradeRepository.GetQuery().FirstOrDefaultAsync(x => x.Active && x.Id == gradeId);

        if (grade is null)
            throw new NotFoundException("Belə bir sinif tapılmadı");

        grade.Value = request.Grade;

        await _gradeRepository.UpdateAsync(grade);

        return gradeId;
    }
    
    public async Task Delete(int gradeId)
    {
        var grade = await _gradeRepository.GetQuery().Where(x => x.Active && x.Id == gradeId)
            .Include(x=>x.PupilGrades)
            .Include(x=>x.LessonGrades)
            .FirstOrDefaultAsync();

        if (grade is null)
            throw new NotFoundException("Belə bir sinif tapılmadı");

        if (grade.PupilGrades.Count > 0 || grade.LessonGrades.Count > 0)
            throw new NotFoundException("Sinifə bağlı şagird və ya dərs olduğu üçün silinə bilməz");

        await _gradeRepository.DeleteAsync(grade);
    }
}