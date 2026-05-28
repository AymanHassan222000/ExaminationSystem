using ExaminationSystem.ViewModels.QuestionViewModel;
using FluentValidation;

namespace ExaminationSystem.Validator.QuestionValidators;

public class UpdateQuestionDtoValidator : AbstractValidator<UpdateQuestionViewModel>
{
    public UpdateQuestionDtoValidator()
    {
        //Head Validation
        RuleFor(q => q.Head)
            .MinimumLength(3).WithMessage("Minimum length is 3 characters.")
            .MaximumLength(200).WithMessage("Maximum length is 200 characters.")
            .When(q => q.Head != null);

        //Grad Validation
        RuleFor(q => q.Grade)
            .Must(g => g > 1)
            .WithMessage("The grade can't be less than 1 degre.")
            .When(q => q.Grade.HasValue);

        //Question Level Validation
        RuleFor(q => q.Level)
            .IsInEnum()
            .When(q => q.Level.HasValue)
            .WithMessage("Invalid level in list");
    }
}
