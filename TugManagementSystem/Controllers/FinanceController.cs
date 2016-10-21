using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TugDataModel;
using TugBusinessLogic;
using System.Transactions;
using Newtonsoft.Json;
using TugBusinessLogic.Module;

namespace TugManagementSystem.Controllers
{
    public class FinanceController : BaseController
    {
        //
        // GET: /Finance/
        [Authorize]
        public ActionResult Invoice(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            //ViewBag.Services = TugBusinessLogic.Utils.GetServices();
            ViewBag.BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            ViewBag.TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");
            ViewBag.Nodes = GetNodes();
            ViewBag.Persons = GetPersons();
            return View();
        }

        [Authorize]
        public ActionResult Invoice2(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            //ViewBag.Services = TugBusinessLogic.Utils.GetServices();
            ViewBag.BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            ViewBag.TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");
            ViewBag.Nodes = GetNodes();
            ViewBag.Persons = GetPersons();
            return View();
        }

        [Authorize]
        public ActionResult SpecialInvoice(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            //ViewBag.Services = TugBusinessLogic.Utils.GetServices();
            //ViewBag.BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            //ViewBag.TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");
            ViewBag.Nodes = GetNodes();
            ViewBag.Persons = GetPersons();
            return View();
        }


        public ActionResult DiscountBill(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            //ViewBag.Services = TugBusinessLogic.Utils.GetServices();
            //ViewBag.BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            //ViewBag.TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");
            ViewBag.Nodes = GetNodes();
            ViewBag.Persons = GetPersons();
            return View();
        }

        public string GetPersons()
        {
            string[] labels = null;
            int i = 0;
            if (labels == null)
            {
                TugDataEntities db = new TugDataEntities();
                List<UserInfor> list = db.UserInfor.Where(u => u.IsGuest == "false" && u.UserName != "admin" && u.UserDefinedCol1=="app").OrderBy(u => u.Name1).ToList<UserInfor>();
                labels = new string[list.Count];
                foreach (var itm in list)
                {
                    labels[i] = itm.Name1;
                    i++;
                }
            }
            //return labels;
            return JsonConvert.SerializeObject(labels);
        }
        public string GetNodes()
        {
            string[] labels = null;
            int i = 0;
            if (labels == null)
            {
                TugDataEntities db = new TugDataEntities();
                List<CustomField> list = db.CustomField.Where(u => u.CustomName == "Task.Node").OrderBy(u => u.CustomValue).ToList<CustomField>();
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
        /// <summary>
        /// 获取账单页面账单数据
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        //[Authorize]
        //public ActionResult GetInvoiceData(bool _search, string sidx, string sord, int page, int rows)
        //{
        //    this.Internationalization();

        //    try
        //    {
        //        TugDataEntities db = new TugDataEntities();

        //        if (_search == true)
        //        {
        //            string searchOption = Request.QueryString["filters"];
        //            List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.SearchForInvoice(sidx, sord, searchOption);

        //            int totalRecordNum = orders.Count;
        //            if (page != 0 && totalRecordNum % rows == 0) page -= 1;
        //            int pageSize = rows;
        //            int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

        //            List<V_OrderBilling> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderBilling>();

        //            var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
        //            return Json(jsonData, JsonRequestBehavior.AllowGet);
        //            //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
        //            List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.LoadDataForInvoice(sidx, sord);
        //            int totalRecordNum = orders.Count;
        //            if (page != 0 && totalRecordNum % rows == 0) page -= 1;
        //            int pageSize = rows;
        //            int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

        //            List<V_OrderBilling> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderBilling>();

        //            var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
        //            return Json(jsonData, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
        //    }
        //}


        
        [Authorize]
        [HttpGet]
        public ActionResult ViewInvoice(string lan, int? orderId)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            //return RedirectToAction("Login", "Home");

            //MyInvoice invoice = TugBusinessLogic.Module.FinanceLogic.GenerateInvoice((int)orderId);

            ViewBag.OrderID = orderId;
            return View();
        }
        public JsonResult GetInitData()
        {
            var jsonData = new[]
                     {
                         new[] {"","",""}
                    };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }   

        #region 账单操作

        /// <summary>
        /// 获取账单
        /// </summary>
        /// <param name="lan"></param>
        /// <param name="custId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult GetInvoice(string lan, int? custId, int? orderId, string orderDate, string shipLength, string shipTEUS)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            //return RedirectToAction("Login", "Home");

            MyInvoice _invoice = TugBusinessLogic.Module.FinanceLogic.GenerateInvoice((int)orderId, orderDate);

            List<TugDataModel.MyCustomField> Items = new List<MyCustomField>();
            if (_invoice.BillingTypeID == 7 || _invoice.BillingTypeValue == "1" || _invoice.BillingTypeLabel == "半包")
                Items = TugBusinessLogic.Module.FinanceLogic.GetBanBaoShowItems();

            else if (_invoice.BillingTypeID == 8 || _invoice.BillingTypeValue == "2" || _invoice.BillingTypeLabel == "计时")
                Items = TugBusinessLogic.Module.FinanceLogic.GetTiaoKuanShowItems();

            //当前账单使用的计费方案的项目
            List<MyBillingItem> customerSchemeItems = null;

            customerSchemeItems = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemeItems(_invoice.BillingTemplateID);

            //当前账单使用的计费方案
            V_BillingTemplate bt = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillScheme(_invoice.BillingTemplateID);

            //客户的计费方案
            int length = TugBusinessLogic.Module.Util.toint(shipLength);
            int teus = TugBusinessLogic.Module.Util.toint(shipTEUS);
            //List<V_BillingTemplate> customerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemes((int)custId);
            List<TugDataModel.V_BillingTemplate> customerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomersBillingTemplateByLengthAndTEUS((int)custId, length, teus);

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, invoice = _invoice, items = Items, customer_scheme = customerSchemeItems, billing_template = bt, customer_billing_schemes = customerBillingSchemes };


            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 新建账单
        /// </summary>
        /// <param name="lan"></param>
        /// <param name="orderId"></param>
        /// <param name="customerBillingScheme"></param>
        /// <param name="billingTypeId"></param>
        /// <param name="billingTypeValue"></param>
        /// <param name="billingTypeLabel"></param>
        /// <param name="timeTypeId"></param>
        /// <param name="timeTypeValue"></param>
        /// <param name="timeTypeLabel"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult NewInvoice(string lan, int? orderId, string orderDate, string customerBillingScheme,
            int billingTypeId, string billingTypeValue, string billingTypeLabel,
            int timeTypeId, string timeTypeValue, string timeTypeLabel, double discount)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            //ViewBag.CustomerBillSchemes = ;
            //List<TugDataModel.CustomField>BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            //List<TugDataModel.CustomField>TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");

            TugDataModel.MyInvoice _invoice = TugBusinessLogic.Module.FinanceLogic.NewInvoice((int)orderId, customerBillingScheme,
            billingTypeId, billingTypeValue, billingTypeLabel, timeTypeId, timeTypeValue, timeTypeLabel, discount);

            //List<TugDataModel.CustomField> Items = new List<CustomField>();
            List<TugDataModel.MyCustomField> Items = new List<MyCustomField>();
            if (billingTypeId == 7 || billingTypeValue == "1" || billingTypeLabel == "半包")
                Items = TugBusinessLogic.Module.FinanceLogic.GetBanBaoShowItems();

            else if (billingTypeId == 8 || billingTypeValue == "2" || billingTypeLabel == "计时")
                Items = TugBusinessLogic.Module.FinanceLogic.GetTiaoKuanShowItems();

