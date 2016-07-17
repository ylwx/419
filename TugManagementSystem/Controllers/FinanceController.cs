using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TugDataModel;
using TugBusinessLogic;

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

            return View();
        }

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

        [Authorize]
        [HttpGet]
        public ActionResult GetInvoice(string lan, int? custId, int? orderId)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            //return RedirectToAction("Login", "Home");

            MyInvoice _invoice = TugBusinessLogic.Module.FinanceLogic.GenerateInvoice((int)orderId);

            List<TugDataModel.MyCustomField> Items = new List<MyCustomField>();
            if (_invoice.BillingTypeID == 7 || _invoice.BillingTypeValue == "1" || _invoice.BillingTypeLabel == "全包加特别条款")
                Items = TugBusinessLogic.Module.FinanceLogic.GetBanBaoShowItems();

            else if (_invoice.BillingTypeID == 8 || _invoice.BillingTypeValue == "2" || _invoice.BillingTypeLabel == "条款")
                Items = TugBusinessLogic.Module.FinanceLogic.GetTiaoKuanShowItems();

            //当前账单使用的计费方案的项目
            List<MyBillingItem> customerSchemeItems = null;

            customerSchemeItems = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemeItems(_invoice.BillingTemplateID);

            //当前账单使用的计费方案
            V_BillingTemplate bt = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillScheme(_invoice.BillingTemplateID);

            //客户的计费方案
            List<V_BillingTemplate> customerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemes((int)custId);

            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, invoice = _invoice, items = Items, customer_scheme = customerSchemeItems, billing_template = bt, customer_billing_schemes = customerBillingSchemes };


            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult NewInvoice(string lan, int? orderId, string customerBillingScheme,
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
            if (billingTypeId == 7 || billingTypeValue == "1" || billingTypeLabel == "全包加特别条款")
                Items = TugBusinessLogic.Module.FinanceLogic.GetBanBaoShowItems();

            else if (billingTypeId == 8 || billingTypeValue == "2" || billingTypeLabel == "条款")
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
        public ActionResult InitFilter(string lan, int? custId, int? orderId)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            List<TugDataModel.V_BillingTemplate> CustomerBillingSchemes = TugBusinessLogic.Module.FinanceLogic.GetCustomerBillSchemes((int)custId);
            List<TugDataModel.CustomField> BillingTemplateTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.BillingTemplateType");
            List<TugDataModel.CustomField> TimeTypes = TugBusinessLogic.Utils.GetCustomField2("BillingTemplate.TimeTypeID");


            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE, customer_billing_schemes = CustomerBillingSchemes, time_types = TimeTypes, billing_template_types = BillingTemplateTypes };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DeleteInvoice()
        {
            this.Internationalization();

            try
            {
                Expression condition = Expression.Equal(Expression.Constant(1, typeof(int)), Expression.Constant(1, typeof(int)));
                ParameterExpression parameter = Expression.Parameter(typeof(Billing));

                string strBillIds = Request.Form["billIds"];

                if (strBillIds != "")
                {
                    List<string> listBillIds = strBillIds.Split(',').ToList();

                    TugDataEntities db = new TugDataEntities();
                    foreach (string billId in listBillIds)
                    {
                        Expression cdt = Expression.Equal(Expression.PropertyOrField(parameter, "IDX"), Expression.Constant(Convert.ToInt32(billId)));
                        condition = Expression.OrElse(condition, cdt);

                        //int idx = Convert.ToInt32(billId);
                        //Billing aOrder = db.Billing.FirstOrDefault(u => u.IDX == idx);
                        //if (aOrder != null)
                        //{
                        //    db.Billing.Remove(aOrder);
                        //    db.SaveChanges();
                        //    return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                        //}
                        //else
                        //{
                        //    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                        //}
                    }

                    var lamda = Expression.Lambda<Func<Billing, bool>>(condition, parameter);
                    List<Billing> orders = db.Billing.Where(lamda).Select(u => u).ToList<Billing>();
                    if (orders != null)
                    {
                        db.Billing.RemoveRange(orders);
                        db.SaveChanges();
                        return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                    }
                    else
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
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
        public ActionResult AddBill(int orderId, int billingTemplateId, int billingTypeId, int timeTypeId, double discount, double amount, string jsonArrayItems)
        {

            this.Internationalization();

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
                    aScheduler.Amount = amount;


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
                            bi.UserID = Session.GetDataFromSession<int>("userid"); ;
                            bi.CreateDate = bi.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            bi.UserDefinedCol1 = "";
                            bi.UserDefinedCol2 = "";
                            bi.UserDefinedCol3 = "";
                            bi.UserDefinedCol4 = "";

                            //if (Request.Form["UserDefinedCol5"].Trim() != "")
                            //    aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                            //if (Request.Form["UserDefinedCol6"].Trim() != "")
                            //    aScheduler.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                            //if (Request.Form["UserDefinedCol7"].Trim() != "")
                            //    aScheduler.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                            //if (Request.Form["UserDefinedCol8"].Trim() != "")
                            //    aScheduler.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                            bi.UserDefinedCol9 = "";
                            bi.UserDefinedCol10 = "";

                            bi = db.BillingItem.Add(bi);
                            db.SaveChanges();
                        }
                    }

                    var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                    //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                    return Json(ret, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
        }

    }

}