using FluentValidation;

namespace ExamApp.Business.Models.Teachers;

public class SaveLessonGradeTeacherRequestValidator:  BaseValidator<SaveLessonGradeTeacherRequest>
{
    public SaveLessonGradeTeacherRequestValidator() : base()
    {
        RuleFor(e => e.GradeId)
            .NotEmpty().WithMessage("Sinif mütləq seçilməlidir");
        RuleFor(e => e.LessonId)
            .NotEmpty().WithMessage("Dərs mütləq seçilməlidir");
    }
}