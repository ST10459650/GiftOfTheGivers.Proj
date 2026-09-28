using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Web.Models
{
    public class VolunteerRegistrationModel
    {

        [Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Province / Region")]
        public string Province { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select at least one primary skill.")]
        [Display(Name = "Primary Skill / Field of Expertise")]
        public string PrimarySkill { get; set; } = string.Empty;

        [Required]
        public string Availability { get; set; } = "Weekends"; // Weekends, On-Call Emergency, Full-Time

        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }
}
