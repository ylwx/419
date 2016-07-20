//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TugDataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_Invoice
    {
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> OrderID { get; set; }
        public string OrderCode { get; set; }
        public string OrderWorkDate { get; set; }
        public string OrderWorkTime { get; set; }
        public string OrderEstimatedCompletionTime { get; set; }
        public string HasInvoice { get; set; }
        public Nullable<int> ServiceNatureID { get; set; }
        public string ServiceNatureLabel { get; set; }
        public string ServiceWorkDate { get; set; }
        public string ServiceWorkPlace { get; set; }
        public Nullable<int> TugID { get; set; }
        public string TugName1 { get; set; }
        public string TugName2 { get; set; }
        public string TugSimpleName { get; set; }
        public string Power { get; set; }
        public string Class { get; set; }
        public string Speed { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public Nullable<int> JobStateID { get; set; }
        public string JobStateLabel { get; set; }
        public string InformCaptainTime { get; set; }
        public string CaptainConfirmTime { get; set; }
        public string DepartBaseTime { get; set; }
        public string ArrivalShipSideTime { get; set; }
        public string WorkCommencedTime { get; set; }
        public string WorkCompletedTime { get; set; }
        public string ArrivalBaseTime { get; set; }
        public string RopeUsed { get; set; }
        public Nullable<int> RopeNum { get; set; }
        public string OrderSchedulerRemark { get; set; }
        public Nullable<int> OrderSchedulerOwnerID { get; set; }
        public Nullable<int> OrderSchedulerUserID { get; set; }
        public string OrderSchedulerCreateDate { get; set; }
        public string OrderSchedulerLastUpDate { get; set; }
        public int BillingID { get; set; }
        public string JobNo { get; set; }
        public Nullable<int> BillingTemplateID { get; set; }
        public Nullable<int> BillingTypeID { get; set; }
        public string BillingTypeValue { get; set; }
        public string BillingTypeLabel { get; set; }
        public string BillingCode { get; set; }
        public string BillingName { get; set; }
        public Nullable<int> TimeTypeID { get; set; }
        public string TimeTypeValue { get; set; }
        public string TimeTypeLabel { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Amount { get; set; }
        public string BillingRemark { get; set; }
        public string Month { get; set; }
        public Nullable<int> TimesNo { get; set; }
        public string Status { get; set; }
        public Nullable<int> Phase { get; set; }
        public Nullable<int> BillingOwnerID { get; set; }
        public string BillingCreateDate { get; set; }
        public Nullable<int> BillingUserID { get; set; }
        public string BillingLastUpDate { get; set; }
        public Nullable<int> SchedulerID { get; set; }
        public int BillingItemIDX { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string BillingItemValue { get; set; }
        public string BillingItemLabel { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public string Currency { get; set; }
        public Nullable<int> PositionTypeID { get; set; }
    }
}
