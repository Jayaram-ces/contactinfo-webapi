using System.ComponentModel.DataAnnotations;

namespace ContactInfoApi.Model
{
    public class ContactInfoModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }
    }
}
