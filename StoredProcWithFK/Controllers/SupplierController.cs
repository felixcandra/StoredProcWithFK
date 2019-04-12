using Newtonsoft.Json;
using StoredProcWithFK.Controllers.Repository;
using StoredProcWithFK.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoredProcWithFK.Controllers
{
    public class SupplierController : Controller
    {
        SupplierRepository supplierRepo = new SupplierRepository(); 
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Create(Supplier supplier)
        {
            supplierRepo.Create(supplier);
        }

        [HttpPost]
        public void Edit(Supplier supplier)
        {
            supplierRepo.Edit(supplier);
        }

        [HttpPost]
        public bool Delete(int id)
        {
            return supplierRepo.Delete(id);
        }

        [HttpGet]
        public JsonResult GetSupplier()
        {
            return Json(supplierRepo.GetSupplier(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSupplierById(int id)
        {
            return Json(supplierRepo.GetSupplierById(id), JsonRequestBehavior.AllowGet);
        }
    }
}