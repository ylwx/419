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
	}
}