using Newtonsoft.Json;
using StoredProcWithFK.Models;
using StoredProcWithFK.Models.Param;
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
    public class ItemController : Controller
    {
        ItemRepository itemRepo = new ItemRepository();
        // GET: Item
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Create(ItemParam item)
        {
            if (ModelState.IsValid)
            {
                itemRepo.Create(item);
            }
        }

        [HttpPost]
        public void Edit(ItemParam item)
        {
            itemRepo.Edit(item);
        }

        [HttpPost]
        public bool Delete(int id)
        {
           return itemRepo.Delete(id);
        }

        public JsonResult GetSupplier()
        {
            return Json(itemRepo.GetSupplier(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItem()
        {
            return Json(itemRepo.GetItem(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemById(int id)
        {
            return Json(itemRepo.GetItemById(id), JsonRequestBehavior.AllowGet);
        }
    }
}