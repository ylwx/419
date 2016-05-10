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
    
    public partial class BillingTemplate
    {
        public BillingTemplate()
        {
            this.BillingItemTemplate = new HashSet<BillingItemTemplate>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string BillingTemplateType { get; set; }
        public string BillingTemplateCode { get; set; }
        public string BillingTemplateName { get; set; }
        public Nullable<int> TimeTypeID { get; set; }
        public string TemplateCreditContent { get; set; }
        public string Remark { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> LastUpDate { get; set; }
        public string UserDefinedCol1 { get; set; }
        public string UserDefinedCol2 { get; set; }
        public string UserDefinedCol3 { get; set; }
        public string UserDefinedCol4 { get; set; }
        public Nullable<double> UserDefinedCol5 { get; set; }
        public Nullable<int> UserDefinedCol6 { get; set; }
        public Nullable<int> UserDefinedCol7 { get; set; }
        public Nullable<int> UserDefinedCol8 { get; set; }
        public Nullable<System.DateTime> UserDefinedCol9 { get; set; }
        public Nullable<System.DateTime> UserDefinedCol10 { get; set; }
    
        public virtual ICollection<BillingItemTemplate> BillingItemTemplate { get; set; }
    }
}
