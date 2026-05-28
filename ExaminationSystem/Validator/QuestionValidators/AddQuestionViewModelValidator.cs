using ExaminationSystem.ViewModels.QuestionViewModel;
using FluentValidation;

namespace ExaminationSystem.Validator.QuestionValidators;

public class AddQuestionViewModelValidator : AbstractValidator<AddQuestionViewModel>
{
    public AddQuestionViewModelValidator()
    {
        //Head Validation
        RuleFor(q => q.Head)
            .NotNull().WithMessage("Question head is required.")
            .MinimumLength(3).WithMessage("Minimum length is 3 characters.")
            .MaximumLength(200).WithMessage("Maximum length is 200 characters.");

        //Grad Validation
        RuleFor(q => q.Grade)
            .Must(g => g > 1).WithMessage("The grade can't be less than 1 degre.");

        //Question Level Validation
        RuleFor(q => q.Level)
            .IsInEnum().WithMessage("Invalid level in list");

        //CourseID Validation
        RuleFor(q => q.CourseID)
            .Must(courseID => courseID >= 1).WithMessage("Invalid course ID.");

        //Choices Validation
        RuleFor(q => q.Choices)
            .NotNull()
            .WithMessage("Choices is required.")
            .DependentRules(() =>
            {
                RuleFor(q => q.Choices!)
                    .Must(c => c.Count >= 2)
                    .WithMessage("Minimum number of choices is 2.");

                RuleFor(q => q.Choices!)
                    .Must(c => c.Any(choice => choice.IsCorrect))
                    .WithMessage("At least one correct choice is required.");

                RuleFor(q => q.Choices!)
                    .Must(c => c.Count(choice => choice.IsCorrect) == 1)
                    .WithMessage("Only one correct choice is allowed.");
            });
    }
}
