using ExamApp.API.Services.Exams;
using ExamApp.API.Services.Grades;
using ExamApp.API.Services.Lessons;
using ExamApp.API.Services.Pupils;
using ExamApp.API.Services.Teachers;

namespace ExamApp.API.Installers;

public class ServiceInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGradeService, GradeManager>();
        services.AddScoped<ILessonService, LessonManager>();
        services.AddScoped<IPupilService, PupilManager>();
        services.AddScoped<ITeacherService, TeacherManager>();
        services.AddScoped<IExamService, ExamManager>();
        
    }
}