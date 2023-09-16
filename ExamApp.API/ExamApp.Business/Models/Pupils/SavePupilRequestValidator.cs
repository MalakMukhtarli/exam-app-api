using FluentValidation;

namespace ExamApp.Business.Models.Pupils;

public class SavePupilRequestValidator :  BaseValidator<SavePupilRequest>
{
    public SavePupilRequestValidator() : base()
    {
        RuleFor(e => e.Number)
            .NotNull().WithMessage("Şagirdin nömrəsi boş ola bilməz")
            .Must(w => w.ToString().Length < 5).WithMessage("Şagirdin nömrəsinin uzunluğu ən çox 5 ola bilər");
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Şagirdin adı boş ola bilməz")
            .Length(0, 30).WithMessage("Şagirdin adı ən çox 30 simvol ola bilər");
        RuleFor(e => e.Surname)
            .NotEmpty().WithMessage("Şagirdin soyadı boş ola bilməz")
            .Length(0, 30).WithMessage("Şagirdin soyadı ən çox 30 simvol ola bilər");
        RuleFor(e => e.GradeId)
            .NotNull().WithMessage("Şagirdin sinifi boş ola bilməz");
    }

}