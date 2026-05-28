using ExaminationSystem.ViewModels.CourseViewModels;
using FluentValidation;

namespace ExaminationSystem.Validator.CourseValidators;

public class UpdateCourseValidator : AbstractValidator<UpdateCourseViewModel>
{
    public UpdateCourseValidator()
    {
        //Name Validation
        RuleFor(c => c.Name)
            .MaximumLength(200).WithMessage("Max Length Is 200 Character")
            .MinimumLength(2).WithMessage("Minimum Length Is 2 Character");

        //Description Validation
        RuleFor(c => c.Description)
            .MinimumLength(10).WithMessage("Minimum Length Is 10 Character")
            .MaximumLength(2000).WithMessage("MaxLength Is 2000 Character");

        //Hours Validation
        RuleFor(c => c.Hours)
            .Must(h => h == null || h > 0).WithMessage("Couse Hourse Must Be Greeter Than 0");

        
    }
}
