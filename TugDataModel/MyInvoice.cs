using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugDataModel
{
    public class MyBillingItem
    {
        public int IDX { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string ItemValue { get; set; }
        public string ItemLabel { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<double> Price { get; set; }
        public string Currency { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string TypeValue { get; set; }
        public string TypeLabel { get; set; }
    }

    public class MyScheduler
    {
        public int SchedulerID { get; set; }
        public int TugID { get; set; }
        public string TugCnName { get; set; }
        public string TugEnName { get; set; }
        public string TugSimpleName { get; set; }

        public string TugPower { get; set; }

        public string InformCaptainTime { get; set; }
        public string CaptainConfirmTime { get; set; }
        public string DepartBaseTime { get; set; }
        public string ArrivalShipSideTime { get; set; }
        public string WorkCommencedTime { get; set; }
        public string WorkCompletedTime { get; set; }
        public string ArrivalBaseTime { get; set; }

        /// <summary>
        /// 工作时间：ArrivalBaseTime - DepartBaseTime
        /// </summary>
        public string WorkTime { get; set; }
        /// <summary>
        /// 按照计时方式换算后的实际消耗时间
        /// </summary>
        public double WorkTimeConsumption { get; set; }


        /// <summary>
        /// 单价
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// 根据计费类型不同，显示的价格也不同。比如全包：协议收费；条款：按时间收费（WorkTimeConsumption * UnitPrice）
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 合计港币
        /// </summary>
        public double SubTotaHKS { get; set; }

        /// <summary>
        /// 折扣后合计港币
        /// </summary>
        public double DiscountSubTotalHKS { get; set; }

        public string RopeUsed { get; set; }
        public int RopeNum { get; set; }
        public string Remark { get; set; }

        public List<MyBillingItem> BillingItems { get; set; }

        /// <summary>
        /// 总计港币
        /// </summary>
        public double TotalHKs { get; set; }

    }

    public class MyService
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string ServiceWorkDate { get; set; }

        public string ServiceWorkPlace { get; set; }
    }
    public class MyInvoice
    {
        public int CustormerID { get; set; }
        public string CustomerName { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 订单流水号
        /// </summary>
        public string OrderCode { get; set; }


        /// <summary>
        /// 服务内容  key:value = 服务ID : 服务名称
        /// </summary>
        public Dictionary<int, MyService> ServiceNature { get; set; }

        /// <summary>
        /// 账单ID
        /// </summary>
        public int BillingID { get; set; }

        /// <summary>
        /// 账单流水号
        /// </summary>
        public string BillingCode { get; set; }

        /// <summary>
        /// 计费类型ID
        /// </summary>
        public int BillingTypeID { get; set; }

        /// <summary>
        /// 计费类型值
        /// </summary>
        public string BillingTypeValue { get; set; }

        /// <summary>
        /// 计费类型名称
        /// </summary>
        public string BillingTypeLabel { get; set; }

        /// <summary>
        /// 计时方式ID
        /// </summary>
        public int TimeTypeID { get; set; }

        /// <summary>
        /// 计时方式Value
        /// </summary>
        public string TimeTypeValue { get; set; }

        /// <summary>
        /// 计时方式名称
        /// </summary>
        public string TimeTypeLabel { get; set; }

        
        /// <summary>
        /// 折扣系数
        /// </summary>
        public double Discount { get; set; }
        /// <summary>
        /// 多个调度， key:value = 服务ID:调度对象
        /// </summary>
        public Dictionary<int, List<MyScheduler>> Schedulers { get; set; }

        /// <summary>
        /// 共计港币
        /// </summary>
        public double GrandTotalHKS { get; set; }
    }
}
