using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Sample.Core.Models
{
    public class User : IdentityUser
    {
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        [Required(ErrorMessage = "A First Name is required")]
        [StringLength(160)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "A Last Name is required")]
        [StringLength(160)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [ForeignKey("Organization")]
        public long? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }

        public DateTime? PasswordExpiresDate { get; set; }

        public DateTime? DisabledDateTime { get; set; }
    }
}