            List<MyBillingItem> customerSchemeItems = null;
            if (customerBillingScheme != "-1")
            {
                customerSchemeItems = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemeItems(Convert.ToInt32(customerBillingScheme.Split('%')[0].Split('~')[0]));
            }

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, invoice = _invoice, items = Items, customer_scheme = customerSchemeItems };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [Authorize]
        public ActionResult InitFilter(string lan, int? custId, int? orderId, string shipLength, string shipTEUS)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            int length = TugBusinessLogic.Module.Util.toint(shipLength);
            int teus = TugBusinessLogic.Module.Util.toint(shipTEUS);
            //List<TugDataModel.V_BillingTemplate> CustomerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemes((int)custId);
            List<TugDataModel.V_BillingTemplate> CustomerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomersBillingTemplateByLengthAndTEUS((int)custId, length, teus);
            List<TugDataModel.CustomField> BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            List<TugDataModel.CustomField> TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");

            string month = DateTime.Now.Month.ToString() + "月";

            string remark = "";
            List<string> list = TugBusinessLogic.Module.FinanceLogic.GetOrderSchedulerRemarks((int)orderId);
            if (list != null)
            {
                foreach (string item in list)
                {
                    remark += item + "\r\n";
                }
            }

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, customer_billing_schemes = CustomerBillingSchemes, time_types = TimeTypes, billing_template_types = BillingTemplateTypes, month = month, remark = remark };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        
 

        /// <summary>
        /// 增加账单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="billingTemplateId"></param>
        /// <param name="billingTypeId"></param>
        /// <param name="timeTypeId"></param>
        /// <param name="discount"></param>
        /// <param name="amount"></param>
        /// <param name="jsonArrayItems"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult AddInvoice(int orderId, int billingTemplateId, int billingTypeId, int timeTypeId,
            string jobNo, string remark, double discount, double amount, string month, string jsonArrayItems)
        {

            this.Internationalization();

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {

                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.Billing aScheduler = new Billing();

                        aScheduler.BillingTemplateID = billingTemplateId;
                        //aScheduler.BillingCode = TugBusinessLogic.Utils.AutoGenerateBillCode();
                        aScheduler.BillingName = "";

                        aScheduler.BillingTypeID = billingTypeId;
                        aScheduler.CreateDate = aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        aScheduler.Month = DateTime.Now.Month.ToString();
                        aScheduler.OrderID = orderId;
                        aScheduler.OwnerID = -1;

                        aScheduler.Phase = 0;
                        aScheduler.Remark = "";
                        aScheduler.Status = "创建";
                        aScheduler.TimesNo = 0;
                        aScheduler.TimeTypeID = timeTypeId;
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid");
                        aScheduler.Discount = discount;
                        aScheduler.JobNo = jobNo;
                        aScheduler.Remark = remark;
                        aScheduler.Amount = amount;
                        aScheduler.Month = month;

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

                        aScheduler = db.Billing.Add(aScheduler);
                        db.SaveChanges();

                        List<InVoiceItem> listInVoiceItems = new List<InVoiceItem>();
                        listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceItem>(jsonArrayItems);
                        if (listInVoiceItems != null)
                        {
                            foreach (InVoiceItem item in listInVoiceItems)
                            {
                                BillingItem bi = new BillingItem();
                                bi.BillingID = aScheduler.IDX;
                                bi.SchedulerID = item.SchedulerID;
                                bi.ItemID = item.ItemID;
                                bi.UnitPrice = item.UnitPrice;
                                bi.Currency = item.Currency;
                                bi.OwnerID = -1;
                                bi.UserID = Session.GetDataFromSession<int>("userid");
                                bi.CreateDate = bi.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                bi.UserDefinedCol1 = "";
                                bi.UserDefinedCol2 = "";
                                bi.UserDefinedCol3 = "";
                                bi.UserDefinedCol4 = "";

                                //if (Request.Form["UserDefinedCol5"].Trim() != "")
                                //    aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                                //if (Request.Form["UserDefinedCol6"].Trim() != "")
                                //    aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                                //if (Request.Form["UserDefinedCol7"].Trim() != "")
                                //    aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                                //if (Request.Form["UserDefinedCol8"].Trim() != "")
                                //    aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                                bi.UserDefinedCol9 = "";
                                bi.UserDefinedCol10 = "";

                                bi = db.BillingItem.Add(bi);
                                db.SaveChanges();
                            }
                        }

                        //更新订单的字段 V_OrderInfor_HasInvoice	是否已有帳單	
                        {
                            OrderInfor od = db.OrderInfor.FirstOrDefault(u => u.IDX == orderId);
                            //throw new Exception();
                            if (od != null)
                            {
                                od.HasInvoice = "是";
                                db.Entry(od).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        trans.Complete();

                        var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                        //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                        return Json(ret, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception)
                {
                    trans.Dispose();
                    var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                    //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                    return Json(ret, JsonRequestBehavior.AllowGet);
                }
            }
        }



        


        /// <summary>
        /// 删除账单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult DeleteInvoice()
        {
            this.Internationalization();

            try
            {
                Expression condition = Expression.Equal(Expression.Constant(1, typeof(int)), Expression.Constant(2, typeof(int)));
                ParameterExpression parameter = Expression.Parameter(typeof(Billing));

                //string strBillIds = Request.Form["billIds"];
                string orderIds = Request.Form["orderIds"];

                if (orderIds != "")
                {
                    List<string> listOrderIds = orderIds.Split(',').ToList();

                    TugDataEntities db = new TugDataEntities();

                    List<Billing> deletedBillings = new List<Billing>();

                    foreach (string orderId in listOrderIds)
                    {
                        int oid = TugBusinessLogic.Module.Util.toint(orderId);

                        Billing b = db.Billing.FirstOrDefault(u => u.OrderID == oid);
                        if (b != null)
                        {
                            deletedBillings.Add(b);
                        }
                    }

                    {
                        db.Billing.RemoveRange(deletedBillings);
                        db.SaveChanges();


                        ////更新订单的字段 V_OrderInfor_HasInvoice	是否已有帳單	
                        //{
                        //    string strOrderIds = Request.Form["orderIds"];
                        //    if (strOrderIds != "")
                        //    {
                        //        //List<string> listOrderIds = strOrderIds.Split(',').ToList();

                        //        foreach (string item in listOrderIds)
                        //        {
                        //            int orderId = TugBusinessLogic.Module.Util.toint(item);
                        //            OrderInfor od = db.OrderInfor.FirstOrDefault(u => u.IDX == orderId);
                        //            if (od != null)
                        //            {
                        //                od.HasInvoice = "否";
                        //                db.Entry(od).State = System.Data.Entity.EntityState.Modified;
                        //                db.SaveChanges();
                        //            }
                        //        }
                        //    }
                        //}

                        string strOrderIds = Request.Form["orderIds"];
                        if (strOrderIds != "")
                        {
                            foreach (string item in listOrderIds)
                            {
                                int orderId = TugBusinessLogic.Module.Util.toint(item);
                                TugBusinessLogic.Module.FinanceLogic.RejectInvoice(orderId);
                            }
                        }


                        return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                    }
                    //else
                    //{
                    //    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    //}
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
            return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
        }


        /// <summary>
        /// 修改账单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="billingId"></param>
        /// <param name="billingTemplateId"></param>
        /// <param name="billingTypeId"></param>
        /// <param name="timeTypeId"></param>
        /// <param name="discount"></param>
        /// <param name="amount"></param>
        /// <param name="jsonArrayItems"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult EditInvoice(int orderId, int billingId, int billingTemplateId, int billingTypeId, int timeTypeId, 
            string jobNo, string remark, double discount, double amount, string month, string jsonArrayItems)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    Billing oldBilling = db.Billing.FirstOrDefault(u => u.IDX == billingId);

                    if (oldBilling != null)
                    {
                        oldBilling.BillingTemplateID = billingTemplateId;
                        oldBilling.BillingTypeID = billingTypeId;
                        oldBilling.TimeTypeID = timeTypeId;
                        oldBilling.Discount = discount;
                        oldBilling.Amount = amount;
                        oldBilling.JobNo = jobNo;
                        oldBilling.Remark = remark;
                        oldBilling.Month = month;
                        oldBilling.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        db.Entry(oldBilling).State = System.Data.Entity.EntityState.Modified;
                        int ret = db.SaveChanges();

                        if(ret > 0)
                        {
                            List<BillingItem> invoiceItems = db.BillingItem.Where(u => u.BillingID == billingId).ToList();
                            if (invoiceItems != null)
                            {
                                db.BillingItem.RemoveRange(invoiceItems);
                                ret = db.SaveChanges();
                                if (ret > 0)
                                {
                                    List<InVoiceItem> listInVoiceItems = new List<InVoiceItem>();
                                    listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceItem>(jsonArrayItems);
                                    if (listInVoiceItems != null)
                                    {
                                        foreach (InVoiceItem item in listInVoiceItems)
                                        {
                                            BillingItem bi = new BillingItem();
                                            bi.BillingID = billingId;
                                            bi.SchedulerID = item.SchedulerID;
                                            bi.ItemID = item.ItemID;
                                            bi.UnitPrice = item.UnitPrice;
                                            bi.Currency = item.Currency;
                                            bi.OwnerID = -1;
                                            bi.UserID = Session.GetDataFromSession<int>("userid"); ;
                                            bi.CreateDate = bi.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            bi.UserDefinedCol1 = "";
                                            bi.UserDefinedCol2 = "";
                                            bi.UserDefinedCol3 = "";
                                            bi.UserDefinedCol4 = "";

                                            //if (Request.Form["UserDefinedCol5"].Trim() != "")
                                            //    aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                                            //if (Request.Form["UserDefinedCol6"].Trim() != "")
                                            //    aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                                            //if (Request.Form["UserDefinedCol7"].Trim() != "")
                                            //    aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                                            //if (Request.Form["UserDefinedCol8"].Trim() != "")
                                            //    aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                                            bi.UserDefinedCol9 = "";
                                            bi.UserDefinedCol10 = "";

                                            bi = db.BillingItem.Add(bi);
                                            ret = db.SaveChanges();
                                        }
                                    }
                                    else 
                                    {
                                        return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                }
                                
                            }
                            else 
                            {
                                List<InVoiceItem> listInVoiceItems = new List<InVoiceItem>();
                                listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceItem>(jsonArrayItems);
                                if (listInVoiceItems != null)
                                {
                                    foreach (InVoiceItem item in listInVoiceItems)
                                    {
                                        BillingItem bi = new BillingItem();
                                        bi.BillingID = billingId;
                                        bi.SchedulerID = item.SchedulerID;
                                        bi.ItemID = item.ItemID;
                                        bi.UnitPrice = item.UnitPrice;
                                        bi.Currency = item.Currency;
                                        bi.OwnerID = -1;
                                        bi.UserID = Session.GetDataFromSession<int>("userid"); ;
                                        bi.CreateDate = bi.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        bi.UserDefinedCol1 = "";
                                        bi.UserDefinedCol2 = "";
                                        bi.UserDefinedCol3 = "";
                                        bi.UserDefinedCol4 = "";

                                        //if (Request.Form["UserDefinedCol5"].Trim() != "")
                                        //    aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                                        //if (Request.Form["UserDefinedCol6"].Trim() != "")
                                        //    aScheduler.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"].Trim());

                                        //if (Request.Form["UserDefinedCol7"].Trim() != "")
                                        //    aScheduler.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"].Trim());

                                        //if (Request.Form["UserDefinedCol8"].Trim() != "")
                                        //    aScheduler.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"].Trim());

                                        bi.UserDefinedCol9 = "";
                                        bi.UserDefinedCol10 = "";

                                        bi = db.BillingItem.Add(bi);
                                        ret = db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                        {
                            return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
        }


        [HttpPost]
        [Authorize]
        public ActionResult CheckOrderInvoiceStatus(string selectedOrderIDs)
        {
            this.Internationalization();

            Dictionary<int, int> dicHasNoInvoice = new Dictionary<int, int>();
            Dictionary<int, int> dicHasInvoiceNotInFlow = new Dictionary<int, int>();
            Dictionary<int, int> dicHasInvoiceInFow = new Dictionary<int, int>();
            Dictionary<int, int> dicHasInvoiceNotInFlowBills = new Dictionary<int, int>();
            string ret = "";

            TugDataEntities db = new TugDataEntities();
            List<string> list = selectedOrderIDs.Split(',').ToList();
            if (list != null)
            {
                foreach (string item in list)
                {
                    int orderId = Convert .ToInt32(item.Split(':')[1]);
                    V_OrderService_Scheduler ob = db.V_OrderService_Scheduler.FirstOrDefault(u => u.OrderID == orderId);
                    if (ob != null)
                    {
                        ret = "該訂單已排船,不可編輯！";
                    }
                }
            }
            TugBusinessLogic.Module.OrderLogic.GetStatusOfOrderInvoice(selectedOrderIDs, out dicHasNoInvoice, out dicHasInvoiceNotInFlow, out dicHasInvoiceInFow, out dicHasInvoiceNotInFlowBills);
            return Json(new
            {
                code = Resources.Common.SUCCESS_CODE,
                message = Resources.Common.SUCCESS_MESSAGE,
                dic_has_no_invoice = dicHasNoInvoice,
                dic_has_invoice_not_in_flow = dicHasInvoiceNotInFlow,
                dic_has_invoice_in_fow = dicHasInvoiceInFow,
                dic_has_invoice_not_in_flow_bills = dicHasInvoiceNotInFlowBills,
                ret
            }, JsonRequestBehavior.AllowGet);
  

          
        }
        static private string GetStatusOfOrderScheduler(int orderId)
        {

            string ret = "";
            TugDataEntities db = new TugDataEntities();
            V_OrderService_Scheduler ob = db.V_OrderService_Scheduler.FirstOrDefault(u => u.OrderID == orderId);
            if (ob != null)
            {
                ret = "該訂單已排船";
            }
            return ret;
        }

        #endregion


        #region 回扣单操作

        /// <summary>
        /// 获取回扣单数据
        /// </summary>
        /// <returns></returns>
 
        [Authorize]
        public ActionResult GetCreditData(bool _search, string sidx, string sord, int page, int rows, int billingId)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                //int idx = Util.toint(Request.Form["IDX"].Trim());
                {
                    List<MyCredit> orders = new List<MyCredit>();

                    switch (sidx)
                    {
                        case "":
                            {
                                orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditID)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                            }
                            break;

                        case "CreditCode":
                            {
                                if (sord == "asc")
                                {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditCode)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else
                                {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditCode)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;

                        case "CreditContent":
                            {
                                if (sord == "asc") {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditContent)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditContent)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;

                        case "CreditAmount":
                            {
                                if (sord == "asc") {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditAmount)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditAmount)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "Remark":
                            {
                                if (sord == "asc") {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.Remark)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.Remark)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "CreateDate":
                            {
                                if (sord == "asc") {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreateDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreateDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "LastUpDate":
                            {
                                if (sord == "asc") {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.LastUpDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.LastUpDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                            
                        default:
                            break;

                    }
                    
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<MyCredit> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<MyCredit>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 新增回扣单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult AddCredit(int billingId,string billingCode, string creditContent, double creditAmount, string remark) {

            this.Internationalization();
            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    TugDataModel.Credit credit = new Credit();

                    credit.BillingID = billingId;
                    credit.CreditCode = "C" +  billingCode.Substring(1, billingCode.Length - 1 );
                    credit.CreditContent = creditContent;
                    credit.CreditAmount = creditAmount;
                    credit.Remark = remark;
                    credit.CreateDate = credit.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    credit.OwnerID = -1;
                    credit.UserID = credit.UserID = Session.GetDataFromSession<int>("userid");

                    //credit.UserDefinedCol1 = "";
                    //credit.UserDefinedCol2 = "";
                    //credit.UserDefinedCol3 = "";
                    //credit.UserDefinedCol4 = "";
                    //credit.UserDefinedCol5 = 0;
                    //credit.UserDefinedCol6 = 0;
                    //credit.UserDefinedCol7 = 0;
                    //credit.UserDefinedCol8 = 0;

                    //credit.UserDefinedCol9 = "";
                    //credit.UserDefinedCol10 = "";

                    credit = db.Credit.Add(credit);
                    db.SaveChanges();


                    //更新账单中的回扣金额
                    {
                        TugBusinessLogic.Module.FinanceLogic.UpdateTotalRebateOfBilling(billingId);
                    }
                    
                }

                var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改回扣单
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult AddEditCredit() {
            this.Internationalization();

            #region Edit

            if (Request.Form["oper"].Equals("edit"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();

                    int idx = TugBusinessLogic.Module.Util.toint(Request.Form["IDX"].Trim());
                    Credit aOrder = db.Credit.Where(u => u.IDX == idx).FirstOrDefault();

                    if (aOrder == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {

                        aOrder.CreditContent = Request.Form["CreditContent"].Trim();
                        aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        aOrder.CreditAmount = TugBusinessLogic.Module.Util.tonumeric(Request.Form["CreditAmount"].Trim());
                        aOrder.Remark = Request.Form["Remark"].Trim();


                        //aOrder.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        //aOrder.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        //aOrder.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        //aOrder.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        //if (Request.Form["UserDefinedCol5"].Trim() != "")
                        //    aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        //if (Request.Form["UserDefinedCol6"].Trim() != "")
                        //    aOrder.UserDefinedCol6 = TugBusinessLogic.Module.Util.toint(Request.Form["UserDefinedCol6"].Trim());

                        //if (Request.Form["UserDefinedCol7"].Trim() != "")
                        //    aOrder.UserDefinedCol7 = TugBusinessLogic.Module.Util.toint(Request.Form["UserDefinedCol7"].Trim());

                        //if (Request.Form["UserDefinedCol8"].Trim() != "")
                        //    aOrder.UserDefinedCol8 = TugBusinessLogic.Module.Util.toint(Request.Form["UserDefinedCol8"].Trim());

                        //aOrder.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        //aOrder.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        db.Entry(aOrder).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        //更新账单的回扣金额
                        {
                            TugBusinessLogic.Module.FinanceLogic.UpdateTotalRebateOfBilling((int)aOrder.BillingID);
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

        /// <summary>
        /// 删除回扣单
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult DeleteCredit(int creditId) {

            this.Internationalization();

            TugDataEntities db = new TugDataEntities();

            var list = db.Credit.Where(u => u.IDX == creditId).ToList();
            if(list != null)
            {
                //保存删除的回扣单对应的billingId
                int billingIdOfDeletedCredit = (int)list[0].BillingID;
                

                db.Credit.RemoveRange(list);
                if(db.SaveChanges() > 0)
                {
                    //更新账单的回扣金额
                    {
                        TugBusinessLogic.Module.FinanceLogic.UpdateTotalRebateOfBilling(billingIdOfDeletedCredit);
                    }
                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                }
                else
                {
                    return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE });
                }
            }
            return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE });
        }

        #endregion



        #region 账单操作2


        public ActionResult GetBillingDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    //List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.SearchForInvoice(sidx, sord, searchOption);
                    List<V_Billing2> orders = TugBusinessLogic.Module.FinanceLogic.SearchDataForBilling(sidx, sord, searchOption);

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_Billing2> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_Billing2>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                    //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<V_Billing2> orders = TugBusinessLogic.Module.FinanceLogic.LoadDataForBilling(sidx, sord);
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_Billing2> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_Billing2>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }
       /// <summary>
       /// 驳回账单之后，要删除账单，删除账单的同时调用此Action
       /// </summary>
       /// <param name="billingId"></param>
       /// <returns></returns>
        public ActionResult RejectBilling2(int billingId)
        {
            TugBusinessLogic.Module.FinanceLogic.RejectInvoice2(billingId);
            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RejectBilling(int billingId)
        {
            TugBusinessLogic.Module.FinanceLogic.RejectInvoice(billingId);
            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [Authorize]
        public ActionResult InitFilter2(string lan, int? custId, string orderIds, string shipLength, string shipTEUS)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            int length = TugBusinessLogic.Module.Util.toint(shipLength);
            int teus = TugBusinessLogic.Module.Util.toint(shipTEUS);
            //List<TugDataModel.V_BillingTemplate> CustomerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemes((int)custId);
            //List<TugDataModel.V_BillingTemplate> CustomerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomersBillingTemplateByLengthAndTEUS((int)custId, length, teus);
            List<TugDataModel.V_BillingTemplate> CustomerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomersBillingTemplateByLengthAndTEUS2((int)custId, length, teus);
            List<TugDataModel.CustomField> BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            List<TugDataModel.CustomField> TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");

            string month = DateTime.Now.ToString("yyyy-MM");

            string remark = "";

            List<string> orderIDs = orderIds.Split(',').ToList();
            if (orderIDs != null)
            {
                foreach (var item in orderIDs)
                {
                    int orderId = TugBusinessLogic.Module.Util.toint(item);
                    List<string> list = TugBusinessLogic.Module.FinanceLogic.GetOrderSchedulerRemarks((int)orderId);
                    if (list != null)
                    {
                        foreach (string item2 in list)
                        {
                            remark += item2 + "\r\n";
                        }
                    }
                }

            }

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, customer_billing_schemes = CustomerBillingSchemes, time_types = TimeTypes, billing_template_types = BillingTemplateTypes, month = month, remark = remark };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 新建账单
        /// </summary>
        /// <param name="lan"></param>
        /// <param name="orderId"></param>
        /// <param name="customerBillingScheme"></param>
        /// <param name="billingTypeId"></param>
        /// <param name="billingTypeValue"></param>
        /// <param name="billingTypeLabel"></param>
        /// <param name="timeTypeId"></param>
        /// <param name="timeTypeValue"></param>
        /// <param name="timeTypeLabel"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult NewInvoice2(string lan, string orderIds, string customerBillingScheme,
            int billingTypeId, string billingTypeValue, string billingTypeLabel,
            int timeTypeId, string timeTypeValue, string timeTypeLabel, double discount)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            //ViewBag.CustomerBillSchemes = ;
            //List<TugDataModel.CustomField>BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            //List<TugDataModel.CustomField>TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");

            TugDataModel.MyInvoice _invoice = TugBusinessLogic.Module.FinanceLogic.NewInvoice2(orderIds, customerBillingScheme,
            billingTypeId, billingTypeValue, billingTypeLabel, timeTypeId, timeTypeValue, timeTypeLabel, discount);

            //List<TugDataModel.CustomField> Items = new List<CustomField>();
            List<TugDataModel.MyCustomField> Items = new List<MyCustomField>();
            if (billingTypeId == 7 || billingTypeValue == "1" || billingTypeLabel == "半包")
                Items = TugBusinessLogic.Module.FinanceLogic.GetBanBaoShowItems();

            else if (billingTypeId == 8 || billingTypeValue == "2" || billingTypeLabel == "计时")
                Items = TugBusinessLogic.Module.FinanceLogic.GetTiaoKuanShowItems();

            List<MyBillingItem> customerSchemeItems = null;
            if (customerBillingScheme != "-1")
            {
                customerSchemeItems = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemeItems(Convert.ToInt32(customerBillingScheme.Split('%')[0].Split('~')[0]));
            }

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, invoice = _invoice, items = Items, customer_scheme = customerSchemeItems };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        public ActionResult AddInvoice2(int custId, int custShipId, string orderIds, string orderServiceIds, int billingTemplateId, int billingTypeId, int timeTypeId,
            string jobNo, string billing_code, string remark, double discount, double amount, string month, int customer_ship_length, 
            int customer_ship_teus, string isShowShipLengthRule, string isShowShipTEUSRule,
            float? ratio1, float? ratio2, float? ratio3, float? ratio4, float? ratio5, float? ratio6, float? minTime,
            string jsonArrayItems, string jsonArraySummaryItems)
        {

            this.Internationalization();

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    

                    List<string> strOrderIDs = orderIds.Split(',').ToList();
                    List<int> iOrderIDs = new List<int>();
                    if (strOrderIDs != null)
                    {
                        foreach (var item in strOrderIDs)
                        {
                            iOrderIDs.Add(TugBusinessLogic.Module.Util.toint(item));
                        }
                    }


                    TugDataEntities db = new TugDataEntities();
                    {
                        //0.验证账单编号是否已存在
                        var tmp = db.Billing.FirstOrDefault(u => u.BillingCode == billing_code);
                        if (tmp != null)
                        {
                            var ret2 = new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE };
                            //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                            return Json(ret2, JsonRequestBehavior.AllowGet);
                        }

                        //1.插入账单
                        TugDataModel.Billing aScheduler = new Billing();

                        aScheduler.CustomerID = custId;
                        aScheduler.JobNo = jobNo;
                        aScheduler.IsShowShipLengthRule = isShowShipLengthRule;
                        aScheduler.IsShowShipTEUSRule = isShowShipTEUSRule;
                        aScheduler.InvoiceType = "普通账单";

                        aScheduler.BillingTemplateID = billingTemplateId;
                        aScheduler.BillingTypeID = billingTypeId;
                        //aScheduler.BillingCode = TugBusinessLogic.Utils.AutoGenerateBillCode();
                        aScheduler.BillingCode = billing_code;
                        aScheduler.BillingName = "";
                        aScheduler.TimeTypeID = timeTypeId;
                        aScheduler.Discount = discount;
                        aScheduler.TotalRebate = 0;
                        aScheduler.Amount = amount;
                        aScheduler.Remark = remark;
                        aScheduler.Month = month;

                        if(ratio1 != null)
                        aScheduler.Ratio1 = (int?)Math.Round((float)ratio1, 2);
                        if (ratio1 != null)
                        aScheduler.Ratio2 = (int?)Math.Round((float)ratio2, 2);
                        if (ratio1 != null)
                        aScheduler.Ratio3 = (int?)Math.Round((float)ratio3, 2);
                        if (ratio1 != null)
                        aScheduler.Ratio4 = (int?)Math.Round((float)ratio4, 2);
                        if (ratio1 != null)
                        aScheduler.Ratio5 = (int?)Math.Round((float)ratio5, 2);
                        if (ratio1 != null)
                        aScheduler.Ratio6 = (int?)Math.Round((float)ratio6, 2);
                        if (ratio1 != null)
                        aScheduler.MinTime = (int?)Math.Round((float)minTime, 2);

                        aScheduler.TimesNo = 0;
                        aScheduler.Status = "创建";
                        aScheduler.Phase = 0;

                        aScheduler.OwnerID = -1;
                        aScheduler.CreateDate = aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid");


                        aScheduler.UserDefinedCol1 = "";
                        aScheduler.UserDefinedCol2 = "";
                        aScheduler.UserDefinedCol3 = "";
                        aScheduler.UserDefinedCol4 = "";

                        aScheduler.UserDefinedCol9 = "";
                        aScheduler.UserDefinedCol10 = "";

                        aScheduler = db.Billing.Add(aScheduler);
                        db.SaveChanges();


                        //2.插入账单、多个订单
                        List<BillingOrder> listBillingOrder = new List<BillingOrder>();
                        foreach (int orderId in iOrderIDs)
                        {
                            BillingOrder bo = new BillingOrder();
                            bo.BillingID = aScheduler.IDX;
                            bo.OrderID = orderId;
                            listBillingOrder.Add(bo);
                        }
                        db.BillingOrder.AddRange(listBillingOrder);
                        db.SaveChanges();


                        //3.插入账单的收费项目
                        List<InVoiceItem> listInVoiceItems = new List<InVoiceItem>();
                        listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceItem>(jsonArrayItems);
                        if (listInVoiceItems != null)
                        {
                            foreach (InVoiceItem item in listInVoiceItems)
                            {
                                BillingItem bi = new BillingItem();
                                bi.BillingID = aScheduler.IDX;
                                bi.SchedulerID = item.SchedulerID;
                                bi.ItemID = item.ItemID;
                                bi.UnitPrice = item.UnitPrice;
                                bi.Currency = item.Currency;
                                bi.OwnerID = -1;
                                bi.UserID = Session.GetDataFromSession<int>("userid");
                                bi.CreateDate = bi.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                bi.UserDefinedCol1 = "";
                                bi.UserDefinedCol2 = "";
                                bi.UserDefinedCol3 = "";
                                bi.UserDefinedCol4 = "";

                                bi.UserDefinedCol9 = "";
                                bi.UserDefinedCol10 = "";

                                bi = db.BillingItem.Add(bi);
                                db.SaveChanges();
                            }
                        }

                        //4.插入账单的汇总项目
                        List<InVoiceSummaryItem> listInVoiceSummaryItems = new List<InVoiceSummaryItem>();
                        listInVoiceSummaryItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceSummaryItem>(jsonArraySummaryItems);
                        if (listInVoiceSummaryItems != null)
                        {
                            foreach (InVoiceSummaryItem item in listInVoiceSummaryItems)
                            {
                                AmountSum amtSum = new AmountSum();
                                amtSum.CustomerID = custId;
                                amtSum.CustomerShipID = custShipId;
                                amtSum.BillingID = aScheduler.IDX;
                                amtSum.BillingDateTime = TugBusinessLogic.Utils.CNDateTimeToDateTime(aScheduler.CreateDate);
                                amtSum.SchedulerID = item.SchedulerID;
                                amtSum.Amount = item.Amount;
                                amtSum.FuelAmount = item.FuelPrice;
                                amtSum.Currency = item.Currency;
                                amtSum.Hours = item.Hours;
                                amtSum.Year = aScheduler.Month.Split('-')[0];//DateTime.Now.Year.ToString();
                                amtSum.Month = aScheduler.Month.Split('-')[1];// aScheduler.Month;
                                amtSum.OwnerID = -1;
                                amtSum.CreateDate = amtSum.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                amtSum.UserID = Session.GetDataFromSession<int>("userid");

                                amtSum = db.AmountSum.Add(amtSum);
                                db.SaveChanges();
                            }
                        }

                        //5.更新订单的字段 V_OrderInfor_HasInvoice	是否已有帳單	
                        {
                            List<OrderInfor> odList = db.OrderInfor.Where(u => iOrderIDs.Contains(u.IDX)).ToList();
                            //throw new Exception();
                            if (odList != null)
                            {
                                foreach (OrderInfor od in odList)
                                {
                                    od.HasInvoice = "是";
                                    od.HasInFlow = "否";
                                    db.Entry(od).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }

                        //6.更新订单服务表里的字段 HasBilling、HasBillingInFlow 是否已有帳單	
                        {

                            List<string> strOrderServiceIDs = orderServiceIds.Split(',').ToList();
                            List<int> iOrderServiceIDs = new List<int>();
                            if (strOrderServiceIDs != null)
                            {
                                foreach (var item in strOrderServiceIDs)
                                {
                                    iOrderServiceIDs.Add(TugBusinessLogic.Module.Util.toint(item));
                                }
                            }


                            List<OrderService> ordSrvList = db.OrderService.Where(u => iOrderServiceIDs.Contains((int)u.IDX)).ToList();
                            //throw new Exception();
                            if (ordSrvList != null)
                            {
                                foreach (OrderService ods in ordSrvList)
                                {
                                    ods.HasBilling = "是";
                                    ods.HasBillingInFlow = "否";
                                    ods.BillingType = 0;
                                    db.Entry(ods).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }

                        //7.更新客户船的船长、箱量
                        {
                            CustomerShip cs = db.CustomerShip.FirstOrDefault(u => u.IDX == custShipId);
                            if (cs != null)
                            {
                                cs.Length = customer_ship_length;
                                cs.TEUS = customer_ship_teus;
                                db.Entry(cs).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        trans.Complete();

                        var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, billing_id = aScheduler.IDX };
                        //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                        return Json(ret, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE, billing_id = -1 };
                    //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                    return Json(ret, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 删除账单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult DeleteInvoice2(string billIds)
        {
            this.Internationalization();

            try
            {
                //Expression condition = Expression.Equal(Expression.Constant(1, typeof(int)), Expression.Constant(2, typeof(int)));
                //ParameterExpression parameter = Expression.Parameter(typeof(Billing));

                if (billIds != "")
                {
                    List<string> listBillingIds = billIds.Split(',').ToList();

                    TugDataEntities db = new TugDataEntities();

                    foreach (string billingId in listBillingIds)
                    {
                        int bid = TugBusinessLogic.Module.Util.toint(billingId);

                        Billing b = db.Billing.FirstOrDefault(u => u.IDX == bid);
                        if (b != null)
                        {
                            TugBusinessLogic.Module.FinanceLogic.RejectInvoice2(bid);
                            TugBusinessLogic.Module.FinanceLogic.SetOrderServiceInvoiceStatus(bid, "否");
                            db.Billing.Remove(b);
                            db.SaveChanges();
                            
                        }
                    }

                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
            return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
        }



        [HttpPost]
        [Authorize]
        public ActionResult EditInvoice2(int billingId, int billingTemplateId, int billingTypeId, int timeTypeId,
            string jobNo, string remark, string billing_code, double discount, double amount, string month, int customer_ship_length,
            int customer_ship_teus, string jsonArrayItems, string isShowShipLengthRule,
            float? ratio1, float? ratio2, float? ratio3, float? ratio4, float? ratio5, float? ratio6, float? minTime,
            string isShowShipTEUSRule, string jsonArraySummaryItems)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    Billing oldBilling = db.Billing.FirstOrDefault(u => u.IDX == billingId);

                    Billing tmp = db.Billing.FirstOrDefault(u => u.BillingCode == billing_code);
                    if (tmp != null)
                    {
                        if (tmp.IDX != oldBilling.IDX)
                        {
                            return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (oldBilling != null)
                    {
                        oldBilling.BillingTemplateID = billingTemplateId;
                        oldBilling.BillingTypeID = billingTypeId;
                        oldBilling.TimeTypeID = timeTypeId;
                        oldBilling.Discount = discount;
                        oldBilling.Amount = amount;
                        oldBilling.BillingCode = billing_code;
                        oldBilling.JobNo = jobNo;
                        oldBilling.Ratio1 = (double?)Math.Round((double)ratio1, 2);
                        oldBilling.Ratio2 = (double?)Math.Round((double)ratio2, 2);
                        oldBilling.Ratio3 = (double?)Math.Round((double)ratio3, 2);
                        oldBilling.Ratio4 = (double?)Math.Round((double)ratio4, 2);
                        oldBilling.Ratio5 = (double?)Math.Round((double)ratio5, 2);
                        oldBilling.Ratio6 = (double?)Math.Round((double)ratio6, 2);
                        oldBilling.MinTime = (double?)Math.Round((double)minTime, 2); 
                        oldBilling.Remark = remark;
                        oldBilling.Month = month;
                        oldBilling.IsShowShipLengthRule = isShowShipLengthRule;
                        oldBilling.IsShowShipTEUSRule = isShowShipTEUSRule;
                        oldBilling.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        db.Entry(oldBilling).State = System.Data.Entity.EntityState.Modified;
                        int ret = db.SaveChanges();

                        if (ret > 0)
                        {
                            #region 更新客户船长、箱量
                            V_Billing2 vb2 = db.V_Billing2.FirstOrDefault(u => u.IDX == billingId);
                            if (vb2 != null)
                            {
                                //vb2.ShipID;
                                CustomerShip cs = db.CustomerShip.FirstOrDefault(u => u.IDX == vb2.ShipID);
                                if (cs != null)
                                {
                                    cs.Length = customer_ship_length;
                                    cs.TEUS = customer_ship_teus;
                                    db.Entry(cs).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            
                            #endregion

                            #region 订单收费项
                            //1.订单收费项
                            List<BillingItem> invoiceItems = db.BillingItem.Where(u => u.BillingID == billingId).ToList();
                            if (invoiceItems != null)
                            {
                                db.BillingItem.RemoveRange(invoiceItems);
                                ret = db.SaveChanges();
                                if (ret > 0)
                                {
                                    List<InVoiceItem> listInVoiceItems = new List<InVoiceItem>();
                                    listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceItem>(jsonArrayItems);
                                    if (listInVoiceItems != null)
                                    {
                                        foreach (InVoiceItem item in listInVoiceItems)
                                        {
                                            BillingItem bi = new BillingItem();
                                            bi.BillingID = billingId;
                                            bi.SchedulerID = item.SchedulerID;
                                            bi.ItemID = item.ItemID;
                                            bi.UnitPrice = item.UnitPrice;
                                            bi.Currency = item.Currency;
                                            bi.OwnerID = -1;
                                            bi.UserID = Session.GetDataFromSession<int>("userid"); ;
                                            bi.CreateDate = bi.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            bi.UserDefinedCol1 = "";
                                            bi.UserDefinedCol2 = "";
                                            bi.UserDefinedCol3 = "";
                                            bi.UserDefinedCol4 = "";

                                            bi.UserDefinedCol9 = "";
                                            bi.UserDefinedCol10 = "";

                                            bi = db.BillingItem.Add(bi);
                                            ret = db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                }

                            }
                            else
                            {
                                List<InVoiceItem> listInVoiceItems = new List<InVoiceItem>();
                                listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceItem>(jsonArrayItems);
                                if (listInVoiceItems != null)
                                {
                                    foreach (InVoiceItem item in listInVoiceItems)
                                    {
                                        BillingItem bi = new BillingItem();
                                        bi.BillingID = billingId;
                                        bi.SchedulerID = item.SchedulerID;
                                        bi.ItemID = item.ItemID;
                                        bi.UnitPrice = item.UnitPrice;
                                        bi.Currency = item.Currency;
                                        bi.OwnerID = -1;
                                        bi.UserID = Session.GetDataFromSession<int>("userid"); ;
                                        bi.CreateDate = bi.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        bi.UserDefinedCol1 = "";
                                        bi.UserDefinedCol2 = "";
                                        bi.UserDefinedCol3 = "";
                                        bi.UserDefinedCol4 = "";

                                        bi.UserDefinedCol9 = "";
                                        bi.UserDefinedCol10 = "";

                                        bi = db.BillingItem.Add(bi);
                                        ret = db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            #endregion

                            #region 账单的汇总项目
                            //2.插入账单的汇总项目

                            List<AmountSum> oldAmountSumList = db.AmountSum.Where(u => u.BillingID == oldBilling.IDX).ToList();
                            db.AmountSum.RemoveRange(oldAmountSumList);
                            db.SaveChanges();

                            List<InVoiceSummaryItem> listInVoiceSummaryItems = new List<InVoiceSummaryItem>();
                            listInVoiceSummaryItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceSummaryItem>(jsonArraySummaryItems);
                            if (listInVoiceSummaryItems != null)
                            {
                                V_Invoice2 vi2 = db.V_Invoice2.FirstOrDefault(u => u.BillingID == oldBilling.IDX);

                                foreach (InVoiceSummaryItem item in listInVoiceSummaryItems)
                                {
                                    AmountSum amtSum = new AmountSum();
                                    amtSum.CustomerID = vi2.CustomerID;
                                    amtSum.CustomerShipID = vi2.ShipID;
                                    amtSum.BillingID = oldBilling.IDX;
                                    amtSum.BillingDateTime = TugBusinessLogic.Utils.CNDateTimeToDateTime(oldBilling.CreateDate);
                                    amtSum.SchedulerID = item.SchedulerID;
                                    amtSum.Amount = item.Amount;
                                    amtSum.FuelAmount = item.FuelPrice;
                                    amtSum.Currency = item.Currency;
                                    amtSum.Hours = item.Hours;
                                    amtSum.Year = oldBilling.Month.Split('-')[0];//DateTime.Now.Year.ToString();
                                    amtSum.Month = oldBilling.Month.Split('-')[1];//oldBilling.Month;
                                    amtSum.OwnerID = -1;
                                    amtSum.CreateDate = amtSum.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    amtSum.UserID = Session.GetDataFromSession<int>("userid");

                                    amtSum = db.AmountSum.Add(amtSum);
                                    db.SaveChanges();
                                }
                            }
                            #endregion

                            #region 更新回扣单编号
                            System.Linq.Expressions.Expression<Func<Credit, bool>> expCredit = u => u.BillingID == billingId;
                            List<Credit> tmpCredit = db.Credit.Where(expCredit).Select(u => u).ToList<Credit>();
                            //Credit tmpCredit = db.Credit.Where(expCredit).FirstOrDefault();
                            if (tmpCredit.Count != 0)
                            {
                                foreach (var item in tmpCredit)
                                {
                                    item.CreditCode = "C" + billing_code.Substring(1, billing_code.Length - 1);
                                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_CODE }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取账单
        /// </summary>
        /// <param name="lan"></param>
        /// <param name="custId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult GetInvoice2(string lan, int billingId)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            //return RedirectToAction("Login", "Home");

            MyInvoice _invoice = TugBusinessLogic.Module.FinanceLogic.GenerateInvoice2((int)billingId);

            List<TugDataModel.MyCustomField> Items = new List<MyCustomField>();
            if (_invoice.BillingTypeID == 7 || _invoice.BillingTypeValue == "1" || _invoice.BillingTypeLabel == "半包")
                Items = TugBusinessLogic.Module.FinanceLogic.GetBanBaoShowItems();

            else if (_invoice.BillingTypeID == 8 || _invoice.BillingTypeValue == "2" || _invoice.BillingTypeLabel == "计时")
                Items = TugBusinessLogic.Module.FinanceLogic.GetTiaoKuanShowItems();

            //当前账单使用的计费方案的项目
            List<MyBillingItem> customerSchemeItems = null;

            customerSchemeItems = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemeItems(_invoice.BillingTemplateID);

            //当前账单使用的计费方案
            V_BillingTemplate bt = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillScheme(_invoice.BillingTemplateID);

            //客户的计费方案
            //TugDataEntities db = new TugDataEntities();
            //V_Invoice2 vi = db.V_Invoice2.FirstOrDefault(u => u.BillingID == billingId);
            //int length = TugBusinessLogic.Module.Util.toint(vi.Length);
            //int teus = TugBusinessLogic.Module.Util.toint(vi.TEUS);
            //List<V_BillingTemplate> customerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemes((int)custId);
            List<TugDataModel.V_BillingTemplate> customerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomersBillingTemplateByLengthAndTEUS(_invoice.CustomerID, _invoice.CustomerShipLength, _invoice.CustomerShipTEUS);

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, invoice = _invoice, items = Items, customer_scheme = customerSchemeItems, billing_template = bt, customer_billing_schemes = customerBillingSchemes };


            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取账单的状态，在流程中，不在流程中
        /// </summary>
        /// <param name="selectedBillingIDs">行号:账单ID</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult CheckBillingStatus(string selectedBillingIDs, string billingType)
        {
            this.Internationalization();

            Dictionary<int, int> dicNotInFlow = new Dictionary<int, int>();
            Dictionary<int, int> dicInFow = new Dictionary<int, int>();

            TugBusinessLogic.Module.FinanceLogic.GetStatuOfBillings(selectedBillingIDs, billingType, out dicNotInFlow, out dicInFow);

            return Json(new
            {
                code = Resources.Common.SUCCESS_CODE,
                message = Resources.Common.SUCCESS_MESSAGE,
                dic_has_invoice_not_in_flow = dicNotInFlow,
                dic_has_invoice_in_fow = dicInFow,
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 特殊账单


        public ActionResult GetServiceDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    //List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.SearchForInvoice(sidx, sord, searchOption);
                    //List<V_Billing2> orders = TugBusinessLogic.Module.FinanceLogic.SearchDataForSpecialBilling(sidx, sord, searchOption);

                    List<V_OrderService> orders = db.V_OrderService.Select(u => u).Where(u => u.OrderID == -1 && (u.ServiceNatureID == 24 || u.ServiceNatureID == 28
                        || u.ServiceNatureValue == "A0" || u.ServiceNatureValue == "A4"
                        || u.ServiceNatureLabel == "泊码头" || u.ServiceNatureLabel == "离码头"))
                        .OrderByDescending(u => u.ShipName).ThenByDescending(u => u.ServiceWorkDate).ToList();
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderService> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderService>();

                    var jsonData = new { /*page = page,*/ records = totalRecordNum, /*total = totalPageNum*/ rows = orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                    //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    //List<V_Billing2> orders = TugBusinessLogic.Module.FinanceLogic.LoadDataForSpecialBilling(sidx, sord);
                    List<V_OrderService> orders = db.V_OrderService.Where(u => u.OrderID == -1 && (u.ServiceNatureID == 24 || u.ServiceNatureID == 28
                        || u.ServiceNatureValue == "A0" || u.ServiceNatureValue == "A4"
                        || u.ServiceNatureLabel == "泊码头" || u.ServiceNatureLabel == "离码头") && (u.HasBilling == "否") && u.HasBillingInFlow == "否")
                        .Select(u => u).OrderByDescending(u => u.ShipName).ThenByDescending(u => u.ServiceWorkDate).ToList();
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderService> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderService>();

                    var jsonData = new { /*page = page,*/ records = totalRecordNum, /*total = totalPageNum,*/ rows = orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult SearchServiceDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows, int custId,  string startDate, string endDate)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    //List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.SearchForInvoice(sidx, sord, searchOption);
                    //List<V_Billing2> orders = TugBusinessLogic.Module.FinanceLogic.SearchDataForSpecialBilling(sidx, sord, searchOption);

                    List<V_OrderService> orders = db.V_OrderService.Select(u => u).Where(u => (u.ServiceNatureID == 24 || u.ServiceNatureID == 28
                        || u.ServiceNatureValue == "A0" || u.ServiceNatureValue == "A4"
                        || u.ServiceNatureLabel == "泊码头" || u.ServiceNatureLabel == "离码头") && u.UserDefinedCol4 == "1" && (u.HasBilling == "否") && u.HasBillingInFlow == "否" && u.CustomerID == custId
                        && u.ServiceWorkDate.CompareTo(startDate) >= 0 && u.ServiceWorkDate.CompareTo(endDate) <= 0)
                        //.OrderByDescending(u => u.ShipName).ThenByDescending(u => u.ServiceWorkDate).ToList();
                        .OrderBy(u => u.ServiceWorkDate).ThenBy(u => u.ShipName).ThenBy(u => u.ServiceNatureLabel).ToList();
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderService> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderService>();

                    var jsonData = new {/* page = page,*/ records = totalRecordNum, /*total = totalPageNum,*/ rows = orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                    //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    //List<V_Billing2> orders = TugBusinessLogic.Module.FinanceLogic.LoadDataForSpecialBilling(sidx, sord);
                    List<V_OrderService> orders = db.V_OrderService.Select(u => u).Where(u => (u.ServiceNatureID == 24 || u.ServiceNatureID == 28
                        || u.ServiceNatureValue == "A0" || u.ServiceNatureValue == "A4"
                        || u.ServiceNatureLabel == "泊码头" || u.ServiceNatureLabel == "离码头") && u.UserDefinedCol4 == "1" && (u.HasBilling == "否") && u.HasBillingInFlow == "否" && u.CustomerID == custId 
                        && u.ServiceWorkDate.CompareTo(startDate) >= 0 && u.ServiceWorkDate.CompareTo(endDate) <= 0)
                        //.OrderByDescending(u => u.ShipName).ThenByDescending(u => u.ServiceWorkDate).ToList();
                        .OrderBy(u => u.ServiceWorkDate).ThenBy(u => u.ShipName).ThenBy(u => u.ServiceNatureLabel).ToList();
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    //int pageSize = rows;
                    int pageSize = orders.Count;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderService> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderService>();

                    var jsonData = new { /*page = page,*/ records = totalRecordNum, /*total = totalPageNum,*/ rows = orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }
        

        public ActionResult GetSpecialBillingDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    //List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.SearchForInvoice(sidx, sord, searchOption);
                    List<V_Billing3> orders = TugBusinessLogic.Module.FinanceLogic.SearchDataForSpecialBilling(sidx, sord, searchOption);

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_Billing3> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_Billing3>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                    //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<V_Billing3> orders = TugBusinessLogic.Module.FinanceLogic.LoadDataForSpecialBilling(sidx, sord);
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_Billing3> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_Billing3>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }


        public ActionResult AddSpecialInvoice(int custId, double amount, string month, string billingCode, string jsonArrayItems)
        {

            this.Internationalization();

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                   
                    TugDataEntities db = new TugDataEntities();
                    {
                        //0.验证账单编号是否已存在
                        var tmp = db.Billing.FirstOrDefault(u => u.BillingCode == billingCode);
                        if (tmp != null)
                        {
                            var ret2 = new { code = Resources.Common.FAIL_CODE, message = "帳單編號已存在!" };
                            //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                            return Json(ret2, JsonRequestBehavior.AllowGet);
                        }

                        //1.插入账单
                        TugDataModel.Billing aScheduler = new Billing();

                        aScheduler.CustomerID = custId;
                        aScheduler.Amount = amount;
                        aScheduler.Month = month;
                        aScheduler.InvoiceType = "特殊账单";
                        aScheduler.BillingCode = billingCode;

                        aScheduler.TimesNo = 0;
                        aScheduler.Status = "创建";
                        aScheduler.Phase = 0;

                        aScheduler.OwnerID = -1;
                        aScheduler.CreateDate = aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        aScheduler.UserID = Session.GetDataFromSession<int>("userid");


                        aScheduler.UserDefinedCol1 = "";
                        aScheduler.UserDefinedCol2 = "";
                        aScheduler.UserDefinedCol3 = "";
                        aScheduler.UserDefinedCol4 = "";

                        aScheduler.UserDefinedCol9 = "";
                        aScheduler.UserDefinedCol10 = "";

                        aScheduler = db.Billing.Add(aScheduler);
                        db.SaveChanges();



                        //3.插入账单的收费项目
                        List<MySpecialBillingItem> listInVoiceItems = new List<MySpecialBillingItem>();
                        listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<MySpecialBillingItem>(jsonArrayItems);
                        if (listInVoiceItems != null)
                        {
                            foreach (MySpecialBillingItem item in listInVoiceItems)
                            {
                                SpecialBillingItem bi = new SpecialBillingItem();
                                bi.SpecialBillingID = aScheduler.IDX;
                                bi.OrderServiceID = item.OrderServiceID;
                                bi.CustomerShipName = item.CustomerShipName;
                                bi.FeulUnitPrice = item.FeulUnitPrice;
                                bi.ServiceDate = item.ServiceDate;
                                bi.ServiceNatureID = item.ServiceNatureID;
                                bi.ServiceNatureValue = item.ServiceNatureValue;
                                bi.ServiceNature = item.ServiceNature;
                                bi.ServiceUnitPrice = item.ServiceUnitPrice;
                                bi.TugNumber = item.TugNumber;

                                bi = db.SpecialBillingItem.Add(bi);
                                db.SaveChanges();

                                //更新订单服务的账单标记
                                {
                                    OrderService os = db.OrderService.First(u => u.IDX == item.OrderServiceID);
                                    os.HasBilling = "是";
                                    os.HasBillingInFlow = "否";
                                    os.BillingType = 1;
                                    db.Entry(os).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();

                                    //更新服务对应的订单的账单标记;因为现在一个订单对应一个服务
                                    OrderInfor ordInfor = db.OrderInfor.FirstOrDefault(u => u.IDX == os.OrderID);
                                    if (ordInfor != null)
                                    {
                                        ordInfor.HasInvoice = "是";
                                        db.Entry(ordInfor).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }

                                
                            }
                        }

                        //4.插入账单的汇总项目
                        TugBusinessLogic.Module.FinanceLogic.UpdateSpecialBillingSummarizeItems(aScheduler.IDX, (int)aScheduler.UserID);
                        //List<InVoiceSummaryItem> listInVoiceSummaryItems = new List<InVoiceSummaryItem>();
                        //listInVoiceSummaryItems = TugBusinessLogic.Utils.JSONStringToList<InVoiceSummaryItem>(jsonArraySummaryItems);
                        //if (listInVoiceSummaryItems != null)
                        //{
                        //    foreach (InVoiceSummaryItem item in listInVoiceSummaryItems)
                        //    {
                        //        AmountSum amtSum = new AmountSum();
                        //        amtSum.CustomerID = custId;
                        //        amtSum.CustomerShipID = custShipId;
                        //        amtSum.BillingID = aScheduler.IDX;
                        //        amtSum.BillingDateTime = TugBusinessLogic.Utils.CNDateTimeToDateTime(aScheduler.CreateDate);
                        //        amtSum.SchedulerID = item.SchedulerID;
                        //        amtSum.Amount = item.Amount;
                        //        amtSum.Currency = item.Currency;
                        //        amtSum.Hours = item.Hours;
                        //        amtSum.Year = DateTime.Now.Year.ToString();
                        //        amtSum.Month = aScheduler.Month;
                        //        amtSum.OwnerID = -1;
                        //        amtSum.CreateDate = amtSum.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //        amtSum.UserID = Session.GetDataFromSession<int>("userid");

                        //        amtSum = db.AmountSum.Add(amtSum);
                        //        db.SaveChanges();
                        //    }
                        //}

                        //5.更新订单的字段 V_OrderInfor_HasInvoice	是否已有帳單	
                        //{
                        //    List<OrderInfor> odList = db.OrderInfor.Where(u => iOrderIDs.Contains(u.IDX)).ToList();
                        //    //throw new Exception();
                        //    if (odList != null)
                        //    {
                        //        foreach (OrderInfor od in odList)
                        //        {
                        //            od.HasInvoice = "是";
                        //            od.HasInFlow = "否";
                        //            db.Entry(od).State = System.Data.Entity.EntityState.Modified;
                        //            db.SaveChanges();
                        //        }
                        //    }
                        //}

                        trans.Complete();

                        var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, billing_id = aScheduler.IDX };
                        //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                        return Json(ret, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE, billing_id = -1 };
                    //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                    return Json(ret, JsonRequestBehavior.AllowGet);
                }
            }
        }


        public ActionResult DeleteSpecialInvoice(string billIds)
        {
            this.Internationalization();

            try
            {
                //Expression condition = Expression.Equal(Expression.Constant(1, typeof(int)), Expression.Constant(2, typeof(int)));
                //ParameterExpression parameter = Expression.Parameter(typeof(Billing));

                if (billIds != "")
                {
                    List<string> listBillingIds = billIds.Split(',').ToList();

                    TugDataEntities db = new TugDataEntities();

                    foreach (string billingId in listBillingIds)
                    {
                        int bid = TugBusinessLogic.Module.Util.toint(billingId);

                        Billing b = db.Billing.FirstOrDefault(u => u.IDX == bid);
                        if (b != null)
                        {
                            //TugBusinessLogic.Module.FinanceLogic.RejectInvoice2(bid);
                            var lstOrderServices = db.SpecialBillingItem.Where(u => u.SpecialBillingID == b.IDX).ToList();
                            if (lstOrderServices != null)
                            {
                                foreach (var item in lstOrderServices)
                                {
                                    //更新订单服务的账单标记
                                    {
                                        OrderService os = db.OrderService.First(u => u.IDX == item.OrderServiceID);
                                        os.HasBilling = "否";
                                        os.HasBillingInFlow = "否";
                                        os.BillingType = 0;
                                        db.Entry(os).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();


                                        OrderInfor ordinfor = db.OrderInfor.FirstOrDefault(u => u.IDX == os.OrderID);
                                        if (ordinfor != null)
                                        {
                                            ordinfor.HasInvoice = "否";
                                            db.Entry(ordinfor).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }

                            db.Billing.Remove(b);
                            db.SaveChanges();

                        }
                    }

                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
            return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
        }



        public ActionResult EditSpecialInvoice(int billingId, double amount, string month, string billingCode, string jsonArrayItems)
        {
            this.Internationalization();
            

                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        Billing oldBilling = db.Billing.FirstOrDefault(u => u.IDX == billingId);

                        Billing tmp = db.Billing.FirstOrDefault(u => u.BillingCode == billingCode);
                        if (tmp != null)
                        {
                            if (tmp.IDX != oldBilling.IDX)
                            {
                                return Json(new { code = Resources.Common.FAIL_CODE, message = "帳單編號已存在!" }, JsonRequestBehavior.AllowGet);
                            }
                        }


                        if (oldBilling != null)
                        {
                            oldBilling.BillingCode = billingCode;
                            oldBilling.Amount = amount;
                            oldBilling.Month = month;
                            oldBilling.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            db.Entry(oldBilling).State = System.Data.Entity.EntityState.Modified;
                            int ret = db.SaveChanges();

                            if (ret > 0)
                            {
                                #region 订单收费项
                                //1.订单收费项
                                List<SpecialBillingItem> invoiceItems = db.SpecialBillingItem.Where(u => u.SpecialBillingID == billingId).ToList();
                                if (invoiceItems != null)
                                {
                                    db.SpecialBillingItem.RemoveRange(invoiceItems);
                                    ret = db.SaveChanges();
                                    if (ret > 0)
                                    {
                                        List<MySpecialBillingItem> listInVoiceItems = new List<MySpecialBillingItem>();
                                        listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<MySpecialBillingItem>(jsonArrayItems);
                                        if (listInVoiceItems != null)
                                        {
                                            foreach (MySpecialBillingItem item in listInVoiceItems)
                                            {
                                                SpecialBillingItem bi = new SpecialBillingItem();
                                                bi.SpecialBillingID = billingId;
                                                bi.OrderServiceID = item.OrderServiceID;
                                                bi.ServiceNatureID = item.ServiceNatureID;
                                                bi.ServiceNatureValue = item.ServiceNatureValue;
                                                bi.CustomerShipName = item.CustomerShipName;
                                                bi.FeulUnitPrice = item.FeulUnitPrice;
                                                bi.ServiceDate = item.ServiceDate;
                                                bi.ServiceNature = item.ServiceNature;
                                                bi.ServiceUnitPrice = item.ServiceUnitPrice;
                                                bi.TugNumber = item.TugNumber;


                                                bi = db.SpecialBillingItem.Add(bi);
                                                db.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                    }

                                }
                                else
                                {
                                    List<MySpecialBillingItem> listInVoiceItems = new List<MySpecialBillingItem>();
                                    listInVoiceItems = TugBusinessLogic.Utils.JSONStringToList<MySpecialBillingItem>(jsonArrayItems);
                                    if (listInVoiceItems != null)
                                    {
                                        foreach (MySpecialBillingItem item in listInVoiceItems)
                                        {
                                            SpecialBillingItem bi = new SpecialBillingItem();

                                            bi.SpecialBillingID = billingId;
                                            bi.OrderServiceID = item.OrderServiceID;
                                            bi.ServiceNatureID = item.ServiceNatureID;
                                            bi.ServiceNatureValue = item.ServiceNatureValue;
                                            bi.CustomerShipName = item.CustomerShipName;
                                            bi.FeulUnitPrice = item.FeulUnitPrice;
                                            bi.ServiceDate = item.ServiceDate;
                                            bi.ServiceNature = item.ServiceNature;
                                            bi.ServiceUnitPrice = item.ServiceUnitPrice;
                                            bi.TugNumber = item.TugNumber;

                                            bi = db.SpecialBillingItem.Add(bi);
                                            db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                #endregion

                                //2.插入账单的汇总项目
                                TugBusinessLogic.Module.FinanceLogic.UpdateSpecialBillingSummarizeItems(billingId, Session.GetDataFromSession<int>("userid"));

                                #region 更新回扣单编号
                                System.Linq.Expressions.Expression<Func<Credit, bool>> expCredit = u => u.BillingID == billingId;
                                List<Credit> tmpCredit = db.Credit.Where(expCredit).Select(u => u).ToList<Credit>();
                                //Credit tmpCredit = db.Credit.Where(expCredit).FirstOrDefault();
                                if (tmpCredit.Count != 0)
                                {
                                    foreach (var item in tmpCredit)
                                    {
                                        item.CreditCode = "C" + billingCode.Substring(1, billingCode.Length - 1);
                                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                
                                return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            
                            return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE }, JsonRequestBehavior.AllowGet);
            
        }


        /// <summary>
        /// 获取账单
        /// </summary>
        /// <param name="lan"></param>
        /// <param name="custId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult GetSpecialInvoice(string lan, int billingId)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            //return RedirectToAction("Login", "Home");

            MySpecialInvoice _invoice = TugBusinessLogic.Module.FinanceLogic.GenerateSpecialInvoice((int)billingId);

            
            if (_invoice.SpecialBillingItems.Count >0)
            {
                _invoice.FeulUnitPrice = (double)_invoice.SpecialBillingItems[0].FeulUnitPrice;
                _invoice.ServiceUnitPrice = (double)_invoice.SpecialBillingItems[0].ServiceUnitPrice;
            }

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, invoice = _invoice};


            return Json(ret, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetSpecialBillingCreditData(bool _search, string sidx, string sord, int page, int rows, int billingId)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                //int idx = Util.toint(Request.Form["IDX"].Trim());
                {
                    List<MyCredit> orders = new List<MyCredit>();

                    switch (sidx)
                    {
                        case "":
                            {
                                orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.IDX)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                            }
                            break;

                        case "CreditCode":
                            {
                                if (sord == "asc")
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditCode)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditCode)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;

                        case "CreditContent":
                            {
                                if (sord == "asc")
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditContent)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditContent)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;

                        case "CreditAmount":
                            {
                                if (sord == "asc")
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditAmount)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditAmount)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "Remark":
                            {
                                if (sord == "asc")
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.Remark)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.Remark)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "CreateDate":
                            {
                                if (sord == "asc")
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreateDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreateDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "LastUpDate":
                            {
                                if (sord == "asc")
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.LastUpDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else
                                {
                                    orders = db.V_SpecialBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.LastUpDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.IDX,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.Remark,
                                         OwnerID = u.OwnerID,
                                         CreateDate = u.CreateDate,
                                         UserID = u.UserID,
                                         LastUpDate = u.LastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;

                        default:
                            break;

                    }

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<MyCredit> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<MyCredit>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult GetSpecialInvoiceBillingCode()
        {
            string billingCode = TugBusinessLogic.Utils.AutoGenerateBillCode();

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, billingCode = billingCode };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CheckSchedulerInputTime(string selectedOrderServices)
        {
            this.Internationalization();

            //行号：OrderServiceId
            Dictionary<int, int> dicHasUnCompleteInputTime = new Dictionary<int, int>();
            Dictionary<int, int> dicHasNotSchedulerTug = new Dictionary<int, int>();

            string ret = "";

            TugDataEntities db = new TugDataEntities();
            List<string> list = selectedOrderServices.Split(',').ToList();
            if (list != null)
            {
                foreach (string item in list)
                {
                    int rowId = Convert.ToInt32(item.Split(':')[0]);
                    int orderServiceId = Convert.ToInt32(item.Split(':')[1]);

                    var orderServiceSchedulers = db.V_OrderScheduler.Where(u => u.OrderServiceID == orderServiceId).ToList();
                    if (orderServiceSchedulers != null)
                    {
                        if (orderServiceSchedulers.Count == 0) {
                            dicHasNotSchedulerTug.Add(rowId, orderServiceId);
                        }
                        else
                        {
                            bool flag = false;

                            foreach (var item2 in orderServiceSchedulers)
                            {
                                if (null == item2.DepartBaseTime || "" == item2.DepartBaseTime
                                    || null == item2.ArrivalBaseTime || "" == item2.ArrivalBaseTime)
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            if (flag == true)
                            {
                                dicHasUnCompleteInputTime.Add(rowId, orderServiceId);

                            }
                        }
                    }
                    
                }
            }
            return Json(new
            {
                code = Resources.Common.SUCCESS_CODE,
                message = Resources.Common.SUCCESS_MESSAGE,
                dic_has_uncomplete_input_time = dicHasUnCompleteInputTime,
                dic_has_not_scheduler_tug = dicHasNotSchedulerTug,

                ret
            }, JsonRequestBehavior.AllowGet);
  
        }

        #endregion



        #region 其他賬單

        public ActionResult GetDiscountBillingDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    //List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.SearchForInvoice(sidx, sord, searchOption);
                    List<V_Billing4> orders = TugBusinessLogic.Module.FinanceLogic.SearchDataForDiscountBilling(sidx, sord, searchOption);

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_Billing4> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_Billing4>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                    //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<V_Billing4> orders = TugBusinessLogic.Module.FinanceLogic.LoadDataForDiscountBilling(sidx, sord);
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_Billing4> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_Billing4>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }


        public ActionResult GetDiscountBillingCode()
        {
            string billingCode = TugBusinessLogic.Utils.AutoGenerateDiscountBillCode();

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, billingCode = billingCode };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 删除其他賬單
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult DeleteDiscountBills(string billIds)
        {
            this.Internationalization();

            try
            {
                //Expression condition = Expression.Equal(Expression.Constant(1, typeof(int)), Expression.Constant(2, typeof(int)));
                //ParameterExpression parameter = Expression.Parameter(typeof(Billing));

                if (billIds != "")
                {
                    List<string> listBillingIds = billIds.Split(',').ToList();

                    TugDataEntities db = new TugDataEntities();

                    foreach (string billingId in listBillingIds)
                    {
                        int bid = TugBusinessLogic.Module.Util.toint(billingId);

                        Billing b = db.Billing.FirstOrDefault(u => u.IDX == bid);
                        if (b != null)
                        {
                            db.Billing.Remove(b);
                            db.SaveChanges();

                        }
                    }

                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
            return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
        }



        /// <summary>
        /// 新增其他賬單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult AddDiscountBill(int customerId, string title, string content, double money, string month, string billingCode)
        {

            this.Internationalization();
            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    //0.验证账单编号是否已存在
                    var tmp = db.Billing.FirstOrDefault(u => u.BillingCode == billingCode);
                    if (tmp != null)
                    {
                        var ret2 = new { code = Resources.Common.FAIL_CODE, message = "其他賬單編號已存在!" };
                        //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                        return Json(ret2, JsonRequestBehavior.AllowGet);
                    }


                    TugDataModel.Billing credit = new TugDataModel.Billing();

                    credit.CustomerID = customerId;
                    //credit.CreditCode = "C" +  billingCode.Substring(1, billingCode.Length - 1 );
                    credit.UserDefinedCol1 = title;
                    credit.UserDefinedCol2 = content;
                    credit.UserDefinedCol5 = money;
                    credit.Amount = money;
                    credit.Month = month;
                    credit.InvoiceType = "其他账单";
                    credit.CreateDate = credit.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    credit.OwnerID = -1;
                    credit.UserID = credit.UserID = Session.GetDataFromSession<int>("userid");
                    credit.BillingCode = billingCode;

                    credit.TimesNo = 0;
                    credit.Status = "创建";
                    credit.Phase = 0;

                    credit = db.Billing.Add(credit);
                    db.SaveChanges();

                }

                var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public ActionResult EditDiscountBill(int billingId, int customerId, string title, string content, double money, string month, string billingCode)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();


                Billing aOrder = db.Billing.Where(u => u.IDX == billingId).FirstOrDefault();

                if (aOrder == null)
                {
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                }
                else
                {

                    Billing tmp = db.Billing.FirstOrDefault(u => u.BillingCode == billingCode);
                    if (tmp != null)
                    {
                        if (tmp.IDX != aOrder.IDX)
                        {
                            return Json(new { code = Resources.Common.FAIL_CODE, message = "其他賬單編號已存在!" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    aOrder.CustomerID = customerId;
                    aOrder.UserDefinedCol1 = title;
                    aOrder.UserDefinedCol2 = content;
                    aOrder.UserDefinedCol5 = money;
                    aOrder.Amount = money;
                    aOrder.Month = month;
                    aOrder.BillingCode = billingCode;

                    aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    db.Entry(aOrder).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                }
            }
            catch (Exception exp)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }


            return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
        }


        [Authorize]
        public ActionResult GetDiscountBill(int billingId)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();


                V_Billing4 aOrder = db.V_Billing4.Where(u => u.IDX == billingId).FirstOrDefault();

                if (aOrder == null)
                {
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                }
                else
                {
                    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, discout_bill = aOrder });
                }
            }
            catch (Exception exp)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }

        }
        #endregion
    }

}