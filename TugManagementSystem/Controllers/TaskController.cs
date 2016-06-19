using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class TaskController : BaseController
    {
        #region 待审核

        public ActionResult GetTaskData(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                int curUserId = 0;
                TugDataEntities db = new TugDataEntities();
                System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == User.Identity.Name;
                UserInfor curUser = db.UserInfor.Where(exp).FirstOrDefault();
                if (curUser != null)
                {
                    curUserId = curUser.IDX;
                    if (_search == true)
                    {
                        string s = Request.QueryString["filters"];
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        List<V_NeedApproveBilling> objs = db.V_NeedApproveBilling.Where(u => u.FlowUser_ID == curUserId).OrderByDescending(u => u.IDX).ToList<V_NeedApproveBilling>();

                        int totalRecordNum = objs.Count;
                        if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                        int pageSize = rows;
                        int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);
                        List<V_NeedApproveBilling> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<V_NeedApproveBilling>();
                        var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult NeedCheck(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            ViewBag.Services = TugBusinessLogic.Utils.GetServices();
            return View();
        }

        #endregion 待审核

        #region 已审核

        public ActionResult Checked(string lan, int? id)
        {
            lan = this.Internationalization();
            return View();
        }

        public ActionResult GetCheckedData(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                int curUserId = 0;
                TugDataEntities db = new TugDataEntities();
                System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == User.Identity.Name;
                UserInfor curUser = db.UserInfor.Where(exp).FirstOrDefault();
                if (curUser != null)
                {
                    curUserId = curUser.IDX;   //當前用戶ID
                    if (_search == true)
                    {
                        string s = Request.QueryString["filters"];
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        List<V_NeedApproveBilling> objs = db.V_NeedApproveBilling.Where(u => u.FlowUser_ID == curUserId).OrderByDescending(u => u.IDX).ToList<V_NeedApproveBilling>();

                        int totalRecordNum = objs.Count;
                        if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                        int pageSize = rows;
                        int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);
                        List<V_NeedApproveBilling> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<V_NeedApproveBilling>();
                        var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        #endregion 已审核
    }
}