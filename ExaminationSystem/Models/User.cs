using ExaminationSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Models
{
    public class User : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(12)]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public UserRoles Role { get; set; } = UserRoles.Student;

        public Instructor Instructor { get; set; }
        public Student Student { get; set; }

    }
}
