using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TugManagementSystem.Controllers
{
    public class FlowController : BaseController
    {
        public ActionResult CreateFlow()
        {
            return View();
        }

        //
        // GET: /Flow/
        public ActionResult Index()
        {
            return View();
        }
    }
}