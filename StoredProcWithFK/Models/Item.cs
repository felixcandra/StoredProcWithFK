using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoredProcWithFK.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public virtual Supplier Suppliers { get; set; }
    }
}