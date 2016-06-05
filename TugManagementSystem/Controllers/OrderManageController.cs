using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class OrderManageController : BaseController
    {
        #region 页面Action

        // GET: OrderManage
        public ActionResult OrderManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            return View();
        }

        //GET: OrderScheduling
        public ActionResult OrderScheduling(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            return View();
        }

        #endregion 页面Action

        #region 订单管理页面Action

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

                        aOrder.Code = TugBusinessLogic.Utils.GetOrderSequenceNo();

                        aOrder.CreateDate = aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        aOrder.CustomerID = Convert.ToInt32(Request.Form["CustomerID"]);
                        aOrder.CustomerName = Request.Form["CustomerName"];
                        aOrder.OrdTime = Request.Form["OrdTime"];
                        aOrder.WorkTime = Request.Form["WorkTime"];
                        aOrder.EstimatedCompletionTime = Request.Form["EstimatedCompletionTime"];

                        aOrder.IsGuest = Request.Form["IsGuest"].ToLower();
                        aOrder.LinkMan = Request.Form["LinkMan"];
                        aOrder.LinkPhone = Request.Form["LinkPhone"];
                        aOrder.LinkEmail = Request.Form["LinkEmail"];

                        if (Request.Form["BigTugNum"] != "")
                            aOrder.BigTugNum = Convert.ToInt32(Request.Form["BigTugNum"]);
                        if (Request.Form["MiddleTugNum"] != "")
                            aOrder.MiddleTugNum = Convert.ToInt32(Request.Form["MiddleTugNum"]);
                        if (Request.Form["SmallTugNum"] != "")
                            aOrder.SmallTugNum = Convert.ToInt32(Request.Form["SmallTugNum"]);

                        aOrder.OwnerID = -1;
                        aOrder.Remark = Request.Form["Remark"];
                        aOrder.ShipID = Convert.ToInt32(Request.Form["ShipID"]);
                        aOrder.ShipName = Request.Form["ShipName"];
                        aOrder.UserID = -1;
                        aOrder.WorkPlace = Request.Form["WorkPlace"];
                        aOrder.WorkStateID = Convert.ToInt32(Request.Form["WorkStateName"]); //Convert.ToInt32(Request.Form["WorkStateID"]);
                        aOrder.WorkStateName = Request.Form["WorkStateID"];

                        aOrder.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        aOrder.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        aOrder.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        aOrder.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            aOrder.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            aOrder.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            aOrder.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        aOrder.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        aOrder.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

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

                    int idx = Convert.ToInt32(Request.Form["IDX"]);
                    OrderInfor aOrder = db.OrderInfor.Where(u => u.IDX == idx).FirstOrDefault();

                    if (aOrder == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        aOrder.CustomerID = Convert.ToInt32(Request.Form["CustomerID"]);
                        aOrder.CustomerName = Request.Form["CustomerName"];
                        aOrder.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");

                        aOrder.LinkMan = Request.Form["LinkMan"];
                        aOrder.LinkPhone = Request.Form["LinkPhone"];
                        aOrder.LinkEmail = Request.Form["LinkEmail"];

                        aOrder.OrdTime = Request.Form["OrdTime"];
                        aOrder.WorkTime = Request.Form["WorkTime"];
                        aOrder.EstimatedCompletionTime = Request.Form["EstimatedCompletionTime"];

                        aOrder.ShipID = Convert.ToInt32(Request.Form["ShipID"]);
                        aOrder.ShipName = Request.Form["ShipName"];
                        if (Request.Form["BigTugNum"] != "")
                            aOrder.BigTugNum = Convert.ToInt32(Request.Form["BigTugNum"]);
                        if (Request.Form["MiddleTugNum"] != "")
                            aOrder.MiddleTugNum = Convert.ToInt32(Request.Form["MiddleTugNum"]);
                        if (Request.Form["SmallTugNum"] != "")
                            aOrder.SmallTugNum = Convert.ToInt32(Request.Form["SmallTugNum"]);
                        aOrder.WorkPlace = Request.Form["WorkPlace"];
                        aOrder.WorkStateID = Convert.ToInt32(Request.Form["WorkStateName"]);//Convert.ToInt32(Request.Form["WorkStateID"]);
                        aOrder.WorkStateName = Request.Form["WorkStateID"];
                        aOrder.Remark = Request.Form["Remark"];

                        aOrder.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        aOrder.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        aOrder.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        aOrder.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            aOrder.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            aOrder.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            aOrder.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            aOrder.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        aOrder.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        aOrder.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

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
            List<object> source = new List<object>();
            source.Add(new { CustomerID = "123", ShipName = "abc" });
            source.Add(new { CustomerID = "234", ShipName = "cde" });
            source.Add(new { CustomerID = "345", ShipName = "efg" });
            source.Add(new { CustomerID = "456", ShipName = "ghi" });

            var p = Request.Params;

            List<object> list = new List<object>();

            list.Add(source[0]);
            list.Add(source[1]);

            var jsonData = new { list = list };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public string GetCustomField(string CustomName)
        {
            string[] query = { "未审核", "已确认", "备货中", "已发货", "已完成" };

            string s = "<select>";
            for (int i = 0; i < query.Length; i++)
            {
                s += string.Format("<option value={0}>{1}</option>", i + 1, query[i]);
            }
            s += "</select>";

            return s;
        }

        public ActionResult GetData(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            //string s = Request.QueryString[6];

            try
            {
                TugDataEntities db = new TugDataEntities();
                List<OrderInfor> orders = db.OrderInfor.Select(u => u).ToList<OrderInfor>();
                int totalRecordNum = orders.Count;
                if (totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                List<OrderInfor> page_orders = orders.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<OrderInfor>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_orders };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                List<OrderInfor> orders = db.OrderInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<OrderInfor>();
                int totalRecordNum = orders.Count;
                if (totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<OrderInfor> page_orders = orders.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<OrderInfor>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = orders };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        #endregion 订单管理页面Action
    }
}