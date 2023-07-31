using FluentValidation;

namespace ExamApp.API.Models.Exams;

public class SaveExamRequestValidator : BaseValidator<SaveExamRequest>
{
    public SaveExamRequestValidator() : base()
    {
        RuleFor(e => e.ExamDate)
            .NotNull().WithMessage("İmtahan saatı boş ola bilməz");
        RuleFor(e => e.GradeId)
            .NotEmpty().WithMessage("Sinif boş ola bilməz");
        RuleFor(e => e.LessonId)
            .NotNull().WithMessage("Dərs boş ola bilməz");
    }
}