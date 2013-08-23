using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vendord.DAL;

namespace Vendord.Models
{
    public class Vendor
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Vendor Name", Prompt = "Type the vendor name here.")]
        public string Name { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }

        public virtual ICollection<VendorProduct> VendorProducts { get; set; }
    
    }

    public class VendorCreateViewModel : IValidatableObject
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Vendor Name", Prompt = "Type the vendor name here.")]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            VendordContext db = new VendordContext();
            if (db.Vendors.Any(v => v.Name == this.Name))
            {
                yield return new ValidationResult("The Vendor Name must be unique.", new string[] { "Name" });
            }
        }
    }

}