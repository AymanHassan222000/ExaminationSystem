namespace ExaminationSystem.DTOs.ExamDTOs;

public record CreateExamManualDTO(
    string Title,
    ExamTypes Type,
    int CourseID,
    int DurationInMinutes,
    DateTime Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    IList<int> QuestionIDs
);
