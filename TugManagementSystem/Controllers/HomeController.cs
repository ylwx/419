using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public ActionResult Logout(string lan, int? id)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult UserInfor(string lan, int? id)
        {
            lan = this.Internationalization();
            TugDataEntities db = new TugDataEntities();
            System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == User.Identity.Name;
            UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
            UserInfor curUser = new UserInfor();
            curUser.UserName = user.UserName;
            curUser.CnName = user.CnName;
            curUser.EnName = user.EnName;
            curUser.Dept = user.Dept;
            curUser.Sec = user.Sec;
            curUser.Sex = user.Sex;
            curUser.WorkNumber = user.WorkNumber;
            curUser.Tel = user.Tel;
            curUser.Email = user.Email;
            return View(curUser);
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
            string pwd = Request.Form["Pwd"].ToString();
            string newpwd = Request.Form["newPwd"].ToString();
            TugDataEntities db = new TugDataEntities();
            UserInfor newUser = new UserInfor();
            System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == User.Identity.Name && u.Pwd == pwd;
            UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
            if (user != null)    //原密码验证通过
            {
                user.Pwd = newpwd;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "新密码已生效，请重新登陆！" });
            }
            else   //原密码错误
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Response.StatusCode = 404;
                return Json(new { message = "原密码不正确，请重新输入！" });
            }
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
                return RedirectToAction("OrderManage", "OrderManage");
            }
            else
            {
                ViewBag.Message = "用户名或密码错误，登录失败！";
                return View();
            }
        }

        public ActionResult SaveNewUser()
        {
            string tmpUser = Request.Form["UserName"].ToString();
            TugDataEntities db = new TugDataEntities();
            UserInfor newUser = new UserInfor();
            System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == tmpUser;
            UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
            if (user != null)  //用户名已被占用
            {
                Response.StatusCode = 404;
                return Json(new { code = Resources.Common.Information_CODE, message = Resources.Common.Information_MESSAGE });
            }
            else   //注册成功
            {
                newUser.CnName = Request.Form["CnName"].ToString();
                newUser.UserName = Request.Form["UserName"].ToString();
                newUser.Email = Request.Form["Email"].ToString();
                newUser.Pwd = Request.Form["Pwd"].ToString();
                newUser = db.UserInfor.Add(newUser);
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(tmpUser, false);
                return Json(new { message = "注册成功！" });
            }
        }

        public ActionResult UpdateUserInfor(string UserName)
        {
            string tmpUser = UserName;
            TugDataEntities db = new TugDataEntities();
            System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == tmpUser;
            try
            {
                UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
                if (user != null)  //更新用户信息
                {
                    user.CnName = Request.Form["CnName"].ToString();
                    user.EnName = Request.Form["EnName"].ToString();
                    user.Email = Request.Form["Email"].ToString();
                    user.Tel = Request.Form["Tel"].ToString();
                    user.Sex = Request.Form["Sex"].ToString();
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { message = "个人信息已更新！" });
                }
                else   //失败
                {
                    return Json(new { message = "未找到当前用户信息！" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { message = ex.Message });
                //throw;
            }
        }
    }
}