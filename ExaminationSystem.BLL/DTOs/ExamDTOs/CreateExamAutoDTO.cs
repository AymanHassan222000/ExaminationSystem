namespace ExaminationSystem.DTOs.ExamDTOs;

public record CreateExamAutoDTO(
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


//{
//    public string Title { get; set; } 
//    public ExamTypes Type { get; set; }
//    public int DurationInMinutes { get; set; }
//    public DateTime Date { get; set; }
//    public TimeOnly StartTime { get; set; }
//    public TimeOnly EndTime { get; set; }
//    public int NumberOfSample { get; set; }
//    public int NumberOfMedium { get; set; }
//    public int NumberOfHard { get; set; }
//    public int CourseID { get; set; }
//}
