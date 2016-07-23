using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugBusinessLogic;
using TugBusinessLogic.Module;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class CustomEntity
    {
        public int IDX;
        public string Name;
    }

    public class CustomDataController : BaseController
    {
        //
        // GET: /CommonData/
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult CustomDataManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            List<CustomEntity> list = new List<CustomEntity>();
            CustomEntity ctmobj=new CustomEntity();
            ctmobj.IDX = 0;
            ctmobj.Name = "Location";
            list.Add(ctmobj);

            ViewBag.TotalPageNum = 1;
            ViewBag.CurPage = 1;
            return View(list);
        }
        public ActionResult GetCustomData(bool _search, string sidx, string sord, int page, int rows, string ctmName)
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
                    List<CustomField> objs = db.CustomField.Where(u => u.CustomName == ctmName).Select(u => u).OrderByDescending(u => u.LastUpDate).ToList<CustomField>();
                    int totalRecordNum = objs.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<CustomField> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<CustomField>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }
        [JsonExceptionFilterAttribute]
        public ActionResult AddEdit(int ctmId)
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

                        ship.CustomerID = ctmId;// Util.toint(Request.Form["CustomerID"]);
                        ship.ShipTypeID = -1;//Util.toint(Request.Form["ShipTypeID"]);
                        ship.Name1 = Request.Form["Name1"];
                        ship.Name2 = Request.Form["Name2"];
                        ship.SimpleName = Request.Form["SimpleName"];
                        ship.DeadWeight = Util.toint(Request.Form["DeadWeight"]);
                        ship.Length = Util.toint(Request.Form["Length"]);
                        ship.Width = Util.toint(Request.Form["Width"]);
                        ship.TEUS = Util.toint(Request.Form["TEUS"]);
                        ship.Class = Request.Form["Class"];
                        ship.Remark = Request.Form["Remark"];
                        ship.OwnerID = -1;
                        ship.CreateDate = ship.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                        ship.UserID = Session.GetDataFromSession<int>("userid");
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
                    string name1 = Request.Form["Name1"];
                    System.Linq.Expressions.Expression<Func<Customer, bool>> exp = u => u.Name1 == name1 && u.IDX != idx;
                    Customer tmpUserName = db.Customer.Where(exp).FirstOrDefault();
                    if (tmpUserName != null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = "船名称已存在！" });//Resources.Common.ERROR_MESSAGE
                    }

                    CustomerShip ship = db.CustomerShip.Where(u => u.IDX == idx).FirstOrDefault();

                    if (ship == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        ship.CustomerID = ctmId;// Util.toint(Request.Form["CustomerID"]);
                        ship.ShipTypeID = -1; //Util.toint(Request.Form["ShipTypeID"]);
                        ship.Name1 = Request.Form["Name1"];
                        ship.Name2 = Request.Form["Name2"];
                        ship.SimpleName = Request.Form["SimpleName"];
                        ship.DeadWeight = Util.toint(Request.Form["DeadWeight"]);
                        ship.Length = Util.toint(Request.Form["Length"]);
                        ship.Width = Util.toint(Request.Form["Width"]);
                        ship.TEUS = Util.toint(Request.Form["TEUS"]);
                        ship.Class = Request.Form["Class"];
                        ship.Remark = Request.Form["Remark"];
                        ship.OwnerID = -1;
                        ship.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                        ship.UserID = Session.GetDataFromSession<int>("userid");
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
	}
}