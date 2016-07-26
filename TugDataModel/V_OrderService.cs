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
    
    public partial class V_OrderService
    {
        public Nullable<int> OrderID { get; set; }
        public string HasInvoice { get; set; }
        public string OrderCode { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string OrdDate { get; set; }
        public Nullable<int> ShipID { get; set; }
        public string ShipName { get; set; }
        public Nullable<int> DeadWeight { get; set; }
        public Nullable<int> Length { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> TEUS { get; set; }
        public string Class { get; set; }
        public int OrderServiceID { get; set; }
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
        public Nullable<int> ServiceJobStateID { get; set; }
        public string ServiceJobStateValue { get; set; }
        public string ServiceJobStateLabel { get; set; }
        public string Remark { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public string LastUpDate { get; set; }
    }
}
