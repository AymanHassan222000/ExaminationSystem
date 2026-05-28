using ExaminationSystem.ViewModels.ExamViewModels;
using FluentValidation;

namespace ExaminationSystem.Validator.ExamValidators;

public class UpdateExamValidator : AbstractValidator<UpdateExamViewModel>
{
    public UpdateExamValidator()
    {
        // Title Validation
        When(e => e.Title is not null, () =>
        {
            RuleFor(e => e.Title)
                .MinimumLength(5).WithMessage("Minimum length is 5 characters.")
                .MaximumLength(150).WithMessage("Maximum length is 150 characters.");
        });


        // Exam Type Validation
        When(e => e.Type.HasValue, () =>
        {
            RuleFor(e => e.Type)
                .IsInEnum().WithMessage("Invalid exam type.");
        });


        // Exam Duration Validation
        When(e => e.DurationInMinutes.HasValue, () =>
        {
            RuleFor(e => e.DurationInMinutes)
                .GreaterThan(1)
                .WithMessage("Exam duration must be greater than 1 minute.");
        });


        // Exam Date Validation
        When(e => e.Date.HasValue, () =>
        {
            RuleFor(e => e.Date)
                .Must(date => date > DateTime.UtcNow)
                .WithMessage("Exam date must be greater than current date.");
        });


        // Start Time Validation
        When(e => e.StartTime.HasValue, () =>
        {
            RuleFor(e => e.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.");
        });


        // End Time Validation
        When(e => e.EndTime.HasValue, () =>
        {
            RuleFor(e => e.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(e => e.StartTime)
                .WithMessage("End time must be greater than start time.");
        });

    }
}
