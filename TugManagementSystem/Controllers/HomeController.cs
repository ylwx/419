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
    }
}
