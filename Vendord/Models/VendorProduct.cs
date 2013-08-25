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
        public int ID { get; set; }
        public int VendorID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public byte[] Timestamp { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual Product Product { get; set; }
    }
}