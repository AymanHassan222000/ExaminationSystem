namespace ExaminationSystem.ViewModels.ChoiceViewModel;

public class ChoiceDetailseViewModel
{
    public int ChoiceID { get; set; }
    public string ChoiceText { get; set; } = null!;
    public bool IsCorrect { get; set; } = false;
    public int QuestionID { get; set; }

}
