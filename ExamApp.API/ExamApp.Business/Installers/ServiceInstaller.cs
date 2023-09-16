using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExamApp.Business.Installers;

public class ServiceInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        // services.AddScoped<IGradeService, GradeManager>();
        // services.AddScoped<ILessonService, LessonManager>();
        // services.AddScoped<IPupilService, PupilManager>();
        // services.AddScoped<ITeacherService, TeacherManager>();
        // services.AddScoped<IExamService, ExamManager>();
        
    }
}