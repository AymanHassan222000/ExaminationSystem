namespace ExaminationSystem.DAL.Models;

public class RefreshToken : BaseModel
{
    public string TokenHash { get; set; }

    public DateTime ExpiresOn { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    public DateTime? RevokedOn { get; set; }
    public bool IsActive => RevokedOn == null && !IsExpired; 

    public int UserID { get; set; }
    public User User { get; set; }
}
