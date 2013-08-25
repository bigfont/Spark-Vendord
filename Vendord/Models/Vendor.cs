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
        public int ID { get; set; }
        public string Name { get; set; }
        public Byte[] TimeStamp { get; set; }

        public virtual ICollection<VendorProduct> VendorProducts { get; set; }
    
    }
}