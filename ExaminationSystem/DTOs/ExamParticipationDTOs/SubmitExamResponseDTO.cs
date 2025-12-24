namespace ExaminationSystem.DTOs.ExamParticipationDTOs;

public class SubmitExamResponseDTO
{
    public int ExamAttemptID { get; set; }
    public bool IsSubmitted { get; set; }
    public DateTime SubmittedAt { get; set; }
}
