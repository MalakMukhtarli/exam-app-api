using FluentValidation;

namespace ExamApp.API.Models.PupilExams;

public class UpdatePupilExamRequestValidator : BaseValidator<UpdatePupilExamRequest>
{
    public UpdatePupilExamRequestValidator() : base()
    {
        RuleFor(e => e.PupilExamId)
            .NotEmpty().WithMessage("PupilExamId boş ola bilməz");
        RuleFor(e => e.Mark)
            .NotNull().WithMessage("Dərs üçün sinif seçilməlidir")
            .Must(w => w.ToString().Length <= 1).WithMessage("Qiymətin uzunluğu ən çox 1 ola bilər");
    }
}