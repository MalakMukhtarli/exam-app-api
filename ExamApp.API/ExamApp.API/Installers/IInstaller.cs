namespace ExamApp.API.Installers;

public interface IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration);
}