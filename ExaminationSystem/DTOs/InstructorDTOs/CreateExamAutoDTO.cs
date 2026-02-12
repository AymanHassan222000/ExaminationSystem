namespace ExaminationSystem.DTOs.InstructorDTOs;

public class CreateExamAutoDTO
{
    public string Title { get; set; } = null!;
    public ExamTypes Types { get; set; }
    public int NumberOfQuestions { get; set; }
    public int DurationInMinutes { get; set; }

    public int? NumberOfSample { get; set; }
    public int? NumberOfMedium { get; set; }
    public int? NumberOfHard { get; set; }

    public int CourseID { get; set; }
}
