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

        public string GetPersons()
        {
            string[] labels = null;
            int i = 0;
            if (labels == null)
            {
                TugDataEntities db = new TugDataEntities();
                List<UserInfor> list = db.UserInfor.Where(u => u.IsGuest == "false" && u.UserName != "admin").OrderBy(u => u.Name1).ToList<UserInfor>();
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
        [Authorize]
        public ActionResult GetInvoiceData(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.SearchForInvoice(sidx, sord, searchOption);

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderBilling> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderBilling>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                    //return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<V_OrderBilling> orders = TugBusinessLogic.Module.FinanceLogic.LoadDataForInvoice(sidx, sord);
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_OrderBilling> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_OrderBilling>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }


        
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

            TugDataModel.MyInvoice _invoice = TugBusinessLogic.Module.FinanceLogic.NewInvoice((int)orderId, orderDate, customerBillingScheme,
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
                        aScheduler.BillingCode = TugBusinessLogic.Utils.AutoGenerateBillCode();
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

            TugBusinessLogic.Module.OrderLogic.GetStatusOfOrderInvoice(selectedOrderIDs, out dicHasNoInvoice, out dicHasInvoiceNotInFlow, out dicHasInvoiceInFow, out dicHasInvoiceNotInFlowBills);

            return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE,
                              dic_has_no_invoice = dicHasNoInvoice,
                              dic_has_invoice_not_in_flow = dicHasInvoiceNotInFlow,
                              dic_has_invoice_in_fow = dicHasInvoiceInFow,
                              dic_has_invoice_not_in_flow_bills = dicHasInvoiceNotInFlowBills
            }, JsonRequestBehavior.AllowGet);
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
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
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
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else
                                {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditCode)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
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
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditContent)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
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
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditAmount)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "Remark":
                            {
                                if (sord == "asc") {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditRemark)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditRemark)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "CreateDate":
                            {
                                if (sord == "asc") {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditCreateDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditCreateDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                            }
                            break;
                        case "LastUpDate":
                            {
                                if (sord == "asc") {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderBy(u => u.CreditLastUpDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
                                     }).ToList<MyCredit>();
                                }
                                else {
                                    orders = db.V_OrderBillingCredit.Where(u => u.BillingID == billingId)
                                     .OrderByDescending(u => u.CreditLastUpDate)
                                     .Select(u => new MyCredit
                                     {
                                         IDX = (int)u.CreditID,
                                         OrderID = (int)u.OrderID,
                                         BillingID = u.BillingID,
                                         CreditCode = u.CreditCode,
                                         CreditContent = u.CreditContent,
                                         CreditAmount = u.CreditAmount,
                                         Remark = u.CreditRemark,
                                         OwnerID = u.CreditOwnerID,
                                         CreateDate = u.CreditCreateDate,
                                         UserID = u.CreditUserID,
                                         LastUpDate = u.CreditLastUpDate
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


                        aOrder.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        aOrder.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        aOrder.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        aOrder.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            aOrder.UserDefinedCol6 = TugBusinessLogic.Module.Util.toint(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            aOrder.UserDefinedCol7 = TugBusinessLogic.Module.Util.toint(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            aOrder.UserDefinedCol8 = TugBusinessLogic.Module.Util.toint(Request.Form["UserDefinedCol8"].Trim());

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
                db.Credit.RemoveRange(list);
                if(db.SaveChanges() > 0)
                {
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

        #endregion
    }

}