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
    
    public partial class TugInfor
    {
        public TugInfor()
        {
            this.Arrangement = new HashSet<Arrangement>();
            this.Scheduler = new HashSet<Scheduler>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string CnName { get; set; }
        public string EnName { get; set; }
        public string SimpleName { get; set; }
        public string Power { get; set; }
        public string Class { get; set; }
        public string Speed { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
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
    
        public virtual ICollection<Arrangement> Arrangement { get; set; }
        public virtual ICollection<Scheduler> Scheduler { get; set; }
    }
}
