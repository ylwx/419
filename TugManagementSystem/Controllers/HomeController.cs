using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string lan, int? id)
        {
            lan = this.Internationalization();

            var p = Request.Params;
            var q = Request.RawUrl; ;
            ViewBag.Title = "Home Page";
            ViewBag.Language = lan;
            ViewBag.Controller = "Home";

            TugDataModel.OrderInfor order = new OrderInfor();
            order.Code = "123";

            return View(order);
        }

    }
}
