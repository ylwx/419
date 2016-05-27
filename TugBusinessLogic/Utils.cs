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
                        ret = msg + (MaxOrderInforId() + 1).ToString("000000");

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
    }
}
