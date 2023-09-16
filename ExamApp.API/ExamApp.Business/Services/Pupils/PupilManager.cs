using ExamApp.Business.Exceptions;
using ExamApp.Business.Models.Pupils;
using ExamApp.Business.Services.Grades;
using ExamApp.Core.Entities.Models;
using ExamApp.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Business.Services.Pupils;

public class PupilManager : IPupilService
{
    private readonly IPupilRepository _pupilRepository;
    private readonly IRepositoryAsync<PupilGrade> _pupilGradeRepository;
    private readonly IRepositoryAsync<PupilExam> _pupilExamRepository;
    private readonly IGradeService _gradeService;
    private readonly IGradeRepository _gradeRepository;

    public PupilManager(IPupilRepository pupilRepository, IGradeService gradeService,
        IRepositoryAsync<PupilGrade> pupilGradeRepository, IGradeRepository gradeRepository, IRepositoryAsync<PupilExam> pupilExamRepository)
    {
        _pupilRepository = pupilRepository;
        _gradeService = gradeService;
        _pupilGradeRepository = pupilGradeRepository;
        _gradeRepository = gradeRepository;
        _pupilExamRepository = pupilExamRepository;
    }

    public async Task<List<PupilDto>> GetAll()
    {
        var pupils = await _pupilRepository.GetQuery().Where(x => x.Active)
            .Include(x => x.PupilGrades.Where(y => !y.Deleted))
            .ThenInclude(x => x.Grade)
            .Select(x => new PupilDto
            {
                Id = x.Id,
                Name = x.Name, Surname = x.Surname, Number = x.Number,
                Grade = x.PupilGrades.FirstOrDefault().Grade.Value
            })
            .ToListAsync();

        return pupils;
    }

    public async Task<int> Create(SavePupilRequest request)
    {
        if (request is null)
            throw new BadHttpRequestException("Məlumatlar doldurulmayıb");

        var isExist = await _pupilRepository.GetQuery()
            .FirstOrDefaultAsync(x => x.Active && x.Number == request.Number);

        if (isExist is not null)
            throw new DuplicateConflictException("Bu nömrəli şagird daha əvvəl yaradılıb");

        await _gradeService.CheckById(request.GradeId);

        var newPupil = new Pupil
        {
            Number = request.Number,
            Name = request.Name,
            Surname = request.Surname,
            PupilGrades = new List<PupilGrade> { new PupilGrade { GradeId = request.GradeId } }
        };

        var grade = await _gradeRepository.GetQuery().Where(x=> x.Id == request.GradeId)
            .Include(x=>x.LessonGrades)
            .ThenInclude(x=>x.Exams)
            .FirstOrDefaultAsync()
            ;

        await _pupilRepository.BeginTransaction();
        
        var exam = grade.LessonGrades.SelectMany(x => x.Exams);
        if (exam.Count() > 0)
        {
            var pupilExam = new PupilExam { Pupil = newPupil, ExamId = exam.FirstOrDefault().Id };
            await _pupilExamRepository.AddAsync(pupilExam);
        }

        var newGrade = await _pupilRepository.AddAsync(newPupil);

        await _pupilRepository.Commit();

        return newGrade.Id;
    }

    public async Task<List<PupilGrade>> GetByGradeId(int gradeId)
    {
        var pupilGrades = await _pupilGradeRepository.GetQuery().Where(x => x.GradeId == gradeId)
            .ToListAsync();

        return pupilGrades;
    }

    public async Task<PupilDto> GetById(int pupilId)
    {
        var pupil = await _pupilRepository.GetQuery().Where(x => x.Active && x.Id == pupilId)
            .Include(x => x.PupilGrades.Where(y => !y.Deleted))
            .ThenInclude(x => x.Grade)
            .Select(x => new PupilDto
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Number = x.Number,
                Grade = x.PupilGrades.FirstOrDefault().Grade.Value
            })
            .FirstOrDefaultAsync();

        if (pupil is null)
            throw new NotFoundException("Belə bir şagird tapılmadı");


        return pupil;
    }

    public async Task<int> Update(int pupilId, UpdatePupilRequest request)
    {
        var pupil = await _pupilRepository.GetQuery().Where(x => x.Active && x.Id == pupilId)
            .Include(x => x.PupilGrades.Where(y => !y.Deleted))
            .ThenInclude(x => x.Grade)
            .FirstOrDefaultAsync();

        if (pupil is null)
            throw new NotFoundException("Belə bir şagird tapılmadı");

        await _gradeService.CheckById(request.GradeId);

        if (pupil.PupilGrades.FirstOrDefault()?.GradeId == request.GradeId)
            throw new DuplicateConflictException("Daha əvvəl əlavə olunmuşdur");

        pupil.PupilGrades.FirstOrDefault().GradeId = request.GradeId;

        await _pupilRepository.UpdateAsync(pupil);

        return pupilId;
    }
    
    public async Task Delete(int pupilId)
    {
        var pupil = await _pupilRepository.GetQuery().Where(x => x.Active && x.Id == pupilId)
            .Include(x => x.PupilGrades.Where(y => !y.Deleted))
            .Include(x=>x.PupilExams.Where(x=>!x.Deleted))
            .FirstOrDefaultAsync();
        if (pupil is null)
            throw new NotFoundException("Belə bir şagird tapılmadı");

        await _pupilRepository.BeginTransaction();
        
        await _pupilRepository.DeleteAsync(pupil);
        await _pupilGradeRepository.DeleteRangeAsync(pupil.PupilGrades.ToList());
        await _pupilExamRepository.DeleteRangeAsync(pupil.PupilExams.ToList()); 
        
        await _pupilRepository.Commit();

    }
}