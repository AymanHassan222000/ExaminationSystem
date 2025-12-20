using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    public class ExamQuestion : BaseModel
    {
        public int ExamID { get; set; }
        public int QuestionID { get; set; }

        [ForeignKey("ExamID")]
        public Exam Exam { get; set; }

        [ForeignKey("QuestionID")]
        public Question Question { get; set; }
    }
}
