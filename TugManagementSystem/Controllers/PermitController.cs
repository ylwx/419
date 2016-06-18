using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugBusinessLogic.Module;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class PermitController : BaseController
    {
        private static int _DefaultPageSie = 7;
        #region 角色
        [HttpGet]
        public ActionResult GetRoles(int curPage, string queryName = "")
        {
            ViewBag.Language = this.Internationalization();

            int totalRecordNum, totalPageNum;
            List<Role> list = GetRoles(curPage, _DefaultPageSie, out totalRecordNum, out totalPageNum, queryName);
            ViewBag.TotalPageNum = totalPageNum;
            ViewBag.CurPage = curPage;
            ViewBag.QueryName = queryName;

            return View("RoleUserManage", list);
        }

        public List<Role> GetRoles(int curPage, int pageSize, out int totalRecordNum, out int totalPageNum, string queryName = "")
        {
            try
            {
                TugDataEntities db = new TugDataEntities();

                List<Role> Roles = null;
                if (queryName == "")
                {
                    Roles = db.Role.Select(u => u).OrderByDescending(u => u.IDX).ToList<Role>();
                }
                else
                {
                    Roles = db.Role.Where(u => u.RoleName.Contains(queryName))
                        .Select(u => u).OrderByDescending(u => u.IDX).ToList<Role>();
                }

                totalRecordNum = Roles.Count;
                //if (totalRecordNum % pageSize == 0) page -= 1;
                //int pageSize = rows;
                totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                List<Role> page_Roles = Roles.Skip((curPage - 1) * pageSize).Take(pageSize).ToList<Role>();
                return page_Roles;
            }
            catch (Exception)
            {
                totalRecordNum = totalPageNum = 0;
                return null;
            }
        }
        #endregion

        #region 角色页面Action

        public ActionResult AddEditRole()
        {
            this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.Role obj = new Role();

                        obj.RoleName = Request.Form["RoleName"];
                        obj.Dept = Request.Form["Dept"];
                        obj.System = Request.Form["System"];
                        obj.Remark = Request.Form["Remark"];
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

                        obj = db.Role.Add(obj);
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
                    Role obj = db.Role.Where(u => u.IDX == idx).FirstOrDefault();

                    if (obj == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        obj.RoleName = Request.Form["RoleName"];
                        obj.Dept = Request.Form["Dept"];
                        obj.System = Request.Form["System"];
                        obj.Remark = Request.Form["Remark"];
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

        public ActionResult DeleteRole()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                Role obj = db.Role.FirstOrDefault(u => u.IDX == idx);
                if (obj != null)
                {
                    db.Role.Remove(obj);
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

        public ActionResult GetRoleData(bool _search, string sidx, string sord, int page, int rows)
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
                    List<Role> objs = db.Role.Select(u => u).OrderByDescending(u => u.IDX).ToList<Role>();
                    int totalRecordNum = objs.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<Role> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<Role>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult RoleManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            ViewBag.Services = TugBusinessLogic.Utils.GetServices();

            return View();
        }

        #endregion 角色页面Action

        #region 模块页面Action

        public ActionResult AddEditModule()
        {
            this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.FunctionModule obj = new FunctionModule();

                        obj.ModuleCode = Request.Form["ModuleCode"];
                        obj.ModuleName = Request.Form["ModuleName"];
                        obj.System = Request.Form["System"];
                        obj.Remark = Request.Form["Remark"];
                        //obj.OwnerID = -1;
                        //obj.CreateDate = obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        //obj.UserID = -1;
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

                        obj = db.FunctionModule.Add(obj);
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
                    FunctionModule obj = db.FunctionModule.Where(u => u.IDX == idx).FirstOrDefault();

                    if (obj == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        obj.ModuleCode = Request.Form["ModuleCode"];
                        obj.ModuleName = Request.Form["ModuleName"];
                        obj.System = Request.Form["System"];
                        obj.Remark = Request.Form["Remark"];
                        //obj.OwnerID = -1;
                        //obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        //obj.UserID = -1;

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

        public ActionResult DeleteModule()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                FunctionModule obj = db.FunctionModule.FirstOrDefault(u => u.IDX == idx);
                if (obj != null)
                {
                    db.FunctionModule.Remove(obj);
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
        public ActionResult GetModuleDataForOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            //string s = Request.QueryString[6];

            try
            {
                TugDataEntities db = new TugDataEntities();

                //db.Configuration.ProxyCreationEnabled = false;
                List<FunctionModule> FunctionModules = db.FunctionModule.Select(u => u).OrderByDescending(u => u.IDX).ToList<FunctionModule>();
                int totalRecordNum = FunctionModules.Count;
                if (totalRecordNum % rows == 0) page -= 1;
                int pageSize = rows;
                int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                //List<FunctionModule> page_FunctionModules = FunctionModules.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<FunctionModule>();

                var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = FunctionModules };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }
        public ActionResult GetModuleData(bool _search, string sidx, string sord, int page, int rows)
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
                    List<FunctionModule> objs = db.FunctionModule.Select(u => u).OrderByDescending(u => u.IDX).ToList<FunctionModule>();
                    int totalRecordNum = objs.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<FunctionModule> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<FunctionModule>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult ModuleManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            ViewBag.Services = TugBusinessLogic.Utils.GetServices();

            return View();
        }

        #endregion 模块页面Action

        #region 角色人页面Action

        public ActionResult AddEditRowUser(int rolId)
        {
            //this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.UsersRole obj = new UsersRole();

                        obj.UserID = Util.toint(Request.Form["UserID"]);
                        obj.RoleID = rolId;
                        obj.IsAdmin = Request.Form["IsAdmin"];
                        obj.System = "Role";
                        obj.CreateDate = obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");

                        obj.OwnerID = -1;
                        obj.AddUserID = -1;
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

                        obj = db.UsersRole.Add(obj);
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
                    UsersRole obj = db.UsersRole.Where(u => u.IDX == idx).FirstOrDefault();

                    if (obj == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        obj.UserID = Util.toint(Request.Form["UserID"]);
                        obj.RoleID = Util.toint(Request.Form["RoleID"]);
                        obj.IsAdmin = Request.Form["IsAdmin"];
                        obj.System = "Role";
                        obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");

                        obj.OwnerID = -1;
                        obj.AddUserID = -1;
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

        public ActionResult DeleteRoleUser()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                UsersRole obj = db.UsersRole.FirstOrDefault(u => u.IDX == idx);
                if (obj != null)
                {
                    db.UsersRole.Remove(obj);
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


        public ActionResult GetRowUsers(bool _search, string sidx, string sord, int page, int rows, int rolId)
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
                    List<V_RoleUser> objs = db.V_RoleUser.Where(u => u.RoleID == rolId).Select(u => u).OrderByDescending(u => u.UserID).ToList<V_RoleUser>();
                    int totalRecordNum = objs.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_RoleUser> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<V_RoleUser>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult RoleUserManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            int totalRecordNum, totalPageNum;
            List<Role> list = GetRoles(1, _DefaultPageSie, out totalRecordNum, out totalPageNum);
            ViewBag.TotalPageNum = totalPageNum;
            ViewBag.CurPage = 1;

            return View(list);
        }

        #endregion 角色人页面Action

        #region 角色模块Action
        public ActionResult DeleteRoleModule()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                RoleModule obj = db.RoleModule.FirstOrDefault(u => u.IDX == idx);
                if (obj != null)
                {
                    db.RoleModule.Remove(obj);
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
        public ActionResult AddEditRowModule(int rolId)
        {
            //this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.RoleModule obj = new RoleModule();
                        obj.RoleID = rolId;
                        obj.ModuleID = Util.toint(Request.Form["ModuleID"]);

                        obj.IsAdmin = Request.Form["IsAdmin"];
                        obj.System = "Role";
                        obj.CreateDate = obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");

                        obj.OwnerID = -1;
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

                        obj = db.RoleModule.Add(obj);
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
                    RoleModule obj = db.RoleModule.Where(u => u.IDX == idx).FirstOrDefault();

                    if (obj == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        obj.RoleID = rolId;
                        obj.ModuleID = Util.toint(Request.Form["ModuleID"]);

                        obj.IsAdmin = Request.Form["IsAdmin"];
                        obj.System = "Role";
                        obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");

                        obj.OwnerID = -1;
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
        public ActionResult GetRowModules(bool _search, string sidx, string sord, int page, int rows, int rolId)
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
                    List<V_RoleModule> objs = db.V_RoleModule.Where(u => u.RoleID == rolId).Select(u => u).OrderByDescending(u => u.ModuleID).ToList<V_RoleModule>();
                    int totalRecordNum = objs.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_RoleModule> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<V_RoleModule>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }
        public ActionResult RoleModuleManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            int totalRecordNum, totalPageNum;
            List<Role> list = GetRoles(1, _DefaultPageSie, out totalRecordNum, out totalPageNum);
            ViewBag.TotalPageNum = totalPageNum;
            ViewBag.CurPage = 1;

            return View(list);
        }

        #endregion 角色模块Action

        #region 角色菜单模块Action

        public ActionResult RoleMenuManage(string lan, int? id)
        {
            lan = this.Internationalization();
            ViewBag.Language = lan;

            int totalRecordNum, totalPageNum;
            List<Role> list = GetRoles(1, _DefaultPageSie, out totalRecordNum, out totalPageNum);
            ViewBag.TotalPageNum = totalPageNum;
            ViewBag.CurPage = 1;

            return View(list);
        }
        public ActionResult GetRowMenus(bool _search, string sidx, string sord, int page, int rows, int rolId)
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
                    List<V_RoleMenu> objs = db.V_RoleMenu.Where(u => u.RoleID == rolId).Select(u => u).OrderByDescending(u => u.MenuName).ToList<V_RoleMenu>();
                    int totalRecordNum = objs.Count;
                    if (page != 0 && totalRecordNum % rows == 0) page -= 1;
                    int pageSize = rows;
                    int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                    List<V_RoleMenu> page_objs = objs.Skip((page - 1) * rows).Take(rows).ToList<V_RoleMenu>();

                    var jsonData = new { page = page, records = totalRecordNum, total = totalPageNum, rows = page_objs };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        public ActionResult AddEditRowMenu(int rolId)
        {
            //this.Internationalization();

            #region Add

            if (Request.Form["oper"].Equals("add"))
            {
                try
                {
                    TugDataEntities db = new TugDataEntities();
                    {
                        TugDataModel.RoleMenu obj = new RoleMenu();

                        obj.Page = Request.Form["Page"];
                        obj.Menu = Request.Form["Menu"];
                        obj.MenuName =Request.Form["MenuName"];
                        obj.Visible = Request.Form["Visible"];
                        obj.IsAdmin = Request.Form["IsAdmin"];
                        obj.RoleID = rolId;

                        obj.Remark = Request.Form["Remark"];
                        obj.System = "Role";
                        //obj.CreateDate = obj.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd");
                        //obj.OwnerID = -1;
                        //obj.AddUserID = -1;
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

                        obj = db.RoleMenu.Add(obj);
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
                    RoleMenu obj = db.RoleMenu.Where(u => u.IDX == idx).FirstOrDefault();

                    if (obj == null)
                    {
                        return Json(new { code = Resources.Common.ERROR_CODE, message = Resources.Common.ERROR_MESSAGE });
                    }
                    else
                    {
                        obj.Page = Request.Form["Page"];
                        obj.Menu = Request.Form["Menu"];
                        obj.MenuName = Request.Form["MenuName"];
                        obj.Visible = Request.Form["Visible"];
                        obj.IsAdmin = Request.Form["IsAdmin"];
                        obj.RoleID = rolId;

                        obj.Remark = Request.Form["Remark"];
                        obj.System = "Role";
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

        public ActionResult DeleteRoleMenu()
        {
            this.Internationalization();

            try
            {
                var f = Request.Form;

                int idx = Convert.ToInt32(Request.Form["data[IDX]"]);

                TugDataEntities db = new TugDataEntities();
                RoleMenu obj = db.RoleMenu.FirstOrDefault(u => u.IDX == idx);
                if (obj != null)
                {
                    db.RoleMenu.Remove(obj);
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

        #endregion 角色菜单模块Action

        //
        // GET: /Permit/
        public ActionResult Index()
        {
            return View();
        }
    }
}