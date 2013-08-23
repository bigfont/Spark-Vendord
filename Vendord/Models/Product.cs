using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vendord.DAL;

namespace Vendord.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Product Name", Prompt = "Type the product name here.")]
        public string Name { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }

        public virtual ICollection<VendorProduct> VendorProducts { get; set; }
    }

    public class ProductCreateViewModel : IValidatableObject
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Product Name", Prompt = "Type the product name here.")]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            VendordContext db = new VendordContext();
            if (db.Products.Any(p => p.Name == this.Name))
            {
                yield return new ValidationResult("The Product Name must be unique.", new string[] { "Name" });
            }
        }
    }
}