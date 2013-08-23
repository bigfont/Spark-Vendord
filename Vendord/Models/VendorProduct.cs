using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Vendord.DAL;

namespace Vendord.Models
{
    public class VendorProduct 
    {
        [Key]
        public int ID { get; set; }
        public int VendorID { get; set; }
        public int ProductID { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        [Display(Name = "Unit Price", Prompt = "0.00")]
        [DefaultValue(0.00)]
        public decimal UnitPrice { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

        [ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }

    public class VendorProductCreateViewModel : IValidatableObject
    {
        public int VendorID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            VendordContext db = new VendordContext();
            if (db.VendorProducts.Any(vp => vp.VendorID == this.VendorID && vp.ProductID == this.ProductID))
            {
                yield return new ValidationResult("The vendor already sells this product.");
            }
        }
    }
}