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
    
    public partial class V_Inv_Head
    {
        public int IDX { get; set; }
        public string BillingCode { get; set; }
        public string CreateDate { get; set; }
        public string CustomerName { get; set; }
        public string ShipName { get; set; }
        public Nullable<int> TimeTypeID { get; set; }
        public string Remark { get; set; }
        public Nullable<int> BillingTemplateID { get; set; }
        public string IsShowShipLengthRule { get; set; }
        public string IsShowShipTEUSRule { get; set; }
        public Nullable<int> Length { get; set; }
        public Nullable<int> TEUS { get; set; }
        public Nullable<int> ShipID { get; set; }
        public string JobNo { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Ratio1 { get; set; }
        public Nullable<double> Ratio2 { get; set; }
        public Nullable<double> Ratio3 { get; set; }
        public Nullable<double> Ratio4 { get; set; }
        public Nullable<double> Ratio5 { get; set; }
        public Nullable<double> Ratio6 { get; set; }
        public string ShipLength { get; set; }
        public string ShipTEUS { get; set; }
        public string ExpiryDate { get; set; }
        public Nullable<int> BillingTemplateTypeID { get; set; }
        public string BillingTemplateName { get; set; }
        public string BillDate { get; set; }
    }
}
