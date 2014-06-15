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

        [Required]
        [Display(Name = "User Name (email address)")]
        public override string Email { get; set; }

        //[Display(Name = "Created")]
        //[JsonIgnore]
        //public DateTime CreateDate { get; set; }

        //[Display(Name = "Created By")]
        //[StringLength(50)]
        //[JsonIgnore]
        //public string CreatedBy { get; set; }

        //[Display(Name = "Modified")]
        //[JsonIgnore]
        //public DateTime ModifiedDate { get; set; }

        //[Display(Name = "Modified By")]
        //[StringLength(50)]
        //[JsonIgnore]
        //public string ModifiedBy { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }

        public DateTime? PasswordExpiresDate { get; set; }

        public DateTime? DisabledDateTime { get; set; }

        //public UserPreference UserPreferences { get; set; }

        //public bool IsInOrg(ITenant tenant)
        //{
        //    return OrganizationId == tenant.Id;
        //}

        //public bool IsExpired()
        //{
        //    if (PasswordExpiresDate == null)
        //        return true;
        //    return PasswordExpiresDate < DateTime.UtcNow;
        //}
    }
}