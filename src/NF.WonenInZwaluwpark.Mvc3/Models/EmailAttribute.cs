using System.ComponentModel.DataAnnotations;

namespace NF.WonenInZwaluwpark.Mvc3.Models
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")
        {
        }
    }
}