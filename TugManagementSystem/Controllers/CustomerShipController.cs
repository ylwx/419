using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugBusinessLogic.Module;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class CustomerShipController : BaseController
    {
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
                        TugDataModel.CustomerShip ship = new CustomerShip();

                        ship.CustomerID = 4;// Util.toint(Request.Form["CustomerID"]);
                        ship.ShipTypeID = -1;//Util.toint(Request.Form["ShipTypeID"]);
                        ship.CnName = Request.Form["CnName"];
                        ship.EnName = Request.Form["EnName"];
                        ship.SimpleName = Request.Form["SimpleName"];
                        ship.DeadWeight = Util.toint(Request.Form["DeadWeight"]);
                        ship.Length = Util.toint(Request.Form["Length"]);
                        ship.Width = Util.toint(Request.Form["Width"]);
                        ship.TEUS = Util.toint(Request.Form["TEUS"]);
                        ship.Class = Request.Form["Class"];
                        ship.Remark = Request.Form["Remark"];
                        ship.OwnerID = -1;
                        ship.CreateDate = ship.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        ship.UserID = -1;
                        ship.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        ship.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        ship.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        ship.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            ship.UserDefinedCol5 = Util.tonumeric(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            ship.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            ship.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            ship.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"]);

                        ship.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        ship.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        ship = db.CustomerShip.Add(ship);
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

                    int idx = Util.toint(Request.Form["IDX"]);
                    CustomerShip ship = db.CustomerShip.Where(u => u.IDX == idx).FirstOrDefault();

                    if (ship == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        ship.CustomerID = 4;// Util.toint(Request.Form["CustomerID"]);
                        ship.ShipTypeID = -1; //Util.toint(Request.Form["ShipTypeID"]);
                        ship.CnName = Request.Form["CnName"];
                        ship.EnName = Request.Form["EnName"];
                        ship.SimpleName = Request.Form["SimpleName"];
                        ship.DeadWeight = Util.toint(Request.Form["DeadWeight"]);
                        ship.Length = Util.toint(Request.Form["Length"]);
                        ship.Width = Util.toint(Request.Form["Width"]);
                        ship.TEUS = Util.toint(Request.Form["TEUS"]);
                        ship.Class = Request.Form["Class"];
                        ship.Remark = Request.Form["Remark"];
                        ship.OwnerID = -1;
                        ship.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        ship.UserID = -1;
                        ship.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        ship.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        ship.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        ship.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            ship.UserDefinedCol5 = Util.tonumeric(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            ship.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            ship.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            ship.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"]);

                        ship.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        ship.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        db.Entry(ship).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult CustomerShipManage(string lan, int? id)
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
                CustomerShip ship = db.CustomerShip.FirstOrDefault(u => u.IDX == idx);
                if (ship != null)
                {
                    db.CustomerShip.Remove(ship);
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
                List<CustomerShip> customers = db.CustomerShip.Select(u => u).OrderByDescending(u => u.IDX).ToList<CustomerShip>();
                int totalRecordNum = customers.Count;
                if (totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<CustomerShip> page_customers = customers.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<CustomerShip>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = customers };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        //
        // GET: /CustomerShip/
        public ActionResult Index()
        {
            return View();
        }
    }
}