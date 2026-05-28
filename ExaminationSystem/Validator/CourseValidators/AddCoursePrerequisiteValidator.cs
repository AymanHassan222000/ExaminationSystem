using ExaminationSystem.API.ViewModels.CourseViewModels;
using FluentValidation;

namespace ExaminationSystem.API.Validator.CourseValidators;

public class AddCoursePrerequisiteValidator : AbstractValidator<AddCoursePrerequisiteViewModel>
{
    public AddCoursePrerequisiteValidator()
    {
        //MainCourseID Validation
        RuleFor(c => c.MainCourseID)
            .NotNull().WithMessage("Main Course ID Is Required")
            .Must(id => id > 0).WithMessage("No Course With ID 0 Was Found");

        //Prerequisites Validation
        RuleFor(c => c.Prerequisites)
            .NotNull().WithMessage("Prerequisite IDs Are Required")
            .Must(p => p.Count > 0).WithMessage("At Least One Prerequisite Course Is Required")
            .Must(p => p.All(id => id.PrerequisiteCourseID > 0)).WithMessage("No Course With ID 0 Was Found");
    }
}
