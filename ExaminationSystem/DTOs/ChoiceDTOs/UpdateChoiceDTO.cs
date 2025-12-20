namespace ExaminationSystem.DTOs.ChoiceDTOs;

public class UpdateChoiceDTO
{
    public string ChoiceText { get; set; } = null!;
    public bool IsCorrect { get; set; } = false;
    public int QuestionID { get; set; }
}
