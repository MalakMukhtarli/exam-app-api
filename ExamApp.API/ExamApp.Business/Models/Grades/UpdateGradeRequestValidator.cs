using FluentValidation;

namespace ExamApp.Business.Models.Grades;

public class UpdateGradeRequestValidator : BaseValidator<UpdateGradeRequest>
{
    public UpdateGradeRequestValidator() : base()
    {
        RuleFor(e => e.Grade)
            .NotEmpty().WithMessage("Sinif boş ola bilməz")
            .Must(w => w.ToString().Length <= 2).WithMessage("Sinifin uzunluğu ən çox 2 ola bilər");
    }
}