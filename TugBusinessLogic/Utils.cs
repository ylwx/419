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
                    return Int32.Parse(list[0].ToString());;
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

        static public Dictionary<string, string> GetServices(string content)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            List<string> lst = content.Split('/').ToList();
            string s_values = "/", s_labels = "/";
            if (lst != null && lst.Count > 2)
            {
                for (int i = 1; i < lst.Count - 1; i++)
                {
                    s_values += lst[i].Split(':')[0] + "/";
                    s_labels += lst[i].Split(':')[1] + "/";
                }
            }

            dic.Add("values", s_values);
            dic.Add("labels", s_labels);

            return dic;
        }
    }
}
