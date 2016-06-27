using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TugManagementSystem.Controllers
{
    public class FinanceController : BaseController
    {
        //
        // GET: /Finance/
        public ActionResult Invoice(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            //ViewBag.Services = TugBusinessLogic.Utils.GetServices();

            return View();
        }


        [HttpGet]
        public ActionResult ViewInvoice(string lan, int? orderId)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            //return RedirectToAction("Login", "Home");
            return View();
        }
	}

}