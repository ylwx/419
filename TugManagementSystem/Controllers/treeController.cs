using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugDataModel;

namespace TugManagementSystem.Controllers
{
    public class treeController : BaseController
    {
        public ActionResult GetDataForLoadOnce(bool _search, string sidx, string sord, int page, int rows)
        {
            this.Internationalization();

            //string s = Request.QueryString[6];

            try
            {
                //TugDataEntities db = new TugDataEntities();
                //List<BaseTreeItems> trees = db.BaseTreeItems.Select(u => u).OrderByDescending(u => u.IDX).ToList<BaseTreeItems>();
                //int totalRecordNum = trees.Count;
                //if (totalRecordNum % rows == 0) page -= 1;
                //int pageSize = rows;
                //int totalPageNum = (int)Math.Ceiling((double)totalRecordNum / pageSize);

                ////List<TugInfor> page_trees = trees.Skip((page - 1) * rows).Take(rows).OrderBy(u => u.IDX).ToList<TugInfor>();

                List<object> source = new List<object>();
                source.Add(new { emp_id = "10", name = "中国", fatherid = System.DBNull.Value, level = 0, isLeaf = "false", loaded = true, expanded = true });
                source.Add(new { emp_id = "11", name = "上海", fatherid = "10", level = 1, isLeaf = "false", loaded = true, expanded = true });
                source.Add(new { emp_id = "12", name = "浦东", fatherid = "11", level = 2, isLeaf = "true", loaded = true, expanded = true });
                source.Add(new { emp_id = "13", name = "徐汇", fatherid = "11", level = 2, isLeaf = "true", loaded = true, expanded = true });
                source.Add(new { emp_id = "14", name = "北京", fatherid = "10", level = 1, isLeaf = "false", loaded = true, expanded = true });
                source.Add(new { emp_id = "15", name = "海淀", fatherid = "14", level = 2, isLeaf = "true", loaded = true, expanded = true });
                source.Add(new { emp_id = "16", name = "通州", fatherid = "14", level = 2, isLeaf = "true", loaded = true, expanded = true });

                List<object> list = new List<object>();

                list.Add(source[0]);
                list.Add(source[1]);
                list.Add(source[2]);
                list.Add(source[3]);
                list.Add(source[4]);
                list.Add(source[5]);
                list.Add(source[6]);
                //var jsonData = new { list = list };
                var jsonData = new { page = 1, records = 10, total = 6, rows = list };

                return Json(jsonData, JsonRequestBehavior.AllowGet);

                //       var aa={
                //{"emp_id":"13","name":"Donna","salary":"800.00","boss_id":"12","level":2,"isLeaf":"true","loaded":"true","expanded":"true"},
                //{"emp_id":"14","name":"Eddie","salary":"700.00","boss_id":"12","level":2,"isLeaf":"true","loaded":"true","expanded":"true"},
                //{"emp_id":"15","name":"Fred","salary":"600.00","boss_id":"12","level":2,"isLeaf":"true","loaded":"true","expanded":"true"}
                //    };
            }
            catch (Exception)
            {
                return Json(new { code = Resources.Common.EXCEPTION_CODE, message = Resources.Common.EXCEPTION_MESSAGE });
            }
        }

        //
        // GET: /tree/
        public ActionResult Index()
        {
            return View();
        }
    }
}