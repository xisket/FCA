using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLayer.Models
{
    public class ProviderViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long and a maximum of 100.", MinimumLength = 1)]
        [Display(Name = "Name")]
        public string Name { get; set; }
         
    }
     
}
