namespace ExaminationSystem.DTOs.ExamParticipationDTOs;

public class SubmitExamRequestDTO
{
    public int ExamAttempitID { get; set; }
    public List<SubmitExamAnswerDTO> Answers { get; set; }
}
