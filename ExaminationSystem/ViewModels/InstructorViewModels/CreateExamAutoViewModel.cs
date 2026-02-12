using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.ViewModels.InstructorViewModels;

public class CreateExamAutoViewModel
{
    [Required(ErrorMessage = "Exam Title is Required")]
    [MinLength(2)]
    public string Title { get; set; } = null!;

    [Required]
    [EnumDataType(typeof(ExamTypes), ErrorMessage = "Exam Type is Required")]
    public ExamTypes Types { get; set; }

    [Required]
    public int NumberOfQuestions { get; set; }

    [Required]
    public int DurationInMinutes { get; set; }

    public int? NumberOfSample { get; set; }
    public int? NumberOfMedium { get; set; }
    public int? NumberOfHard { get; set; }

    public int CourseID { get;set; }

}
