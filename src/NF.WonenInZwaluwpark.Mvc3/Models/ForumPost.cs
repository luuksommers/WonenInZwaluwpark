using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NF.WonenInZwaluwpark.Mvc3.Models
{
    [MetadataType(typeof(ForumPostMD))]
    public partial class ForumPost
    {
        public class ForumPostMD
        {
            [DisplayName("Titel")]
            [Required(ErrorMessage="Titel mag niet leeg zijn")]
            [StringLength(80, ErrorMessage="Titel mag maximaal 80 karakters lang zijn")]
            public string Title { get; set; }

            [DisplayName("Bericht")]
            [Required(ErrorMessage = "Bericht mag niet leeg zijn")]
            [StringLength(1000, ErrorMessage = "Bericht mag maximaal 1000 karakters lang zijn")]
            public string Content { get; set; }
        }
    }
}