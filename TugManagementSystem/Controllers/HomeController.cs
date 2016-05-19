using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            TugDataModel.OrderInfor order = new OrderInfor();
            order.Code = "123";

            return View(order);
        }

        public ActionResult GetData(bool _search, string sidx, string sord, int page, int rows)
        {
            try
            {
                using(TugDataEntities db = new TugDataEntities())
                {
                    //List <OrderInfor> orders = db.OrderInfor.Select(u => u).OrderBy(u => u.ID).ToList<OrderInfor>();
                    List<object> list = new List<object>();

                    for (int i = 0; i < 30; i++)
                    {
                        var o = new { OrderID = (i+1).ToString(), CustomerID = (i+1).ToString(), OrderDate = DateTime.Now.Date.ToString(), Freight = (i+1).ToString(), ShipName = (i+1).ToString() };
                        list.Add(o);
                    }


                    var jsonData = new { page = page, records = 30, total = 2, rows = list };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception)
            {
                return Json(new { code = 3, message = "出现异常，修改失败！" });
            }
        }

        public ActionResult AddEdit()
        {
            var f = Request.Form;

            #region Add
            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    using(TugDataEntities db = new TugDataEntities())
                    {
                        if(null == db.OrderInfor.Where(u => u.Code == "123").FirstOrDefault())
                        {
                            TugDataModel.OrderInfor o = new OrderInfor();
                            o.Code = "123";

                            o = db.OrderInfor.Add(o);
                            db.SaveChanges();
                            Response.Write(o);
                            return Json(o);

                        }
                    }
                }
                catch(Exception)
                {
                    var ret = new { code = 4, message = "出现异常，新增失败！" };
                    Response.Write(ret);
                    return Json(ret);
                }

            }
            #endregion

            #region Edit
            else if (Request.Form["oper"].Equals("edit"))
            {
                try
                {
                    using (TugDataEntities db = new TugDataEntities())
                    {
                        OrderInfor aOrder = db.OrderInfor.Where(u => u.Code == "123").FirstOrDefault();

                        if (aOrder != null && aOrder.ID != 1 && aOrder.Code.Equals("123"))
                        {
                            return Json(new { code = 1, message = "订单名称已存在，请重新输入！" });
                        }
                        else
                        {
                            using (System.Transactions.TransactionScope transaction = new System.Transactions.TransactionScope())
                            {
                                aOrder = db.OrderInfor.FirstOrDefault(u => u.ID == 1);
                                aOrder.Code = "456";

                                db.Entry(aOrder).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();

                                transaction.Complete();
                            }

                            return Json(new { code = 2, message = "修改成功！" });
                        }
                    }
                }
                catch(Exception)
                {
                    return Json(new { code = 3, message = "出现异常，修改失败！" });
                }
            }
            #endregion

            return Json(new { });
        }

        public ActionResult Delete()
        {
            try
            {
                var f = Request.Form;
                using (TugDataEntities db = new TugDataEntities())
                {
                    OrderInfor aOrder = db.OrderInfor.FirstOrDefault(u => u.ID == 1);
                    if (aOrder != null)
                    {
                        db.OrderInfor.Remove(aOrder);
                        db.SaveChanges();
                        return Json(new { code = 1, message = "删除成功！" });
                    }
                    else
                    {
                        return Json(new { code = 2, message = "无效基地，删除失败！" });
                    }
                }
            }
            catch (Exception)
            {
                //throw e.InnerException;
                //return Json("删除失败！" + e.InnerException.Message);
                return Json(new { code = 3, message = "该基地与业务有关联，请先删除关联业务，再删除此基地！" });
            }

        }
    }
}
