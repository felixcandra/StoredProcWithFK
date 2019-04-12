using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoredProcWithFK.Models.Param
{
    public class ItemParam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Suppliers_Id { get; set; }
    }
}