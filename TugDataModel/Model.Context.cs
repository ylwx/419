﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TugDataEntities : DbContext
    {
        public TugDataEntities()
            : base("name=TugDataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AmountSum> AmountSum { get; set; }
        public virtual DbSet<Approve> Approve { get; set; }
        public virtual DbSet<Arrangement> Arrangement { get; set; }
        public virtual DbSet<BaseTreeItems> BaseTreeItems { get; set; }
        public virtual DbSet<Billing> Billing { get; set; }
        public virtual DbSet<BillingItem> BillingItem { get; set; }
        public virtual DbSet<BillingItemTemplate> BillingItemTemplate { get; set; }
        public virtual DbSet<BillingOrder> BillingOrder { get; set; }
        public virtual DbSet<BillingTemplate> BillingTemplate { get; set; }
        public virtual DbSet<Credit> Credit { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerShip> CustomerShip { get; set; }
        public virtual DbSet<CustomField> CustomField { get; set; }
        public virtual DbSet<FileInfor> FileInfor { get; set; }
        public virtual DbSet<Flow> Flow { get; set; }
        public virtual DbSet<Fuelprice> Fuelprice { get; set; }
        public virtual DbSet<FunctionModule> FunctionModule { get; set; }
        public virtual DbSet<LogProcess> LogProcess { get; set; }
        public virtual DbSet<OrderInfor> OrderInfor { get; set; }
        public virtual DbSet<OrderService> OrderService { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleMenu> RoleMenu { get; set; }
        public virtual DbSet<RoleModule> RoleModule { get; set; }
        public virtual DbSet<Scheduler> Scheduler { get; set; }
        public virtual DbSet<SpecialBillingItem> SpecialBillingItem { get; set; }
        public virtual DbSet<testinv> testinv { get; set; }
        public virtual DbSet<TugInfor> TugInfor { get; set; }
        public virtual DbSet<UserInfor> UserInfor { get; set; }
        public virtual DbSet<UsersRole> UsersRole { get; set; }
        public virtual DbSet<V_1> V_1 { get; set; }
        public virtual DbSet<V_AmountSum_Billing> V_AmountSum_Billing { get; set; }
        public virtual DbSet<V_Approve_Billing> V_Approve_Billing { get; set; }
        public virtual DbSet<V_Arrangement> V_Arrangement { get; set; }
        public virtual DbSet<V_BaseTreeItems> V_BaseTreeItems { get; set; }
        public virtual DbSet<V_Billing> V_Billing { get; set; }
        public virtual DbSet<V_Billing2> V_Billing2 { get; set; }
        public virtual DbSet<V_Billing3> V_Billing3 { get; set; }
        public virtual DbSet<V_BillingItem> V_BillingItem { get; set; }
        public virtual DbSet<V_BillingItemTemplate> V_BillingItemTemplate { get; set; }
        public virtual DbSet<V_BillingOrders> V_BillingOrders { get; set; }
        public virtual DbSet<V_BillingTask> V_BillingTask { get; set; }
        public virtual DbSet<V_BillingTemplate> V_BillingTemplate { get; set; }
        public virtual DbSet<V_CustomerShip> V_CustomerShip { get; set; }
        public virtual DbSet<V_Flow> V_Flow { get; set; }
        public virtual DbSet<V_Inv_BillingItem> V_Inv_BillingItem { get; set; }
        public virtual DbSet<V_Inv_Head> V_Inv_Head { get; set; }
        public virtual DbSet<V_Inv_Head_Special> V_Inv_Head_Special { get; set; }
        public virtual DbSet<V_Inv_OrdService> V_Inv_OrdService { get; set; }
        public virtual DbSet<V_Inv_Scheduler> V_Inv_Scheduler { get; set; }
        public virtual DbSet<V_Invoice> V_Invoice { get; set; }
        public virtual DbSet<V_Invoice2> V_Invoice2 { get; set; }
        public virtual DbSet<V_Module_Role_User> V_Module_Role_User { get; set; }
        public virtual DbSet<V_NeedApproveBilling> V_NeedApproveBilling { get; set; }
        public virtual DbSet<V_NeedApproveOrderBilling> V_NeedApproveOrderBilling { get; set; }
        public virtual DbSet<V_NeedApproveOrderBillingSpecial> V_NeedApproveOrderBillingSpecial { get; set; }
        public virtual DbSet<V_OrderBilling> V_OrderBilling { get; set; }
        public virtual DbSet<V_OrderBillingCredit> V_OrderBillingCredit { get; set; }
        public virtual DbSet<V_OrderInfor> V_OrderInfor { get; set; }
        public virtual DbSet<V_OrderScheduler> V_OrderScheduler { get; set; }
        public virtual DbSet<V_OrderService> V_OrderService { get; set; }
        public virtual DbSet<V_RoleMenu> V_RoleMenu { get; set; }
        public virtual DbSet<V_RoleModule> V_RoleModule { get; set; }
        public virtual DbSet<V_RoleUser> V_RoleUser { get; set; }
        public virtual DbSet<V_SpecialBillingCredit> V_SpecialBillingCredit { get; set; }
        public virtual DbSet<V_SpecialBillingItem> V_SpecialBillingItem { get; set; }
        public virtual DbSet<V_SpecialBillingItem_OrderService> V_SpecialBillingItem_OrderService { get; set; }
        public virtual DbSet<V_SpecialBillingSummarizeItem> V_SpecialBillingSummarizeItem { get; set; }
        public virtual DbSet<V_Users> V_Users { get; set; }
    
        public virtual ObjectResult<proc_inv_item_Result> proc_inv_item(Nullable<int> billingID, Nullable<int> timeTypeValue)
        {
            var billingIDParameter = billingID.HasValue ?
                new ObjectParameter("BillingID", billingID) :
                new ObjectParameter("BillingID", typeof(int));
    
            var timeTypeValueParameter = timeTypeValue.HasValue ?
                new ObjectParameter("TimeTypeValue", timeTypeValue) :
                new ObjectParameter("TimeTypeValue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_inv_item_Result>("proc_inv_item", billingIDParameter, timeTypeValueParameter);
        }
    
        public virtual ObjectResult<proc_inv_item_xy_Result> proc_inv_item_xy(Nullable<int> billingID, Nullable<int> timeTypeValue)
        {
            var billingIDParameter = billingID.HasValue ?
                new ObjectParameter("BillingID", billingID) :
                new ObjectParameter("BillingID", typeof(int));
    
            var timeTypeValueParameter = timeTypeValue.HasValue ?
                new ObjectParameter("TimeTypeValue", timeTypeValue) :
                new ObjectParameter("TimeTypeValue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_inv_item_xy_Result>("proc_inv_item_xy", billingIDParameter, timeTypeValueParameter);
        }
    
        public virtual ObjectResult<Nullable<double>> proc_inv_SrvHourNumeric(string departBaseTime, string arrivalBaseTime, Nullable<int> timeTypeValue)
        {
            var departBaseTimeParameter = departBaseTime != null ?
                new ObjectParameter("DepartBaseTime", departBaseTime) :
                new ObjectParameter("DepartBaseTime", typeof(string));
    
            var arrivalBaseTimeParameter = arrivalBaseTime != null ?
                new ObjectParameter("ArrivalBaseTime", arrivalBaseTime) :
                new ObjectParameter("ArrivalBaseTime", typeof(string));
    
            var timeTypeValueParameter = timeTypeValue.HasValue ?
                new ObjectParameter("TimeTypeValue", timeTypeValue) :
                new ObjectParameter("TimeTypeValue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<double>>("proc_inv_SrvHourNumeric", departBaseTimeParameter, arrivalBaseTimeParameter, timeTypeValueParameter);
        }
    
        public virtual ObjectResult<string> proc_inv_SrvHourString(string departBaseTime, string arrivalBaseTime, Nullable<int> timeTypeValue)
        {
            var departBaseTimeParameter = departBaseTime != null ?
                new ObjectParameter("DepartBaseTime", departBaseTime) :
                new ObjectParameter("DepartBaseTime", typeof(string));
    
            var arrivalBaseTimeParameter = arrivalBaseTime != null ?
                new ObjectParameter("ArrivalBaseTime", arrivalBaseTime) :
                new ObjectParameter("ArrivalBaseTime", typeof(string));
    
            var timeTypeValueParameter = timeTypeValue.HasValue ?
                new ObjectParameter("TimeTypeValue", timeTypeValue) :
                new ObjectParameter("TimeTypeValue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("proc_inv_SrvHourString", departBaseTimeParameter, arrivalBaseTimeParameter, timeTypeValueParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual ObjectResult<proc_needapprove_Result> proc_needapprove(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_needapprove_Result>("proc_needapprove", userIDParameter);
        }
    
        public virtual ObjectResult<proc_approved_Result> proc_approved(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_approved_Result>("proc_approved", userIDParameter);
        }
    }
}
