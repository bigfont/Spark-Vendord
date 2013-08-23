using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vendord.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name="Product Name")]
        public string Name { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }

        public virtual ICollection<VendorProduct> VendorProducts { get; set; }
    }
}