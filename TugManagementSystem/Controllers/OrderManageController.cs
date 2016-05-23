using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TugManagementSystem.Controllers
{
    public class OrderManageController : BaseController
    {
        // GET: OrderManage
        public ActionResult Index(string lan, int? id)
        {
            this.Internationalization();

            ViewBag.Language = lan;
            ViewBag.Controller = "OrderManage";

            return View();
        }
    }
}