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
    
    public partial class Billing
    {
        public Billing()
        {
            this.AmountSum = new HashSet<AmountSum>();
            this.BillingItem = new HashSet<BillingItem>();
            this.BillingOrder = new HashSet<BillingOrder>();
            this.Credit = new HashSet<Credit>();
            this.SpecialBillingItem = new HashSet<SpecialBillingItem>();
        }
    
        public int IDX { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public string JobNo { get; set; }
        public string IsShowShipLengthRule { get; set; }
        public string IsShowShipTEUSRule { get; set; }
        public Nullable<int> BillingTemplateID { get; set; }
        public Nullable<int> BillingTypeID { get; set; }
        public string BillingCode { get; set; }
        public string BillingName { get; set; }
        public Nullable<int> TimeTypeID { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Remark { get; set; }
        public string InvoiceType { get; set; }
        public string Month { get; set; }
        public Nullable<int> TimesNo { get; set; }
        public string Status { get; set; }
        public Nullable<int> Phase { get; set; }
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
    
        public virtual ICollection<AmountSum> AmountSum { get; set; }
        public virtual ICollection<BillingItem> BillingItem { get; set; }
        public virtual ICollection<BillingOrder> BillingOrder { get; set; }
        public virtual ICollection<Credit> Credit { get; set; }
        public virtual ICollection<SpecialBillingItem> SpecialBillingItem { get; set; }
    }
}
