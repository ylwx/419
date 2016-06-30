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
        static void GenerateInvoice(int orderId)
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            MyInvoice _invoice = new MyInvoice();

            List<V_Invoice> list = db.V_Invoice.Where(u => u.OrderID == orderId).OrderBy(u => u.ServiceNatureID).Select(u => u).ToList();

            if (list != null && list.Count > 0)
            {
                _invoice.CustormerID = (int)list[0].CustomerID;
                _invoice.CustomerName = list[0].CustomerName;
                _invoice.OrderID = (int)list[0].OrderID;
                _invoice.OrderCode = list[0].OrderCode;

                Dictionary<int, string> dicServiceNature = new Dictionary<int, string>();
                var services = list.Select(u => new {u.ServiceNatureID, u.ServiceNatureLabel}).Distinct().ToList();

                Dictionary<int, List<MyScheduler>> dicSchedulers = new Dictionary<int, List<MyScheduler>>();

                double grandTotal = 0;

                if(services != null && services.Count > 0)
                {
                    foreach(var item in services)
                    {
                        dicServiceNature.Add((int)item.ServiceNatureID, item.ServiceNatureLabel);

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

                                    sch.WorkTimeConsumption = TugBusinessLogic.Utils.CalculateTimeConsumption(iDiffHour, iDiffMinute, (int)list[0].TimeTypeID, list[0].TimeTypeValue, list[0].TimeTypeLabel);

                                    sch.UnitPrice = (double)schedulers[0].UnitPrice;
                                    if (((int)list[0].BillingTypeID == 5 || list[0].BillingTypeValue == "0" || list[0].BillingTypeLabel == "全包")
                                        || ((int)list[0].BillingTypeID == 6 || list[0].BillingTypeValue == "1" || list[0].BillingTypeLabel == "全包加特别条款"))
                                        sch.Price = (double)schedulers[0].UnitPrice;
                                    else
                                        sch.Price = (double)schedulers[0].UnitPrice * sch.WorkTimeConsumption;

                                    sch.SubTotaHKS = sch.Price;

                                    sch.RopeUsed = schedulers[0].RopeUsed;
                                    sch.RopeNum = (int)schedulers[0].RopeNum;
                                    sch.Remark = schedulers[0].OrderSchedulerRemark;


                                    double total = sch.SubTotaHKS;

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

                                        if (((int)list[0].BillingTypeID == 5 || list[0].BillingTypeValue == "0" || list[0].BillingTypeLabel == "全包")
                                        || ((int)list[0].BillingTypeID == 6 || list[0].BillingTypeValue == "1" || list[0].BillingTypeLabel == "全包加特别条款"))
                                            bit.Price = subItem.UnitPrice;
                                        else
                                            bit.Price = subItem.UnitPrice * sch.WorkTimeConsumption;

                                        
                                        bit.Currency = subItem.Currency;
                                        bit.TypeID = subItem.PositionTypeID;
                                        bit.TypeValue = subItem.PositionTypeValue;
                                        bit.TypeLabel = subItem.PositionTypeLabel;

                                        

                                        if (subItem.PositionTypeID == 13 || list[0].BillingTypeValue == "0" || list[0].BillingTypeLabel == "上")
                                            sch.SubTotaHKS += (double)bit.Price;

                                        total += (double)bit.Price;

                                        billingItems.Add(bit);
                                    }
                                    #endregion

                                    sch.TotalHKs = total;

                                    sch.BillingItems = billingItems;

                                    grandTotal += total;
                                }

                                listScheduler.Add(sch);
                            }

                        }

                        dicSchedulers.Add((int)item.ServiceNatureID, listScheduler);
                        
                    }
                }

                _invoice.ServiceNature = dicServiceNature;
                _invoice.Schedulers = dicSchedulers;

                _invoice.BillingID = list[0].BillingID;
                _invoice.BillingCode = list[0].BillingCode;
                _invoice.BillingTypeID = (int)list[0].BillingTypeID;
                _invoice.BillingTypeValue = list[0].BillingTypeValue;
                _invoice.BillingTypeLabel = list[0].BillingTypeLabel;
                _invoice.TimeTypeID = (int)list[0].TimeTypeID;
                _invoice.TimeTypeValue = list[0].TimeTypeValue;
                _invoice.TimeTypeLabel = list[0].TimeTypeLabel;

        
                _invoice.GrandTotalHKS = grandTotal;
            } 
        }
    }
}
