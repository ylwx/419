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
        //static public string MaxNumberGet(string msg)
        //{
        //    TugDataEntities db = new TugDataEntities();
            
        //    acc.BeginTransaction();
        //    try
        //    {
        //        return msg + (MaxOrderInforId() + 1).ToString("000000");
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
