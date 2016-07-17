﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TugBusinessLogic;
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
            curUser.Name1 = user.Name1;
            curUser.Name2 = user.Name2;
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

        public ActionResult GetUserName()
        {
            return Json(new { message = User.Identity.Name });
        }

        public ActionResult NeedApproveCount()
        {
            int curUserId = 0;
            int NeedApprovedCount;
            TugDataEntities db = new TugDataEntities();
            curUserId = Session.GetDataFromSession<int>("userid");
            List<V_NeedApproveBilling> objs = db.V_NeedApproveBilling.Where(u => u.FlowUserID == curUserId).OrderByDescending(u => u.IDX).ToList<V_NeedApproveBilling>();
            NeedApprovedCount = objs.Count;
            return Json(new { message = NeedApprovedCount });
        }

        public ActionResult ApproveCount()
        {
            int curUserId = 0, ApprovedCount;
            TugDataEntities db = new TugDataEntities();
            curUserId = Session.GetDataFromSession<int>("userid");    //當前用戶ID
            List<Approve> ApproveList = db.Approve.Where(u => u.PersonID == curUserId).Select(u => u).ToList<Approve>();
            if (ApproveList.Count != 0)
            {
                List<Billing> BillList = db.Billing.Where(u => u.IDX == -1).Select(u => u).ToList<Billing>();
                foreach (Approve obj in ApproveList)
                {
                    if (Convert.ToInt32(obj.Accept) > 2) continue;
                    System.Linq.Expressions.Expression<Func<Billing, bool>> expB = u => u.IDX == obj.BillingID;
                    Billing billData = db.Billing.Where(expB).FirstOrDefault();
                    if (billData != null)
                    {
                        //撤销提交的为待提交任务
                        if (Convert.ToInt32(billData.Phase) == 0 && billData.Status == "已撤销提交") continue;
                        //驳回或撤销通过的为待完成任务
                        if (Convert.ToInt32(billData.Phase) == 0 && billData.Status.ToString().Length >= 3) continue;
                        BillList.Add(billData);
                    }
                }
                ApprovedCount = BillList.Count;
                return Json(new { message = ApprovedCount });
            }
            else
            {
                Response.StatusCode = 404;
                return Json(new { message = "0" });
            }
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
                Session.SetDataInSession<int>("userid", user.IDX);
                Session.SetDataInSession<string>("username", user.UserName);
                Session.SetDataInSession<string>("Name1", user.Name1);

                int userid = Session.GetDataFromSession<int>("userid");
                Console.WriteLine(userid);
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
                newUser.Name1 = Request.Form["Name1"].ToString();
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
            TugDataEntities db = new TugDataEntities();
            System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == UserName;
            try
            {
                UserInfor user = db.UserInfor.Where(exp).FirstOrDefault();
                if (user != null)  //更新用户信息
                {
                    user.Name1 = Request.Form["Name1"].ToString();
                    user.Name2 = Request.Form["Name2"].ToString();
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

        public ActionResult SetMenuHidden()
        {
            TugDataEntities db = new TugDataEntities();
            List<object> MenuNone=new List<object>();
            int usid=Session.GetDataFromSession<int>("userid");
            List<FunctionModule> modules =  GetModules();

            foreach (FunctionModule md in modules)
            {
                List<V_Module_Role_User> objs = db.V_Module_Role_User.Where(u => u.UserID ==usid && u.ModuleCode == md.ModuleCode).ToList<V_Module_Role_User>();
          
                if (objs.Count==0)
                {
                    MenuNone.Add(new {menuid=md.ModuleCode,display="none"});
                }
            }
            var jsondata = new { list = MenuNone };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        List<FunctionModule> GetModules()
        {
            TugDataEntities db = new TugDataEntities();
            List<FunctionModule> module = db.FunctionModule.OrderByDescending(u => u.IDX).ToList<FunctionModule>();
             return module;
        }
    }
}