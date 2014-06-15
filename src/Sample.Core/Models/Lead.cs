using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sample.Core.Models
{
    public class Lead : Person, IValidatableObject
    {
        [DisplayName("Email")]
        [StringLength(100)]
        public string Email { get; set; }

        public Address Address { get; set; }

        [DisplayName("Phone")]
        [StringLength(20)]
        public string Phone { get; set; }

        [DisplayName("Description ")]
        [StringLength(1000)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IList<ValidationResult> result = new List<ValidationResult>();

            // Need an email, phone, or address, otherwise how will we contact them?
            bool hasEmail = !String.IsNullOrEmpty(Email);
            bool hasPhone = !String.IsNullOrEmpty(Phone);
            bool hasAddress =
                !String.IsNullOrEmpty(Address.City) &&
                !String.IsNullOrEmpty(Address.Street) &&
                !String.IsNullOrEmpty(Address.Zip);

            if (!hasEmail && !hasPhone && !hasAddress)
            {
                result.Add(
                    new ValidationResult(
                        "A Lead must have one of the following: a valid email, phone, or mailing address"));
            }

            if (!String.IsNullOrEmpty(Email))
            {
                var emailRegex =
                    new Regex(
                        @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
                if (!emailRegex.IsMatch(Email))
                {
                    result.Add(new ValidationResult("Email is invalid: " + Email, new[] { "Email" }));
                }
            }

            return result;
        }
    }
}