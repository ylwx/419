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
        static public int MaxOrderInforId()
        {

            TugDataEntities db = new TugDataEntities();

            List<int> list = db.OrderInfor.OrderByDescending(u => u.ID).Select(u => u.ID).ToList();

            if (list != null && list.Count > 0)
                if (list != null)
                    return Int32.Parse(list[0].ToString());;
            return 0;

        }
        /// <summary>
        /// 获取最大活动编号
        /// </summary>
        /// <returns></returns>
        //public string MaxNumberGet(string msg)
        //{

        //    acc.BeginTransaction();
        //    try
        //    {
        //        return msg + (MaxId + 1).ToString("000000");
        //    }
        //    catch
        //    {
        //        acc.RollBack();
        //        throw;
        //    }
        //    finally
        //    {
        //        acc.CloseSession();
        //    }
        //}
    }
}
