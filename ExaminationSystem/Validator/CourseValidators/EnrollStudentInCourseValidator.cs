using ExaminationSystem.API.ViewModels.CourseViewModels;
using FluentValidation;

namespace ExaminationSystem.API.Validator.CourseValidators
{
    public class EnrollStudentInCourseValidator: AbstractValidator<EnrollStudentInCourseViewModel>
    {
        public EnrollStudentInCourseValidator()
        {
            // Validate that StudentID is greater than 0
            RuleFor(x => x.StudentID)
                .GreaterThan(0).WithMessage("StudentID must be greater than 0.");

            // Validate that CourseID is greater than 0
            RuleFor(x => x.CourseID)
                .GreaterThan(0).WithMessage("CourseID must be greater than 0.");
        }
    }
}
