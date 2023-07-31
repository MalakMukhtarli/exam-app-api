using ExamApp.API.Entities.Models;
using ExamApp.API.Exceptions;
using ExamApp.API.Models.Lessons;
using ExamApp.API.Repository;
using ExamApp.API.Services.Grades;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.API.Services.Lessons;

public class LessonManager : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IRepositoryAsync<LessonGrade> _lessonGradeRepository;
    private readonly IRepositoryAsync<LessonGradeTeacher> _lessonGradeTeacherRepository;
    private readonly IGradeService _gradeService;

    public LessonManager(ILessonRepository lessonRepository, IGradeService gradeService,
        IRepositoryAsync<LessonGrade> lessonGradeRepository,
        IRepositoryAsync<LessonGradeTeacher> lessonGradeTeacherRepository)
    {
        _lessonRepository = lessonRepository;
        _gradeService = gradeService;
        _lessonGradeRepository = lessonGradeRepository;
        _lessonGradeTeacherRepository = lessonGradeTeacherRepository;
    }

    public async Task<List<LessonDto>> GetAll()
    {
        var lessons = await _lessonRepository.GetQuery().Where(x => x.Active)
            .Include(x => x.LessonGrades.Where(y => !y.Deleted))
            .ThenInclude(x => x.Grade)
            .Select(x => new LessonDto
            {
                Id = x.Id, Code = x.Code, Name = x.Name, Grades = x.LessonGrades.Select(x => x.Grade.Value).ToList()
            })
            .ToListAsync();

        return lessons;
    }

    public async Task<int> Create(SaveLessonRequest request)
    {
        if (request is null)
            throw new BadHttpRequestException("Məlumatlar doldurulmayıb");

        var isExist = await _lessonRepository.GetQuery()
            .FirstOrDefaultAsync(x => x.Active && x.Code == request.Code);

        if (isExist is not null)
            throw new DuplicateConflictException("Bu nömrəli dərs daha əvvəl yaradılıb");

        foreach (var gradeId in request.GradeIds)
        {
            await _gradeService.CheckById(gradeId);
        }

        var lesson = new Lesson
        {
            Code = request.Code,
            Name = request.Name,
            LessonGrades = request.GradeIds.Select(x => new LessonGrade { GradeId = x }).ToList()
        };

        var newLesson = await _lessonRepository.AddAsync(lesson);

        return newLesson.Id;
    }

    public async Task CheckById(int lessonId)
    {
        var lesson = await _lessonRepository.GetQuery().AnyAsync(x => x.Active && x.Id == lessonId);

        if (!lesson)
            throw new NotFoundException("Belə bir dərs tapılmadı");
    }

    public async Task<LessonGrade> CheckByGradeId(int lessonId, int gradeId)
    {
        var lessonGrade = await _lessonGradeRepository.GetQuery()
            .FirstOrDefaultAsync(x => x.LessonId == lessonId && x.GradeId == gradeId);

        if (lessonGrade is null)
            throw new NotFoundException("Dərs və ya Sinif düzgün seçilməyib");

        return lessonGrade;
    }

    public async Task<LessonDto> GetById(int lessonId)
    {
        var lesson = await _lessonRepository.GetQuery().Where(x => x.Active && x.Id == lessonId)
            .Include(x => x.LessonGrades)
            .ThenInclude(x => x.Grade)
            .Select(x => new LessonDto
            {
                Id = x.Id, Code = x.Code, Name = x.Name, Grades = x.LessonGrades.Select(x => x.Grade.Value).ToList()
            })
            .FirstOrDefaultAsync();

        if (lesson is null)
            throw new NotFoundException("Belə bir dərs tapılmadı");

        return lesson;
    }

    public async Task<int> Update(int lessonId, UpdateLessonRequest request)
    {
        var lesson = await _lessonRepository.GetQuery().Where(x => x.Active && x.Id == lessonId)
            .Include(x => x.LessonGrades)
            .ThenInclude(x => x.Grade)
            .Include(x => x.LessonGrades)
            .ThenInclude(x => x.Exams)
            .Include(x => x.LessonGrades)
            .ThenInclude(x => x.LessonGradeTeachers)
            .FirstOrDefaultAsync();

        if (lesson is null)
            throw new NotFoundException("Belə bir dərs tapılmadı");

        var newLessonGrades = new List<LessonGrade>();
        var deleteLessonGrades = new List<LessonGrade>();

        foreach (var gradeId in request.GradeIds)
        {
            await _gradeService.CheckById(gradeId);

            var lessonGrade = lesson.LessonGrades.FirstOrDefault(x => x.GradeId == gradeId);

            if (lessonGrade is null)
            {
                newLessonGrades.Add(new LessonGrade { LessonId = lessonId, GradeId = gradeId });
            }
        }

        foreach (var lessonGrade in lesson.LessonGrades)
        {
            var lessonGradeCheck = request.GradeIds.Any(x => x == lessonGrade.GradeId);

            if (!lessonGradeCheck && lessonGrade.Exams.Count == 0 && lessonGrade.LessonGradeTeachers.Count == 0)
                deleteLessonGrades.Add(lessonGrade);
            else if (!lessonGradeCheck && lessonGrade.Exams.Count > 0)
                throw new BadHttpRequestException(
                    $"{lessonGrade.Lesson.Name} {lessonGrade.Grade.Value} sinifi listdən çıxara bilmərsiniz.{lessonGrade.Lesson.Name} {lessonGrade.Grade.Value} sinifi üçün imtahan mövcuddur. ");
            else if (!lessonGradeCheck && lessonGrade.LessonGradeTeachers.Count > 0)
                throw new BadHttpRequestException(
                    $"{lessonGrade.Lesson.Name} {lessonGrade.Grade.Value} sinifi listdən çıxara bilmərsiniz.{lessonGrade.Lesson.Name} {lessonGrade.Grade.Value} sinifinə bağlı müəllim mövcuddur. ");
        }

        await _lessonGradeRepository.BeginTransaction();

        await _lessonGradeRepository.AddRangeAsync(newLessonGrades);
        await _lessonGradeRepository.DeleteRangeAsync(deleteLessonGrades);

        await _lessonGradeRepository.Commit();

        return lessonId;
    }


    public async Task Delete(int lessonId)
    {
        var lesson = await _lessonRepository.GetQuery().Where(x => x.Active && x.Id == lessonId)
            .Include(x => x.LessonGrades)
            .ThenInclude(x => x.Grade)
            .Include(x => x.LessonGrades)
            .ThenInclude(x => x.Exams)
            .Include(x => x.LessonGrades)
            .ThenInclude(x => x.LessonGradeTeachers)
            .FirstOrDefaultAsync();

        if (lesson is null)
            throw new NotFoundException("Belə bir dərs tapılmadı");

        var deleteLessonGrades = new List<LessonGrade>();

        foreach (var lessonGrade in lesson.LessonGrades)
        {
            if (lessonGrade.Exams.Count == 0)
                deleteLessonGrades.Add(lessonGrade);
            else if (lessonGrade.Exams.Count > 0)
                throw new BadHttpRequestException(
                    $"{lessonGrade.Lesson.Name} dərsini silə bilmərsiniz.{lessonGrade.Lesson.Name} dərsi üçün imtahan mövcuddur");
        }

        await _lessonGradeRepository.BeginTransaction();

        await _lessonRepository.DeleteAsync(lesson);
        await _lessonGradeRepository.DeleteRangeAsync(deleteLessonGrades);
        await _lessonGradeTeacherRepository.DeleteRangeAsync(lesson.LessonGrades.SelectMany(x => x.LessonGradeTeachers)
            .ToList());

        await _lessonGradeRepository.Commit();
    }
}