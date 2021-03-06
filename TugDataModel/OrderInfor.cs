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
    
    public partial class OrderInfor
    {
        public OrderInfor()
        {
            this.BillingOrder = new HashSet<BillingOrder>();
            this.OrderService = new HashSet<OrderService>();
        }
    
        public int IDX { get; set; }
        public string IsGuest { get; set; }
        public string Code { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string OrdDate { get; set; }
        public Nullable<int> ShipID { get; set; }
        public string ShipName { get; set; }
        public string LinkMan { get; set; }
        public string LinkPhone { get; set; }
        public string LinkEmail { get; set; }
        public Nullable<int> WorkStateID { get; set; }
        public string Remark { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public string LastUpDate { get; set; }
        public string HasInvoice { get; set; }
        public string HasInFlow { get; set; }
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
    
        public virtual ICollection<BillingOrder> BillingOrder { get; set; }
        public virtual ICollection<OrderService> OrderService { get; set; }
    }
}
