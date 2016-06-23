using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TugDataModel;

namespace TugBusinessLogic
{
    public class Utils
    {
        static private int MaxOrderInforId()
        {
            TugDataEntities db = new TugDataEntities();

            List<int> list = db.OrderInfor.OrderByDescending(u => u.IDX).Select(u => u.IDX).ToList();

            if (list != null && list.Count > 0)
                if (list != null)
                    return Int32.Parse(list[0].ToString()); ;
            return 0;
        }

        /// <summary>
        /// 获取最大活动编号
        /// </summary>
        /// <returns></returns>
        static public string GetOrderSequenceNo(string msg = "")
        {
            string ret = "";
            using (TugDataEntities db = new TugDataEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ret = msg + "T" + (MaxOrderInforId() + 1).ToString("00000") + "/" + DateTime.Now.Year.ToString();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

            return ret;
        }

        static public Dictionary<string, string> ResolveServices(string content)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            List<string> lst = content.Split('/').ToList();
            string s_ids = "", s_values = "", s_labels = "";
            if (lst != null && lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    s_ids += lst[i].Split('~')[0] + "/";
                    s_values += lst[i].Split('~')[1] + "/";
                    s_labels += lst[i].Split('~')[2] + "/";
                }

                if (lst.Count > 0)
                {
                    s_ids = s_ids.Substring(0, s_ids.Length - 1);
                    s_values = s_values.Substring(0, s_values.Length - 1);
                    s_labels = s_labels.Substring(0, s_labels.Length - 1);
                }
            }

            dic.Add("ids", s_ids);
            dic.Add("values", s_values);
            dic.Add("labels", s_labels);

            return dic;
        }


        /// <summary>
        /// 获得拖轮的服务项
        /// </summary>
        /// <returns></returns>
        static public List<CustomField> GetServices()
        {
            List<CustomField> list = new List<CustomField>();
            TugDataEntities db = new TugDataEntities();
            list = db.CustomField.Where(u => u.CustomName == "OrderInfor.ServiceNatureID").OrderBy(u => u.CustomValue).ToList<CustomField>();
            return list;
        }
        
        static public List<CustomField> GetJobStates()
        {
            List<CustomField> list = new List<CustomField>();
            TugDataEntities db = new TugDataEntities();
            list = db.CustomField.Where(u => u.CustomName == "Scheduler.JobStateID").OrderBy(u => u.CustomValue).ToList<CustomField>();
            return list;
        }

        /// <summary>
        /// 获取自定义字段
        /// </summary>
        /// <param name="CustomName">自定义字段的名称CustomField表里面的CustomName字段的名字</param>
        /// <returns>返回的格式Value：IDX-CustomValue-CustomLabel， Lable：CustomLabel</returns>
        static public string GetCustomField(string CustomName)
        {
            string s = string.Empty;

            try
            {
                TugDataEntities db = new TugDataEntities();
                List<CustomField> list = db.CustomField.Where(u => u.CustomName == CustomName).OrderBy(u => u.CustomValue).ToList<CustomField>();

                if (list != null && list.Count > 0)
                {
                    s += "<select><option value=-1~-1~请选择>请选择</option>";
                    foreach (CustomField item in list)
                    {
                        s += string.Format("<option value={0}>{1}</option>", item.IDX + "~" + item.CustomValue + "~" + item.CustomLabel, item.CustomLabel);
                    }
                    s += "</select>";
                }
            }
            catch (Exception ex)
            {
            }
            return s;
        }
    }
}