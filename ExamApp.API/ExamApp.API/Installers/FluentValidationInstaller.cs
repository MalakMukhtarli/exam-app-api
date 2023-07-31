using FluentValidation;
using FluentValidation.AspNetCore;

namespace ExamApp.API.Installers;

public class FluentValidationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<IInstaller>();
    }
}