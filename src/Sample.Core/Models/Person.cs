using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Core.Models
{
    public abstract class Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Column("MiddleName")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                var name = FirstName;
                if (!String.IsNullOrEmpty(MiddleName))
                {
                    name += " " + MiddleName;
                }
                name += " " + LastName;
                return name;
            }
        }
    }
}