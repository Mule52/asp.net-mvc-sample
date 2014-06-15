using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Sample.Core.Models
{
    [ComplexType]
    public class Address : IValidatableObject
    {
        [MaxLength(1000)]
        [DataType(DataType.Text)]
        [Display(Name = "Street address")]
        public string Street { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Unit # or Apt.")]
        public string Unit { get; set; }

        [MaxLength(255)]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        [MaxLength(255)]
        [DataType(DataType.Text)]
        [Display(Name = "State or region")]
        public string State { get; set; }

        [MaxLength(50)]
        [DataType(DataType.PostalCode, ErrorMessage = "Invalid zip code")]
        [Display(Name = "Zip or postal code")]
        public string Zip { get; set; }

        [MaxLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            // if street is not null then enforce validation for everything.
            if (!String.IsNullOrEmpty(Street))
            {
                var zipRegex = new Regex(@"^\d{5}(?:[-\s]\d{4})?$");
                if (!zipRegex.IsMatch(Zip))
                {
                    result.Add(new ValidationResult("Zip code is invalid"));
                }
            }
            return result;
        }
    }
}