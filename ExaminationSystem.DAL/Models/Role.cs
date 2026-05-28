namespace ExaminationSystem.DAL.Models;

public class Role : BaseModel
{
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}
