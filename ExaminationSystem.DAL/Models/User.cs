using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models
{
    public class User : BaseModel
    {
        public required string FirstName { get; set; } 

        public required string LastName { get; set; }

        public required string PhoneNumber { get; set; } 

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; } = true;

        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Roles { get; set; }
        
        public Instructor Instructor { get; set; }
        public Student Student { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
