using FluentValidation;

namespace ExamApp.API.Models.Teachers;

public class SaveTeacherRequestValidator:  BaseValidator<SaveTeacherRequest>
{
    public SaveTeacherRequestValidator() : base()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Müəllimin adı boş ola bilməz")
            .Length(0, 20).WithMessage("Müəllimin adı ən çox 20 simvol ola bilər");
        RuleFor(e => e.Surname)
            .NotEmpty().WithMessage("Müəllimin soyadı boş ola bilməz")
            .Length(0, 20).WithMessage("Müəllimin soyadı ən çox 20 simvol ola bilər");
    }
}