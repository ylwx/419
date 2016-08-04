﻿using FastReport.Data;
using FastReport.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugDataModel;
using TugBusinessLogic;
using TugBusinessLogic.Module;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace TugManagementSystem.Controllers
{
    public class ReportController : BaseController
    {
        //
        // GET: /Report/
        public ActionResult Index()
        {
            return View();
        }

        #region 金额汇总接口
        //这个Tuple类型数组的amountlist中依次存储 int SchedulerID, int TugID, float Amount, float Hours
        //一个账单调用一次
        public ActionResult AmountSumAdd_Update(int CustomerID, int CustomerShipID, int BillingID, DateTime BillingDate, Tuple<int, int, float, float>[] amountlist) 
        {
            try
            {
                TugDataEntities db = new TugDataEntities();
                //先删除
                System.Linq.Expressions.Expression<Func<AmountSum, bool>> exp = u => u.BillingID == BillingID;
                var entitys = db.AmountSum.Where(exp);
                entitys.ToList().ForEach(entity => db.Entry(entity).State = System.Data.Entity.EntityState.Deleted); //不加这句也可以
                db.AmountSum.RemoveRange(entitys);
                db.SaveChanges();
                //新增
                foreach(var obj in amountlist)
                {
                    TugDataModel.AmountSum newobj = new AmountSum();
                    newobj.CustomerID = CustomerID;
                    newobj.CustomerShipID = CustomerShipID;
                    newobj.BillingID = BillingID;
                    newobj.BillingDateTime = BillingDate;
                    newobj.SchedulerID = obj.Item1;//SchedulerID;
                    newobj.TugID = obj.Item2;;
                    newobj.Amount = obj.Item3; ;
                    newobj.Hours = obj.Item4; ;
                    newobj.OwnerID = -1;
                    newobj.CreateDate = newobj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;//.ToString("yyyy-MM-dd");
                    newobj.UserID = Session.GetDataFromSession<int>("userid");
                    newobj.UserDefinedCol1 = "";
                    newobj.UserDefinedCol2 = "";
                    newobj.UserDefinedCol3 = "";
                    newobj.UserDefinedCol4 = "";
                    //if (Request.Form["UserDefinedCol5"].Trim() != "")
                    //    newobj.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                    //if (Request.Form["UserDefinedCol6"].Trim() != "")
                    //    newobj.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                    //if (Request.Form["UserDefinedCol7"].Trim() != "")
                    //    newobj.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                    //if (Request.Form["UserDefinedCol8"].Trim() != "")
                    //    newobj.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                    newobj.UserDefinedCol9 = "";
                    newobj.UserDefinedCol10 = "";

                    newobj = db.AmountSum.Add(newobj);
                }

                db.SaveChanges();

                var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                return Json(ret);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }
#endregion

        #region Report 拖轮 每月汇总
        public ActionResult Amout_Tug()//int OrderID, int CreditID
        {
            //int OrderID; int CreditID;
            //OrderID = 10; CreditID = 1;//临时测试用
            SetReport();
            WebReport webReport = new WebReport(); // create object
            webReport.Width = 768;  // set width
            webReport.Height = 1366; // set height

            //读取文件到 MemoryStream
            FileStream stream = new FileStream(this.Server.MapPath(@"\Report\Amount_Tug_Month.frx"), FileMode.Open);
            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            webReport.Report.Load(stream); //从内存加载模板到report中
            stream.Close();
            Report_DataRegister_Amout_Tug(webReport.Report);
            var reportPage = (FastReport.ReportPage)(webReport.Report.Pages[0]);
            webReport.Prepare();

            ViewBag.WebReport_Amout_Tug = webReport; // send object to the View
            return View();
        }
        private void Report_DataRegister_Amout_Tug(FastReport.Report FReport)
        {
            DataTable dt = null;
            string str_report = string.Format(" ID > {0}", 0);
            //data
            dt = SqlHelper.GetDataTableData("V_AmountSum_Billing", str_report);
            FReport.RegisterData(dt, dt.TableName);
        }
        #endregion       

        #region Report 拖轮 全包/半包匯總
        public ActionResult Amount_BillType()//int OrderID, int CreditID
        {
            //int OrderID; int CreditID;
            //OrderID = 10; CreditID = 1;//临时测试用
            SetReport();
            WebReport webReport = new WebReport(); // create object
            webReport.Width = 768;  // set width
            webReport.Height = 1366; // set height

            //读取文件到 MemoryStream
            FileStream stream = new FileStream(this.Server.MapPath(@"\Report\Amout_BillingType.frx"), FileMode.Open);
            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            webReport.Report.Load(stream); //从内存加载模板到report中
            stream.Close();
            Report_DataRegister_Amount_BillType(webReport.Report);
            var reportPage = (FastReport.ReportPage)(webReport.Report.Pages[0]);
            webReport.Prepare();

            ViewBag.WebReport_Amount_BillType = webReport; // send object to the View
            return View();
        }
        private void Report_DataRegister_Amount_BillType(FastReport.Report FReport)
        {
            DataTable dt = null;
            string str_report = string.Format(" ID > {0}", 0);
            //data
            dt = SqlHelper.GetDataTableData("V_AmountSum_Billing", str_report);
            FReport.RegisterData(dt, dt.TableName);
        }
        #endregion       

        #region Report 拖轮 客戶汇总
        public ActionResult Amount_Customer()//int OrderID, int CreditID
        {
            //int OrderID; int CreditID;
            //OrderID = 10; CreditID = 1;//临时测试用
            SetReport();
            WebReport webReport = new WebReport(); // create object
            webReport.Width = 768;  // set width
            webReport.Height = 1366; // set height

            //读取文件到 MemoryStream
            FileStream stream = new FileStream(this.Server.MapPath(@"\Report\Amout_Customer.frx"), FileMode.Open);
            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            webReport.Report.Load(stream); //从内存加载模板到report中
            stream.Close();
            Report_DataRegister_Amount_Customer(webReport.Report);
            var reportPage = (FastReport.ReportPage)(webReport.Report.Pages[0]);
            webReport.Prepare();

            ViewBag.WebReport_Amount_Customer = webReport; // send object to the View
            return View();
        }
        private void Report_DataRegister_Amount_Customer(FastReport.Report FReport)
        {
            DataTable dt = null;
            string str_report = string.Format(" ID > {0}", 0);
            //data
            dt = SqlHelper.GetDataTableData("V_AmountSum_Billing", str_report);
            FReport.RegisterData(dt, dt.TableName);
        }
        #endregion       

        #region Credit Note
        public ActionResult CreditNotePage(int OrderID,int CreditID)
        {
            //int OrderID; int CreditID;
            //OrderID = 10; CreditID = 1;//临时测试用
            SetReport();
            WebReport webReport = new WebReport(); // create object
            webReport.Width = 768;  // set width
            webReport.Height = 1366; // set height

            //读取文件到 MemoryStream
            FileStream stream = new FileStream(this.Server.MapPath(@"\Report\invoice_credit.frx"), FileMode.Open);
            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            webReport.Report.Load(stream); //从内存加载模板到report中
            stream.Close();
            Report_DataRegister_credit(webReport.Report, OrderID, CreditID);
            var reportPage = (FastReport.ReportPage)(webReport.Report.Pages[0]);
            webReport.Prepare();

            ViewBag.WebReport_credit = webReport; // send object to the View
            return View();
        }
        private void Report_DataRegister_credit(FastReport.Report FReport, int OrderID, int CreditID)
        {
            DataTable dtV_Inv_Head = null; DataTable dt_Credit = null; 
            string strV_Inv_Head = string.Format(" OrderID = {0}", OrderID);
            string str_Credit = string.Format(" IDX = {0}", CreditID);
            //head
            dtV_Inv_Head = SqlHelper.GetDataTableData("V_Inv_Head", strV_Inv_Head);
            FReport.RegisterData(dtV_Inv_Head, dtV_Inv_Head.TableName);
            //creditnote,refundhk$
            dt_Credit = SqlHelper.GetDataTableData("Credit", str_Credit);
            FReport.Parameters.FindByName("CreditNote").Value = dt_Credit.Rows[0]["CreditContent"];
            FReport.Parameters.FindByName("RefundHK$").Value = dt_Credit.Rows[0]["CreditAmount"];
        }
        #endregion       

        #region 发票，计时
        public ActionResult Invoice_tk(int OrderID,int TimeTypeValue)
        {
            //int OrderID; int TimeTypeValue;
            //OrderID = 10; TimeTypeValue = 0;//临时测试用
            DataSet dataSet = null;
            SetReport();
            WebReport webReport = new WebReport(); // create object
            webReport.Width = 768;  // set width
            webReport.Height = 1366; // set height

            //读取文件到 MemoryStream
            FileStream stream = new FileStream(this.Server.MapPath(@"\Report\invoice_tk.frx"), FileMode.Open);
            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            webReport.Report.Load(stream); //从内存加载模板到report中
            stream.Close();
            Report_DataRegister_tk(webReport.Report, OrderID, TimeTypeValue);
            var reportPage = (FastReport.ReportPage)(webReport.Report.Pages[0]);
            webReport.Prepare();

            ViewBag.WebReport_tk = webReport; // send object to the View
            return View();
        }
        private void Report_DataRegister_tk(FastReport.Report FReport, int OrderID, int TimeTypeValue)
        {
            DataTable dtV_Inv_Head = null; DataTable dtV_Inv_OrdService = null; DataTable dtContenData = null;
            DataTable dtMData; DataTable dtSubTotal; DataTable dtDData; DataTable dtTotal; DataTable dtGrandTotal;
            string strV_Inv_Head = string.Format(" OrderID = {0}", OrderID);
            string strV_Inv_OrdService = string.Format(" OrderID = {0}", OrderID);
            string strMData = string.Format(" OrderID = {0}", OrderID);
            //head
            dtV_Inv_Head = SqlHelper.GetDataTableData("V_Inv_Head", strV_Inv_Head);
            FReport.RegisterData(dtV_Inv_Head, dtV_Inv_Head.TableName);
            //上
            dtV_Inv_OrdService = SqlHelper.GetDataTableData("V_Inv_OrdService", strV_Inv_OrdService);
            FReport.RegisterData(dtV_Inv_OrdService, dtV_Inv_OrdService.TableName);

            //获取内容数据，再过滤加载到内容区
            SqlParameter para1 = new SqlParameter()
            {
                ParameterName = "@OrderID",
                Direction = ParameterDirection.Input,
                Value = OrderID,
                DbType = DbType.Int16
            };
            SqlParameter para2 = new SqlParameter()
            {
                ParameterName = "@TimeTypeValue",
                Direction = ParameterDirection.Input,
                Value = TimeTypeValue,
                DbType = DbType.Int16
            };
            SqlParameter[] param = new SqlParameter[] { para1, para2 };
            dtContenData = SqlHelper.GetDatatableBySP("proc_inv_item", param);
            //中
            dtMData=TugBusinessLogic.Utils.TableToChildTB(dtContenData,"ItemCode like 'A%' or ItemCode like 'B%'");
            FReport.RegisterData(dtMData, "MData");
            //中，Sub-total HK$
            dtSubTotal = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode = 'T0'");
            FReport.RegisterData(dtSubTotal, "SubTotal");
            //下
            dtDData = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode like 'C%'");
            FReport.RegisterData(dtDData, "DData");
            //下,Total HK$
            dtTotal = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode = 'T1'");
            FReport.RegisterData(dtTotal, "Total");
            //脚,Grand Total HK$
            dtGrandTotal = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode = 'T2'");
            FReport.Parameters.FindByName("GrandTotalHK$").Value = dtGrandTotal.Rows[0]["Value"];
        }
        #endregion

        #region 全包，半包
        public ActionResult Invoice_qborbb(int OrderID, int TimeTypeValue)//
        {
            //int OrderID; int TimeTypeValue;
            //OrderID = 10; TimeTypeValue = 0;//临时测试用
            DataSet dataSet = null;
            SetReport();
            WebReport webReport = new WebReport(); // create object
            webReport.Width = 768;  // set width
            webReport.Height = 1366; // set height
            //webReport.Report.RegisterData(dataSet, "AppData"); // data binding
            // webReport.ReportFile = this.Server.MapPath("~/Report/orderlist.frx");  // load the report from the file
            //webReport.ReportFile = this.Server.MapPath("~/Report/test.frx");
            //Report_DataRegister(webReport.Report);

            //读取文件到 MemoryStream
            FileStream stream = new FileStream(this.Server.MapPath(@"\Report\invoice_qb.frx"), FileMode.Open);
            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            webReport.Report.Load(stream); //从内存加载模板到report中
            stream.Close();
            Report_DataRegister_qborbb(webReport.Report, OrderID, TimeTypeValue);
            var reportPage = (FastReport.ReportPage)(webReport.Report.Pages[0]);
            webReport.Prepare();

            ViewBag.WebReport_qborbb = webReport; // send object to the View
            return View();
        }
        private void Report_DataRegister_qborbb(FastReport.Report FReport,int OrderID,int TimeTypeValue)
        {
            DataTable dtV_Inv_Head = null; DataTable dtV_Inv_OrdService = null; DataTable dtContenData = null;
            DataTable dtMData; DataTable dtSubTotal; DataTable dtDData; DataTable dtTotal; DataTable dtGrandTotal;
            string strV_Inv_Head = string.Format(" OrderID = {0}", OrderID);
            string strV_Inv_OrdService = string.Format(" OrderID = {0}", OrderID);
            string strMData = string.Format(" OrderID = {0}", OrderID);
            //head
            dtV_Inv_Head = SqlHelper.GetDataTableData("V_Inv_Head", strV_Inv_Head);
            FReport.RegisterData(dtV_Inv_Head, dtV_Inv_Head.TableName);
            //上
            dtV_Inv_OrdService = SqlHelper.GetDataTableData("V_Inv_OrdService", strV_Inv_OrdService);
            FReport.RegisterData(dtV_Inv_OrdService, dtV_Inv_OrdService.TableName);

            //获取内容数据，再过滤加载到内容区
            SqlParameter para1 = new SqlParameter()
            {
                ParameterName = "@OrderID",
                Direction = ParameterDirection.Input,
                Value = OrderID,
                DbType = DbType.Int16
            };
            SqlParameter para2 = new SqlParameter()
            {
                ParameterName = "@TimeTypeValue",
                Direction = ParameterDirection.Input,
                Value = TimeTypeValue,
                DbType = DbType.Int16
            };
            SqlParameter[] param = new SqlParameter[] { para1, para2 };
            dtContenData = SqlHelper.GetDatatableBySP("proc_inv_item_xy", param);
            //中
            dtMData = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode like 'A%' or ItemCode like 'B%'","ItemCode");
            FReport.RegisterData(dtMData, "MData");
            //中，Sub-total HK$
            dtSubTotal = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode = 'T0'");
            FReport.RegisterData(dtSubTotal, "SubTotal");
            //下
            dtDData = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode like 'C%'");
            FReport.RegisterData(dtDData, "DData");
            //下,Total HK$
            dtTotal = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode = 'T1'");
            FReport.RegisterData(dtTotal, "Total");
            //脚,Grand Total HK$
            dtGrandTotal = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode = 'T2'");
            FReport.Parameters.FindByName("GrandTotalHK$").Value = dtGrandTotal.Rows[0]["Value"];

        }
        #endregion

        #region OrderList,test
        public ActionResult OrderList()
        {
            DataSet dataSet = null;
            SetReport();
            WebReport webReport = new WebReport(); // create object
            webReport.Width = 600;  // set width
            webReport.Height = 800; // set height
            //webReport.Report.RegisterData(dataSet, "AppData"); // data binding
            // webReport.ReportFile = this.Server.MapPath("~/Report/orderlist.frx");  // load the report from the file
            //webReport.ReportFile = this.Server.MapPath("~/Report/test.frx");
            //Report_DataRegister(webReport.Report);

            //读取文件到 MemoryStream
            FileStream stream = new FileStream(@"D:\WDoc\SRC\SHIPWAY\419\419\TugManagementSystem\Report\orderlist.frx", FileMode.Open);
            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            webReport.Report.Load(stream); //从内存加载模板到report中
            Report_DataRegister(webReport.Report);
            var reportPage = (FastReport.ReportPage)(webReport.Report.Pages[0]);
            webReport.Prepare();

            ViewBag.WebReport = webReport; // send object to the View
            return View();
        }

        private void Report_DataRegister(FastReport.Report FReport)
        {
            string strWhereHead = string.Empty; string strWhereDetail = string.Empty;
            DataTable dtHead = null; DataTable dtDetail = null;
            strWhereHead = string.Format(" ID = {0}", 1);
            strWhereDetail = string.Format(" IDX > {0}", 1);
            foreach (DataSourceBase item in FReport.Dictionary.DataSources)
            {
                if (item.Alias.ToLower().Contains("head"))
                {
                    //headAlias = item.Alias;
                    dtHead = SqlHelper.GetDataTableData(item.Name, strWhereHead);
                }
                if (item.Alias.ToLower().Contains("detail"))
                {
                    //detailAlias = item.Alias;
                    dtDetail = SqlHelper.GetDataTableData(item.Name, strWhereDetail);
                }
            }
            //FReport.RegisterData(dtHead, dtHead.TableName);

            dtDetail = SqlHelper.GetDataTableData("OrderInfor", "IDX>1");
            FReport.RegisterData(dtDetail, dtDetail.TableName);
        }
        #endregion
        private void SetReport()
        {
            string report_path = Request.PhysicalApplicationPath;
            //DataSet dataSet=this.cr
        }
    }
}