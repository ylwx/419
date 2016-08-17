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
    
    public partial class V_Invoice2
    {
        public int BillingID { get; set; }
        public string JobNo { get; set; }
        public string InvoiceType { get; set; }
        public string IsShowShipLengthRule { get; set; }
        public string IsShowShipTEUSRule { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> BillingTemplateID { get; set; }
        public string BillingTemplateName { get; set; }
        public string ShipLength { get; set; }
        public string ShipTEUS { get; set; }
        public string ExpiryDate { get; set; }
        public Nullable<int> BillingTypeID { get; set; }
        public string BillingTemplateTypeValue { get; set; }
        public string BillingTemplateTypeLabel { get; set; }
        public Nullable<int> TimeTypeID { get; set; }
        public string TimeTypeValue { get; set; }
        public string TimeTypeLabel { get; set; }
        public string BillingCode { get; set; }
        public string BillingName { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Remark { get; set; }
        public string Month { get; set; }
        public Nullable<int> TimesNo { get; set; }
        public string Status { get; set; }
        public Nullable<int> Phase { get; set; }
        public int OrderID { get; set; }
        public string HasInvoice { get; set; }
        public string HasInFlow { get; set; }
        public Nullable<int> OrderServiceID { get; set; }
        public Nullable<int> ServiceNatureID { get; set; }
        public string ServiceNatureValue { get; set; }
        public string ServiceNatureLabel { get; set; }
        public string ServiceWorkDate { get; set; }
        public string ServiceWorkTime { get; set; }
        public string EstimatedCompletionTime { get; set; }
        public string ServiceWorkPlace { get; set; }
        public Nullable<int> BigTugNum { get; set; }
        public Nullable<int> MiddleTugNum { get; set; }
        public Nullable<int> SmallTugNum { get; set; }
        public Nullable<int> ShipID { get; set; }
        public string ShipName { get; set; }
        public Nullable<int> DeadWeight { get; set; }
        public Nullable<int> Length { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> TEUS { get; set; }
        public int SchedulerID { get; set; }
        public Nullable<int> TugID { get; set; }
        public string TugName1 { get; set; }
        public string TugName2 { get; set; }
        public string SimpleName { get; set; }
        public string IsCaptainConfirm { get; set; }
        public string DepartBaseTime { get; set; }
        public string ArrivalBaseTime { get; set; }
        public string ServiceHours { get; set; }
        public string RopeUsed { get; set; }
        public Nullable<int> RopeNum { get; set; }
        public string SchedulerRemark { get; set; }
        public int BillingItemIDX { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string BillingItemValue { get; set; }
        public string BillingItemLabel { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public string Currency { get; set; }
        public string IsVisible { get; set; }
    }
}
