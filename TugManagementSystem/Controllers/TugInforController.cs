using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugDataModel;
using TugBusinessLogic;

namespace TugManagementSystem.Controllers
{
    public class TugInforController : BaseController
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
                        TugDataModel.TugInfor tug = new TugInfor();

                        tug.Code = Request.Form["Code"];
                        tug.CnName = Request.Form["CnName"];
                        tug.EnName = Request.Form["EnName"];
                        tug.SimpleName = Request.Form["SimpleName"];
                        tug.Power = Request.Form["Power"];
                        tug.Class = Request.Form["Class"];
                        tug.Speed = Request.Form["Speed"];
                        tug.Length = Request.Form["Length"];
                        tug.Width = Request.Form["Width"];
                        tug.Remark = Request.Form["Remark"];
                        tug.OwnerID = -1;
                        tug.CreateDate = tug.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");;
                        tug.UserID = Session.GetDataFromSession<int>("userid"); 
                        tug.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        tug.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        tug.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        tug.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            tug.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            tug.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            tug.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            tug.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        tug.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        tug.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        tug = db.TugInfor.Add(tug);
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
                    TugInfor tug = db.TugInfor.Where(u => u.IDX == idx).FirstOrDefault();

                    if (tug == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        tug.Code = Request.Form["Code"];
                        tug.CnName = Request.Form["CnName"];
                        tug.EnName = Request.Form["EnName"];
                        tug.SimpleName = Request.Form["SimpleName"];
                        tug.Power = Request.Form["Power"];
                        tug.Class = Request.Form["Class"];
                        tug.Speed = Request.Form["Speed"];
                        tug.Length = Request.Form["Length"];
                        tug.Width = Request.Form["Width"];
                        tug.Remark = Request.Form["Remark"];
                        tug.OwnerID = -1;
                        tug.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");;
                        tug.UserID = Session.GetDataFromSession<int>("userid"); 
                        tug.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        tug.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        tug.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        tug.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            tug.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            tug.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            tug.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            tug.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        tug.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        tug.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        db.Entry(tug).State = System.Data.Entity.EntityState.Modified;
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
                TugInfor tug = db.TugInfor.FirstOrDefault(u => u.IDX == idx);
                if (tug != null)
                {
                    db.TugInfor.Remove(tug);
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
                List<TugInfor> TugInfors = db.TugInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<TugInfor>();
                int totalRecordNum = TugInfors.Count;
                if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<TugInfor> page_TugInfors = TugInfors.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<TugInfor>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = TugInfors };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        //
        // GET: /TugInfor/
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult TugInforManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            return View();
        }


        #region written by lg
        public ActionResult GetTugEx(bool _search, string sidx, string sord, int page, int rows, string workDate)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                List<TugInfor> TugInfors = db.TugInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<TugInfor>();

                int totalRecordNum = TugInfors.Count;
                if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<TugInfor> page_TugInfors = TugInfors.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<TugInfor>();


                List<TugBusinessLogic.TugEx> lst = new List<TugBusinessLogic.TugEx>();

                if (TugInfors != null)
                {
                    foreach (TugInfor tug in TugInfors)
                    {
                        TugBusinessLogic.TugEx o = new TugBusinessLogic.TugEx();
                        o.TugID = tug.IDX;
                        o.CnName = tug.CnName;
                        o.EnName = tug.EnName;
                        o.SimpleName = tug.SimpleName;
                        o.Code = tug.Code;

                        o = TugBusinessLogic.Module.OrderLogic.GetTugSchedulerBusyState(tug.IDX, o, workDate);

                        lst.Add(o);
                    }
                }

                

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = lst };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }
        #endregion
    }
}