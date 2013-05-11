using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NF.WonenInZwaluwpark.Mvc3.Models
{
    //[MetadataType(typeof(UserProfileMD))]
    public partial class UserProfile
    {
        public class UserProfileMD
        {
            [DisplayName("E-mailadres")]
            [Required(ErrorMessage = "E-mailadres mag niet leeg zijn")]
            [Email(ErrorMessage = "E-mailadres ongeldig")]
            public string EmailAddress { get; set; }

            [DisplayName("Je naam")]
            [Required(ErrorMessage = "Je naam mag niet leeg zijn")]
            [StringLength(100, ErrorMessage = "Je naam mag maximaal 100 karakters lang zijn")]
            public string FriendlyName { get; set; }

            [DisplayName("Bouwnummer")]
            [Required(ErrorMessage = "Bouwnummer mag niet leeg zijn")]
            [Range(1, 104, ErrorMessage = "Bouwnummer moet tussen 1 en 104 liggen")]
            public int HouseNbr { get; set; }
        }
    }
}