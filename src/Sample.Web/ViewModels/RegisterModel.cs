using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sample.Web.ViewModels
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email (User Name)")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }
}