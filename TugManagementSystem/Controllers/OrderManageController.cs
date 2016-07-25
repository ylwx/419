using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TugDataModel;
using TugBusinessLogic;
using TugBusinessLogic.Module;
using Newtonsoft.Json;

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

            ViewBag.ServiceLabels = GetServiceLabels();
            ViewBag.Locations = GetLocations();
            return View();
        }
        [HttpGet]
        public ActionResult GetOrder(int orderId)
        {
            try
            {
                TugDataEntities db = new TugDataEntities();
                OrderInfor aOrder = db.OrderInfor.Where(u => u.IDX == orderId).FirstOrDefault();
                if (aOrder != null)
                {
                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, order = aOrder }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_CODE }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public ActionResult GetOrderServiceData(int orderId)
        {
            try
            {
                TugDataEntities db = new TugDataEntities();
                List<V_OrderService> list = db.V_OrderService.Where(u => u.OrderID == orderId).OrderBy(u => u.OrderServiceIDX).ToList<V_OrderService>();

                List<string[]> jsonData = new List<string[]>();
                foreach (var itm in list)
                {
                    string[] sev = new string[7] { itm.ServiceNatureLabel, itm.ServiceWorkDate,itm.ServiceWorkTime, itm.BigTugNum.ToString(),itm.MiddleTugNum.ToString(),itm.SmallTugNum.ToString(),itm.ServiceWorkPlace};
                    jsonData.Add(sev);
                }

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public string GetLocations()
        {
            string[] labels =null;
            int i = 0;
            if (labels == null)
            {
                TugDataEntities db = new TugDataEntities();
                List<CustomField> list = db.CustomField.Where(u => u.CustomName == "OrderService.Location").OrderBy(u => u.CustomValue).ToList<CustomField>();
                labels = new string[list.Count];
                foreach (var itm in list)
                {
                    labels[i] = itm.CustomLabel;
                    i++;
                }
            }
            //return labels;
            return JsonConvert.SerializeObject(labels);
        }
        public string GetServiceLabels()
        {
            string[] labels =null;
            int i = 0;
            if (labels == null)
            {
                TugDataEntities db = new TugDataEntities();
                List<CustomField> list = db.CustomField.Where(u => u.CustomName == "OrderInfor.ServiceNatureID").OrderBy(u => u.CustomValue).ToList<CustomField>();
                labels = new string[list.Count];
                foreach (var itm in list)
                {
                    labels[i] = itm.CustomLabel;
                    i++;
                }
            }
            //return labels;
            return JsonConvert.SerializeObject(labels);
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
        [Authorize]
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
                        aOrder.CustomerID = Util.toint(Request.Form["CustomerID"]);
                        aOrder.CustomerName = Request.Form["CustomerName"].Trim();
                        aOrder.OrdDate = Request.Form["OrdDate"].Trim();
                        //aOrder.WorkDate = Request.Form["WorkDate"].Trim();
                        //aOrder.WorkTime = Request.Form["WorkTime"].Trim();
                        //aOrder.EstimatedCompletionTime = Request.Form["EstimatedCompletionTime"].Trim();

                        aOrder.IsGuest = "否"; // Request.Form["IsGuest"].Trim();
                        aOrder.LinkMan = Request.Form["LinkMan"].Trim();
                        aOrder.LinkPhone = Request.Form["LinkPhone"].Trim();
                        aOrder.LinkEmail = Request.Form["LinkEmail"].Trim();

                        //if (Request.Form["BigTugNum"].Trim() != "")
                        //    aOrder.BigTugNum = Util.toint(Request.Form["BigTugNum"].Trim());
                        //if (Request.Form["MiddleTugNum"].Trim() != "")
                        //    aOrder.MiddleTugNum = Util.toint(Request.Form["MiddleTugNum"].Trim());
                        //if (Request.Form["SmallTugNum"].Trim() != "")
                        //    aOrder.SmallTugNum = Util.toint(Request.Form["SmallTugNum"].Trim());

                        aOrder.OwnerID = -1;
                        aOrder.Remark = Request.Form["Remark"].Trim();
                        aOrder.ShipID = Util.toint(Request.Form["ShipID"].Trim());
                        aOrder.ShipName = Request.Form["ShipName"].Trim();
                        aOrder.UserID = Session.GetDataFromSession<int>("userid"); 
                        //aOrder.WorkPlace = Request.Form["WorkPlace"].Trim();

                        //Dictionary<string, string> dic = TugBusinessLogic.Utils.ResolveServices(Request.Form["ServiceNatureNames"].Trim());
                        //aOrder.ServiceNatureIDS = dic["ids"];
                        //aOrder.ServiceNatureNames = dic["labels"];

                        //aOrder.WorkStateID = Util.toint(Request.Form["WorkStateID"].Trim());
                        aOrder.WorkStateID = 2; //CustomField表里面的OrderInfor.WorkStateID的IDX

                        aOrder.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aOrder.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aOrder.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aOrder.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aOrder.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aOrder.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aOrder.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

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

                    int idx = Util.toint(Request.Form["IDX"].Trim());
                    OrderInfor aOrder = db.OrderInfor.Where(u => u.IDX == idx).FirstOrDefault();

                    if (aOrder == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        aOrder.CustomerID = Util.toint(Request.Form["CustomerID"].Trim());
                        aOrder.CustomerName = Request.Form["CustomerName"].Trim();
                        aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        //aOrder.IsGuest = Request.Form["IsGuest"].Trim();
                        aOrder.LinkMan = Request.Form["LinkMan"].Trim();
                        aOrder.LinkPhone = Request.Form["LinkPhone"].Trim();
                        aOrder.LinkEmail = Request.Form["LinkEmail"].Trim();

                        aOrder.OrdDate = Request.Form["OrdDate"].Trim();
                        //aOrder.WorkDate = Request.Form["WorkDate"].Trim();
                        //aOrder.WorkTime = Request.Form["WorkTime"].Trim();
                        //aOrder.EstimatedCompletionTime = Request.Form["EstimatedCompletionTime"].Trim();

                        aOrder.ShipID = Util.toint(Request.Form["ShipID"].Trim());
                        aOrder.ShipName = Request.Form["ShipName"].Trim();
                        //if (Request.Form["BigTugNum"].Trim() != "")
                        //    aOrder.BigTugNum = Util.toint(Request.Form["BigTugNum"].Trim());
                        //if (Request.Form["MiddleTugNum"].Trim() != "")
                        //    aOrder.MiddleTugNum = Util.toint(Request.Form["MiddleTugNum"].Trim());
                        //if (Request.Form["SmallTugNum"].Trim() != "")
                        //    aOrder.SmallTugNum = Util.toint(Request.Form["SmallTugNum"].Trim());
                        //aOrder.WorkPlace = Request.Form["WorkPlace"].Trim();

                        //Dictionary<string, string> dic = TugBusinessLogic.Utils.ResolveServices(Request.Form["ServiceNatureNames"].Trim());
                        //aOrder.ServiceNatureIDS = dic["ids"];
                        //aOrder.ServiceNatureNames = dic["labels"];

                        //aOrder.WorkStateID = Util.toint(Request.Form["WorkStateID"].Trim());
                        
                        aOrder.Remark = Request.Form["Remark"].Trim();

                        aOrder.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aOrder.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aOrder.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aOrder.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aOrder.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aOrder.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aOrder.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

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
        
        public JsonResult GetInitServiceData()  //初始化服务项table
        {
            var jsonData = new[]
                     {
                         new[] {"","","","","","",""},
                    };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }    
        public ActionResult Add_EditOrder(string oper,int orderId,int customerId, string customerName, string ordDate,
            int shipId, string shipName, string linkMan, string linkPhone, string linkEmail, string remark,List<string[]> dataListFromTable) 
        {
            TugDataModel.OrderInfor aOrder=null;
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    if (oper=="add")
                    {
                        aOrder = new OrderInfor();
                        aOrder.Code = TugBusinessLogic.Utils.AutoGenerateOrderSequenceNo();
                        aOrder.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else if (oper == "edit")
                    {
                        aOrder = db.OrderInfor.Where(u => u.IDX == orderId).FirstOrDefault();
                    }
                    aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    aOrder.CustomerID = customerId;
                    aOrder.CustomerName = customerName;
                    aOrder.OrdDate = ordDate;
                    //aOrder.WorkDate = workDate;
                    //aOrder.WorkTime = workTime;
                    //aOrder.EstimatedCompletionTime = estimatedCompletionTime;

                    aOrder.IsGuest = "否";
                    aOrder.LinkMan = linkMan;
                    aOrder.LinkPhone = linkPhone;
                    aOrder.LinkEmail = linkEmail;

                    //if (bigTugNum != "")
                    //    aOrder.BigTugNum = Convert.ToInt32(bigTugNum);
                    //if (middleTugNum != "")
                    //    aOrder.MiddleTugNum = Convert.ToInt32(middleTugNum);
                    //if (smallTugNum != "")
                    //    aOrder.SmallTugNum = Convert.ToInt32(smallTugNum);

                    aOrder.OwnerID = -1;
                    aOrder.Remark = remark;
                    aOrder.ShipID = shipId;
                    aOrder.ShipName = shipName;
                    aOrder.UserID = Session.GetDataFromSession<int>("userid"); 
                    //aOrder.WorkPlace = workPlace;
                    aOrder.HasInvoice = "否"; //没有账单

                    //aOrder.ServiceNatureIDS = serviceNatureIds;
                    //aOrder.ServiceNatureNames = serviceNatureNames;

                    //aOrder.WorkStateID = Util.toint(Request.Form["WorkStateID"].Trim());
                    aOrder.WorkStateID = 2; //CustomField表里面的OrderInfor.WorkStateID的IDX

                    aOrder.UserDefinedCol1 = "";
                    aOrder.UserDefinedCol2 = "";
                    aOrder.UserDefinedCol3 = "";
                    aOrder.UserDefinedCol4 = "";

                    //if (Request.Form["UserDefinedCol5"].Trim() != "")
                    //    aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                    //if (Request.Form["UserDefinedCol6"].Trim() != "")
                    //    aOrder.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                    //if (Request.Form["UserDefinedCol7"].Trim() != "")
                    //    aOrder.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                    //if (Request.Form["UserDefinedCol8"].Trim() != "")
                    //    aOrder.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                    aOrder.UserDefinedCol9 = "";
                    aOrder.UserDefinedCol10 = "";

                    aOrder = db.OrderInfor.Add(aOrder);
                    db.SaveChanges();

                    //将服务项信息写入OrderService表
                    #region

                    //先删除订单下的所有服务项
                     System.Linq.Expressions.Expression<Func<OrderService, bool>> exp = u => u.OrderID == orderId;
                     var entitys = db.OrderService.Where(exp);
                     entitys.ToList().ForEach(entity => db.Entry(entity).State = System.Data.Entity.EntityState.Deleted); //不加这句也可以
                     db.OrderService.RemoveRange(entitys);
                     db.SaveChanges();
                    //保存
                     //获取服务项
                     List<CustomField> listServ;
                     listServ = TugBusinessLogic.Utils.GetServices();
                    for (int i = 0; i < dataListFromTable.Count - 1; i++)//最后一行空行
                    {
                        TugDataModel.OrderService obj = new OrderService();
                        obj.OrderID = aOrder.IDX;
                        string serName = dataListFromTable[i][0];
                        CustomField sv = listServ.Where(u => u.CustomLabel == serName).FirstOrDefault();
                        obj.ServiceNatureID = sv.IDX;
                        obj.ServiceWorkDate = dataListFromTable[i][1];
                        obj.ServiceWorkTime = dataListFromTable[i][2];
                        //obj.EstimatedCompletionTime=
                        obj.ServiceWorkPlace = dataListFromTable[i][6];
                        obj.BigTugNum =Util.toint(dataListFromTable[i][3]);
                        obj.MiddleTugNum = Util.toint(dataListFromTable[i][4]);
                        obj.SmallTugNum = Util.toint(dataListFromTable[i][5]);
                        //obj.Remark = "";

                        obj.OwnerID = -1;
                        obj.CreateDate = aOrder.CreateDate;
                        obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                        obj.UserID = Session.GetDataFromSession<int>("userid");

                        //obj.UserDefinedCol1 = "";
                        //obj.UserDefinedCol2 = "";
                        //obj.UserDefinedCol3 = "";
                        //obj.UserDefinedCol4 = "";
                        //if (Request.Form["UserDefinedCol5"] != "")
                        //    obj.UserDefinedCol5 = Util.toint(Request.Form["UserDefinedCol5"]);
                        //obj.UserDefinedCol6 =;
                        //obj.UserDefinedCol7 =;
                        //obj.UserDefinedCol8 =;
                        //obj.UserDefinedCol9 = "";
                        //obj.UserDefinedCol10 = "";
                        obj = db.OrderService.Add(obj);
                        db.SaveChanges();
                    }
                    #endregion

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

                int idx = Util.toint(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                OrderInfor aOrder = db.OrderInfor.FirstOrDefault(u => u.IDX == idx);
                if (aOrder != null)
                {
                    //先删除订单下的所有服务项
                    System.Linq.Expressions.Expression<Func<OrderService, bool>> exp = u => u.OrderID == idx;
                    var entitys = db.OrderService.Where(exp);
                    entitys.ToList().ForEach(entity => db.Entry(entity).State = System.Data.Entity.EntityState.Deleted); //不加这句也可以
                    db.OrderService.RemoveRange(entitys);
                    db.SaveChanges();
                    //删除订单
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

        public ActionResult GetDataOfOrderScheduling(bool _search, string sidx, string sord, int page, int rows)
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
                    List<V_OrderInfor> orders = TugBusinessLogic.Module.OrderLogic.LoadDataForOrderScheduling(sidx, sord);
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

                        aScheduler.JobStateID = Util.toint(Request.Form["JobStateID"].Trim()); ;

                        aScheduler.OrderID = Util.toint(Request.Form["OrderID"].Trim());
                        aScheduler.OwnerID = -1;
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid"); 
                        aScheduler.Remark = Request.Form["Remark"].Trim(); ;

                        //aScheduler.RopeUsed = Request.Form["RopeUsed"].Trim();
                        //if (aScheduler.RopeUsed.Equals("是"))
                        //    aScheduler.RopeNum = Util.toint(Request.Form["RopeNum"].Trim());
                        //else
                        //    aScheduler.RopeNum = 0;

                        aScheduler.ServiceNatureID = Util.toint(Request.Form["ServiceNatureLabel"].Trim().Split('~')[0]);
                        aScheduler.TugID = Util.toint(Request.Form["TugID"].Trim());
                        aScheduler.CreateDate = aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        aScheduler.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aScheduler.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aScheduler.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aScheduler.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                        aScheduler.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        aScheduler.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        aScheduler = db.Scheduler.Add(aScheduler);
                        db.SaveChanges();

                        {
                            //更新订单状态
                            OrderInfor tmpOrder = db.OrderInfor.Where(u => u.IDX == aScheduler.OrderID).FirstOrDefault();
                            if(tmpOrder != null)
                            {
                                tmpOrder.WorkStateID = 3; //已排船
                                db.Entry(tmpOrder).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        {
                            OrderService os = db.OrderService.Where(u => u.OrderID == aScheduler.OrderID && u.ServiceNatureID == aScheduler.ServiceNatureID).FirstOrDefault();
                            if (os == null)
                            {
                                os = new OrderService();
                                os.CreateDate = os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                os.OrderID = aScheduler.OrderID;
                                os.OwnerID = -1;
                                os.ServiceNatureID = aScheduler.ServiceNatureID;
                                os.ServiceWorkDate = Request.Form["ServiceWorkDate"].Trim(); 
                                os.ServiceWorkPlace = Request.Form["ServiceWorkPlace"].Trim(); 
                                os.UserID = Session.GetDataFromSession<int>("userid");
                                os = db.OrderService.Add(os);
                                db.SaveChanges();
                            }
                            else
                            {
                                os.ServiceWorkDate = Request.Form["ServiceWorkDate"].Trim(); 
                                os.ServiceWorkPlace = Request.Form["ServiceWorkPlace"].Trim(); 
                                os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                db.Entry(os).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

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

                    int idx = Util.toint(Request.Form["IDX"].Trim());
                    Scheduler aScheduler = db.Scheduler.Where(u => u.IDX == idx).FirstOrDefault();

                    if (aScheduler == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        //aScheduler.ArrivalBaseTime = Request.Form["ArrivalBaseTime"].Trim();
                        //aScheduler.ArrivalShipSideTime = Request.Form["ArrivalShipSideTime"].Trim();
                        //aScheduler.CaptainConfirmTime = Request.Form["CaptainConfirmTime"].Trim();
                        //aScheduler.DepartBaseTime = Request.Form["DepartBaseTime"].Trim();
                        //aScheduler.InformCaptainTime = Request.Form["InformCaptainTime"].Trim();
                        //aScheduler.WorkCommencedTime = Request.Form["WorkCommencedTime"].Trim();
                        //aScheduler.WorkCompletedTime = Request.Form["WorkCompletedTime"].Trim();

                        aScheduler.JobStateID = Util.toint(Request.Form["JobStateID"].Trim()); ;

                        aScheduler.OrderID = Util.toint(Request.Form["OrderID"].Trim());
                        aScheduler.OwnerID = -1;
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid"); 
                        aScheduler.Remark = Request.Form["Remark".Trim()]; 

                        //aScheduler.RopeUsed = Request.Form["RopeUsed"].Trim();
                        //if (aScheduler.RopeUsed.Equals("是"))
                        //    aScheduler.RopeNum = Util.toint(Request.Form["RopeNum"].Trim());
                        //else
                        //    aScheduler.RopeNum = 0;

                        aScheduler.ServiceNatureID = Util.toint(Request.Form["ServiceNatureLabel"].Trim().Split('~')[0]);

                        aScheduler.TugID = Util.toint(Request.Form["TugID"].Trim());
                        aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        aScheduler.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aScheduler.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aScheduler.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aScheduler.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                        aScheduler.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        aScheduler.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        db.Entry(aScheduler).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        {
                            OrderService os = db.OrderService.Where(u => u.OrderID == aScheduler.OrderID && u.ServiceNatureID == aScheduler.ServiceNatureID).FirstOrDefault();
                            if (os == null)
                            {
                                os = new OrderService();
                                os.CreateDate = os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                os.OrderID = aScheduler.OrderID;
                                os.OwnerID = -1;
                                os.ServiceNatureID = aScheduler.ServiceNatureID;
                                os.ServiceWorkDate = Request.Form["ServiceWorkDate"].Trim();
                                os.ServiceWorkPlace = Request.Form["ServiceWorkPlace"].Trim();
                                os.UserID = Session.GetDataFromSession<int>("userid");
                                os = db.OrderService.Add(os);
                                db.SaveChanges();
                            }
                            else
                            {
                                os.ServiceWorkDate = Request.Form["ServiceWorkDate"].Trim();
                                os.ServiceWorkPlace = Request.Form["ServiceWorkPlace"].Trim();
                                os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                db.Entry(os).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

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


        public ActionResult AddScheduler(int orderId, int serviceNatureId, string serviceWorkDate, string serviceWorkPlace, string tugId,
            string informCaptainTime, string captainConfirmTime, int jobStateId, string ropeUsed, int ropeNum, string remark)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    //插入多个调度到数据库
                    {
                        List<string> lstTugIds = tugId.Split(',').ToList();
                        if (lstTugIds != null && lstTugIds.Count > 0)
                        {
                            List<Scheduler> lstSchedulers = new List<Scheduler>();
                            foreach (string item in lstTugIds)
                            {
                                TugDataModel.Scheduler aScheduler = new Scheduler();

                                aScheduler.OrderID = orderId;
                                aScheduler.ServiceNatureID = serviceNatureId;

                                aScheduler.TugID = Util.toint(item);
                                aScheduler.JobStateID = jobStateId;
                                aScheduler.RopeUsed = ropeUsed;
                                aScheduler.RopeNum = ropeNum;
                                aScheduler.Remark = remark;

                                aScheduler.InformCaptainTime = informCaptainTime;
                                aScheduler.CaptainConfirmTime = captainConfirmTime;

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
                                //    aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                                //if (Request.Form["UserDefinedCol7"].Trim() != "")
                                //    aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                                //if (Request.Form["UserDefinedCol8"].Trim() != "")
                                //    aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                                aScheduler.UserDefinedCol9 = "";
                                aScheduler.UserDefinedCol10 = "";

                                lstSchedulers.Add(aScheduler);
                            }

                            db.Scheduler.AddRange(lstSchedulers);
                            db.SaveChanges();
                        }
                    }


                    {
                        //更新订单状态
                        OrderInfor tmpOrder = db.OrderInfor.Where(u => u.IDX == orderId).FirstOrDefault();
                        if (tmpOrder != null)
                        {
                            tmpOrder.WorkStateID = 3; //已排船
                            db.Entry(tmpOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    {
                        OrderService os = db.OrderService.Where(u => u.OrderID == orderId && u.ServiceNatureID == serviceNatureId).FirstOrDefault();
                        if (os == null)
                        {
                            os = new OrderService();
                            os.CreateDate = os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            os.OrderID = orderId;
                            os.OwnerID = -1;
                            os.ServiceNatureID = serviceNatureId;
                            os.ServiceWorkDate = serviceWorkDate;
                            os.ServiceWorkPlace = serviceWorkPlace;
                            os.UserID = Session.GetDataFromSession<int>("userid");
                            os = db.OrderService.Add(os);
                            db.SaveChanges();
                        }
                        else
                        {
                            os.ServiceWorkDate = serviceWorkDate;
                            os.ServiceWorkPlace = serviceWorkPlace;
                            os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            db.Entry(os).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                        }
                    }

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



        public ActionResult EditScheduler(int orderId, int serviceNatureId, string serviceWorkDate, string serviceWorkPlace, int schedulerId, int oldTugId, string newTugIds,
            string informCaptainTime, string captainConfirmTime, int jobStateId, string ropeUsed, int ropeNum, string remark)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    #region 修改调度时，没有重新换过拖轮
                    if (newTugIds == "-1")
                    {
                        Scheduler aScheduler = db.Scheduler.Where(u => u.IDX == schedulerId).FirstOrDefault();
                        if(aScheduler != null)
                        {

                            aScheduler.OrderID = orderId;
                            aScheduler.ServiceNatureID = serviceNatureId;
                            aScheduler.TugID = oldTugId;
                            aScheduler.JobStateID = jobStateId;
                            aScheduler.InformCaptainTime = informCaptainTime;
                            aScheduler.CaptainConfirmTime = captainConfirmTime;

                            aScheduler.Remark = remark;

                            //aScheduler.RopeUsed = Request.Form["RopeUsed"].Trim();
                            //if (aScheduler.RopeUsed.Equals("是"))
                            //    aScheduler.RopeNum = Util.toint(Request.Form["RopeNum"].Trim());
                            //else
                            //    aScheduler.RopeNum = 0;


                            aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            db.Entry(aScheduler).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    #endregion

                    #region 修改调度时，重新选择了拖轮
                    else
                    //插入多个调度到数据库
                    {
                        //1.删除原来的调度
                        db.Scheduler.RemoveRange(db.Scheduler.Where(u => u.IDX == schedulerId).ToList());
                        db.SaveChanges();

                        //2.插入新的拖轮调度
                        List<string> lstTugIds = newTugIds.Split(',').ToList();
                        if (lstTugIds != null && lstTugIds.Count > 0)
                        {
                            List<Scheduler> lstSchedulers = new List<Scheduler>();
                            foreach (string item in lstTugIds)
                            {
                                TugDataModel.Scheduler aScheduler = new Scheduler();

                                aScheduler.OrderID = orderId;
                                aScheduler.ServiceNatureID = serviceNatureId;

                                aScheduler.TugID = Util.toint(item);
                                aScheduler.JobStateID = jobStateId;
                                aScheduler.RopeUsed = ropeUsed;
                                aScheduler.RopeNum = ropeNum;
                                aScheduler.Remark = remark;

                                aScheduler.InformCaptainTime = informCaptainTime;
                                aScheduler.CaptainConfirmTime = captainConfirmTime;

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
                                //    aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                                //if (Request.Form["UserDefinedCol7"].Trim() != "")
                                //    aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                                //if (Request.Form["UserDefinedCol8"].Trim() != "")
                                //    aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                                aScheduler.UserDefinedCol9 = "";
                                aScheduler.UserDefinedCol10 = "";

                                lstSchedulers.Add(aScheduler);
                            }

                            db.Scheduler.AddRange(lstSchedulers);
                            db.SaveChanges();
                        }
                    }
                    #endregion

                    {
                        //更新订单状态
                        OrderInfor tmpOrder = db.OrderInfor.Where(u => u.IDX == orderId).FirstOrDefault();
                        if (tmpOrder != null)
                        {
                            tmpOrder.WorkStateID = 3; //已排船
                            db.Entry(tmpOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    {
                        OrderService os = db.OrderService.Where(u => u.OrderID == orderId && u.ServiceNatureID == serviceNatureId).FirstOrDefault();
                        if (os == null)
                        {
                            os = new OrderService();
                            os.CreateDate = os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            os.OrderID = orderId;
                            os.OwnerID = -1;
                            os.ServiceNatureID = serviceNatureId;
                            os.ServiceWorkDate = serviceWorkDate;
                            os.ServiceWorkPlace = serviceWorkPlace;
                            os.UserID = Session.GetDataFromSession<int>("userid");
                            os = db.OrderService.Add(os);
                            db.SaveChanges();
                        }
                        else
                        {
                            os.ServiceWorkDate = serviceWorkDate;
                            os.ServiceWorkPlace = serviceWorkPlace;
                            os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            db.Entry(os).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                        }
                    }

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

                int idx = Util.toint(Request.Form["data[IDX]"].Trim());

                TugDataEntities db = new TugDataEntities();
                Scheduler aScheduler = db.Scheduler.FirstOrDefault(u => u.IDX == idx);
                if (aScheduler != null)
                {
                    int orderId = (int)aScheduler.OrderID;
                    db.Scheduler.Remove(aScheduler);
                    db.SaveChanges();

                    //删除一个调度之后，要看是否还剩下调度
                    {
                        var list = db.V_OrderScheduler.Where(u => u.OrderID == orderId).ToList();
                        if (list == null || list.Count == 0)
                        {
                            //更新订单状态
                            OrderInfor tmpOrder = db.OrderInfor.Where(u => u.IDX == orderId).FirstOrDefault();
                            if (tmpOrder != null)
                            {
                                tmpOrder.WorkStateID = 2; //未排船
                                db.Entry(tmpOrder).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            //更新订单状态
                            OrderInfor tmpOrder = db.OrderInfor.Where(u => u.IDX == orderId).FirstOrDefault();
                            if (tmpOrder != null)
                            {
                                if (true == TugBusinessLogic.Module.OrderLogic.OrderJobInformationInputIsComplete(orderId))
                                {
                                    tmpOrder.WorkStateID = 5; //已完工
                                    db.Entry(tmpOrder).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
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

                    List<V_OrderScheduler> schedulers = db.V_OrderScheduler.Where(u => u.TugID == tugId && u.ServiceWorkDate == workDate)
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

        public ActionResult GetServiceDateAndPlace(int orderId, int serviceNatureId)
        {
            TugDataEntities db = new TugDataEntities();
            OrderService os = db.OrderService.Where(u => u.OrderID == orderId && u.ServiceNatureID == serviceNatureId).FirstOrDefault();
            if (os != null) {
                return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, service_place = os.ServiceWorkPlace, service_date = os.ServiceWorkDate }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, service_place = "", service_date = DateTime.Now.ToString("yyyy-MM-dd") }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion 订单调度页面Action


        #region 作业信息

        public ActionResult GetDataOfJobInformation(bool _search, string sidx, string sord, int page, int rows)
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
                    List<V_OrderInfor> orders = TugBusinessLogic.Module.OrderLogic.LoadDataForJobInformation(sidx, sord);
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

        public ActionResult AddEditJobInformation()
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

                        aScheduler.JobStateID = Util.toint(Request.Form["JobStateID"].Trim()); ;

                        aScheduler.OrderID = Util.toint(Request.Form["OrderID"].Trim());
                        aScheduler.OwnerID = -1;
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid");
                        aScheduler.Remark = Request.Form["Remark"].Trim(); ;

                        aScheduler.RopeUsed = Request.Form["RopeUsed"].Trim();
                        if (aScheduler.RopeUsed.Equals("是"))
                            aScheduler.RopeNum = Util.toint(Request.Form["RopeNum"].Trim());
                        else
                            aScheduler.RopeNum = 0;

                        aScheduler.ServiceNatureID = Util.toint(Request.Form["ServiceNatureLabel"].Trim().Split('~')[0]);
                        aScheduler.TugID = Util.toint(Request.Form["TugID"].Trim());
                        aScheduler.CreateDate = aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        aScheduler.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aScheduler.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aScheduler.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aScheduler.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                        aScheduler.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        aScheduler.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        aScheduler = db.Scheduler.Add(aScheduler);
                        db.SaveChanges();

                        {
                            OrderService os = db.OrderService.Where(u => u.OrderID == aScheduler.OrderID && u.ServiceNatureID == aScheduler.ServiceNatureID).FirstOrDefault();
                            if (os == null)
                            {
                                os = new OrderService();
                                os.CreateDate = os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                os.OrderID = aScheduler.OrderID;
                                os.OwnerID = -1;
                                os.ServiceNatureID = aScheduler.ServiceNatureID;
                                os.ServiceWorkPlace = Request.Form["ServiceWorkPlace"].Trim();
                                os.UserID = Session.GetDataFromSession<int>("userid");
                                os = db.OrderService.Add(os);
                                db.SaveChanges();
                            }
                            else
                            {
                                os.ServiceWorkPlace = Request.Form["ServiceWorkPlace"].Trim();
                                os.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                db.Entry(os).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

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

                    int idx = Util.toint(Request.Form["IDX"].Trim());
                    Scheduler aScheduler = db.Scheduler.Where(u => u.IDX == idx).FirstOrDefault();

                    if (aScheduler == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        aScheduler.ArrivalBaseTime = Request.Form["ArrivalBaseTime"].Trim();
                        aScheduler.ArrivalShipSideTime = Request.Form["ArrivalShipSideTime"].Trim();
                        //aScheduler.CaptainConfirmTime = Request.Form["CaptainConfirmTime"].Trim();
                        aScheduler.DepartBaseTime = Request.Form["DepartBaseTime"].Trim();
                        //aScheduler.InformCaptainTime = Request.Form["InformCaptainTime"].Trim();
                        aScheduler.WorkCommencedTime = Request.Form["WorkCommencedTime"].Trim();
                        aScheduler.WorkCompletedTime = Request.Form["WorkCompletedTime"].Trim();

                        aScheduler.JobStateID = Util.toint(Request.Form["JobStateID"].Trim()); ;

                        aScheduler.OrderID = Util.toint(Request.Form["OrderID"].Trim());
                        aScheduler.OwnerID = -1;
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid");
                        aScheduler.Remark = Request.Form["Remark".Trim()];

                        aScheduler.RopeUsed = Request.Form["RopeUsed"].Trim();
                        if (aScheduler.RopeUsed.Equals("是"))
                            aScheduler.RopeNum = 1;//Util.toint(Request.Form["RopeNum"].Trim());
                        else
                            aScheduler.RopeNum = 0;

                        //aScheduler.ServiceNatureID = Util.toint(Request.Form["ServiceNatureLabel"].Trim().Split('~')[0]);

                        //aScheduler.TugID = Util.toint(Request.Form["TugID"].Trim());
                        aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        aScheduler.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aScheduler.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aScheduler.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aScheduler.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                        aScheduler.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        aScheduler.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        db.Entry(aScheduler).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        {
                            //更新订单状态
                            OrderInfor tmpOrder = db.OrderInfor.Where(u => u.IDX == aScheduler.OrderID).FirstOrDefault();
                            if (tmpOrder != null)
                            {
                                if (true == TugBusinessLogic.Module.OrderLogic.OrderJobInformationInputIsComplete((int)aScheduler.OrderID))
                                {
                                    tmpOrder.WorkStateID = 5; //已完工
                                    db.Entry(tmpOrder).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                                //else
                                //{
                                //    tmpOrder.WorkStateID = 5; //已完工
                                //    db.Entry(tmpOrder).State = System.Data.Entity.EntityState.Modified;
                                //    db.SaveChanges();
                                //}
                            }
                        }

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
        #endregion


        [HttpPost]
        [Authorize]
        public ActionResult CheckOrderInvoiceStatus(int orderId)
        {
            this.Internationalization();

            TugDataEntities db = new TugDataEntities();
            OrderInfor order = db.OrderInfor.FirstOrDefault(u => u.IDX == orderId);

            string ret = "否";
            if (order != null)
            {
                ret = order.HasInvoice;
            }

            return Json(new
            {
                code = Resources.Common.SUCCESS_CODE,
                message = Resources.Common.SUCCESS_MESSAGE,
                has_invoice = ret
            }, JsonRequestBehavior.AllowGet);
        }
    }
}