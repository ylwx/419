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
    
    public partial class V_OrderBillingCredit
    {
        public Nullable<int> BillingID { get; set; }
        public int CreditID { get; set; }
        public string CreditCode { get; set; }
        public string CreditContent { get; set; }
        public Nullable<double> CreditAmount { get; set; }
        public string Remark { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public string LastUpDate { get; set; }
    }
}
