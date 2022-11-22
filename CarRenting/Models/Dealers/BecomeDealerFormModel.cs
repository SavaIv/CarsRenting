using System.ComponentModel.DataAnnotations;
using static CarRenting.Data.DataConstants.Dealer;

namespace CarRenting.Models.Dealers
{
    public class BecomeDealerFormModel
    {
        [Required]
        [StringLength(DealerNameMaxLength, MinimumLength = DealerNameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DealerPhoneMaxLength, MinimumLength = DealerPhoneMinLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
