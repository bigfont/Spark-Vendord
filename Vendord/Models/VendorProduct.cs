using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vendord.Models
{
    public class VendorProduct
    {
        [Key]
        public int ID { get; set; }
        public int VendorID { get; set; }
        public int ProductID { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

        [ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}