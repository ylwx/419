﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TugBusinessLogic.Module;
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
        static private int MaxBillCode()
        {
            int maxCode = 0;
            int tmpCodeNo = 0;
            try
            {
                TugDataEntities db = new TugDataEntities();
                System.Linq.Expressions.Expression<Func<Billing, bool>> exp = u => u.BillingCode.Substring(7,4) == DateTime.Now.Year.ToString();
                List<Billing> BillingList = db.Billing.Where(exp).OrderByDescending(u => u.BillingCode).ToList<Billing>();
                    if (BillingList.Count == 0)
                    {
                        maxCode = 0;
                        return maxCode;
                    }
                    if (BillingList.Count == 1)
                    {
                        maxCode = Convert.ToInt32(BillingList[0].BillingCode.Substring(2,4));
                        return maxCode;
                    }
                    for (int i = 0; i < BillingList.Count - 1; i++)
                    {
                        int codeNo = Convert.ToInt32(BillingList[i].BillingCode.Substring(2, 4));
                        int codeNoNext = Convert.ToInt32(BillingList[i + 1].BillingCode.Substring(2, 4));
                        if (i == 0)
                        {
                            if (codeNo > codeNoNext)
                            {
                                tmpCodeNo = codeNo;
                            }
                            else
                            {
                                tmpCodeNo = codeNoNext;
                            }
                        }
                        else
                        {
                            if (tmpCodeNo < codeNoNext)
                            {
                                tmpCodeNo = codeNoNext;
                            }
                        }

                    }
                maxCode = tmpCodeNo;
            }
            catch (Exception ex)
            {

            }
            return maxCode;

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
                        ret = msg + "T-" + (MaxBillCode() + 1).ToString("0000") + "/" + DateTime.Now.Year.ToString();
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
        static public string AutoGenerateDiscountBillCode(string msg = "")
        {
            string ret = "";
            using (TugDataEntities db = new TugDataEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ret = msg + "C-" + (MaxBillCode() + 1).ToString("0000") + "/" + DateTime.Now.Year.ToString();
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

            if (iEndHour < iStartHour) {
                iEndHour += 24;
                //不够减，要向小时借
                if (iEndMinute < iStartMinute)
                {
                    iDiffMinute = iEndMinute + 60 - iStartMinute;
                    iDiffHour = iEndHour - 1 - iStartHour;
                    return true;
                }
                if (iEndMinute >= iStartMinute)
                {
                    iDiffMinute = iEndMinute - iStartMinute;
                    iDiffHour = iEndHour - iStartHour;
                    return true;
                }

                return true; 
            }
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


        static public double CalculateTimeConsumption2(int hour, int minute, int timeTypeId, string timeTypeValue, string timeTypeLabel)
        {
            int totalMinutes = hour * 60 + minute;

            #region 一刻钟
            if (timeTypeId == 8 || timeTypeValue == "0" || timeTypeLabel == "15分钟")
            {
                if (totalMinutes <= 60) { return 1; }

                int count = 0;
                count += hour * 60 / 15;
                count += minute / 15;
                if (minute % 15 > 0)
                {
                    count++;
                }

                return (count * 15.0) / 60;
            }
            #endregion

            #region 半小时
            if (timeTypeId == 9 || timeTypeValue == "1" || timeTypeLabel == "半小时")
            {
                if (totalMinutes <= 60) { return 1; }

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
                if (totalMinutes <= 60) { return 1; }

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
                if (totalMinutes <= 65) { return 1; }

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
                if (totalMinutes <= 65) { return 1; }

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
                if (totalMinutes <= 65) { return 1; }

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


        #region 解析计费方案中的船长和箱量值
        public static bool GetShipLengthLessValue(string strShipLength, out int lessValue)
        {
            string c = "<=";
            int index = -1;
            lessValue = Int32.MinValue;
            index = strShipLength.Trim().IndexOf(c);
            if (index >= 0)
            {
                string subString = strShipLength.Trim().Substring(index + c.Length, strShipLength.Trim().Length - index - c.Length);

                string strInt = "";
                for (int i = 0; i < subString.Length; i++)
                {
                    if (Char.IsDigit(subString[i]))
                    {
                        strInt += subString[i];
                    }
                    else
                    {
                        break;
                    }
                }

                lessValue = Module.Util.toint(strInt);
                return true;
            }
            return false;
        }
        public static bool GetShipLengthGreaterValue(string strShipLength, out int greaterValue)
        {
            string c = ">=";
            int index = -1;
            greaterValue = Int32.MaxValue;
            index = strShipLength.Trim().IndexOf(c);
            if (index >= 0)
            {
                string subString = strShipLength.Trim().Substring(index + c.Length, strShipLength.Trim().Length - index - c.Length);

                string strInt = "";
                for (int i = 0; i < subString.Length; i++)
                {
                    if (Char.IsDigit(subString[i]))
                    {
                        strInt += subString[i];
                    }
                    else
                    {
                        break;
                    }
                }

                greaterValue = Module.Util.toint(strInt);
                return true;
            }
            return false;
        }
        public static bool GetShipLengthBetweenValue(string strShipLength, out int beginValue, out int endValue)
        {
            string c = "-";
            int index = -1;
            beginValue = Int32.MaxValue;
            endValue = Int32.MinValue;
            index = strShipLength.Trim().IndexOf(c);

            if (index >= 0)
            {
                string subBeginString = strShipLength.Trim().Substring(0, index);
                string subEndString = strShipLength.Trim().Substring(index + c.Length, strShipLength.Trim().Length - index - c.Length);

                string strBeginInt = "";
                for (int i = 0; i < subBeginString.Length; i++)
                {
                    if (Char.IsDigit(subBeginString[i]))
                    {
                        strBeginInt += subBeginString[i];
                    }
                    else
                    {
                        break;
                    }
                }

                beginValue = Module.Util.toint(strBeginInt);


                string strEndInt = "";
                for (int i = 0; i < subEndString.Length; i++)
                {
                    if (Char.IsDigit(subEndString[i]))
                    {
                        strEndInt += subEndString[i];
                    }
                    else
                    {
                        break;
                    }
                }

                endValue = Module.Util.toint(strEndInt);
                return true;
            }
            return false;
        }


        public static bool GetShipTEUSLessValue(string strShipTEUS, out int lessValue)
        {
            string c = "<=";
            int index = -1;
            lessValue = Int32.MinValue;
            index = strShipTEUS.Trim().IndexOf(c);
            if (index >= 0)
            {
                string subString = strShipTEUS.Trim().Substring(index + c.Length, strShipTEUS.Trim().Length - index - c.Length);

                string strInt = "";
                for (int i = 0; i < subString.Length; i++)
                {
                    if (Char.IsDigit(subString[i]))
                    {
                        strInt += subString[i];
                    }
                    else
                    {
                        break;
                    }
                }

                lessValue = Module.Util.toint(strInt);
                return true;

            }

            return false;
        }
        public static bool GetShipTEUSGreaterValue(string strShipTEUS, out int greaterValue)
        {
            string c = ">=";
            int index = -1;
            greaterValue = Int32.MaxValue;
            index = strShipTEUS.Trim().IndexOf(c);
            if (index >= 0)
            {
                string subString = strShipTEUS.Trim().Substring(index + c.Length, strShipTEUS.Trim().Length - index - c.Length);

                string strInt = "";
                for (int i = 0; i < subString.Length; i++)
                {
                    if (Char.IsDigit(subString[i]))
                    {
                        strInt += subString[i];
                    }
                    else
                    {
                        break;
                    }
                }

                greaterValue = Module.Util.toint(strInt);
                return true;
            }

            return false;
        }
        public static bool GetShipTEUSBetweenValue(string strShipTEUS, out int beginValue, out int endValue)
        {
            string c = "-";
            int index = -1;
            beginValue = Int32.MaxValue;
            endValue = Int32.MinValue;
            index = strShipTEUS.Trim().IndexOf(c);

            if (index >= 0)
            {
                string subBeginString = strShipTEUS.Trim().Substring(0, index);
                string subEndString = strShipTEUS.Trim().Substring(index + c.Length, strShipTEUS.Trim().Length - index - c.Length);

                string strBeginInt = "";
                for (int i = 0; i < subBeginString.Length; i++)
                {
                    if (Char.IsDigit(subBeginString[i]))
                    {
                        strBeginInt += subBeginString[i];
                    }
                    else
                    {
                        break;
                    }
                }

                beginValue = Module.Util.toint(strBeginInt);


                string strEndInt = "";
                for (int i = 0; i < subEndString.Length; i++)
                {
                    if (Char.IsDigit(subEndString[i]))
                    {
                        strEndInt += subEndString[i];
                    }
                    else
                    {
                        break;
                    }
                }

                endValue = Module.Util.toint(strEndInt);

                return true;
            }

            return false;
        }

        #endregion




        #region 拖轮名称1更改之后，服务表里面拖轮名称的冗余字段更新

        public static void UpDateTugName1(int tugIDX, string oldTugName1, string newTugName1)
        {
            TugDataEntities db = new TugDataEntities();

            try
            {
                var list = db.OrderService.Where(u => u.UserDefinedCol9.Contains(tugIDX.ToString())).Select(u => u).ToList();

                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item.UserDefinedCol10 != null)
                        {
                            string update = "";

                            var tugNames = item.UserDefinedCol10.Split(',').ToList();
                            if (tugNames != null && tugNames.Count > 0)
                            {
                                foreach (var item2 in tugNames)
                                {
                                    if (item2 == oldTugName1)
                                    {
                                        update += newTugName1 + ",";
                                    }
                                    else
                                    {
                                        update += item2 + ",";
                                    }
                                }

                                if (update.Length > 0)
                                {
                                    update = update.Substring(0, update.Length - 1);
                                }
                            }

                            item.UserDefinedCol10 = update;

                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion


        #region 全包数据插入汇总表
        public static void QBInsertToAmount()
        {
            TugDataEntities db = new TugDataEntities();


            //1.先找出全部的全包类型账单
            List<Billing> qbList = db.Billing.Where(u => u.BillingTypeID == 6).ToList();
            //List<Billing> qbList = db.Billing.Where(u => u.IDX == 1180).ToList();
            if (qbList != null)
            {
                foreach (var item in qbList)
                {
                    List<V_Invoice2> schList = db.V_Invoice2.Where(u => u.BillingID == item.IDX).OrderBy(u => u.SchedulerID).ToList();
                    if (schList != null)
                    {
                        foreach (var sch in schList)
                        {
                            AmountSum objAs = new AmountSum();
                            objAs.CustomerID = sch.CustomerID;
                            objAs.CustomerShipID = sch.ShipID;
                            objAs.BillingID = sch.BillingID;
                            objAs.BillingDateTime = Convert.ToDateTime(sch.CreateDate);
                            objAs.SchedulerID = sch.SchedulerID;
                            objAs.Amount = sch.UnitPrice;
                            objAs.FuelAmount = 0;
                            objAs.Currency = sch.Currency;

                            int iDiffHour, iDiffMinute;
                            TugBusinessLogic.Utils.CalculateTimeDiff(sch.DepartBaseTime, sch.ArrivalBaseTime, out iDiffHour, out iDiffMinute);
                            objAs.Hours = TugBusinessLogic.Utils.CalculateTimeConsumption2(iDiffHour, iDiffMinute,
                                (int)sch.TimeTypeID, sch.TimeTypeValue, sch.TimeTypeLabel);
                            
                             
                            objAs.Year = sch.Month.Split('-')[0];
                            objAs.Month =sch.Month.Split('-')[1];
                            objAs.OwnerID = -1;
                            objAs.UserID = sch.UserID;
                            objAs.CreateDate = objAs.LastUpDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            objAs.UserDefinedCol6 = -1;

                            db.AmountSum.Add(objAs);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
        #endregion

        #region 比对Billing表中的Amount字段的值是否正确
        public static void Billing_Amount_Value_HasError()
        {
            DataTable dtContenData;
            DataTable dtGrandTotal;
            TugDataEntities db = new TugDataEntities();


            //1.先找出全部的全包类型账单
            List<V_Billing_TimeTypeValue> qbList = db.V_Billing_TimeTypeValue.ToList();
            //List<Billing> qbList = db.Billing.Where(u => u.IDX == 1180).ToList();
            if (qbList != null)
            {
                int i = 0;
                foreach (var item in qbList)
                {
                    SqlParameter para1 = new SqlParameter()
                    {
                        ParameterName = "@BillingID",
                        Direction = ParameterDirection.Input,
                        Value = item.IDX ,
                        DbType = DbType.Int16
                    };
                    SqlParameter para2 = new SqlParameter()
                    {
                        ParameterName = "@TimeTypeValue",
                        Direction = ParameterDirection.Input,
                        Value = item.TimeTypeValue ,
                        DbType = DbType.Int16
                    };
                    SqlParameter[] param = new SqlParameter[] { para1, para2 };
                    string proc = "proc_inv_item_xy";
                    if (item.BillingTypeID == 8) proc = "proc_inv_item";

                    dtContenData = SqlHelper.GetDatatableBySP(proc, param);
                    dtGrandTotal = TugBusinessLogic.Utils.TableToChildTB(dtContenData, "ItemCode = 'T2'");
                    double  ndiscount = Util.tonumeric(item.Discount);
                    double amount = Util.tonumeric(dtGrandTotal.Rows[0]["Value"]);
                    double  GrandTotal = Util.tonumeric(dtGrandTotal.Rows[0]["Value"]) - ndiscount * Util.tonumeric(dtGrandTotal.Rows[0]["Value"]) / 100;
                    if (item.Amount != amount)
                    {
                        i = i + 1;
                        string aa = item.IDX + "," + item.BillingCode;
                        Console.WriteLine(aa);
                    }

                }
                Console.WriteLine("ok");
            }
        }
        #endregion
    }
}