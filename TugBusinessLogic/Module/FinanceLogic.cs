using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
