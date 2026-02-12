namespace ExaminationSystem.DTOs.InstructorDTOs;

public class CreateExamManualDTO
{
    public string Title { get; set; } = null!;
    public ExamTypes Types { get; set; }
    public int NumberOfQuestions { get; set; }

    public List<int> QuestionIDs { get; set; } = new();

    public int CourseID { get; set; }
}
