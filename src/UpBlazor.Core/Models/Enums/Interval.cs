using System.ComponentModel.DataAnnotations;

namespace UpBlazor.Core.Models.Enums
{
    public enum Interval
    {
        [Display(Name = "Days")]
        Daily,
        [Display(Name = "Weeks")]
        Weekly,
        [Display(Name = "Fortnights")]
        Fortnightly,
        [Display(Name = "Months")]
        Monthly,
        [Display(Name = "Years")]
        Yearly
    }
}