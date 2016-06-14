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
        private static int _DefaultPageSie = 10;
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
                        cstmer.CreateDate = cstmer.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
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
                        cstmer.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
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
                if (totalRecordNum % rows == 0) page -= 1;
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

            return View(list);
        }

        [HttpGet]
        public ActionResult GetCustomers(int curPage, string queryName="")
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
                    customers = db.Customer.Where(u=>u.CnName.Contains(queryName) || u.EnName.Contains(queryName) || u.SimpleName.Contains(queryName))
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
        #endregion
    }
}