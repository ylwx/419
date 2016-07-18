using FastReport.Data;
using FastReport.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugBusinessLogic.Module;

namespace TugManagementSystem.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        public ActionResult Index()
        {
            return View();
        }
        #region 发票，条款
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

        #region 全包，半包+条款
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