using FluentValidation;

namespace ExamApp.API.Models.Lessons;

public class SaveLessonRequestValidator : BaseValidator<SaveLessonRequest>
{
    public SaveLessonRequestValidator() : base()
    {
        RuleFor(e => e.Code)
            .NotEmpty().WithMessage("Dərsin kodu boş ola bilməz")
            .Length(0, 3).WithMessage("Dərs kodunun uzunluğu ən çox 3 ola bilər");
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Dərsin adı boş ola bilməz")
            .Length(0, 30).WithMessage("Dərsin adı ən çox 30 simvol ola bilər");
        RuleFor(e => e.GradeIds)
            .NotNull().WithMessage("Dərs üçün sinif seçilməlidir");
    }
}