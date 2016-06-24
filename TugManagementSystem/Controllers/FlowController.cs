using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TugBusinessLogic.Module;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class FlowController : BaseController
    {
        #region handsontable方式实现，模态框中下拉、日期均可实现
        public ActionResult FlowView_Handsontable(string lan, int? id)  //复杂版，显示组织结构，人员
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            return View();
        }
        public ActionResult GetNodes()
        {
            string[] mynodes=new string[2];
            mynodes[0] = "创建";
            mynodes[1] = "校对";
            mynodes[2] = "审核";
            return Json(mynodes, JsonRequestBehavior.AllowGet);
            //List<object> source = new List<object>();
            //source.Add(new { FlowUserID = "123", CnName = "张三" });
            //source.Add(new { FlowUserID = "234", CnName = "李四" });
            //source.Add(new { FlowUserID = "345", CnName = "王五" });
            //source.Add(new { FlowUserID = "456", CnName = "赵六" });

            //var p = Request.Params;

            //List<object> list = new List<object>();

            //list.Add(source[0]);
            //list.Add(source[1]);
            //list.Add(source[2]);
            //list.Add(source[3]);

            //var jsonData = new { list = list };
            //return Json(jsonData, JsonRequestBehavior.AllowGet);
        }        
        #endregion

        #region jqgrid方式实现，有问题：模态框中下拉、日期不能正常显示
        public ActionResult AddEdit()
        {
            string newcode;
            int level;

            this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    var fatherid = Request.Form["FatherID"];

                    if (fatherid == "")
                    {
                        newcode = NewInCode("O");
                        level = 0;
                    }
                    else
                    {
                        int curid = Util.toint(fatherid);
                        BaseTreeItems curobj;
                        curobj = db.BaseTreeItems.Where(u => u.IDX == curid).FirstOrDefault();
                        string curincode = curobj.InCode;
                        level = Util.toint(curobj.LevelValue) + 1;
                        newcode = NewInCode(curincode);
                    }
                    {
                        TugDataModel.BaseTreeItems obj = new BaseTreeItems();

                        obj.InCode = newcode;
                        if (fatherid != "") obj.FatherID = Util.toint(fatherid);
                        obj.LevelValue = level;
                        obj.IsLeaf = "true";
                        obj.CNName = Request.Form["CNName"];
                        obj.ENName = "";
                        obj.SType = "Organizion";
                        obj.SortNum = 0;
                        obj.Remark = "";
                        obj.OwnerID = -1;
                        obj.CreateDate = obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        obj.UserID = -1;
                        obj.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        obj.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        obj.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        obj.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            obj.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            obj.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            obj.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            obj.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        obj.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        obj.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        obj = db.BaseTreeItems.Add(obj);
                        db.SaveChanges();

                        //将父节点的isleaf设为false
                        if (fatherid != "")
                        {
                            int fid = Util.toint(fatherid);
                            BaseTreeItems fobj = db.BaseTreeItems.Where(u => u.IDX == fid).FirstOrDefault();
                            fobj.IsLeaf = "false";
                            db.Entry(fobj).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

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
                    BaseTreeItems obj = db.BaseTreeItems.Where(u => u.IDX == idx).FirstOrDefault();

                    if (obj == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        obj.InCode = "";
                        //obj.FatherID = System.DBNull.Value;
                        obj.LevelValue = 0;
                        obj.IsLeaf = "false";
                        obj.CNName = Request.Form["CNName"];
                        obj.ENName = "";
                        obj.SType = "Organizion";
                        obj.SortNum = 0;
                        obj.Remark = "";
                        obj.OwnerID = -1;
                        obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        obj.UserID = -1;
                        obj.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        obj.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        obj.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        obj.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            obj.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            obj.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            obj.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            obj.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        obj.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        obj.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult AddFlow(string lan, int? id)  //复杂版，显示组织结构，人员
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            return View();
        }

        public ActionResult CreateFlow(string lan, int? id) //只是流程节点的管理
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;
            return View();
        }

        public ActionResult FlowAddEdit()
        {
            this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.Flow obj = new Flow();

                        obj.BillingID = -1;
                        obj.MarkID = -1;
                        obj.Phase = -1;
                        obj.Task = Request.Form["Task"];
                        obj.FlowUserID = Util.toint(Request.Form["FlowUserID"]);
                        obj.StDate = Request.Form["StDate"];
                        obj.EndDate = Request.Form["EndDate"];
                        obj.System = "Billing";
                        obj.OwnerID = -1;
                        obj.CreateDate = obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        obj.UserID = -1;
                        obj.State = -1;
                        obj.Sign = "";
                        //obj.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        //obj.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        //obj.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        //obj.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        //if (Request.Form["UserDefinedCol5"] != "")
                        //    obj.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        //if (Request.Form["UserDefinedCol6"] != "")
                        //    obj.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        //if (Request.Form["UserDefinedCol7"] != "")
                        //    obj.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        //if (Request.Form["UserDefinedCol8"] != "")
                        //    obj.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        //obj.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        //obj.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        obj = db.Flow.Add(obj);
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
                    Flow obj = db.Flow.Where(u => u.IDX == idx).FirstOrDefault();

                    if (obj == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        obj.BillingID = -1;
                        obj.MarkID = -1;
                        obj.Phase = -1;
                        obj.Task = Request.Form["Task"];
                        obj.FlowUserID = Util.toint(Request.Form["FlowUserID"]);
                        obj.StDate = Request.Form["StDate"];
                        obj.EndDate = Request.Form["EndDate"];
                        obj.System = "Billing";
                        obj.OwnerID = -1;
                        obj.CreateDate = obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        obj.UserID = -1;
                        obj.State = -1;
                        obj.Sign = "";
                        obj.UserDefinedCol1 = Request.Form["UserDefinedCol1"];
                        obj.UserDefinedCol2 = Request.Form["UserDefinedCol2"];
                        obj.UserDefinedCol3 = Request.Form["UserDefinedCol3"];
                        obj.UserDefinedCol4 = Request.Form["UserDefinedCol4"];

                        if (Request.Form["UserDefinedCol5"] != "")
                            obj.UserDefinedCol5 = Convert.ToDouble(Request.Form["UserDefinedCol5"]);

                        if (Request.Form["UserDefinedCol6"] != "")
                            obj.UserDefinedCol6 = Convert.ToInt32(Request.Form["UserDefinedCol6"]);

                        if (Request.Form["UserDefinedCol7"] != "")
                            obj.UserDefinedCol7 = Convert.ToInt32(Request.Form["UserDefinedCol7"]);

                        if (Request.Form["UserDefinedCol8"] != "")
                            obj.UserDefinedCol8 = Convert.ToInt32(Request.Form["UserDefinedCol8"]);

                        obj.UserDefinedCol9 = Request.Form["UserDefinedCol9"];
                        obj.UserDefinedCol10 = Request.Form["UserDefinedCol10"];

                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult FlowDelete()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                Flow obj = db.Flow.FirstOrDefault(u => u.IDX == idx);
                if (obj != null)
                {
                    db.Flow.Remove(obj);
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

        public ActionResult GetUser(string term)
        {
            List<object> source = new List<object>();
            source.Add(new { FlowUserID = "123", CnName = "张三" });
            source.Add(new { FlowUserID = "234", CnName = "李四" });
            source.Add(new { FlowUserID = "345", CnName = "王五" });
            source.Add(new { FlowUserID = "456", CnName = "赵六" });

            var p = Request.Params;

            List<object> list = new List<object>();

            list.Add(source[0]);
            list.Add(source[1]);
            list.Add(source[2]);
            list.Add(source[3]);

            var jsonData = new { list = list };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Flow/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadFlow(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                //db.Configuration.ProxyCreationEnabled = false;
                List<V_Flow> objs = db.V_Flow.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_Flow>();
                int totalRecordNum = objs.Count;
                if (totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<V_Flow> page_objs = objs.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<V_Flow>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = objs };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public JsonResult LoadOrganizationOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            //string s = Request.QueryString[6];

            try
            {
                TugDataEntities db = new TugDataEntities();

                //db.Configuration.ProxyCreationEnabled = false;
                List<V_BaseTreeItems> objs = db.V_BaseTreeItems.Select(u => u).OrderBy(u => u.IDX).ToList<V_BaseTreeItems>();
                int totalRecordNum = objs.Count;
                if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<V_BaseTreeItems> page_objs = objs.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<V_BaseTreeItems>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = objs };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public JsonResult LoadUsers(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            try
            {
                TugDataEntities db = new TugDataEntities();

                //db.Configuration.ProxyCreationEnabled = false;
                List<V_Users> objs = db.V_Users.Select(u => u).OrderByDescending(u => u.IDX).ToList<V_Users>();
                int totalRecordNum = objs.Count;
                if (totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<V_Users> page_objs = objs.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<V_Users>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = objs };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public JsonResult LoadUsersFilter(/*bool _search, string sidx, string sord, int page, int rows*/)
        {
            this.Internationalization();
            try
            {
                TugDataEntities db = new TugDataEntities();
                //string incode = "001002001";
                string incode = Request["incode"]; //"001002001"; //
                Console.WriteLine(incode);

                List<V_Users> objs = db.V_Users.Where(u => u.InCode.StartsWith(incode)).OrderByDescending(u => u.IDX).ToList<V_Users>();
                int totalRecordNum = objs.Count;
                //if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                //int pageSize = rows;
                //int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<V_Users> page_objs = objs.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<V_Users>();

                //var jsonData = new { page = 1, records = totalRecordNum, total = totalPageNum, rows = objs };
                var jsonData = new { page = 1, records = totalRecordNum, total = 1, rows = objs };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string NewInCode(string curInCode)
        {
            //获取结构树的已有筛选数据
            int SectionLen = 3;
            string error = null;
            string NewInCode;
            TugDataEntities db = new TugDataEntities();
            BaseTreeItems obj = new BaseTreeItems();
            System.Linq.Expressions.Expression<Func<BaseTreeItems, bool>> expression = u => u.InCode.StartsWith(curInCode) && u.InCode.Length == curInCode.Length + SectionLen;
            List<BaseTreeItems> objs = db.BaseTreeItems.Where(expression).OrderByDescending(u => u.IDX).ToList<BaseTreeItems>();
            if (objs.Count == 0)
            {
                NewInCode = curInCode + string.Format("{0:D" + SectionLen + "}", 1);
            }
            else
            {
                string No;
                string maxInCode;
                maxInCode = objs.First().InCode.ToString();
                No = maxInCode.Substring(maxInCode.Length - SectionLen);
                NewInCode = curInCode + string.Format("{0:D" + SectionLen + "}", Convert.ToInt32(No) + 1);
            }
            return NewInCode;
        }
        #endregion
    }
}