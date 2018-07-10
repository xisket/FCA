using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLayer.Models
{
    public class ProductProviderViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "ProductName")]
        public string ProductName { get; set;}
        public int ProviderId { get; set; }
        public string ProviderName { get;set; }

    }

}
