using ExaminationSystem.ViewModels.CourseViewModels;
using FluentValidation;

namespace ExaminationSystem.Validator.CourseValidators;

public class AddCourseValidator : AbstractValidator<AddCourseViewModel>
{
    public AddCourseValidator()
    {
        //Name Validation
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Course Name Is Required")
            .MaximumLength(200).WithMessage("Max Length Is 200 Character")
            .MinimumLength(2).WithMessage("Minimum Length Is 2 Character");

        //Description Validation
        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Course Description Is Required")
            .MinimumLength(10).WithMessage("Minimum Length Is 10 Character")
            .MaximumLength(2000).WithMessage("MaxLength Is 2000 Character");

        //Hours Validation
        RuleFor(c => c.Hours)
            .NotNull().WithMessage("Hours Is Required")
            .Must(h => h > 0).WithMessage("Couse Hourse Must Be Greeter Than 0");

        //InstructoID Validation
        RuleFor(c => c.InstructorID)
            .NotNull().WithMessage("Instructor ID Is Required")
            .Must(id => id > 0).WithMessage("No Instructor With ID 0 Was Found");
    }
}
