using ExaminationSystem.API.ViewModels.CourseViewModels;
using FluentValidation;

namespace ExaminationSystem.API.Validator.CourseValidators;

public class RemoveCoursePrerequisiteValidator : AbstractValidator<RemoveCoursePrerequisiteViewModel>
{
    public RemoveCoursePrerequisiteValidator()
    {
        //MainCourseID Validation
        RuleFor(c => c.courseID)
            .GreaterThan(0).WithMessage("Invalid course id.");

        //Prerequisites Validation
        RuleFor(c => c.PrerequisiteID)
            .GreaterThan(0).WithMessage("Invalid prerequist id.");
    }
}
