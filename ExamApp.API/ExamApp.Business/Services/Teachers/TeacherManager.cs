using ExamApp.Business.Exceptions;
using ExamApp.Business.Models.LessonGrades;
using ExamApp.Business.Models.Teachers;
using ExamApp.Business.Services.Grades;
using ExamApp.Business.Services.Lessons;
using ExamApp.Core.Entities.Models;
using ExamApp.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Business.Services.Teachers;

public class TeacherManager : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IGradeService _gradeService;
    private readonly ILessonService _lessonService;
    private readonly IRepositoryAsync<LessonGradeTeacher> _lessonGradeTeacherRepository;

    public TeacherManager(ITeacherRepository teacherRepository, ILessonService lessonService,
        IGradeService gradeService, IRepositoryAsync<LessonGradeTeacher> lessonGradeTeacherRepository)
    {
        _teacherRepository = teacherRepository;
        _lessonService = lessonService;
        _gradeService = gradeService;
        _lessonGradeTeacherRepository = lessonGradeTeacherRepository;
    }

    public async Task<List<TeacherDto>> GetAll()
    {
        var teachers = await _teacherRepository.GetQuery().Where(x => x.Active)
            .Include(x => x.LessonGradeTeachers.Where(y => !y.Deleted))
            .ThenInclude(x => x.LessonGrade)
            .ThenInclude(x => x.Grade)
            .Include(x => x.LessonGradeTeachers.Where(y => !y.Deleted))
            .ThenInclude(x => x.LessonGrade)
            .ThenInclude(x => x.Lesson)
            .Select(x => new TeacherDto
            {
                Id = x.Id, Name = x.Name, Surname = x.Surname, LessonGrades = x.LessonGradeTeachers
                    .Select(y => new LessonGradeDto
                        { Id = y.Id, Lesson = y.LessonGrade.Lesson.Name, Grade = y.LessonGrade.Grade.Value }).ToList()
            })
            .ToListAsync();

        return teachers;
    }

    public async Task<int> Create(SaveTeacherRequest request)
    {
        if (request is null)
            throw new BadHttpRequestException("Məlumatlar doldurulmayıb");

        var teacher = new Teacher
        {
            Name = request.Name,
            Surname = request.Surname
        };

        var newTeacher = await _teacherRepository.AddAsync(teacher);

        return newTeacher.Id;
    }

    public async Task CreateLessonGradeTeacher(int teacherId, List<SaveLessonGradeTeacherRequest> requests)
    {
        if (requests is null)
            throw new BadHttpRequestException("Məlumatlar doldurulmayıb");

        var isExistTeacher = await _teacherRepository.GetQuery().AnyAsync(x => x.Id == teacherId);

        if (!isExistTeacher)
            throw new NotFoundException("Müəllim mövcud deyil");

        var lessonGradeTeachers = new List<LessonGradeTeacher>();

        foreach (var request in requests)
        {
            await _lessonService.CheckById(request.LessonId);
            await _gradeService.CheckById(request.GradeId);

            var lessonGrade = await _lessonService.CheckByGradeId(request.LessonId, request.GradeId);

            var isExistLessonGradeTeacher = await _lessonGradeTeacherRepository.GetQuery()
                .AnyAsync(x => x.TeacherId == teacherId && x.LessonGradeId == lessonGrade.Id);

            if (isExistLessonGradeTeacher)
                throw new DuplicateConflictException(
                    $"Bu müəllim, seçdiyiniz dərs və sinif üçün daha əvvəl əlavə edilmişdir");

            lessonGradeTeachers.Add(new LessonGradeTeacher { TeacherId = teacherId, LessonGradeId = lessonGrade.Id });
        }

        await _lessonGradeTeacherRepository.AddRangeAsync(lessonGradeTeachers);
    }

    public async Task<TeacherDto> GetById(int teacherId)
    {
        var teacher = await _teacherRepository.GetQuery().Where(x => x.Active && x.Id == teacherId)
            .Include(x => x.LessonGradeTeachers)
            .ThenInclude(x => x.LessonGrade)
            .ThenInclude(x => x.Grade)
            .Include(x => x.LessonGradeTeachers)
            .ThenInclude(x => x.LessonGrade)
            .ThenInclude(x => x.Lesson)
            .Select(x => new TeacherDto
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                LessonGrades = x.LessonGradeTeachers
                    .Select(y => new LessonGradeDto
                        { Id = y.Id, Lesson = y.LessonGrade.Lesson.Name, Grade = y.LessonGrade.Grade.Value }).ToList()
            })
            .FirstOrDefaultAsync();

        if (teacher is null)
            throw new NotFoundException("Belə bir mmüəllim tapılmadı");

        return teacher;
    }

    public async Task<int> Update(int teacherId, List<UpdateTeacherRequest> requests)
    {
        var teacher = await _teacherRepository.GetQuery().Where(x => x.Active && x.Id == teacherId)
            .Include(x => x.LessonGradeTeachers)
            .ThenInclude(x => x.LessonGrade)
            .ThenInclude(x => x.Grade)
            .Include(x => x.LessonGradeTeachers)
            .ThenInclude(x => x.LessonGrade)
            .ThenInclude(x => x.Lesson)
            .FirstOrDefaultAsync();

        if (teacher is null)
            throw new NotFoundException("Belə bir mmüəllim tapılmadı");

        var newLessonGradeTeachers = new List<LessonGradeTeacher>();
        var deleteLessonGradeTeachers = new List<LessonGradeTeacher>();

        foreach (var request in requests)
        {
            await _lessonService.CheckById(request.LessonId);
            await _gradeService.CheckById(request.GradeId);

            var lessonGrade = await _lessonService.CheckByGradeId(request.LessonId, request.GradeId);

            var lessonGradeTeacher =
                teacher.LessonGradeTeachers.FirstOrDefault(x => x.LessonGradeId == lessonGrade.Id);

            if (lessonGradeTeacher is null)
            {
                newLessonGradeTeachers.Add(new LessonGradeTeacher
                    { TeacherId = teacherId, LessonGradeId = lessonGrade.Id });
            }
        }

        foreach (var lessonGradeTeacher in teacher.LessonGradeTeachers)
        {
            var lessonGradeTeacherCheck = requests.Any(x =>
                x.LessonId == lessonGradeTeacher.LessonGrade.LessonId &&
                x.GradeId == lessonGradeTeacher.LessonGrade.GradeId);

            if (!lessonGradeTeacherCheck)
                deleteLessonGradeTeachers.Add(lessonGradeTeacher);
        }

        await _lessonGradeTeacherRepository.BeginTransaction();

        await _lessonGradeTeacherRepository.AddRangeAsync(newLessonGradeTeachers);
        await _lessonGradeTeacherRepository.DeleteRangeAsync(deleteLessonGradeTeachers);

        await _lessonGradeTeacherRepository.Commit();

        return teacherId;
    }

    public async Task Delete(int teacherId)
    {
        var teacher = await _teacherRepository.GetQuery().Where(x => x.Active && x.Id == teacherId)
            .Include(x => x.LessonGradeTeachers)
            .ThenInclude(x => x.LessonGrade)
            .ThenInclude(x => x.Grade)
            .Include(x => x.LessonGradeTeachers)
            .ThenInclude(x => x.LessonGrade)
            .ThenInclude(x => x.Lesson)
            .FirstOrDefaultAsync();

        if (teacher is null)
            throw new NotFoundException("Belə bir mmüəllim tapılmadı");

        await _lessonGradeTeacherRepository.BeginTransaction();

        await _teacherRepository.DeleteAsync(teacher);
        await _lessonGradeTeacherRepository.DeleteRangeAsync(teacher.LessonGradeTeachers.ToList());

        await _lessonGradeTeacherRepository.Commit();
    }
}