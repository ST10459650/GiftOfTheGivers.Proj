using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Web.Models
{
    public class DonationModel
    {

        public int DonationId { get; set; }

        [Required(ErrorMessage = "Please enter an amount.")]
        [Range(1, 1000000, ErrorMessage = "Donation amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR"; // ZAR, USD, EUR

        [Required]
        public string Frequency { get; set; } = "One-Time"; // One-Time, Monthly

        [Display(Name = "Donor Name")]
        public string? DonorName { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address (For Tax Receipt)")]
        public string? DonorEmail { get; set; }

        [Display(Name = "Donate Anonymously")]
        public bool IsAnonymous { get; set; }

        [Display(Name = "Target Relief Project")]
        public string? TargetProject { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.Now;
        public string CertificateNumber { get; set; } = string.Empty;
    }
}
