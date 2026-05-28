namespace ExaminationSystem.API.ViewModels.ExamViewModels;

public record CreateExamAutoViewModel (
    string Title,
    ExamTypes Type,
    int DurationInMinutes,
    DateTime Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int NumberOfSample,
    int NumberOfMedium,
    int NumberOfHard,
    int CourseID
);
