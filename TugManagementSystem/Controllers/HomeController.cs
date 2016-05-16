using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            TugDataModel.OrderInfor order = new OrderInfor();
            order.Code = "123";

            return View(order);
        }

        public ActionResult AddEdit()
        {
            var f = Request.Form;
            TugDataModel.OrderInfor o = new OrderInfor();

            if (Request.Form["oper"].Equals("add"))
            {
                o.Code = "123";
                Response.Write(o);
            }
            else if (Request.Form["oper"].Equals("edit"))
            {
                o.Code = "456";
            }
            return Json(o);
        }

        public ActionResult Delete()
        {
            var f = Request.Form;
            TugDataModel.OrderInfor o = new OrderInfor();
            o.Code = "789";
            return Json(o);
        }
    }
}
