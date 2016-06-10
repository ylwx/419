using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TugBusinessLogic.Module;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Login(string lan, int? id)
        {
            lan = this.Internationalization();
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePwd(string lan, int? id)
        {
            lan = this.Internationalization();

            return View();
        }

        public ActionResult SavePwd()
        {
            TugDataEntities db = new TugDataEntities();
            UserInfor newUser = new UserInfor();
            System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == User.Identity.Name && u.Pwd == Request.Form["Pwd"].ToString();
            UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
            if (user != null)
            {
                user.Pwd = Request.Form["Pwd2"].ToString();
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "新密码已生效，请重新登陆！";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Message = "原密码不正确，请重新输入！";
                return View();
            }

            //Console.WriteLine(User.Identity.Name);
        }

        public ActionResult Login(string userName, string password)
        {
            TugDataEntities db = new TugDataEntities();
            System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == userName && u.Pwd == password;
            //List<UserInfor> users = db.UserInfor.Where(exp).Select(u => u).ToList<UserInfor>();
            UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "用户名或密码错误，登录失败！";
                return View();
            }
        }

        public JsonResult IsValidUser()
        {
            try
            {
                string tmpUser = Request.Form["data[us]"];
                TugDataEntities db = new TugDataEntities();
                UserInfor newUser = new UserInfor();
                System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == tmpUser;
                UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
                if (user != null)
                {
                    return Json(new { code = Resources.Common.ERROR_CODE, message = "您输入的用户名已被占用！" });
                }
                else
                {
                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = "sfdssfdsfds！" });
                }
            }
            catch (Exception)
            {
                var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                return Json(ret);
            }
        }

        public ActionResult SaveNewUser()
        {
            string tmpUser = Request.Form["UserName"].ToString();
            TugDataEntities db = new TugDataEntities();
            UserInfor newUser = new UserInfor();
            //System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == tmpUser;
            //UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
            newUser.CnName = Request.Form["CnName"].ToString();
            newUser.UserName = Request.Form["UserName"].ToString();
            newUser.Email = Request.Form["Email"].ToString();
            newUser.Pwd = Request.Form["Pwd"].ToString();
            newUser = db.UserInfor.Add(newUser);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Index(string lan, int? id)
        {
            lan = this.Internationalization();

            var p = Request.Params;
            var q = Request.RawUrl;
            ViewBag.Title = "Home Page";
            ViewBag.Language = lan;
            ViewBag.Controller = "Home";
            Console.WriteLine(User.Identity.Name);
            TugDataModel.OrderInfor order = new OrderInfor();
            order.Code = "123";

            return View();
        }
    }
}