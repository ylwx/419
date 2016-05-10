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
    
    public partial class Role
    {
        public Role()
        {
            this.RoleMenu = new HashSet<RoleMenu>();
            this.RoleModule = new HashSet<RoleModule>();
            this.UsersRole = new HashSet<UsersRole>();
        }
    
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string Dept { get; set; }
        public string Remark { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> LastUpDate { get; set; }
        public string System { get; set; }
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
    
        public virtual ICollection<RoleMenu> RoleMenu { get; set; }
        public virtual ICollection<RoleModule> RoleModule { get; set; }
        public virtual ICollection<UsersRole> UsersRole { get; set; }
    }
}
