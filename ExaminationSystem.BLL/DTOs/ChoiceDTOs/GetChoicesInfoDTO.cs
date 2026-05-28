namespace ExaminationSystem.DTOs.ChoiceDTOs
{
    public class GetChoicesInfoDTO
    {
        public int ID { get; set; } 
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
