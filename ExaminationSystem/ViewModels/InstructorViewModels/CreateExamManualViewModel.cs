using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.ViewModels.InstructorViewModels;

public class CreateExamManualViewModel
{
    [Required(ErrorMessage = "Exam Title is Required")]
    [MinLength(2)]
    public string Title { get; set; } = null!;

    [Required]
    [EnumDataType(typeof(ExamTypes),ErrorMessage = "Exam Type is Required")]
    public ExamTypes Types { get; set; }

    [Required]
    public int DurationInMinutes { get; set; }

    [Required]
    public List<int> QuestionIDs { get; set; } = new();

    public int CourseID { get; set; }

}
