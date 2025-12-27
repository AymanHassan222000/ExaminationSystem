namespace ExaminationSystem.DTOs.ChoiceDTOs;

public class ChoiceDetailsDTO
{
    public int ID { get; set; }
    public string ChoiceText { get; set; } = null!;
    public bool IsCorrect { get; set; } = false;
    public int QuestionID { get; set; }
}
