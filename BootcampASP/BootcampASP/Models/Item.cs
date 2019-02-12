using BootcampASP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootcampASP.Models
{
    public class Item : BaseModel
    {
        public string Name { get; set; }
        public int Stok { get; set; }
        public int Price { get; set; } 
        public virtual Supplier Suppliers { get; set; }
    }
}