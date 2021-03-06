﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugBusinessLogic.Module;
using TugDataModel;
using TugBusinessLogic;

namespace TugManagementSystem.Controllers
{
    public class ArrangementController : BaseController
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
                        TugDataModel.Arrangement arr = new Arrangement();
                        arr.WorkDate = Request.Form["WorkDate"];
                        arr.TugID = 2;//Util.toint(Request.Form["TugID"]);
                        arr.SortNo = Util.toint(Request.Form["SortNo"]);
                        arr.TeamName = Request.Form["TeamName"];
                        arr.Remark = Request.Form["Remark"];
                        arr.OwnerID = -1;
                        arr.CreateDate = arr.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");;
                        arr.UserID = Session.GetDataFromSession<int>("userid"); 
                        arr.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        arr.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        arr.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        arr.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            arr.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            arr.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            arr.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            arr.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"]);

                        arr.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        arr.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        arr = db.Arrangement.Add(arr);
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
                    Arrangement arr = db.Arrangement.Where(u => u.IDX == idx).FirstOrDefault();

                    if (arr == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        arr.WorkDate = Request.Form["WorkDate"];
                        arr.TugID = 2;//Util.toint(Request.Form["TugID"]);
                        arr.SortNo = Util.toint(Request.Form["SortNo"]);
                        arr.TeamName = Request.Form["TeamName"];
                        arr.Remark = Request.Form["Remark"];
                        arr.OwnerID = -1;
                        arr.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");;
                        arr.UserID = Session.GetDataFromSession<int>("userid"); 
                        arr.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        arr.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        arr.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        arr.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            arr.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            arr.UserDefinedCol6 = Util.toint(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            arr.UserDefinedCol7 = Util.toint(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            arr.UserDefinedCol8 = Util.toint(Request.Form["UserDefinedCol8"]);

                        arr.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        arr.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        db.Entry(arr).State = System.Data.Entity.EntityState.Modified;
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
        [Authorize]
        public ActionResult ArrangementManage(string lan, int? id)
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

                int idx = Util.toint(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                Arrangement tug = db.Arrangement.FirstOrDefault(u => u.IDX == idx);
                if (tug != null)
                {
                    db.Arrangement.Remove(tug);
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
                List<V_Arrangement> Arrangements = db.V_Arrangement.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_Arrangement>();
                int totalRecordNum = Arrangements.Count;
                if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<V_Arrangement> page_Arrangements = Arrangements.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<Arrangement>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = Arrangements };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        //
        // GET: /Arrangement/
        public ActionResult Index()
        {
            return View();
        }
    }
}