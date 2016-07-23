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

    public class FuelpriceController : BaseController
    {
        [HttpGet]
        public ActionResult Fuelprice(string lan, int? id)
        {
            lan = this.Internationalization();
            return View();
        }
        public ActionResult LoadFuelprice(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();
            try
            {
                //int curUserId = 0;
                TugDataEntities db = new TugDataEntities();
                //curUserId = Session.GetDataFromSession<int>("userid");
                if (_search == true)
                {
                    string s = Request.QueryString["filters"];
                    return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<Fuelprice> objs = db.Fuelprice.ToList<Fuelprice>();
                    int totalRecordNum = objs.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);
                    List<Fuelprice> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<Fuelprice>();
                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult AddFuelprice(string EffectiveDate, double Price)
        {
            this.Internationalization();
            try
            {
                TugDataEntities db = new TugDataEntities();
                TugDataModel.Fuelprice price = new Fuelprice();
                price.EffectiveDate = EffectiveDate;
                price.Price = Price;
                price.Type = "";
                price.Unit = "";
                price.CreateDate = price.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
                price.AddUserID = Session.GetDataFromSession<int>("userid");
                price = db.Fuelprice.Add(price);
                db.SaveChanges();

                    var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
                    return Json(ret);
                }
 
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteFuelprice()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Util.toint(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                Fuelprice price = db.Fuelprice.FirstOrDefault(u => u.IDX == idx);
                if (price != null)
                {
                    db.Fuelprice.Remove(price);
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
        public ActionResult AddEdit(string EffectiveDate, double Price)
        {
            this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.Fuelprice price = new Fuelprice();

                        price.EffectiveDate = EffectiveDate;
                        price.Price = Price;
                        price.Unit = "";
                        price.Type ="";
                        price.CreateDate = price.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                        price.AddUserID = Session.GetDataFromSession<int>("userid");
                        price = db.Fuelprice.Add(price);
                        db.SaveChanges();

                        var ret = new { code = Resources.Common.SUCCESS_CODE, message = Resources.Common.SUCCESS_MESSAGE };
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
                    Fuelprice price = db.Fuelprice.Where(u => u.IDX == idx).FirstOrDefault();

                    if (price == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        price.EffectiveDate = EffectiveDate;
                        price.Price = Price;
                        price.Unit = "";
                        price.Type = "";
                        price.CreateDate = price.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                        price.AddUserID = Session.GetDataFromSession<int>("userid");
                        db.Entry(price).State = System.Data.Entity.EntityState.Modified;
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