using ExamApp.API.Repository;
using ExamApp.API.Repository.Concrete;

namespace ExamApp.API.Installers;

public class RepositoryInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryBase<>));
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IPupilRepository, PupilRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();

    }
}