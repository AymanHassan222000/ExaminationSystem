namespace ExaminationSystem.API.ViewModels.ExamViewModels;

public record CreateExamManualViewModel(
    string Title,
    ExamTypes Type,
    int CourseID,
    int DurationInMinutes,
    DateTime Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    IList<int> QuestionIDs
);



