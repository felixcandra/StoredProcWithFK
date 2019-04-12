using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoredProcWithFK.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        [Display(Name = "Supplier Name")]
        public string Name { get; set; }
    }
}