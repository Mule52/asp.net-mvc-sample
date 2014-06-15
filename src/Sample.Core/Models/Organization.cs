using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sample.Core.Models
{
    public class Organization : IValidatableObject
    {
        public Organization()
        {
            Users = new HashSet<User>();
            Address = new Address();
            SessionTimeoutMinutes = 10; // Default to 10 minutes.
        }

        [Key]
        [ScaffoldColumn(false)]
        public long Id { get; set; }

        [StringLength(255)]
        [DisplayName("Name")]
        public string Name { get; set; }

        public Address Address { get; set; }

        [DisplayName("Primary phone")]
        [StringLength(20)]
        public string Phone { get; set; }

        [DisplayName("Primary fax")]
        [StringLength(20)]
        public string Fax { get; set; }

        [DisplayName("Description ")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Range(1, 20)]
        public int SessionTimeoutMinutes { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IList<ValidationResult> result = new List<ValidationResult>();

            // For Community Profile, if it is not a region base we then not validate the address.
            if (String.IsNullOrEmpty(Name))
            {
                result.Add(new ValidationResult("Name is required", new[] {String.Format("{0}.Name", Name)}));
            }

            return result;
        }
    }
}