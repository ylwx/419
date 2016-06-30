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
        /// 计算时间差
        /// </summary>
        /// <param name="startTime">开始时间 格式：19:33</param>
        /// <param name="endTime">结束时间 格式：21:21</param>
        /// <param name="iDiffHour">相差的小时数</param>
        /// <param name="iDiffMinute">相差的分钟数</param>
        /// <returns>成功：true， 失败：false</returns>
        static public bool CalculateTimeDiff(string startTime, string endTime, out int iDiffHour, out int iDiffMinute)
        {
            iDiffHour = 0;
            iDiffMinute = 0;

            string strStartHour = startTime.Split(':')[0];
            string strStartMinute = startTime.Split(':')[1];
            int iStartHour = Convert.ToInt32(strStartHour);
            int iStartMinute = Convert.ToInt32(strStartMinute);

            string strEndHour = endTime.Split(':')[0];
            string strEndMinute = endTime.Split(':')[1];
            int iEndHour = Convert.ToInt32(strEndHour);
            int iEndMinute = Convert.ToInt32(strEndMinute);

            if (iEndHour < iStartHour) return false;
            if (iEndHour == iStartHour)
            {
                if (iEndMinute < iStartMinute) return false;
                if (iEndMinute == iStartMinute) return true;
                if (iEndMinute > iStartMinute)
                {
                    iDiffMinute = iEndMinute - iStartMinute;
                    return true;
                }
            }
            if (iEndHour > iStartHour)
            {
                //不够减，要向小时借
                if (iEndMinute < iStartMinute)
                {
                    iDiffMinute = iEndMinute + 60 - iStartMinute;
                    iDiffHour = iEndHour - 1 - iStartHour;
                    return true;
                }
                if(iEndMinute >= iStartMinute)
                {
                    iDiffMinute = iEndMinute - iStartMinute;
                    iDiffHour = iEndHour - iStartHour;
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// 根据不同的计时方式，计算实际消耗时间
        /// </summary>
        /// <param name="hour">时间差的小时</param>
        /// <param name="minute">时间差的分钟</param>
        /// <param name="timeTypeId">计时方式id，参见CustomField表里面的IDX（CustomName：BillingTemplate.TimeTypeID）</param>
        /// <param name="timeTypeValue">计时方式value，参见CustomField表里面的CustomrValue（CustomName：BillingTemplate.TimeTypeID）</param>
        /// <param name="timeTypeLabel">计时方式label，参见CustomField表里面的CustomrLabel（CustomName：BillingTemplate.TimeTypeID）</param>
        /// <returns>消耗的小时数</returns>
        static public double CalculateTimeConsumption(int hour, int minute, int timeTypeId, string timeTypeValue, string timeTypeLabel)
        {
            #region 一刻钟
            if (timeTypeId == 8 || timeTypeValue == "0" || timeTypeLabel == "一刻钟")
            {
                int count = 0;
                count += hour * 60 / 15 ;
                count += minute / 15;
                if (minute % 15 > 0) {
                    count++;
                }

                return (count * 15.0) / 60;
            }
            #endregion

            #region 半小时
            if (timeTypeId == 9 || timeTypeValue == "1" || timeTypeLabel == "半小时") 
            {
                int count = 0;
                count += hour * 60 / 30;
                count += minute / 30;
                if (minute % 30 > 0)
                {
                    count++;
                }

                return (count * 30.0) / 60;
            }
            #endregion

            #region 一小时
            if (timeTypeId == 10 || timeTypeValue == "2" || timeTypeLabel == "一小时") 
            {
                int count = 0;
                count += hour * 60 / 60;
                count += minute / 60;
                if (minute % 60 > 0)
                {
                    count++;
                }

                return (count * 60.0) / 60;
            }
            #endregion

            #region 一刻钟/5min
            if (timeTypeId == 11 || timeTypeValue == "3" || timeTypeLabel == "一刻钟/5min") 
            {
                int count = 0;

                if (minute >= 5)
                {
                    minute -= 5;
                }
                else 
                {
                    if (hour > 0) {
                        hour -= 1;
                        minute += 60;
                    }
                    else
                    {
                        hour = 0;
                    }
                }
                    
                count += hour * 60 / 15;

                count += minute / 15;
                if (minute % 15 > 0)
                {
                    count++;
                }

                return (count * 15.0) / 60;
            }
            #endregion

            #region 半小时/5min
            if (timeTypeId == 12 || timeTypeValue == "4" || timeTypeLabel == "半小时/5min") 
            {
                int count = 0;

                if (minute >= 5)
                {
                    minute -= 5;
                }
                else
                {
                    if (hour > 0)
                    {
                        hour -= 1;
                        minute += 60;
                    }
                    else
                    {
                        hour = 0;
                    }
                }


                count += hour * 60 / 30;
                count += minute / 30;
                if (minute % 30 > 0)
                {
                    count++;
                }

                return (count * 30.0) / 60;
            }
            #endregion

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