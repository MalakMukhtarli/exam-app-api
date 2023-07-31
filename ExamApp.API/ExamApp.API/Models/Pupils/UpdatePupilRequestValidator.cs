using FluentValidation;

namespace ExamApp.API.Models.Pupils;

public class UpdatePupilRequestValidator :  BaseValidator<UpdatePupilRequest>
{
    public UpdatePupilRequestValidator() : base()
    {
        RuleFor(e => e.GradeId)
            .NotNull().WithMessage("Şagirdin sinifi boş ola bilməz");
    }
}