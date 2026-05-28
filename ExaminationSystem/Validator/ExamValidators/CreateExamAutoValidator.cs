using ExaminationSystem.API.ViewModels.ExamViewModels;
using FluentValidation;

namespace ExaminationSystem.API.Validator.ExamValidators;

public class CreateExamAutoValidator : AbstractValidator<CreateExamAutoViewModel>
{
    public CreateExamAutoValidator()
    {
        // Title Validation
        RuleFor(e => e.Title)
            .NotNull().WithMessage("Exam title is required.")
            .MinimumLength(5).WithMessage("Minimum length is 5 characters")
            .MaximumLength(150).WithMessage("Maximum length is 150 characters");

        // Exam Type Validation
        RuleFor(e => e.Type)
            .NotNull().WithMessage("Exam type is required.")
            .IsInEnum().WithMessage("Invalid exam type.");

        // Course ID Validation
        RuleFor(e => e.CourseID)
            .GreaterThan(0).WithMessage("No course id with ID `0`.");

        // Exam Duration Validation
        RuleFor(e => e.DurationInMinutes)
            .GreaterThan(1).WithMessage("Exam duration must be greater than 1 minute.");

        // Exam Date Validation
        RuleFor(e => e.Date)
            .Must(e => e > DateTime.Now).WithMessage("Exam date must be greate than date time for now.");

        //Start date Validation
        RuleFor(e => e.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        //End date Validation 
        RuleFor(e => e.EndTime)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(e => e.StartTime).WithMessage("End than must be greate than start time.");

        //Question validation
        RuleFor(e => e)
            .Must(e =>
                e.NumberOfSample +
                e.NumberOfMedium +
                e.NumberOfHard > 0)
            .WithMessage("Exam must contain at least one question.");

        //Course ID Validation
        RuleFor(e => e.CourseID)
            .GreaterThan(0).WithMessage("Course id must be greater than 0.");
    }
}
