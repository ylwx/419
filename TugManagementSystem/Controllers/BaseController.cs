using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TugManagementSystem.MyClass;

namespace TugManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        public string Internationalization()
        {
            if (Request.Cookies["SelectedLanguage"] != null)
            {
                HttpCookie lanCookie = Request.Cookies["SelectedLanguage"];
                //从Cookie里面读取
                string language = lanCookie["lan"];
                //当前线程的语言采用哪种语言（比如zh，en等）
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
                //决定各种数据类型是如何组织，如数字与日期
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(language);
                return language;
            }
            else
            {
                HttpCookie lanCookie = new HttpCookie("SelectedLanguage");
                //默认为中文
                lanCookie["lan"] = "zh-HK";
                Response.Cookies.Add(lanCookie);
                return "zh-HK";
            }
        }

        // GET: Base
        public ActionResult SetLanguage(string lan)
        {
            if (string.IsNullOrEmpty(lan))
            {
                lan = "zh-HK";
            }
            ViewBag.Language = lan;
            HttpCookie lanCookie = Request.Cookies["SelectedLanguage"];
            lanCookie["lan"] = lan;
            Response.Cookies.Add(lanCookie);
            //刷新当前页面
            return Redirect(Request.UrlReferrer.ToString());
        }

        public string GetCustomField(string CustomName)
        {
            return TugBusinessLogic.Utils.GetCustomField(CustomName);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}