﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TugDataModel;

namespace TugBusinessLogic
{
    public class Utils
    {
        static private int MaxOrderInforId()
        {
            int maxId = 0;
            try 
            {
                TugDataEntities db = new TugDataEntities();
                maxId = db.OrderInfor.Max(u => u.IDX);
            }
            catch(Exception)
            {

            }
            return maxId;
        }

        static private int MaxBillId()
        {
            int maxId = 0;
            try
            {
                TugDataEntities db = new TugDataEntities();
                maxId = db.Billing.Max(u => u.IDX);
            }
            catch (Exception ex)
            {

            }
            return maxId;
        }

        static public string AutoGenerateOrderSequenceNo(string msg = "")
        {
            string ret = "";
            using (TugDataEntities db = new TugDataEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        DateTime now = DateTime.Today;
                        ret = now.Year.ToString("0000") + now.Month.ToString("00") + now.Day.ToString("00") + (MaxOrderInforId() + 1).ToString("00000");

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

        /// <summary>
        /// 获取最大活动编号
        /// </summary>
        /// <returns></returns>
        static public string AutoGenerateBillCode(string msg = "")
        {
            string ret = "";
            using (TugDataEntities db = new TugDataEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ret = msg + "T" + (MaxBillId() + 1).ToString("00000") + "/" + DateTime.Now.Year.ToString();

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
            if (timeTypeId == 8 || timeTypeValue == "0" || timeTypeLabel == "15分钟")
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
            if (timeTypeId == 11 || timeTypeValue == "3" || timeTypeLabel == "15分钟/5min") 
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
                        minute += 60 - 5;
                    }
                    else
                    {
                        hour = 0;
                        minute = 0;
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
                        minute += 60 - 5;
                    }
                    else
                    {
                        hour = 0;
                        minute = 0;
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

            #region 一小时/5min
            if (timeTypeId == 42 || timeTypeValue == "5" || timeTypeLabel == "一小时/5min")
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
                        minute += 60 - 5;
                    }
                    else
                    {
                        hour = 0;
                        minute = 0;
                    }
                }


                count += hour * 60 / 60;
                count += minute / 60;
                if (minute % 60 > 0)
                {
                    count++;
                }

                return (count * 60.0) / 60;
            }
            #endregion

            return 0;
        }


        /// <summary>
        /// 根据模板ID和ItemID获取价格
        /// </summary>
        /// <param name="billingTemplateId"></param>
        /// <param name="billingTemplateItemId"></param>
        /// <returns></returns>
        static public double GetServicePrice(int billingTemplateId, int billingTemplateItemId)
        {
            TugDataModel.TugDataEntities db = new TugDataEntities();
            double price = (double)db.BillingItemTemplate.Where(u => u.BillingTemplateID == billingTemplateId && u.ItemID == billingTemplateItemId)
                .Select(u => u.UnitPrice).FirstOrDefault();
            return price;
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


        static public List<CustomField> GetCustomField2(string CustomName)
        {
            TugDataEntities db = new TugDataEntities();
            List<CustomField> list = db.CustomField.Where(u => u.CustomName == CustomName).OrderBy(u => u.CustomValue).ToList<CustomField>();
            return list;
        }




        public static DateTime HKDateTimeToDateTime(string hkDatetime)
        {
            IFormatProvider cultureF = new System.Globalization.CultureInfo("fr-FR", true);
            DateTime dt2 = DateTime.Parse(hkDatetime, cultureF, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            return dt2;
        }

        public static DateTime CNDateTimeToDateTime(string cnDatetime)
        {
            IFormatProvider cultureF = new System.Globalization.CultureInfo("zh-CN", true);
            DateTime dt2 = DateTime.Parse(cnDatetime, cultureF, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            return dt2;
        }

        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
        //父表的select结果集，返回datatable
        public static DataTable TableToChildTB(DataTable dt, string condition,string sort="")
        {
            DataTable newdt = new DataTable();
            newdt = dt.Clone(); // 克隆dt 的结构，包括所有 dt 架构和约束,并无数据；
            DataRow[] rows = dt.Select(condition,sort); // 从dt 中查询符合条件的记录；
            foreach (DataRow row in rows)  // 将查询的结果添加到dt中；
            {
                newdt.Rows.Add(row.ItemArray);
            }
            return newdt;
        }

    }
}