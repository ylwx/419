<<<<<<< HEAD
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
    
    public partial class OrderService
    {
        public OrderService()
        {
            this.Scheduler = new HashSet<Scheduler>();
        }
    
        public int IDX { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> BillingType { get; set; }
        public Nullable<int> ServiceNatureID { get; set; }
        public string ServiceWorkDate { get; set; }
        public string ServiceWorkTime { get; set; }
        public string EstimatedCompletionTime { get; set; }
        public string ServiceWorkPlace { get; set; }
        public Nullable<int> BigTugNum { get; set; }
        public Nullable<int> MiddleTugNum { get; set; }
        public Nullable<int> SmallTugNum { get; set; }
        public Nullable<int> JobStateID { get; set; }
        public string Remark { get; set; }
        public string HasBilling { get; set; }
        public string HasBillingInFlow { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public string LastUpDate { get; set; }
        public string UserDefinedCol1 { get; set; }
        public string UserDefinedCol2 { get; set; }
        public string UserDefinedCol3 { get; set; }
        public string UserDefinedCol4 { get; set; }
        public Nullable<double> UserDefinedCol5 { get; set; }
        public Nullable<int> UserDefinedCol6 { get; set; }
        public Nullable<int> UserDefinedCol7 { get; set; }
        public Nullable<int> UserDefinedCol8 { get; set; }
        public string UserDefinedCol9 { get; set; }
        public string UserDefinedCol10 { get; set; }
    
        public virtual OrderInfor OrderInfor { get; set; }
        public virtual ICollection<Scheduler> Scheduler { get; set; }
    }
}
=======
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
    
    public partial class OrderService
    {
        public OrderService()
        {
            this.Scheduler = new HashSet<Scheduler>();
        }
    
        public int IDX { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> BillingType { get; set; }
        public Nullable<int> ServiceNatureID { get; set; }
        public string ServiceWorkDate { get; set; }
        public string ServiceWorkTime { get; set; }
        public string EstimatedCompletionTime { get; set; }
        public string ServiceWorkPlace { get; set; }
        public Nullable<int> BigTugNum { get; set; }
        public Nullable<int> MiddleTugNum { get; set; }
        public Nullable<int> SmallTugNum { get; set; }
        public Nullable<int> JobStateID { get; set; }
        public string Remark { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public string LastUpDate { get; set; }
        public string HasBilling { get; set; }
        public string HasBillingInFlow { get; set; }
        public string UserDefinedCol1 { get; set; }
        public string UserDefinedCol2 { get; set; }
        public string UserDefinedCol3 { get; set; }
        public string UserDefinedCol4 { get; set; }
        public Nullable<double> UserDefinedCol5 { get; set; }
        public Nullable<int> UserDefinedCol6 { get; set; }
        public Nullable<int> UserDefinedCol7 { get; set; }
        public Nullable<int> UserDefinedCol8 { get; set; }
        public string UserDefinedCol9 { get; set; }
        public string UserDefinedCol10 { get; set; }
    
        public virtual OrderInfor OrderInfor { get; set; }
        public virtual ICollection<Scheduler> Scheduler { get; set; }
    }
}
>>>>>>> 2ac8644f6fd52ac4a4cd64d602b116fc9f93ca44
