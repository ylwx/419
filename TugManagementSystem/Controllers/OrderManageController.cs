using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TugDataModel;
using TugBusinessLogic;

namespace TugManagementSystem.Controllers
{
    public class OrderManageController : BaseController
    {
        #region 页面Action

        // GET: OrderManage
        [Authorize]
        public ActionResult OrderManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            ViewBag.Services = TugBusinessLogic.Utils.GetServices();

            return View();
        }
        [Authorize]
        //GET: OrderScheduling
        public ActionResult OrderScheduling(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            ViewBag.JobStates = TugBusinessLogic.Utils.GetJobStates();
            return View();
        }

        //GET: JobInformation
        public ActionResult JobInformation(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            ViewBag.JobStates = TugBusinessLogic.Utils.GetJobStates();
            return View();
        }

        #endregion 页面Action

        #region 订单管理页面Action

        public ActionResult GetData(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Where(u => u.IDX == -1).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<V_OrderInfor> orders = TugBusinessLogic.Module.OrderLogic.SearchForOrderMange(sidx, sord, searchOption);

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderInfor> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderInfor>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                    //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<V_OrderInfor> orders = TugBusinessLogic.Module.OrderLogic.LoadDataForOrderManage(sidx, sord);
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderInfor> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderInfor>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult GetDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            //string s = Request.QueryString[6];

            try
            {
                TugDataEntities db = new TugDataEntities();
                List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                int totalRecordNum = orders.Count;
                if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<OrderInfor> page_orders = orders.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<OrderInfor>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = orders };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult AddEdit()
        {
            this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.OrderInfor aOrder = new OrderInfor();

                        aOrder.Code = TugBusinessLogic.Utils.AutoGenerateOrderSequenceNo();

                        aOrder.CreateDate = aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        aOrder.CustomerID = Convert.ToInt32(Request.Form["CustomerID"]);
                        aOrder.CustomerName = Request.Form["CustomerName"].Trim();
                        aOrder.WorkDate = Request.Form["WorkDate"].Trim();
                        aOrder.WorkTime = Request.Form["WorkTime"].Trim();
                        aOrder.EstimatedCompletionTime = Request.Form["EstimatedCompletionTime"].Trim();

                        aOrder.IsGuest = "否"; // Request.Form["IsGuest"].Trim();
                        aOrder.LinkMan = Request.Form["LinkMan"].Trim();
                        aOrder.LinkPhone = Request.Form["LinkPhone"].Trim();
                        aOrder.LinkEmail = Request.Form["LinkEmail"].Trim();

                        if (Request.Form["BigTugNum"].Trim() != "")
                            aOrder.BigTugNum = Convert.ToInt32(Request.Form["BigTugNum"].Trim());
                        if (Request.Form["MiddleTugNum"].Trim() != "")
                            aOrder.MiddleTugNum = Convert.ToInt32(Request.Form["MiddleTugNum"].Trim());
                        if (Request.Form["SmallTugNum"].Trim() != "")
                            aOrder.SmallTugNum = Convert.ToInt32(Request.Form["SmallTugNum"].Trim());

                        aOrder.OwnerID = -1;
                        aOrder.Remark = Request.Form["Remark"].Trim();
                        aOrder.ShipID = Convert.ToInt32(Request.Form["ShipID"].Trim());
                        aOrder.ShipName = Request.Form["ShipName"].Trim();
                        aOrder.UserID = Session.GetDataFromSession<int>("userid"); 
                        aOrder.WorkPlace = Request.Form["WorkPlace"].Trim();

                        Dictionary<string, string> dic = TugBusinessLogic.Utils.ResolveServices(Request.Form["ServiceNatureNames"].Trim());
                        aOrder.ServiceNatureIDS = dic["ids"];
                        aOrder.ServiceNatureNames = dic["labels"];

                        //aOrder.WorkStateID = Convert.ToInt32(Request.Form["WorkStateID"].Trim());
                        aOrder.WorkStateID = 2; //CustomField表里面的OrderInfor.WorkStateID的IDX

                        aOrder.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aOrder.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aOrder.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aOrder.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aOrder.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aOrder.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aOrder.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                        aOrder.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        aOrder.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        aOrder = db.OrderInfor.Add(aOrder);
                        db.SaveChanges();

                        var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                        //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                        return Json(ret);
                    }
                }
                catch (Exception)
                {
                    var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                    //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                    return Json(ret);
                }
            }

            #endregion Add

            #region Edit

            if (Request.Form["oper"].Equals("edit"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();

                    int idx = Convert.ToInt32(Request.Form["IDX"].Trim());
                    OrderInfor aOrder = db.OrderInfor.Where(u => u.IDX == idx).FirstOrDefault();

                    if (aOrder == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        aOrder.CustomerID = Convert.ToInt32(Request.Form["CustomerID"].Trim());
                        aOrder.CustomerName = Request.Form["CustomerName"].Trim();
                        aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        //aOrder.IsGuest = Request.Form["IsGuest"].Trim();
                        aOrder.LinkMan = Request.Form["LinkMan"].Trim();
                        aOrder.LinkPhone = Request.Form["LinkPhone"].Trim();
                        aOrder.LinkEmail = Request.Form["LinkEmail"].Trim();

                        aOrder.WorkDate = Request.Form["WorkDate"].Trim();
                        aOrder.WorkTime = Request.Form["WorkTime"].Trim();
                        aOrder.EstimatedCompletionTime = Request.Form["EstimatedCompletionTime"].Trim();

                        aOrder.ShipID = Convert.ToInt32(Request.Form["ShipID"].Trim());
                        aOrder.ShipName = Request.Form["ShipName"].Trim();
                        if (Request.Form["BigTugNum"].Trim() != "")
                            aOrder.BigTugNum = Convert.ToInt32(Request.Form["BigTugNum"].Trim());
                        if (Request.Form["MiddleTugNum"].Trim() != "")
                            aOrder.MiddleTugNum = Convert.ToInt32(Request.Form["MiddleTugNum"].Trim());
                        if (Request.Form["SmallTugNum"].Trim() != "")
                            aOrder.SmallTugNum = Convert.ToInt32(Request.Form["SmallTugNum"].Trim());
                        aOrder.WorkPlace = Request.Form["WorkPlace"].Trim();

                        Dictionary<string, string> dic = TugBusinessLogic.Utils.ResolveServices(Request.Form["ServiceNatureNames"].Trim());
                        aOrder.ServiceNatureIDS = dic["ids"];
                        aOrder.ServiceNatureNames = dic["labels"];

                        //aOrder.WorkStateID = Convert.ToInt32(Request.Form["WorkStateID"].Trim());
                        
                        aOrder.Remark = Request.Form["Remark"].Trim();

                        aOrder.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aOrder.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aOrder.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aOrder.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aOrder.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aOrder.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aOrder.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                        aOrder.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        aOrder.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        db.Entry(aOrder).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                    }
                }
                catch (Exception exp)
                {
                    return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
                }
            }

            #endregion Edit

            return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
        }

        public ActionResult AddOrder(int customerId, string customerName, string workDate, string workTime, string estimatedCompletionTime,
            int shipId, string shipName, string workPlace, string serviceNatureIds, string serviceNatureNames, string bigTugNum,
            string middleTugNum, string smallTugNum, string linkMan, string linkPhone, string linkEmail, string remark) 
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    TugDataModel.OrderInfor aOrder = new OrderInfor();

                    aOrder.Code = TugBusinessLogic.Utils.AutoGenerateOrderSequenceNo();

                    aOrder.CreateDate = aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    aOrder.CustomerID = customerId;
                    aOrder.CustomerName = customerName;
                    aOrder.WorkDate = workDate;
                    aOrder.WorkTime = workTime;
                    aOrder.EstimatedCompletionTime = estimatedCompletionTime;

                    aOrder.IsGuest = "否";
                    aOrder.LinkMan = linkMan;
                    aOrder.LinkPhone = linkPhone;
                    aOrder.LinkEmail = linkEmail;

                    if (bigTugNum != "")
                        aOrder.BigTugNum = Convert.ToInt32(bigTugNum);
                    if (middleTugNum != "")
                        aOrder.MiddleTugNum = Convert.ToInt32(middleTugNum);
                    if (smallTugNum != "")
                        aOrder.SmallTugNum = Convert.ToInt32(smallTugNum);

                    aOrder.OwnerID = -1;
                    aOrder.Remark = remark;
                    aOrder.ShipID = shipId;
                    aOrder.ShipName = shipName;
                    aOrder.UserID = Session.GetDataFromSession<int>("userid"); 
                    aOrder.WorkPlace = workPlace;

                    aOrder.ServiceNatureIDS = serviceNatureIds;
                    aOrder.ServiceNatureNames = serviceNatureNames;

                    //aOrder.WorkStateID = Convert.ToInt32(Request.Form["WorkStateID"].Trim());
                    aOrder.WorkStateID = 2; //CustomField表里面的OrderInfor.WorkStateID的IDX

                    aOrder.UserDefinedCol1 = "";
                    aOrder.UserDefinedCol2 = "";
                    aOrder.UserDefinedCol3 = "";
                    aOrder.UserDefinedCol4 = "";

                    //if (Request.Form["UserDefinedCol5"].Trim() != "")
                    //    aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                    //if (Request.Form["UserDefinedCol6"].Trim() != "")
                    //    aOrder.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                    //if (Request.Form["UserDefinedCol7"].Trim() != "")
                    //    aOrder.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                    //if (Request.Form["UserDefinedCol8"].Trim() != "")
                    //    aOrder.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                    aOrder.UserDefinedCol9 = "";
                    aOrder.UserDefinedCol10 = "";

                    aOrder = db.OrderInfor.Add(aOrder);
                    db.SaveChanges();

                    var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                    //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                    return Json(ret);
                }
            }
            catch (Exception)
            {
                var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                return Json(ret);
            }
        }

        public ActionResult Delete()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                OrderInfor aOrder = db.OrderInfor.FirstOrDefault(u => u.IDX == idx);
                if (aOrder != null)
                {
                    db.OrderInfor.Remove(aOrder);
                    db.SaveChanges();
                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                }
                else
                {
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult GetCustomer(string term)
         {
            TugDataEntities db = new TugDataEntities();
            List<Customer> customers = db.Customer.Where(u => u.Name1.ToLower().Trim().Contains(term.Trim().ToLower()))
                .Select(u => u).OrderBy(u => u.Name1).ToList<Customer>();

            List<object> list = new List<object>();

            if (customers != null)
            {
                foreach (Customer item in customers)
                {
                    list.Add(new { CustomerID = item.IDX, CustomerName1 = item.Name1 });
                }
            }

            var jsonData = new { list = list };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCustomerShips(string term)
        {
            TugDataEntities db = new TugDataEntities();
            List<CustomerShip> ships = db.CustomerShip.Where(u => u.Name1.ToLower().Trim().Contains(term.Trim().ToLower()))
                .Select(u => u).OrderBy(u => u.Name1).ToList<CustomerShip>();

            List<object> list = new List<object>();

            if (ships != null)
            {
                foreach (CustomerShip item in ships)
                {
                    list.Add(new { ShipID = item.IDX, ShipName1 = item.Name1 });
                }
            }

            var jsonData = new { list = list };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得拖轮的服务项
        /// </summary>
        /// <returns></returns>
        public ActionResult GetServices()
        {
            TugDataEntities db = new TugDataEntities();
            List<CustomField> list = db.CustomField.Where(u => u.CustomName == "OrderInfor.ServiceNatureID").OrderBy(u => u.CustomValue).ToList<CustomField>();
            var jsonData = new { list = list };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //public string GetCustomField(string CustomName)
        //{
        //    string s = string.Empty;

        //    try
        //    {
        //        TugDataEntities db = new TugDataEntities();
        //        List<CustomField> list = db.CustomField.Where(u => u.CustomName == CustomName).OrderBy(u => u.CustomValue).ToList<CustomField>();
        //        if (list != null && list.Count > 0)
        //        {
        //            s += "<select>";
        //            foreach (CustomField item in list)
        //            {
        //                s += string.Format("<option value={0}>{1}</option>", item.CustomValue + ":" + item.CustomLabel, item.CustomLabel);
        //            }

        //            s += "</select>";
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //    }

        //    return s;
        //}

        #endregion 订单管理页面Action

        #region 订单调度页面Action

        public ActionResult GetOrderSubSchedulerData(bool _search, string sidx, string sord, int page, int rows, int orderId)
        {
            this.Internationalization();

            try
            {
                //
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string s = Request.QueryString["filters"];
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderScheduler> orders = db.V_OrderScheduler.Where(u => u.OrderID == orderId).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderScheduler>();
                    List<V_OrderScheduler> orders = TugBusinessLogic.Module.OrderLogic.LoadDataForOrderScheduler(sidx, sord, orderId);
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderScheduler> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderScheduler>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetOrderSubSchedulerDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows, int orderId)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string s = Request.QueryString["filters"];
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<V_OrderScheduler> orders = db.V_OrderScheduler.Where(u => u.OrderID == orderId).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderScheduler>();

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetTugsByName1(string value)
        {
            TugDataEntities db = new TugDataEntities();
            List<TugInfor> source = db.TugInfor.Where(u => u.Name1.Contains(value))
                .OrderBy(u => u.Name1).ToList<TugInfor>();

            var jsonData = new { list = source };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTugsByName2(string value)
        {
            TugDataEntities db = new TugDataEntities();
            List<TugInfor> source = db.TugInfor.Where(u => u.Name2.Contains(value))
                .OrderBy(u => u.Name1).ToList<TugInfor>();

            var jsonData = new { list = source };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTugsBySimpleName(string value)
        {
            TugDataEntities db = new TugDataEntities();
            List<TugInfor> source = db.TugInfor.Where(u => u.SimpleName.Contains(value))
                .OrderBy(u => u.Name1).ToList<TugInfor>();

            var jsonData = new { list = source };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEditScheduler()
        {
            this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.Scheduler aScheduler = new Scheduler();
                        aScheduler.ArrivalBaseTime = Request.Form["ArrivalBaseTime"].Trim();
                        aScheduler.ArrivalShipSideTime = Request.Form["ArrivalShipSideTime"].Trim();
                        aScheduler.CaptainConfirmTime = Request.Form["CaptainConfirmTime"].Trim();
                        aScheduler.DepartBaseTime = Request.Form["DepartBaseTime"].Trim();
                        aScheduler.InformCaptainTime = Request.Form["InformCaptainTime"].Trim();
                        aScheduler.WorkCommencedTime = Request.Form["WorkCommencedTime"].Trim();
                        aScheduler.WorkCompletedTime = Request.Form["WorkCompletedTime"].Trim();

                        aScheduler.JobStateID = Convert.ToInt32(Request.Form["JobStateID"].Trim()); ;

                        aScheduler.OrderID = Convert.ToInt32(Request.Form["OrderID"].Trim());
                        aScheduler.OwnerID = -1;
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid"); 
                        aScheduler.Remark = Request.Form["Remark"].Trim(); ;

                        aScheduler.RopeUsed = Request.Form["RopeUsed"].Trim();
                        if (aScheduler.RopeUsed.Equals("是"))
                            aScheduler.RopeNum = Convert.ToInt32(Request.Form["RopeNum"].Trim());
                        else
                            aScheduler.RopeNum = 0;

                        aScheduler.ServiceNatureID = Convert.ToInt32(Request.Form["ServiceNatureLabel"].Trim().Split('~')[0]);
                        aScheduler.TugID = Convert.ToInt32(Request.Form["TugID"].Trim());
                        aScheduler.CreateDate = aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        aScheduler.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aScheduler.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aScheduler.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aScheduler.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aScheduler.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aScheduler.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aScheduler.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                        aScheduler.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        aScheduler.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        aScheduler = db.Scheduler.Add(aScheduler);
                        db.SaveChanges();

                        var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                        //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                        return Json(ret);
                    }
                }
                catch (Exception)
                {
                    var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                    //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                    return Json(ret);
                }
            }

            #endregion Add

            #region Edit

            if (Request.Form["oper"].Equals("edit"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();

                    int idx = Convert.ToInt32(Request.Form["IDX"].Trim());
                    Scheduler aScheduler = db.Scheduler.Where(u => u.IDX == idx).FirstOrDefault();

                    if (aScheduler == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        aScheduler.ArrivalBaseTime = Request.Form["ArrivalBaseTime"].Trim();
                        aScheduler.ArrivalShipSideTime = Request.Form["ArrivalShipSideTime"].Trim();
                        aScheduler.CaptainConfirmTime = Request.Form["CaptainConfirmTime"].Trim();
                        aScheduler.DepartBaseTime = Request.Form["DepartBaseTime"].Trim();
                        aScheduler.InformCaptainTime = Request.Form["InformCaptainTime"].Trim();
                        aScheduler.WorkCommencedTime = Request.Form["WorkCommencedTime"].Trim();
                        aScheduler.WorkCompletedTime = Request.Form["WorkCompletedTime"].Trim();

                        aScheduler.JobStateID = Convert.ToInt32(Request.Form["JobStateID"].Trim()); ;

                        aScheduler.OrderID = Convert.ToInt32(Request.Form["OrderID"].Trim());
                        aScheduler.OwnerID = -1;
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid"); 
                        aScheduler.Remark = Request.Form["Remark".Trim()]; 

                        aScheduler.RopeUsed = Request.Form["RopeUsed"].Trim();
                        if (aScheduler.RopeUsed.Equals("是"))
                            aScheduler.RopeNum = Convert.ToInt32(Request.Form["RopeNum"].Trim());
                        else
                            aScheduler.RopeNum = 0;

                        aScheduler.ServiceNatureID = Convert.ToInt32(Request.Form["ServiceNatureLabel"].Trim().Split('~')[0]);

                        aScheduler.TugID = Convert.ToInt32(Request.Form["TugID"].Trim());
                        aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        aScheduler.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aScheduler.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aScheduler.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aScheduler.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aScheduler.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aScheduler.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aScheduler.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                        aScheduler.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        aScheduler.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        db.Entry(aScheduler).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                    }
                }
                catch (Exception exp)
                {
                    return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
                }
            }

            #endregion Edit

            return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
        }


        public ActionResult AddScheduler(int orderId, int serviceNatureId, int tugId, int jobStateId, string ropeUsed, int ropeNum, string remark)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    TugDataModel.Scheduler aScheduler = new Scheduler();

                    aScheduler.OrderID = orderId;
                    aScheduler.ServiceNatureID = serviceNatureId;
                    aScheduler.TugID = tugId;
                    aScheduler.JobStateID = jobStateId;
                    aScheduler.RopeUsed = ropeUsed;
                    aScheduler.RopeNum = ropeNum;
                    aScheduler.Remark = remark;

                    aScheduler.OwnerID = -1;
                    aScheduler.UserID = Session.GetDataFromSession<int>("userid"); 
                   
                    aScheduler.CreateDate = aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    aScheduler.UserDefinedCol1 = "";
                    aScheduler.UserDefinedCol2 = "";
                    aScheduler.UserDefinedCol3 = "";
                    aScheduler.UserDefinedCol4 = "";

                    //if (Request.Form["UserDefinedCol5"].Trim() != "")
                    //    aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                    //if (Request.Form["UserDefinedCol6"].Trim() != "")
                    //    aScheduler.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                    //if (Request.Form["UserDefinedCol7"].Trim() != "")
                    //    aScheduler.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                    //if (Request.Form["UserDefinedCol8"].Trim() != "")
                    //    aScheduler.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                    aScheduler.UserDefinedCol9 = "";
                    aScheduler.UserDefinedCol10 = "";

                    aScheduler = db.Scheduler.Add(aScheduler);
                    db.SaveChanges();

                    var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                    //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                    return Json(ret);
                }
            }
            catch (Exception)
            {
                var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                return Json(ret);
            }
        }

        public ActionResult DeleteScheduler()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"].Trim());

                TugDataEntities db = new TugDataEntities();
                Scheduler aScheduler = db.Scheduler.FirstOrDefault(u => u.IDX == idx);
                if (aScheduler != null)
                {
                    db.Scheduler.Remove(aScheduler);
                    db.SaveChanges();
                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                }
                else
                {
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult GetTugRelatedOrders(bool _search, string sidx, string sord, int page, int rows, int tugId, string workDate)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string s = Request.QueryString["filters"];
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //string now = DateTime.Now.ToString("yyyy-MM-dd");

                    List<V_OrderScheduler> schedulers = db.V_OrderScheduler.Where(u => u.TugID == tugId && u.WorkDate == workDate)
                        .Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderScheduler>();
                    //List<V_OrderScheduler> orders = TugBusinessLogic.Module.OrderLogic.LoadDataForOrderScheduler(sidx, sord, orderId);

                    List<V_OrderInfor> orders = new List<V_OrderInfor>();

                    if (schedulers != null)
                    {
                        foreach (V_OrderScheduler item in schedulers)
                        {
                            orders.AddRange(db.V_OrderInfor.Where(u => u.IDX == item.OrderID).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>());
                        };
                    }
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    //List<V_OrderScheduler> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderScheduler>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion 订单调度页面Action
    }
}