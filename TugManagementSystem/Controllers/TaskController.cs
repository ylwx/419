﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TugBusinessLogic;
using TugBusinessLogic.Module;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class NeedApprove
    {
        int IDX;
	    string InvoiceType ;
	    string  CustomerName ;
	    string ShipName;
	    string JobNo ;
	    string BillingCode ;
	    string BillingTemplateTypeLabel ;
        string TimeTypeValue;
	    string TimeTypeLabel ;
	    float Amount ;
	    string Status ;
	    string Remark ;
	    string CreateDate ;
	    string LastUpDate ;
        int MarkID ;
        int Phase ;
        string Task;
        int FlowUserID;
        string System;
    }
    public class Approved
    {
        int IDX;
        string InvoiceType;
        string CustomerName;
        string ShipName;
        string JobNo;
        string BillingCode;
        string BillingTemplateTypeLabel;
        string TimeTypeValue;
        string TimeTypeLabel;
        float Amount;
        string Status;
        string Remark;
        string CreateDate;
        string LastUpDate;
    }
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
                curUserId = Session.GetDataFromSession<int>("userid");
                    if (_search == true)
                    {
                        string s = Request.QueryString["filters"];
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var objs = db.proc_needapprove(curUserId).OrderByDescending(u=>u.LastUpDate).ToList();
                        //List<NeedApprove> objs = new List<NeedApprove>();
                        //SqlParameter[] prams = new SqlParameter[1];
                        //prams[0] = new SqlParameter("@userID", curUserId);
                        //objs = db.Database.SqlQuery<NeedApprove>("exec dbo.proc_needapprove @userID", prams).ToList();
                        int totalRecordNum = objs.Count;
                        if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                        int pageSize = rows;
                        int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);
                        //var page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList();
                        var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = objs };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);     
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
            Session.SetDataInSession<string>("HomePage", "/Task/NeedCheck");
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
            int curUserId = 0;
             curUserId = Session.GetDataFromSession<int>("userid");
             if (_search == true)
             {
                 string s = Request.QueryString["filters"];
                 return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
             }
             else
             {
                 try
                 {
                    TugDataEntities db = new TugDataEntities();
                    var objs = db.proc_approved(curUserId).OrderByDescending(u=>u.CreateDate).ToList();
                    int totalRecordNum = objs.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);
                    //var page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList();
                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = objs };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);  
                 }
                 catch (Exception)
                 {                     
                     throw;
                 }

             }
              
            //try
            //{
            //    int curUserId = 0;
            //    TugDataEntities db = new TugDataEntities();
            //    //System.Linq.Expressions.Expression<Func<UserInfor, bool>> exp = u => u.UserName == User.Identity.Name;
            //    //UserInfor curUser = db.UserInfor.Where(exp).FirstOrDefault();

            //    curUserId = Session.GetDataFromSession<int>("userid");   //當前用戶ID
            //        List<Approve> ApproveList = db.Approve.Where(u => u.PersonID == curUserId).Select(u => u).ToList<Approve>();
            //        if (ApproveList.Count != 0)
            //        {
            //            //List<Billing> BillList = db.Billing.Where(u => u.IDX == -1).Select(u => u).ToList<Billing>();
            //            List<V_OrderBilling> BillList = db.V_OrderBilling.Where(u => u.BillingID == -1).Select(u => u).ToList<V_OrderBilling>();

            //            foreach (Approve obj in ApproveList)
            //            {
            //                if (Convert.ToInt32(obj.Accept) > 2) continue;
            //                //System.Linq.Expressions.Expression<Func<Billing, bool>> expB = u => u.IDX == obj.BillingID;
            //                //Billing billData = db.Billing.Where(expB).FirstOrDefault();
            //                System.Linq.Expressions.Expression<Func<V_OrderBilling, bool>> expB = u => u.BillingID == obj.BillingID;
            //                V_OrderBilling billData = db.V_OrderBilling.Where(expB).FirstOrDefault();

            //                if (billData != null)
            //                {
            //                    //撤销提交的为待提交任务
            //                    if (Convert.ToInt32(billData.Phase) == 0 && billData.Status == "已撤销提交") continue;
            //                    //驳回或撤销通过的为待完成任务
            //                    if (Convert.ToInt32(billData.Phase) == 0 && billData.Status.ToString().Length >= 3) continue;
            //                    //BillList.Add(billData);
            //                    BillList.Add(billData);
            //                }

            //            }
            //            int totalRecordNum = BillList.Count;
            //            if (page != 0 && totalRecordNum % rows == 0) page -= 1;
            //            int pageSize = rows;
            //            int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);
            //            //List<V_OrderBilling> page_objs = BillList.Skip((page - 1) * rows).Take(rows).ToList<V_OrderBilling>();
            //            var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = BillList };
            //            return Json(jsonData, JsonRequestBehavior.AllowGet);
            //        }
            //        else
            //        {
            //            List<V_OrderBilling> BillList = db.V_OrderBilling.Where(u => u.BillingID == -1).Select(u => u).ToList<V_OrderBilling>();
            //            int totalRecordNum = BillList.Count;
            //            if (page != 0 && totalRecordNum % rows == 0) page -= 1;
            //            int pageSize = rows;
            //            int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);
            //            //List<V_OrderBilling> page_objs = BillList.Skip((page - 1) * rows).Take(rows).ToList<V_OrderBilling>();
            //            var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = BillList };
            //            return Json(jsonData, JsonRequestBehavior.AllowGet);
            //        }
            //        //List<V_NeedApproveBilling> objs = db.V_NeedApproveBilling.Where(u => u.FlowUserID == curUserId).OrderByDescending(u => u.IDX).ToList<V_NeedApproveBilling>();

            //        //int totalRecordNum1 = objs.Count;
            //        //if (page != 0 && totalRecordNum1 % rows == 0) page -= 1;
            //        //int pageSize1 = rows;
            //        //int totalPageNum1 = (int)Math.Ceiling((double)totalRecordNum1 / pageSize1);
            //        //List<V_NeedApproveBilling> page_objs1 = objs.Skip((page - 1) * rows).Take(rows).ToList<V_NeedApproveBilling>();
            //        //var jsonData1 = new { page = page, records = totalRecordNum1, total = totalPageNum1, rows = page_objs1 };
            //        //return Json(jsonData1, JsonRequestBehavior.AllowGet);
               
            //}
            //catch (Exception)
            //{
            //    return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            //}
        }



        public ActionResult GetCheckedData2(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();
            int curUserId = 0;
            curUserId = Session.GetDataFromSession<int>("userid");

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Where(u => u.IDX == -1).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<proc_approved_Result> orders = TugBusinessLogic.Module.OrderLogic.SearchForTaskChecked(sidx, sord, searchOption, curUserId);

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<proc_approved_Result> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<proc_approved_Result>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                    //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<proc_approved_Result> orders = TugBusinessLogic.Module.OrderLogic.LoadDataForTaskChecked(sidx, sord, curUserId);
                    int totalRecordNum = orders.Count;
                    //if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<proc_approved_Result> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<proc_approved_Result>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }


        #endregion 已审核

        #region 通过


        public ActionResult ApprovePass(List<int> passdata)
        {
            //var ids = Request.Form["data"];
            int curUserId = 0;
            TugDataEntities db = new TugDataEntities();
            curUserId = Session.GetDataFromSession<int>("userid");
            foreach (int id in passdata)
            {
                System.Linq.Expressions.Expression<Func<Billing, bool>> exp = u => u.IDX == id;
                Billing billInfor = db.Billing.Where(exp).FirstOrDefault();

                //写入Approve表
                Approve addApprove = new Approve();
                addApprove.BillingID = id;
                addApprove.FlowMark = billInfor.TimesNo;
                addApprove.Phase = billInfor.Phase;
                addApprove.Task = Task(id, Convert.ToInt32(billInfor.Phase), Convert.ToInt32(billInfor.TimesNo));
                addApprove.Accept = 1;
                addApprove.PersonID = curUserId;
                addApprove.UserID = curUserId;
                addApprove.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                addApprove.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                addApprove = db.Approve.Add(addApprove);
                db.SaveChanges();

                //判断是不是流程最后一步
                System.Linq.Expressions.Expression<Func<Flow, bool>> expFlow = u => u.BillingID == id && u.MarkID == billInfor.TimesNo;
                List<Flow> flowData = db.Flow.Where(expFlow).Select(u => u).ToList<Flow>();
                if (billInfor.Phase + 1 == flowData.Count)  //流程最后一步
                {
                    string billingCode = TugBusinessLogic.Utils.AutoGenerateBillCode(); 
                    //更改Billing状态
                    billInfor.Phase = -1;
                    billInfor.Status = "完成";
                    if (Util.checkdbnull(billInfor.BillingCode)=="") billInfor.BillingCode = billingCode;  //生成账单编号
                    billInfor.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    db.Entry(billInfor).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    //若账单有回扣单生成回扣单编号
                     System.Linq.Expressions.Expression<Func<Credit, bool>> expCredit = u => u.BillingID == id;
                     List<Credit> tmpCredit = db.Credit.Where(expCredit).Select(u => u).ToList<Credit>();
                     //Credit tmpCredit = db.Credit.Where(expCredit).FirstOrDefault();
                     if (tmpCredit.Count  != 0)
                     {
                         foreach (var item in tmpCredit)
                         {
                             if (Util.checkdbnull(item.CreditCode) == "")
                             {
                                 item.CreditCode = "C" + billInfor.BillingCode.Substring(1, billingCode.Length - 1);
                                 db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                 db.SaveChanges();
                             }

                         }
                     }
                }
                else
                {
                    //更改Billing状态
                    billInfor.Phase = billInfor.Phase + 1;
                    billInfor.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    billInfor.Status = Task(id, Convert.ToInt32(billInfor.Phase), Convert.ToInt32(billInfor.TimesNo));
                    db.Entry(billInfor).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(new { message = "审核完成！" });
        }

        private static string Task(int tID, int tPhase, int MarkID)
        {
            TugDataEntities db = new TugDataEntities();
            System.Linq.Expressions.Expression<Func<Flow, bool>> expF = u => u.BillingID == tID && u.MarkID == MarkID && u.Phase == tPhase;
            Flow curFlow = db.Flow.Where(expF).FirstOrDefault();
            return curFlow.Task;
        }

        #endregion 通过

        #region 驳回

        public ActionResult ApproveReject(List<int> rejectdata, string RejectReason)
        {
            int curUserId;
            int BillingType = 0;
            TugDataEntities db = new TugDataEntities();
            curUserId = Session.GetDataFromSession<int>("userid");
            foreach (int id in rejectdata)
            {
                //更改Billing状态
                System.Linq.Expressions.Expression<Func<Billing, bool>> exp = u => u.IDX == id;
                Billing billInfor = db.Billing.Where(exp).FirstOrDefault();
                string billtype = billInfor.InvoiceType.ToString();
                if (billtype == "特殊账单") BillingType = 1;

                billInfor.Phase = 0;
                billInfor.Status = "被駁回";
                db.Entry(billInfor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //修改訂單表
                FinanceLogic.SetOrderServiceFlowingStatus(BillingType, id, "否");

                //写入Approve表
                Approve addApprove = new Approve();
                addApprove.BillingID = id;
                addApprove.FlowMark = billInfor.TimesNo;
                addApprove.Phase = billInfor.Phase;
                addApprove.Task = Task(id, Convert.ToInt32(billInfor.Phase), Convert.ToInt32(billInfor.TimesNo));
                addApprove.Accept = 0;
                addApprove.PersonID = curUserId;
                addApprove.UserID = curUserId;
                addApprove.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                addApprove.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                addApprove = db.Approve.Add(addApprove);
                db.SaveChanges();
            }
            return Json(new { message = "操作完成！" });
        }

        #endregion 驳回

        #region 撤销提交

        public ActionResult RepealSubmit(Billing data)
        {
            int id = data.IDX;
            int BillingType = 0;
            int idx = Util.toint(Request.Form["data[IDX]"].Trim());
            TugDataEntities db = new TugDataEntities();
            System.Linq.Expressions.Expression<Func<Billing, bool>> exp = u => u.IDX == idx;
            Billing billInfor = db.Billing.Where(exp).FirstOrDefault();

            int Phase = Convert.ToInt32(billInfor.Phase);
            int timeNo = Convert.ToInt32(billInfor.TimesNo);
            int curUserId = Session.GetDataFromSession<int>("userid");
            if (Phase > 1 || Phase == -1)  //流程已进入审核环节或已完成全部审核，不能撤销
            {
                var ret = new { code = Resources.Common.ERROR_CODE, message = "流程已进入审核环节，不能撤销！" };
                return Json(ret);
            }
            else
            {
                //更新Billing状态
                billInfor.Phase = 0;
                billInfor.Status = "已撤销提交";
                db.Entry(billInfor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                string billtype = billInfor.InvoiceType.ToString();
                if (billtype == "特殊账单") BillingType = 1;

                //修改訂單表
                FinanceLogic.SetOrderServiceFlowingStatus(BillingType, idx, "否");

                //写入Approve表
                Approve addApprove = new Approve();
                addApprove.BillingID = idx;
                addApprove.FlowMark = billInfor.TimesNo;
                addApprove.Phase = 0;
                addApprove.Task = "创建";
                addApprove.Accept = 3;
                addApprove.PersonID = curUserId;
                addApprove.UserID = curUserId;
                addApprove.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                addApprove.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                addApprove = db.Approve.Add(addApprove);
                db.SaveChanges();
                return Json(new { message = "撤销成功！" });
            }
        }

        #endregion 撤销提交

        #region 撤销通过

        public ActionResult RepealPass()
        {
            //int idx = Util.toint(Request.Form["data[IDX]"].Trim());
            var f = Request.Form;
            int BillingType = 0;
            int idx = Convert.ToInt32(Request.Form["data[IDX]"]);
            TugDataEntities db = new TugDataEntities();
            System.Linq.Expressions.Expression<Func<Billing, bool>> exp = u => u.IDX == idx;
            Billing billInfor = db.Billing.Where(exp).FirstOrDefault();

            int Phase = Convert.ToInt32(billInfor.Phase);
            int timeNo = Convert.ToInt32(billInfor.TimesNo);
            int tmpUserID =  Convert.ToInt32(Request.Form["data[UserID]"]);
            int curUserId = Session.GetDataFromSession<int>("userid");
            string billtype = billInfor.InvoiceType.ToString();
            if (billtype == "特殊账单") BillingType = 1;

            System.Linq.Expressions.Expression<Func<Flow, bool>> expF = u => u.BillingID == idx && u.MarkID == timeNo && u.FlowUserID == curUserId;
            Flow flowData = db.Flow.Where(expF).FirstOrDefault();
            if (tmpUserID == curUserId)
            
            {
                return Json(new { message = "该记录是您的提交任务，您无法撤销通过！" });
            }
            else if (Phase > flowData.Phase + 1)  //流程已进入下一审核环节，不能撤销
            {
                var ret = new { code = Resources.Common.ERROR_CODE, message = "已进入下一审核环节，不能撤销！" };
                return Json(ret);
            }
            else
            {
                //更新Billing表状态
                billInfor.Phase = 0;
                billInfor.Status = "已撤销通过";
                db.Entry(billInfor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //修改訂單表
                FinanceLogic.SetOrderServiceFlowingStatus(BillingType, idx, "否");

                //写入Approve表
                System.Linq.Expressions.Expression<Func<Approve, bool>> expA = u => u.BillingID == idx && u.FlowMark == timeNo && u.PersonID == curUserId;
                Approve approveInfor = db.Approve.Where(expA).FirstOrDefault(); //获取当前用户的审核信息

                Approve addApprove = new Approve();
                addApprove.BillingID = idx;
                addApprove.FlowMark = billInfor.TimesNo;
                addApprove.Phase = approveInfor.Phase;
                addApprove.Task = approveInfor.Task;
                addApprove.Accept = 4;
                addApprove.PersonID = curUserId;
                addApprove.UserID = curUserId;
                addApprove.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                addApprove.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                addApprove = db.Approve.Add(addApprove);
                db.SaveChanges();
                return Json(new { message = "撤销成功！" });
            }
        }

        #endregion 撤销通过

        #region 流程日志
        public static string FlowLog(int billingID, int timesNo)
        {
            int m = 0;
            TugDataEntities db = new TugDataEntities();
            DataTable flowTb = new DataTable();
            flowTb.Columns.Add("流程序號", System.Type.GetType("System.Int32"));
            flowTb.Columns.Add("任務", System.Type.GetType("System.String"));
            flowTb.Columns.Add("人員", System.Type.GetType("System.String"));
            flowTb.Columns.Add("狀態", System.Type.GetType("System.String"));
            flowTb.Columns.Add("審核意見", System.Type.GetType("System.String"));
            flowTb.Columns.Add("日期", System.Type.GetType("System.String"));

            for (int i = 1; i < timesNo; i++)
            {
                System.Linq.Expressions.Expression<Func<Approve, bool>> expA = u => u.BillingID == billingID && u.FlowMark == timesNo;
                List<Approve> approveList = db.Approve.Where(expA).OrderBy(u => u.IDX).Select(u => u).ToList<Approve>();
                foreach (var obj in approveList)
                { 
                 flowTb.Rows.Add();
                 flowTb.Rows[m]["流程序號"] = obj.FlowMark;
                 flowTb.Rows[m]["任務"] = obj.Task;
                 flowTb.Rows[m]["人員"] = obj.PersonID;
                 switch (Convert.ToInt32(obj.Accept))
                 { 
                     case 0:
                         flowTb.Rows[m]["狀態"] = "被駁回";
                         break;
                     case 1:
                         flowTb.Rows[m]["狀態"] = "已通過";
                         break;
                     case 2:
                         flowTb.Rows[m]["狀態"] = "已提交";
                         break;
                     case 3:
                         flowTb.Rows[m]["狀態"] = "撤銷提交";
                         break;
                     case 4:
                         flowTb.Rows[m]["狀態"] = "撤銷通過";
                         break;
                 }
                 flowTb.Rows[m]["審核意見"] = obj.Remark;
                 flowTb.Rows[m]["日期"] = obj.CreateDate;
                 m = m + 1;
                }
                
            }
            string s = DTtoJSON(flowTb);
            return s.ToString();
        }
        public static string DTtoJSON(DataTable dt)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = new ArrayList();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    drow.Add(col.ColumnName, row[col.ColumnName]);
                }
                dic.Add(drow);
            }
            return jss.Serialize(dic);
        }
        #endregion 流程日志
    }
}