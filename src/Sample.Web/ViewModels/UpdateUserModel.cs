using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sample.Web.ViewModels
{
    public class UpdateUserModel : IValidatableObject
    {
        [Required]
        [DisplayName("Id")]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email (User Name)")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IList<ValidationResult> result = new List<ValidationResult>();

            if (!String.IsNullOrEmpty(UserName))
            {
                var emailRegex =
                    new Regex(
                        @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
                if (!emailRegex.IsMatch(UserName))
                {
                    result.Add(new ValidationResult("UserName is invalid: " + UserName, new[] { "Email" }));
                }
            }

            // TODO: Add more password validation
            if (!String.IsNullOrEmpty(NewPassword) && NewPassword != ConfirmPassword)
            {
                result.Add(new ValidationResult("Password", new[] { "ConfirmPassword" }));
            }
            return result;
        }
    }
}