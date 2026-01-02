namespace ExaminationSystem.DTOs.InstructorDTOs;

public class AssignQuestionsToExamDTO
{
    public int ExamID { get; set; }
    public List<int> QuestionIDs { get; set; } = new();
}
