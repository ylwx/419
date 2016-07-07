using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class CustomerController : BaseController
    {
        private static int _DefaultPageSie = 7;

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
                        TugDataModel.Customer cstmer = new Customer();

                        cstmer.Code = Request.Form["Code"];
                        cstmer.CnName = Request.Form["CnName"];
                        cstmer.EnName = Request.Form["EnName"];
                        cstmer.SimpleName = Request.Form["SimpleName"];
                        cstmer.TypeID = Convert.ToInt32(Request.Form["TypeID"]);
                        cstmer.ContactPerson = Request.Form["ContactPerson"];
                        cstmer.Telephone = Request.Form["Telephone"];
                        cstmer.Fax = Request.Form["Fax"];
                        cstmer.Email = Request.Form["Email"];
                        cstmer.Address = Request.Form["Address"];
                        cstmer.MailCode = Request.Form["MailCode"];
                        cstmer.Remark = Request.Form["Remark"];
                        cstmer.OwnerID = -1;
                        cstmer.CreateDate = cstmer.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");;//.ToString("yyyy-MM-dd");
                        cstmer.UserID = -1;
                        cstmer.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        cstmer.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        cstmer.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        cstmer.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            cstmer.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            cstmer.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            cstmer.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            cstmer.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        cstmer.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        cstmer.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        cstmer = db.Customer.Add(cstmer);
                        db.SaveChanges();

                        var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                        Response.Write(@Resources.Common.SUCCESS_MESSAGE);
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

                    int idx = Convert.ToInt32(Request.Form["IDX"]);
                    Customer cstmer = db.Customer.Where(u => u.IDX == idx).FirstOrDefault();

                    if (cstmer == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        cstmer.Code = Request.Form["Code"];
                        cstmer.CnName = Request.Form["CnName"];
                        cstmer.EnName = Request.Form["EnName"];
                        cstmer.SimpleName = Request.Form["SimpleName"];
                        cstmer.TypeID = Convert.ToInt32(Request.Form["TypeID"]);
                        cstmer.ContactPerson = Request.Form["ContactPerson"];
                        cstmer.Telephone = Request.Form["Telephone"];
                        cstmer.Fax = Request.Form["Fax"];
                        cstmer.Email = Request.Form["Email"];
                        cstmer.Address = Request.Form["Address"];
                        cstmer.MailCode = Request.Form["MailCode"];
                        cstmer.Remark = Request.Form["Remark"];
                        cstmer.OwnerID = -1;
                        cstmer.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");;
                        cstmer.UserID = -1;
                        cstmer.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        cstmer.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        cstmer.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        cstmer.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            cstmer.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            cstmer.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            cstmer.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            cstmer.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        cstmer.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        cstmer.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        db.Entry(cstmer).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult CustomerManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            return View();
        }

        public ActionResult Delete()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                Customer cstmer = db.Customer.FirstOrDefault(u => u.IDX == idx);
                if (cstmer != null)
                {
                    db.Customer.Remove(cstmer);
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

        public ActionResult GetDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            //string s = Request.QueryString[6];

            try
            {
                TugDataEntities db = new TugDataEntities();

                //db.Configuration.ProxyCreationEnabled = false;
                List<Customer> customers = db.Customer.Select(u => u).OrderByDescending(u => u.IDX).ToList<Customer>();
                int totalRecordNum = customers.Count;
                if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<Customer> page_customers = customers.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<Customer>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = customers };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        //
        // GET: /Customer/
        public ActionResult Index()
        {
            return View();
        }



        #region 计费方案 Written By lg

        public ActionResult BillingScheme(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            int totalRecordNum, totalPageNum;
            List<Customer> list = GetCustomers(1, _DefaultPageSie, out totalRecordNum, out totalPageNum);
            ViewBag.TotalPageNum = totalPageNum;
            ViewBag.CurPage = 1;

            ViewBag.BillTemplateTypes = GetBillTemplateTypes();
            ViewBag.BillTemplateTimeTypes = GetBillTemplateTimeTypes();
            ViewBag.BillTemplatePayItems = GetBillTemplatePayItems();
            ViewBag.BillTemplatePayItemPosition = GetBillTemplatePayItemPosition();
            

            return View(list);
        }

        [HttpGet]
        public ActionResult GetCustomers(int curPage, string queryName = "")
        {
            ViewBag.Language = this.Internationalization();

            int totalRecordNum, totalPageNum;
            List<Customer> list = GetCustomers(curPage, _DefaultPageSie, out totalRecordNum, out totalPageNum, queryName);
            ViewBag.TotalPageNum = totalPageNum;
            ViewBag.CurPage = curPage;
            ViewBag.QueryName = queryName;

            return View("BillingScheme", list);
        }


        public List<Customer> GetCustomers(int curPage, int pageSize, out int totalRecordNum, out int totalPageNum, string queryName = "")
        {
            try
            {
                TugDataEntities db = new TugDataEntities();

                List<Customer> customers = null;
                if (queryName == "")
                {
                    customers = db.Customer.Select(u => u).OrderByDescending(u => u.IDX).ToList<Customer>();
                }
                else
                {
                    customers = db.Customer.Where(u => u.CnName.Contains(queryName) 
                        /*|| u.EnName.Contains(queryName) || u.SimpleName.Contains(queryName)*/)
                        .Select(u => u).OrderByDescending(u => u.IDX).ToList<Customer>();
                }

                totalRecordNum = customers.Count;
                //if (totalRecordNum % pageSize == 0) page -= 1;
                //int pageSize = rows;
                totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                List<Customer> page_customers = customers.Skip((curPage - 1) * pageSize).Take(pageSize).ToList<Customer>();
                return page_customers;
            }
            catch (Exception)
            {
                totalRecordNum = totalPageNum = 0;
                return null;
            }
        }



        public ActionResult GetCustomerBillSchemes(bool _search, string sidx, string sord, int page, int rows, int custId)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                if (_search == true)
                {
                    string searchOption = Request.QueryString["filters"];
                    //List<V_OrderInfor> orders = db.V_OrderInfor.Where(u => u.IDX == -1).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();
                    List<V_BillingTemplate> orders = TugBusinessLogic.Module.CustomerLogic.SearchForCustomerBillingTemplate(sidx, sord, searchOption, custId);

                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_BillingTemplate> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_BillingTemplate>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //List<V_BillingTemplate> orders = db.V_BillingTemplate.Where(u => u.CustomerID == custId).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_BillingTemplate>();
                    List<V_BillingTemplate> orders = TugBusinessLogic.Module.CustomerLogic.LoadDataForCustomerBillingTemplate(sidx, sord, custId);
                    int totalRecordNum = orders.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_BillingTemplate> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_BillingTemplate>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult AddEditCustomerBillScheme(int custId)
        {
            //this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    //重名了
                    if (true == CheckBillingTemplateName(custId, Request.Form["BillingTemplateName"].Trim(), -1))
                    {
                        return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE });
                    }
                    else
                    {
                        TugDataEntities db = new TugDataEntities();
                        {
                            TugDataModel.BillingTemplate cstmer = new BillingTemplate();

                            cstmer.BillingTemplateCode = Request.Form["BillingTemplateCode"].Trim();
                            cstmer.BillingTemplateName = Request.Form["BillingTemplateName"].Trim();
                            cstmer.BillingTemplateTypeID = Convert.ToInt32(Request.Form["BillingTemplateTypeID"].Trim());

                            cstmer.CreateDate = cstmer.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            cstmer.CustomerID = custId; // Convert.ToInt32(customerId);
                            cstmer.TemplateCreditContent = Request.Form["TemplateCreditContent"].Trim();
                            cstmer.TimeTypeID = Convert.ToInt32(Request.Form["TimeTypeID"].Trim());

                            cstmer.Remark = Request.Form["Remark"].Trim();
                            cstmer.OwnerID = -1;
                            cstmer.UserID = -1;
                            cstmer.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                            cstmer.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                            cstmer.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                            cstmer.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                            if (Request.Form["UserDefinedCol5"].Trim() != "")
                                cstmer.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                            if (Request.Form["UserDefinedCol6"].Trim() != "")
                                cstmer.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                            if (Request.Form["UserDefinedCol7"].Trim() != "")
                                cstmer.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                            if (Request.Form["UserDefinedCol8"].Trim() != "")
                                cstmer.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                            cstmer.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                            cstmer.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                            cstmer = db.BillingTemplate.Add(cstmer);
                            db.SaveChanges();

                            var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                            //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                            return Json(ret);
                        }
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

                    int idx = Convert.ToInt32(Request.Form["IDX"]);
                    BillingTemplate cstmer = db.BillingTemplate.Where(u => u.IDX == idx).FirstOrDefault();

                    if (cstmer == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    //重名了
                    if (true == CheckBillingTemplateName(Convert.ToInt32(Request.Form["CustomerID"].Trim()), Request.Form["BillingTemplateName"].Trim(), idx))
                    {
                        return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE });
                    }
                    else
                    {
                        cstmer.BillingTemplateCode = Request.Form["BillingTemplateCode"].Trim();
                        cstmer.BillingTemplateName = Request.Form["BillingTemplateName"].Trim();
                        cstmer.BillingTemplateTypeID = Convert.ToInt32(Request.Form["BillingTemplateTypeID"].Trim());

                        cstmer.CustomerID = Convert.ToInt32(Request.Form["CustomerID"].Trim());
                        cstmer.TemplateCreditContent = Request.Form["TemplateCreditContent"].Trim();
                        cstmer.TimeTypeID = Convert.ToInt32(Request.Form["TimeTypeID"].Trim());

                        cstmer.Remark = Request.Form["Remark"].Trim();
                        cstmer.OwnerID = -1;
                        cstmer.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        cstmer.UserID = -1;
                        cstmer.UserDefinedCol1 = Request.Form["UserDefinedCol1"].Trim();
                        cstmer.UserDefinedCol2 = Request.Form["UserDefinedCol2"].Trim();
                        cstmer.UserDefinedCol3 = Request.Form["UserDefinedCol3"].Trim();
                        cstmer.UserDefinedCol4 = Request.Form["UserDefinedCol4"].Trim();

                        if (Request.Form["UserDefinedCol5"].Trim() != "")
                            cstmer.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"].Trim());

                        if (Request.Form["UserDefinedCol6"].Trim() != "")
                            cstmer.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"].Trim());

                        if (Request.Form["UserDefinedCol7"].Trim() != "")
                            cstmer.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"].Trim());

                        if (Request.Form["UserDefinedCol8"].Trim() != "")
                            cstmer.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"].Trim());

                        cstmer.UserDefinedCol9 = Request.Form["UserDefinedCol9"].Trim();
                        cstmer.UserDefinedCol10 = Request.Form["UserDefinedCol10"].Trim();

                        db.Entry(cstmer).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return Json(new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE });
                    }
                }
                catch (Exception)
                {
                    return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
                }
            }

            #endregion Edit

            return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
        }

        public ActionResult AddCustomerBillScheme(int custId, int billingTemplateTypeId, string billingTemplateCode, string billingTemplateName, int timeTypeId, string templateCreditContent, string remark)
        {
            try
            {
                //重名了
                if (true == CheckBillingTemplateName(custId, billingTemplateName, -1))
                {
                    return Json(new { code = Resources.Common.FAIL_CODE, message = Resources.Common.FAIL_MESSAGE });
                }
                else
                {
                    TugDataEntities db = new TugDataEntities();
                    TugDataModel.BillingTemplate cstmer = new BillingTemplate();

                    cstmer.BillingTemplateCode = billingTemplateCode;
                    cstmer.BillingTemplateName = billingTemplateName;
                    cstmer.BillingTemplateTypeID = billingTemplateTypeId;

                    cstmer.CreateDate = cstmer.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    cstmer.CustomerID = custId; // Convert.ToInt32(customerId);
                    cstmer.TemplateCreditContent = templateCreditContent;
                    cstmer.TimeTypeID = timeTypeId;

                    cstmer.Remark = remark;
                    cstmer.OwnerID = -1;
                    cstmer.UserID = -1;
                    cstmer.UserDefinedCol1 = "";
                    cstmer.UserDefinedCol2 = "";
                    cstmer.UserDefinedCol3 = "";
                    cstmer.UserDefinedCol4 = "";

                    //if (Request.Form["UserDefinedCol5"] != "")
                    //    cstmer.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                    //if (Request.Form["UserDefinedCol6"] != "")
                    //    cstmer.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                    //if (Request.Form["UserDefinedCol7"] != "")
                    //    cstmer.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                    //if (Request.Form["UserDefinedCol8"] != "")
                    //    cstmer.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                    cstmer.UserDefinedCol9 = "";
                    cstmer.UserDefinedCol10 = "";

                    cstmer = db.BillingTemplate.Add(cstmer);
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

        public ActionResult DeleteCustomerBillScheme()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                BillingTemplate cstmer = db.BillingTemplate.FirstOrDefault(u => u.IDX == idx);
                if (cstmer != null)
                {
                    db.BillingTemplate.Remove(cstmer);
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


        /// <summary>
        /// 检查模板名称是否重名
        /// </summary>
        /// <param name="custId">客户ID</param>
        /// <param name="billingTemplateName">数据的模板名称</param>
        /// <param name="billTemplateId">新增传-1， 编辑传模板ID</param>
        /// <returns>true：重名，false：不重名</returns>
        private bool CheckBillingTemplateName(int custId, string billingTemplateName, int billTemplateId = -1)
        {
            try
            {
                TugDataEntities db = new TugDataEntities();

                if (billTemplateId == -1) //新增
                {
                    int ret = 0;
                    ret = db.BillingTemplate.Where(u => u.CustomerID == custId && u.BillingTemplateName == billingTemplateName).Count();

                    return ret > 0;
                }
                else
                {
                    List<BillingTemplate> list = db.BillingTemplate.Where(u => u.CustomerID == custId && u.BillingTemplateName == billingTemplateName).ToList<BillingTemplate>();
                    if(list == null || list.Count <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (billTemplateId == list[0].IDX) {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public ActionResult GetCustomerBillSchemeItems(bool _search, string sidx, string sord, int page, int rows, int billSchemeId)
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
                    //List<V_BillingItemTemplate> orders = db.V_BillingItemTemplate.Where(u => u.BillingTemplateID == billSchemeId).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_BillingItemTemplate>();
                    List<V_BillingItemTemplate> orders = TugBusinessLogic.Module.CustomerLogic.LoadDataForCustomerBillingItemTemplate(sidx, sord, billSchemeId);
                    int totalRecordNum = orders.Count;
                    
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_BillingItemTemplate> page_orders = orders.Skip((page - 1) * rows).Take(rows).ToList<V_BillingItemTemplate>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult AddEditCustomerBillSchemeItem()
        {
            this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.BillingItemTemplate aScheduler = new BillingItemTemplate();
                        aScheduler.BillingTemplateID = Convert.ToInt32(Request.Form["BillingTemplateID"].Trim());
                        aScheduler.ItemID = Convert.ToInt32(Request.Form["ItemID"].Trim());
                        aScheduler.UnitPrice = Convert.ToDouble(Request.Form["UnitPrice"].Trim());
                        aScheduler.Currency = Request.Form["Currency".Trim()];
                        aScheduler.TypeID = Convert.ToInt32(Request.Form["TypeID"].Trim());
                        
                        aScheduler.OwnerID = -1;
                        aScheduler.UserID = -1;

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

                        aScheduler = db.BillingItemTemplate.Add(aScheduler);
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

                    int idx = Convert.ToInt32(Request.Form["IDX"]);
                    BillingItemTemplate aScheduler = db.BillingItemTemplate.Where(u => u.IDX == idx).FirstOrDefault();

                    if (aScheduler == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        aScheduler.BillingTemplateID = Convert.ToInt32(Request.Form["BillingTemplateID"].Trim());
                        aScheduler.ItemID = Convert.ToInt32(Request.Form["ItemID"].Trim());
                        aScheduler.UnitPrice = Convert.ToDouble(Request.Form["UnitPrice"].Trim());
                        aScheduler.Currency = Request.Form["Currency"].Trim();
                        aScheduler.TypeID = Convert.ToInt32(Request.Form["TypeID"].Trim());

                        
                        aScheduler.OwnerID = -1;
                        aScheduler.UserID = -1;
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

        public ActionResult AddCustomerBillSchemeItem(int billingTemplateId, int itemId, string unitPrice, string currency, int typeId) 
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                {
                    TugDataModel.BillingItemTemplate aScheduler = new BillingItemTemplate();
                    aScheduler.BillingTemplateID = billingTemplateId;
                    aScheduler.ItemID = itemId;
                    aScheduler.UnitPrice = Convert.ToDouble(unitPrice);
                    aScheduler.Currency = currency;
                    aScheduler.TypeID = typeId;

                    aScheduler.OwnerID = -1;
                    aScheduler.UserID = -1;

                    aScheduler.CreateDate = aScheduler.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    aScheduler.UserDefinedCol1 = "";
                    aScheduler.UserDefinedCol2 = "";
                    aScheduler.UserDefinedCol3 = "";
                    aScheduler.UserDefinedCol4 = "";

                    //if (Request.Form["UserDefinedCol5"] != "")
                    //    aScheduler.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                    //if (Request.Form["UserDefinedCol6"] != "")
                    //    aScheduler.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                    //if (Request.Form["UserDefinedCol7"] != "")
                    //    aScheduler.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                    //if (Request.Form["UserDefinedCol8"] != "")
                    //    aScheduler.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                    aScheduler.UserDefinedCol9 = "";
                    aScheduler.UserDefinedCol10 = "";

                    aScheduler = db.BillingItemTemplate.Add(aScheduler);
                    db.SaveChanges();

                    var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                    //Response.Write(@Resources.Common.SUCCESS_MESSAGE);
                    return Json(ret);
                }
            }
            catch (Exception ex)
            {
                var ret = new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE };
                //Response.Write(@Resources.Common.EXCEPTION_MESSAGE);
                return Json(ret);
            }
            return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
        }

        public ActionResult DeleteCustomerBillSchemeItem()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                BillingItemTemplate aScheduler = db.BillingItemTemplate.FirstOrDefault(u => u.IDX == idx);
                if (aScheduler != null)
                {
                    db.BillingItemTemplate.Remove(aScheduler);
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

        public string GetPayItems()
        {
            string s = string.Empty;

            try
            {
                TugDataEntities db = new TugDataEntities();
                List<CustomField> list = db.CustomField.Where(u => u.CustomName == "OrderInfor.ServiceNatureID"
                    || u.CustomName == "BillingItemTemplate.ItemID").OrderBy(u => u.CustomValue).ToList<CustomField>();

                if (list != null && list.Count > 0)
                {
                    s += "<select><option value=-1~-1~请选择>请选择</option>";
                    foreach (CustomField item in list)
                    {
                        s += string.Format("<option value={0}>{1}</option>", item.IDX + "~" + item.CustomValue + "~" + item.CustomLabel, item.CustomLabel);
                    }
                    s += "</select>";
                }
            }
            catch (Exception ex)
            {
            }
            return s;
        }



        /// <summary>
        /// 得到计费模板类型
        /// </summary>
        /// <returns></returns>
        public List<CustomField> GetBillTemplateTypes()
        {
            TugDataEntities db = new TugDataEntities();
            List<CustomField> list = db.CustomField.Where(u => u.CustomName == "BillingTemplate.BillingTemplateType")
                .OrderBy(u => u.CustomValue).ToList<CustomField>();
            return list;
        }

        /// <summary>
        /// 得到计费模板的计时方式
        /// </summary>
        /// <returns></returns>
        public List<CustomField> GetBillTemplateTimeTypes()
        {
            TugDataEntities db = new TugDataEntities();
            List<CustomField> list = db.CustomField.Where(u => u.CustomName == "BillingTemplate.TimeTypeID")
                .OrderBy(u => u.CustomValue).ToList<CustomField>();
            return list;
        }


        /// <summary>
        /// 得到计费模板付费项目
        /// </summary>
        /// <returns></returns>
        public List<CustomField> GetBillTemplatePayItems()
        {

            TugDataEntities db = new TugDataEntities();
            List<CustomField> list = db.CustomField.Where(u => u.CustomName == "OrderInfor.ServiceNatureID"
                || u.CustomName == "BillingItemTemplate.ItemID").OrderBy(u => u.CustomValue).ToList<CustomField>();

            return list;
        }

        /// <summary>
        /// 得到计费模板付费项目在发票中的位置：上中下
        /// </summary>
        /// <returns></returns>
        public List<CustomField> GetBillTemplatePayItemPosition()
        {

            TugDataEntities db = new TugDataEntities();
            List<CustomField> list = db.CustomField.Where(u => u.CustomName == "BillingItemTemplate.TypeID")
                .OrderBy(u => u.CustomValue).ToList<CustomField>();

            return list;
        }

        #endregion 计费方案 Written By lg
    }
}