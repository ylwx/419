﻿using System;
using System.Collections.Generic;
using System.Data;
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
                        List<V_NeedApproveBilling> objs = db.V_NeedApproveBilling.Where(u => u.FlowUserID == curUserId).OrderByDescending(u => u.IDX).ToList<V_NeedApproveBilling>();

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
                                    if (Convert.ToInt32(billData.Phase) == 0 && billData.TaskName == "已撤销提交") continue;
                                    //驳回或撤销通过的为待完成任务
                                    if (Convert.ToInt32(billData.Phase) == 0 && billData.TaskName.ToString().Length >= 3) continue;
                                    BillList.Add(billData);
                                }
                            }
                            int totalRecordNum = BillList.Count;
                            if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                            int pageSize = rows;
                            int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);
                            List<Billing> page_objs = BillList.Skip((page - 1) * rows).Take(rows).ToList<Billing>();
                            var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
                        }
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

        #region 通过

        public ActionResult ApprovePass()
        {
            //判断是不是流程最后一步

            //写入Approve表

            //更改Billing状态

            return View();
        }

        #endregion 通过

        #region 驳回

        public ActionResult ApproveReject()
        {
            //写入Approve表

            //更改Billing状态

            return View();
        }

        #endregion 驳回

        #region 撤销提交

        public ActionResult RepealSubmit()
        {
            return View();
        }

        #endregion 撤销提交

        #region 撤销通过

        public ActionResult RepealPass()
        {
            return View();
        }

        #endregion 撤销通过
    }
}