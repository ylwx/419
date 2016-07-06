using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TugDataModel;

namespace TugBusinessLogic.Module
{
    public class FinanceLogic
    {
        static public void GenerateInvoice(int orderId)
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            MyInvoice _invoice = new MyInvoice();

            //List<V_Invoice> list = db.V_Invoice.Where(u => u.OrderID == orderId).OrderBy(u => u.ServiceNatureID).Select(u => u).ToList();

            var list = db.V_Invoice.Where(u => u.OrderID == orderId).OrderBy(u => u.ServiceNatureID).Select(u => u);

            if (list != null)
            {
                //_invoice.CustormerID = (int)list[0].CustomerID;
                //_invoice.CustomerName = list[0].CustomerName;
                //_invoice.OrderID = (int)list[0].OrderID;
                //_invoice.OrderCode = list[0].OrderCode;

                _invoice.CustormerID = (int)list.FirstOrDefault().CustomerID;
                _invoice.CustomerName = list.FirstOrDefault().CustomerName;
                _invoice.OrderID = (int)list.FirstOrDefault().OrderID;
                _invoice.OrderCode = list.FirstOrDefault().OrderCode;

                Dictionary<int, MyService> dicServiceNature = new Dictionary<int, MyService>();
                //var services2 = list.Select(u => new {u.ServiceNatureID, u.ServiceNatureLabel}).ToList();
                var services = list.Select(u => new { u.ServiceNatureID, u.ServiceNatureLabel, u.OrderSchedulerRemark }).Distinct().ToList();
                //var services = db.V_Invoice.Where(u => u.OrderID == orderId).OrderBy(u => u.ServiceNatureID).Select(u => new {u.ServiceNatureID, u.ServiceNatureLabel}).Distinct().ToList();

                Dictionary<int, List<MyScheduler>> dicSchedulers = new Dictionary<int, List<MyScheduler>>();

                double grandTotal = 0;

                if(services != null && services.Count > 0)
                {
                    foreach(var item in services)
                    {
                        MyService ms = new MyService();
                        ms.ServiceId = (int)item.ServiceNatureID;
                        ms.ServiceName = item.ServiceNatureLabel;
                        ms.ServiceRemark = item.OrderSchedulerRemark;
                        dicServiceNature.Add(ms.ServiceId, ms);

                        var ships = list.Where(u => u.ServiceNatureID == item.ServiceNatureID)
                            .Select(u => new {u.TugID, u.TugCnName, u.TugEnName, u.TugSimpleName, u.Power}).Distinct()
                            .OrderBy(u => u.TugCnName).ToList();

                        List<MyScheduler> listScheduler = new List<MyScheduler>();
                        
                        if(ships != null && ships.Count > 0)
                        {
                            foreach (var ship in ships)
                            {
                                MyScheduler sch = new MyScheduler();
                                sch.TugID = (int)ship.TugID;
                                sch.TugCnName = ship.TugCnName;
                                sch.TugEnName = ship.TugEnName;
                                sch.TugSimpleName = ship.TugSimpleName;
                                sch.TugPower = ship.Power;
                                var schedulers = list.Where(u => u.ServiceNatureID == item.ServiceNatureID && u.TugID == ship.TugID)
                                    .OrderBy(u => u.OrderID).OrderBy(u => u.ServiceNatureID)
                                    .Select(u => new
                                    {
                                        u.TugID,
                                        u.TugCnName,
                                        u.TugEnName,
                                        u.TugSimpleName,
                                        u.Power,
                                        u.InformCaptainTime,
                                        u.CaptainConfirmTime,
                                        u.DepartBaseTime,
                                        u.ArrivalShipSideTime,
                                        u.WorkCommencedTime,
                                        u.WorkCompletedTime,
                                        u.ArrivalBaseTime,
                                        u.UnitPrice,
                                        u.RopeUsed,
                                        u.RopeNum,
                                        u.OrderSchedulerRemark,
                                        u.BillingItemIDX,
                                        u.ItemID,
                                        u.BillingItemValue,
                                        u.BillingItemLabel,
                                        u.Currency,
                                        u.PositionTypeID,
                                        u.PositionTypeValue,
                                        u.PositionTypeLabel
                                    }).OrderBy(u => u.ItemID).ToList();

                                if (schedulers != null && schedulers.Count > 0)
                                {
                                    sch.InformCaptainTime = schedulers[0].InformCaptainTime;
                                    sch.CaptainConfirmTime = schedulers[0].CaptainConfirmTime;
                                    sch.DepartBaseTime = schedulers[0].DepartBaseTime;
                                    sch.ArrivalShipSideTime = schedulers[0].ArrivalShipSideTime;
                                    sch.WorkCommencedTime = schedulers[0].WorkCommencedTime;
                                    sch.WorkCompletedTime = schedulers[0].WorkCompletedTime;
                                    sch.ArrivalBaseTime = schedulers[0].ArrivalBaseTime;

                                    int iDiffHour, iDiffMinute;
                                    TugBusinessLogic.Utils.CalculateTimeDiff(sch.DepartBaseTime, sch.ArrivalBaseTime, out iDiffHour, out iDiffMinute);
                                    sch.WorkTime = iDiffHour.ToString() + "h" + iDiffMinute.ToString() + "m";

                                    //_invoice.BillingTypeID = (int)list[0].BillingTypeID;
                                    //_invoice.BillingTypeValue = list[0].BillingTypeValue;
                                    //_invoice.BillingTypeLabel = list[0].BillingTypeLabel;
                                    //_invoice.TimeTypeID = (int)list[0].TimeTypeID;
                                    //_invoice.TimeTypeValue = list[0].TimeTypeValue;
                                    //_invoice.TimeTypeLabel = list[0].TimeTypeLabel;

                                    sch.WorkTimeConsumption = TugBusinessLogic.Utils.CalculateTimeConsumption(iDiffHour, iDiffMinute, (int)list.FirstOrDefault().TimeTypeID, list.FirstOrDefault().TimeTypeValue, list.FirstOrDefault().TimeTypeLabel);

                                    sch.UnitPrice = (double)schedulers[0].UnitPrice;
                                    if (((int)list.FirstOrDefault().BillingTypeID == 5 || list.FirstOrDefault().BillingTypeValue == "0" || list.FirstOrDefault().BillingTypeLabel == "全包")
                                        || ((int)list.FirstOrDefault().BillingTypeID == 6 || list.FirstOrDefault().BillingTypeValue == "1" || list.FirstOrDefault().BillingTypeLabel == "全包加特别条款"))
                                        sch.Price = (double)schedulers[0].UnitPrice;
                                    else
                                        sch.Price = (double)schedulers[0].UnitPrice * sch.WorkTimeConsumption;


                                    sch.RopeUsed = schedulers[0].RopeUsed;
                                    sch.RopeNum = (int)schedulers[0].RopeNum;
                                    sch.Remark = schedulers[0].OrderSchedulerRemark;



                                    double upTotalPrice = 0;
                                    double midTotalPrice = 0;
                                    double totalPrice = 0;

                                    #region 一条船的费用项目
                                    List<MyBillingItem> billingItems = new List<MyBillingItem>();
                                    foreach (var subItem in schedulers)
                                    {
                                        MyBillingItem bit = new MyBillingItem();
                                        bit.IDX = subItem.BillingItemIDX;
                                        bit.ItemID = subItem.ItemID;
                                        bit.ItemValue = subItem.BillingItemValue;
                                        bit.ItemLabel = subItem.BillingItemLabel;
                                        bit.UnitPrice = subItem.UnitPrice;

                                        if (((int)list.FirstOrDefault().BillingTypeID == 5 || list.FirstOrDefault().BillingTypeValue == "0" || list.FirstOrDefault().BillingTypeLabel == "全包")
                                        || ((int)list.FirstOrDefault().BillingTypeID == 6 || list.FirstOrDefault().BillingTypeValue == "1" || list.FirstOrDefault().BillingTypeLabel == "全包加特别条款"))
                                            bit.Price = subItem.UnitPrice;
                                        else
                                            bit.Price = subItem.UnitPrice * sch.WorkTimeConsumption;
                                        
                                        bit.Currency = subItem.Currency;
                                        bit.TypeID = subItem.PositionTypeID;
                                        bit.TypeValue = subItem.PositionTypeValue;
                                        bit.TypeLabel = subItem.PositionTypeLabel;

                                        if (subItem.PositionTypeID == 13 || subItem.PositionTypeValue == "0" || subItem.PositionTypeLabel == "上")
                                            upTotalPrice += (double)bit.Price;
                                        else if (subItem.PositionTypeID == 14 || subItem.PositionTypeValue == "1" || subItem.PositionTypeLabel == "中")
                                            midTotalPrice += (double)bit.Price;

                                        totalPrice += upTotalPrice + midTotalPrice;

                                        billingItems.Add(bit);
                                    }
                                    #endregion

                                    sch.SubTotaHKS = upTotalPrice;

                                    sch.TotalHKs = totalPrice;

                                    sch.BillingItems = billingItems;

                                    grandTotal += totalPrice;
                                }

                                listScheduler.Add(sch);
                            }

                        }

                        dicSchedulers.Add((int)item.ServiceNatureID, listScheduler);
                        
                    }
                }

                _invoice.ServiceNature = dicServiceNature;
                _invoice.Schedulers = dicSchedulers;

                _invoice.BillingID = list.FirstOrDefault().BillingID;
                _invoice.BillingCode = list.FirstOrDefault().BillingCode;
                _invoice.BillingTypeID = (int)list.FirstOrDefault().BillingTypeID;
                _invoice.BillingTypeValue = list.FirstOrDefault().BillingTypeValue;
                _invoice.BillingTypeLabel = list.FirstOrDefault().BillingTypeLabel;
                _invoice.TimeTypeID = (int)list.FirstOrDefault().TimeTypeID;
                _invoice.TimeTypeValue = list.FirstOrDefault().TimeTypeValue;
                _invoice.TimeTypeLabel = list.FirstOrDefault().TimeTypeLabel;

        
                _invoice.GrandTotalHKS = grandTotal;
            } 
        }


