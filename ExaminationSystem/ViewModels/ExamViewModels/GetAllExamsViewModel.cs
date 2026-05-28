namespace ExaminationSystem.API.ViewModels.ExamViewModels;

public record GetAllExamsViewModel(
    int ID,
    string Title,
    ExamTypes Type,
    int DurationInMinutes,
    DateTime Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string CourseName

);


