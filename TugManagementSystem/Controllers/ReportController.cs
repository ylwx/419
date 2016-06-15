using FastReport.Data;
using FastReport.Web;
using System;
using System.Collections.Generic;
using System.Data;
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

        public ActionResult OrderList()
        {
            DataSet dataSet = null;
            SetReport();
            WebReport webReport = new WebReport(); // create object
            webReport.Width = 600;  // set width
            webReport.Height = 800; // set height
            //webReport.Report.RegisterData(dataSet, "AppData"); // data binding
            webReport.ReportFile = this.Server.MapPath("~/Report/orderlist.frx");  // load the report from the file
            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            //this.WebReport1.Report.Load(stream); //从内存加载模板到report中
            //Report_DataRegister(webReport.Report);
            ViewBag.WebReport = webReport; // send object to the View
            return View();

            //MemoryStream stream = new System.IO.MemoryStream(entTemplate.TemplateFileBin);
            //this.WebReport1.Report.Load(stream); //从内存加载模板到report中
            //this.Report_DataRegister(this.WebReport1.Report);
            //var reportPage = (FastReport.ReportPage)(this.WebReport1.Report.Pages[0]);
            //this.WebReport1.Prepare();
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
            FReport.RegisterData(dtHead, dtHead.TableName);
            FReport.RegisterData(dtDetail, dtDetail.TableName);
        }

        private void SetReport()
        {
            string report_path = Request.PhysicalApplicationPath;
            //DataSet dataSet=this.cr
        }
    }
}