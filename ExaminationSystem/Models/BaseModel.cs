using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Models;

public class BaseModel
{
    [Key]
    public int ID { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}
