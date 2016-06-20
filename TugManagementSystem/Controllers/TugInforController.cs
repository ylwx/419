using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugDataModel;

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
                        tug.CreateDate = tug.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        tug.UserID = -1;
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
                        tug.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        tug.UserID = -1;
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

        public ActionResult TugInforManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            return View();
        }


        #region written by lg
        public ActionResult GetTugEx(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();
                List<TugInfor> TugInfors = db.TugInfor.Select(u => u).OrderByDescending(u => u.IDX).ToList<TugInfor>();
                List<TugManagementSystem.MyClass.TugEx> lst = new List<MyClass.TugEx>();
                foreach (TugInfor tug in TugInfors)
                {
                    MyClass.TugEx o = new MyClass.TugEx();
                    o.TugID = tug.IDX;
                    o.CnName = tug.CnName;
                    o.EnName = tug.EnName;
                    o.SimpleName = tug.SimpleName;
                    o.Code = tug.Code;

                    o.Cell0 = 0;
                    o.Cell1 = 0;
                    o.Cell2 = 0;
                    o.Cell3 = 0;
                    o.Cell4 = 0;
                    o.Cell5 = 1;
                    o.Cell6 = 0;
                    o.Cell7 = 0;
                    o.Cell8 = 0;
                    o.Cell9 = 0;
                    o.Cell10 = 0;
                    o.Cell11 = 0;


                    o.Cell12 = 0;
                    o.Cell13 = 0;
                    o.Cell14 = 0;
                    o.Cell15 = 0;
                    o.Cell16 = 0;
                    o.Cell17 = 0;
                    o.Cell18 = 0;
                    o.Cell19 = 0;
                    o.Cell20 = 0;
                    o.Cell21 = 0;
                    o.Cell22 = 0;
                    o.Cell23 = 0;


                    o.Cell24 = 0;
                    o.Cell25 = 0;
                    o.Cell26 = 0;
                    o.Cell27 = 0;
                    o.Cell28 = 0;
                    o.Cell29 = 0;
                    o.Cell30 = 0;
                    o.Cell31 = 0;
                    o.Cell32 = 0;
                    o.Cell33 = 0;
                    o.Cell34 = 0;
                    o.Cell35 = 0;

                    o.Cell36 = 0;
                    o.Cell37 = 0;
                    o.Cell38 = 0;
                    o.Cell39 = 0;
                    o.Cell40 = 0;
                    o.Cell41 = 0;
                    o.Cell42 = 0;
                    o.Cell43 = 0;
                    o.Cell44 = 0;
                    o.Cell45 = 0;
                    o.Cell46 = 0;
                    o.Cell47 = 0;

                    lst.Add(o);
                }

                int totalRecordNum = TugInfors.Count;
                if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<TugInfor> page_TugInfors = TugInfors.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<TugInfor>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = lst };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }
        #endregion
    }
}