        static public MyInvoice NewInvoice(int orderId, int timeTypeId, string timeTypeValue, string timeTypeLabel) 
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            MyInvoice _invoice = new MyInvoice();

            var list = db.V_OrderScheduler.Where(u => u.OrderID == orderId).OrderBy(u => u.ServiceNatureID).Select(u => u);

            var services = list.Select(u => new { u.ServiceNatureID, u.ServiceNatureLabel, u.Remark}).Distinct().ToList();

            if (services != null)
            {
                Dictionary<int, MyService> dicService = new Dictionary<int, MyService>();
                Dictionary<int, List<MyScheduler>> dicScheduler = new Dictionary<int, List<MyScheduler>>();
                foreach (var service in services)
                {
                    MyService ms = new MyService();
                    ms.ServiceId = (int)service.ServiceNatureID;
                    ms.ServiceName = service.ServiceNatureLabel;
                    ms.ServiceRemark = service.Remark;
                    dicService.Add(ms.ServiceId, ms);

                    var schedulers = list.Where(u => u.ServiceNatureID == (int)service.ServiceNatureID)
                        .Select(u => new {u.TugID, u.CnName, u.EnName, u.SimpleName, u.Power, u.DepartBaseTime, u.ArrivalBaseTime,
                        u.RopeUsed, u.RopeNum, u.Remark}).ToList();

                    if(schedulers != null)
                    {
                        List<MyScheduler> lstScheduler = new List<MyScheduler>();
                        foreach (var scheduler in schedulers)
                        {
                            MyScheduler mySch = new MyScheduler();
                            mySch.TugID = (int)scheduler.TugID;
                            mySch.TugCnName = scheduler.CnName;
                            mySch.TugEnName = scheduler.EnName;
                            mySch.TugSimpleName = scheduler.SimpleName;
                            mySch.TugPower = scheduler.Power;

                            mySch.DepartBaseTime = scheduler.DepartBaseTime;
                            mySch.ArrivalBaseTime = scheduler.ArrivalBaseTime;

                            int iDiffHour, iDiffMinute;
                                    TugBusinessLogic.Utils.CalculateTimeDiff(mySch.DepartBaseTime, mySch.ArrivalBaseTime, out iDiffHour, out iDiffMinute);
                                    mySch.WorkTime = iDiffHour.ToString() + "h" + iDiffMinute.ToString() + "m";

                            mySch.WorkTimeConsumption = TugBusinessLogic.Utils.CalculateTimeConsumption(iDiffHour, iDiffMinute,
                                timeTypeId, timeTypeValue, timeTypeLabel);

                            mySch.RopeUsed = scheduler.RopeUsed;
                            mySch.RopeNum = (int)scheduler.RopeNum;
                            mySch.Remark = scheduler.Remark;

                            lstScheduler.Add(mySch);
                        }
                        dicScheduler.Add((int)service.ServiceNatureID, lstScheduler);

                    }

                }

                _invoice.ServiceNature = dicService;
                _invoice.Schedulers = dicScheduler;
            }


             
            return _invoice;
        }


        static public List<V_BillingTemplate> GetCustomerBillSchemes(int custId)
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            List<V_BillingTemplate> list = db.V_BillingTemplate.Where(u => u.CustomerID == custId).OrderBy(u => u.BillingTemplateName).ToList();

            return list;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderMethod">排序方式asc升序；desc降序</param>
        /// <returns></returns>
        static public List<TugDataModel.V_OrderBilling> LoadDataForInvoice(string orderField, string orderMethod)
        {
            List<V_OrderBilling> orders = null;

            try
            {
                TugDataEntities db = new TugDataEntities();
                orders = db.V_OrderBilling.Select(u => u).ToList<V_OrderBilling>();

                #region 根据排序字段和排序方式排序
                //switch (orderField)
                //{
                //    case "":
                //        {
                //            //if(orderMethod.ToLower().Equals("asc"))
                //            //    orders = orders.OrderBy(u => u.IDX).ToList();
                //            //else
                //            orders = orders.OrderByDescending(u => u.IDX).ToList();
                //        }
                //        break;
                //    case "IsGuest":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.IsGuest).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.IsGuest).ToList();
                //        }
                //        break;
                //    case "Code":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.Code).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.Code).ToList();
                //        }
                //        break;
                //    case "CustomerName":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.CustomerName).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.CustomerName).ToList();
                //        }
                //        break;
                //    case "WorkDate":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.WorkDate).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.WorkDate).ToList();
                //        }
                //        break;
                //    case "WorkTime":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.WorkTime).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.WorkTime).ToList();
                //        }
                //        break;
                //    case "EstimatedCompletionTime":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.EstimatedCompletionTime).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.EstimatedCompletionTime).ToList();
                //        }
                //        break;
                //    case "ShipName":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.ShipName).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.ShipName).ToList();
                //        }
                //        break;
                //    case "LinkMan":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.LinkMan).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.LinkMan).ToList();
                //        }
                //        break;
                //    case "LinkPhone":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.LinkPhone).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.LinkPhone).ToList();
                //        }
                //        break;
                //    case "LinkEmail":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.LinkEmail).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.LinkEmail).ToList();
                //        }
                //        break;
                //    case "WorkPlace":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.WorkPlace).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.WorkPlace).ToList();
                //        }
                //        break;
                //    case "ServiceNatureNames":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.ServiceNatureNames).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.ServiceNatureNames).ToList();
                //        }
                //        break;
                //    case "WorkStateLabel":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.WorkStateLabel).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.WorkStateLabel).ToList();
                //        }
                //        break;
                //    case "BigTugNum":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.BigTugNum).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.BigTugNum).ToList();
                //        }
                //        break;
                //    case "MiddleTugNum":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.MiddleTugNum).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.MiddleTugNum).ToList();
                //        }
                //        break;
                //    case "SmallTugNum":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.SmallTugNum).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.SmallTugNum).ToList();
                //        }
                //        break;
                //    case "Remark":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.Remark).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.Remark).ToList();
                //        }
                //        break;
                //    case "CreateDate":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.CreateDate).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.CreateDate).ToList();
                //        }
                //        break;
                //    case "LastUpDate":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.LastUpDate).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.LastUpDate).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol1":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol1).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol1).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol2":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol2).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol2).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol3":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol3).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol3).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol4":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol4).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol4).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol5":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol5).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol5).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol6":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol6).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol6).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol7":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol7).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol7).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol8":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol8).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol8).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol9":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol9).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol9).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol10":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol10).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol10).ToList();
                //        }
                //        break;
                //    default:
                //        break;
                //}

                #endregion
            }
            catch (Exception ex)
            {
                return null;
            }

            return orders;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchOptions">搜索选项，格式如下</param>
        /// <returns></returns>
        static public List<TugDataModel.V_OrderBilling> SearchForInvoice(string orderField, string orderMethod, string searchOptions)
        {
            List<V_OrderBilling> orders = null;
            try
            {
                //searchOptions的Json字符串格式
                //{
                //    "groupOp":"AND",
                //    "rules":[{"field":"IsGuest","op":"eq","data":"全部"}],
                //    "groups":[
                //        {"groupOp":"AND","groups":[],"rules":[{"data":"1","op":"ge","field":"BigTugNum"},{"data":"2","op":"le","field":"BigTugNum"}]},
                //        {"groupOp":"AND","groups":[],"rules":[{"data":"1","op":"ge","field":"MiddleTugNum"},{"data":"2","op":"le","field":"MiddleTugNum"}]},
                //        {"groupOp":"AND","groups":[],"rules":[{"data":"1","op":"ge","field":"SmallTugNum"},{"data":"2","op":"le","field":"SmallTugNum"}]}
                //    ]

                //}



                TugDataEntities db = new TugDataEntities();
                //orders = db.V_OrderInfor.Select(u => u).ToList<V_OrderInfor>();

                JObject jsonSearchOption = (JObject)JsonConvert.DeserializeObject(searchOptions);
                string groupOp = (string)jsonSearchOption["groupOp"];
                JArray rules = (JArray)jsonSearchOption["rules"];

                Expression condition = Expression.Equal(Expression.Constant(1, typeof(int)), Expression.Constant(1, typeof(int)));
                ParameterExpression parameter = Expression.Parameter(typeof(V_OrderInfor));

                if (rules != null)
                {
                    foreach (JObject item in rules)
                    {
                        string field = (string)item["field"];
                        string op = (string)item["op"];
                        string data = (string)item["data"];

                        #region 根据各字段条件进行条件表达式拼接
                        //switch (field)
                        //{
                        //    #region IsGuest
                        //    case "IsGuest":
                        //        {
                        //            Expression cdt = null;

                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        if (data != "全部")
                        //                        {
                        //                            //orders = orders.Where(u => u.IsGuest == data).ToList();
                        //                            cdt = Expression.Equal(Expression.PropertyOrField(parameter, "IsGuest"), Expression.Constant(data));
                        //                        }
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }

                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region Code
                        //    case "Code":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.Code.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "Code"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.Code.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "Code"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.Code.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "Code"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.Code.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "Code"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }

                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region CustomerName
                        //    case "CustomerName":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.CustomerName.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "CustomerName"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.CustomerName.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "CustomerName"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.CustomerName.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "CustomerName"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.CustomerName.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "CustomerName"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region WorkDate
                        //    case "WorkDate":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkDate == data.Trim()).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.Equal(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkDate.CompareTo(data.Trim()) == -1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkDate.CompareTo(data.Trim()) == -1 || u.WorkDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkDate.CompareTo(data.Trim()) == 1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThan(tmp, Expression.Constant(typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkDate.CompareTo(data.Trim()) == 1 || u.WorkDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region WorkTime
                        //    case "WorkTime":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkTime == data.Trim()).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.Equal(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkTime.CompareTo(data.Trim()) == -1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkTime.CompareTo(data.Trim()) == -1 || u.WorkTime.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkTime.CompareTo(data.Trim()) == 1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkTime.CompareTo(data.Trim()) == 1 || u.WorkTime.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region EstimatedCompletionTime
                        //    case "EstimatedCompletionTime":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.EstimatedCompletionTime == data.Trim()).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), Expression.Constant(data.Trim()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data.Trim()) == -1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data.Trim()) == -1 || u.EstimatedCompletionTime.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data.Trim()) == 1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data.Trim()) == 1 || u.EstimatedCompletionTime.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region ShipName
                        //    case "ShipName":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "ShipName"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "ShipName"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "ShipName"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "ShipName"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region LinkMan
                        //    case "LinkMan":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkMan.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "LinkMan"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkMan.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkMan"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkMan.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkMan"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkMan.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkMan"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region LinkPhone
                        //    case "LinkPhone":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkPhone.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "LinkPhone"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkPhone.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkPhone"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkPhone.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkPhone"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkPhone.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkPhone"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region LinkEmail
                        //    case "LinkEmail":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkEmail.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "LinkEmail"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkEmail.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkEmail"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkEmail.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkEmail"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.LinkEmail.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "LinkEmail"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region WorkPlace
                        //    case "WorkPlace":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkPlace.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "WorkPlace"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkPlace.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "WorkPlace"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkPlace.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "WorkPlace"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.WorkPlace.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "WorkPlace"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region ServiceNatureNames
                        //    case "ServiceNatureNames":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.ServiceNatureNames.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "ServiceNatureNames"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ServiceNatureNames.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "ServiceNatureNames"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ServiceNatureNames.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "ServiceNatureNames"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.ServiceNatureNames.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "ServiceNatureNames"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region WorkStateLabel
                        //    case "WorkStateLabel":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        int workStateId = Convert.ToInt32(data.Split('~')[0]);
                        //                        if (workStateId != -1)
                        //                        {
                        //                            //orders = orders.Where(u => u.WorkStateID == workStateId).ToList();
                        //                            cdt = Expression.Equal(Expression.PropertyOrField(parameter, "WorkStateID"), Expression.Constant(workStateId, typeof(Nullable<int>)));
                        //                        }

                        //                    }
                        //                    break;

                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region BigTugNum
                        //    case "BigTugNum":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.BigTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "BigTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.BigTugNum < Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "BigTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.BigTugNum < Convert.ToInt32(data.Trim()) || u.BigTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "BigTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.BigTugNum > Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "BigTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.BigTugNum > Convert.ToInt32(data.Trim()) || u.BigTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "BigTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region MiddleTugNum
                        //    case "MiddleTugNum":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.MiddleTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "MiddleTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.MiddleTugNum < Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "MiddleTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.MiddleTugNum < Convert.ToInt32(data.Trim()) || u.MiddleTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "MiddleTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.MiddleTugNum > Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "MiddleTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.MiddleTugNum > Convert.ToInt32(data.Trim()) || u.MiddleTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "MiddleTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region SmallTugNum
                        //    case "SmallTugNum":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "SmallTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "SmallTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "SmallTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "SmallTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "SmallTugNum"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region Remark
                        //    case "Remark":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.Remark.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "Remark"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.Remark.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "Remark"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.Remark.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "Remark"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.Remark.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "Remark"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region CreateDate
                        //    case "CreateDate":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.CreateDate == data.Trim()).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "CreateDate"), Expression.Constant(data.Trim()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.CreateDate.CompareTo(data.Trim()) == -1).ToList();
                        //                        //cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "CreateDate"), Expression.Constant(data.Trim()));
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "CreateDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.CreateDate.CompareTo(data.Trim()) == -1 || u.CreateDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "CreateDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.CreateDate.CompareTo(data.Trim()) == 1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "CreateDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.CreateDate.CompareTo(data.Trim()) == 1 || u.CreateDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "CreateDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(0));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region LastUpDate
                        //    case "LastUpDate":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate == data.Trim()).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "LastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.Equal(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == -1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "LastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == -1 || u.LastUpDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "LastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == 1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "LastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThan(tmp, Expression.Constant(typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == 1 || u.LastUpDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "LastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(typeof(Int32)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol1
                        //    case "UserDefinedCol1":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "UserDefinedCol1"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol1"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol1"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol1"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol2
                        //    case "UserDefinedCol2":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "UserDefinedCol2"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol2"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol2"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol2"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol3
                        //    case "UserDefinedCol3":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "UserDefinedCol3"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol3"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol3"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol3"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol4
                        //    case "UserDefinedCol4":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "UserDefinedCol4"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_BW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol4"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_EW:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol4"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_CN:
                        //                    {
                        //                        //orders = orders.Where(u => u.ShipName.ToLower().Contains(data.Trim().ToLower())).ToList();
                        //                        cdt = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol4"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol5
                        //    case "UserDefinedCol5":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "UserDefinedCol5"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "UserDefinedCol5"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "UserDefinedCol5"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "UserDefinedCol5"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "UserDefinedCol5"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol6
                        //    case "UserDefinedCol6":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "UserDefinedCol6"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "UserDefinedCol6"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "UserDefinedCol6"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "UserDefinedCol6"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "UserDefinedCol6"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol7
                        //    case "UserDefinedCol7":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "UserDefinedCol7"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "UserDefinedCol7"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "UserDefinedCol7"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "UserDefinedCol7"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "UserDefinedCol7"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol8
                        //    case "UserDefinedCol8":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.Equal(Expression.PropertyOrField(parameter, "UserDefinedCol8"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "UserDefinedCol8"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "UserDefinedCol8"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "UserDefinedCol8"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                        //                        cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "UserDefinedCol8"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol9
                        //    case "UserDefinedCol9":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate == data.Trim()).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol9"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.Equal(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == -1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol9"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == -1 || u.LastUpDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol9"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == 1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol9"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThan(tmp, Expression.Constant(typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == 1 || u.LastUpDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol9"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(typeof(Int32)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    #region UserDefinedCol10
                        //    case "UserDefinedCol10":
                        //        {
                        //            Expression cdt = null;
                        //            switch (op)
                        //            {
                        //                case ConstValue.ComparisonOperator_EQ:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate == data.Trim()).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol10"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.Equal(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LT:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == -1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol10"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_LE:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == -1 || u.LastUpDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol10"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GT:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == 1).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol10"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThan(tmp, Expression.Constant(typeof(Int32)));
                        //                    }
                        //                    break;
                        //                case ConstValue.ComparisonOperator_GE:
                        //                    {
                        //                        //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == 1 || u.LastUpDate.CompareTo(data.Trim()) == 0).ToList();
                        //                        Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "UserDefinedCol10"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                        //                        cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(typeof(Int32)));
                        //                    }
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //            if (cdt != null)
                        //            {
                        //                condition = Expression.AndAlso(condition, cdt);
                        //            }
                        //        }
                        //        break;
                        //    #endregion

                        //    default:
                        //        break;
                        //}
                        #endregion

                    }

                }

                #region 执行查询
                //if (condition != null)
                //{
                //    var lamda = Expression.Lambda<Func<V_OrderInfor, bool>>(condition, parameter);
                //    orders = db.V_OrderInfor.Where(lamda).Select(u => u).ToList<V_OrderInfor>();
                //}
                //else
                //{
                //    orders = db.V_OrderInfor.Select(u => u).ToList<V_OrderInfor>();
                //}
                #endregion


                #region 对搜索结果根据排序字段和方式进行排序
                //switch (orderField)
                //{
                //    case "":
                //        {

                //            orders = orders.OrderByDescending(u => u.IDX).ToList();
                //        }
                //        break;
                //    case "IsGuest":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.IsGuest).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.IsGuest).ToList();
                //        }
                //        break;
                //    case "Code":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.Code).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.Code).ToList();
                //        }
                //        break;
                //    case "CustomerName":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.CustomerName).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.CustomerName).ToList();
                //        }
                //        break;
                //    case "WorkDate":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.WorkDate).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.WorkDate).ToList();
                //        }
                //        break;
                //    case "WorkTime":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.WorkTime).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.WorkTime).ToList();
                //        }
                //        break;
                //    case "EstimatedCompletionTime":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.EstimatedCompletionTime).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.EstimatedCompletionTime).ToList();
                //        }
                //        break;
                //    case "ShipName":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.ShipName).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.ShipName).ToList();
                //        }
                //        break;
                //    case "LinkMan":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.LinkMan).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.LinkMan).ToList();
                //        }
                //        break;
                //    case "LinkPhone":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.LinkPhone).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.LinkPhone).ToList();
                //        }
                //        break;
                //    case "LinkEmail":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.LinkEmail).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.LinkEmail).ToList();
                //        }
                //        break;
                //    case "WorkPlace":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.WorkPlace).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.WorkPlace).ToList();
                //        }
                //        break;
                //    case "ServiceNatureNames":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.ServiceNatureNames).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.ServiceNatureNames).ToList();
                //        }
                //        break;
                //    case "WorkStateLabel":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.WorkStateLabel).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.WorkStateLabel).ToList();
                //        }
                //        break;
                //    case "BigTugNum":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.BigTugNum).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.BigTugNum).ToList();
                //        }
                //        break;
                //    case "MiddleTugNum":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.MiddleTugNum).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.MiddleTugNum).ToList();
                //        }
                //        break;
                //    case "SmallTugNum":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.SmallTugNum).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.SmallTugNum).ToList();
                //        }
                //        break;
                //    case "Remark":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.Remark).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.Remark).ToList();
                //        }
                //        break;
                //    case "CreateDate":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.CreateDate).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.CreateDate).ToList();
                //        }
                //        break;
                //    case "LastUpDate":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.LastUpDate).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.LastUpDate).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol1":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol1).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol1).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol2":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol2).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol2).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol3":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol3).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol3).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol4":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol4).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol4).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol5":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol5).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol5).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol6":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol6).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol6).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol7":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol7).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol7).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol8":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol8).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol8).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol9":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol9).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol9).ToList();
                //        }
                //        break;
                //    case "UserDefinedCol10":
                //        {
                //            if (orderMethod.ToLower().Equals("asc"))
                //                orders = orders.OrderBy(u => u.UserDefinedCol10).ToList();
                //            else
                //                orders = orders.OrderByDescending(u => u.UserDefinedCol10).ToList();
                //        }
                //        break;
                //    default:
                //        break;
                //}
                #endregion


                JArray groups = (JArray)jsonSearchOption["groups"];
                if (groups != null)
                {
                    foreach (JObject item in groups)
                    {
                        string item_groupOp = (string)item["groupOp"];
                        JArray item_groups = (JArray)item["groups"];
                        JArray item_rules = (JArray)item["rules"];
                        string item_rule0_field = (string)(((JObject)item_rules[0])["field"]);
                        string item_rule0_op = (string)(((JObject)item_rules[0])["op"]);
                        string item_rule0_data = (string)(((JObject)item_rules[0])["data"]);

                        string item_rule1_field = (string)(((JObject)item_rules[1])["field"]);
                        string item_rule1_op = (string)(((JObject)item_rules[1])["op"]);
                        string item_rule1_data = (string)(((JObject)item_rules[1])["data"]);
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return orders;
        }

    }
}
