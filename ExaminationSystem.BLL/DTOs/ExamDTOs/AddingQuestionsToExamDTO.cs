namespace ExaminationSystem.DTOs.ExamDTOs;

public class AddingQuestionsToExamDTO
{
    public int ExamID { get; set; }
    public IList<int> QuestionIDs { get; set; } = new List<int>();
}
