namespace ExaminationSystem.BLL.DTOs.ChoiceDTOs;

public class GetChoicesInfoForEvaluation
{
    public int ChoiceID { get; set; }
    public int QuestionID { get; set; }
    public bool IsCorrect { get; set; } = false;
}
