namespace ExaminationSystem.ViewModels.ChoiceViewModel;

public class UpdateChoiceViewModel
{
    public string ChoiceText { get; set; } = null!;
    public bool IsCorrect { get; set; } = false;
    public int QuestionID { get; set; }

}
