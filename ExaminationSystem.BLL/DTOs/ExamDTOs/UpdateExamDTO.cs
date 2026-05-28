namespace ExaminationSystem.DTOs.ExamDTOs;

public sealed class UpdateExamDTO
{
    public int ID { get; set; }
    public string? Title { get; set; } = null;
    public ExamTypes? Type { get; set; }
    public int? DurationInMinutes { get; set; }
    public DateTime? Date { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
}
