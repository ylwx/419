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
                var services = list.Select(u => new { u.ServiceNatureID, u.ServiceNatureLabel, u.ServiceWorkDate, u.ServiceWorkPlace }).Distinct().ToList();
                //var services = db.V_Invoice.Where(u => u.OrderID == orderId).OrderBy(u => u.ServiceNatureID).Select(u => new {u.ServiceNatureID, u.ServiceNatureLabel}).Distinct().ToList();

                Dictionary<int, List<MyScheduler>> dicSchedulers = new Dictionary<int, List<MyScheduler>>();

                double grandTotal = 0;

                if (services != null && services.Count > 0)
                {
                    foreach (var item in services)
                    {
                        MyService ms = new MyService();
                        ms.ServiceId = (int)item.ServiceNatureID;
                        ms.ServiceName = item.ServiceNatureLabel;
                        ms.ServiceWorkDate = item.ServiceWorkDate;
                        ms.ServiceWorkPlace = item.ServiceWorkPlace;
                        dicServiceNature.Add(ms.ServiceId, ms);

                        var ships = list.Where(u => u.ServiceNatureID == item.ServiceNatureID)
                            .Select(u => new { u.TugID, u.TugName1, u.TugName2, u.TugSimpleName, u.Power }).Distinct()
                            .OrderBy(u => u.TugName1).ToList();

                        List<MyScheduler> listScheduler = new List<MyScheduler>();

                        if (ships != null && ships.Count > 0)
                        {
                            foreach (var ship in ships)
                            {
                                MyScheduler sch = new MyScheduler();
                                sch.TugID = (int)ship.TugID;
                                sch.TugCnName = ship.TugName1;
                                sch.TugEnName = ship.TugName2;
                                sch.TugSimpleName = ship.TugSimpleName;
                                sch.TugPower = ship.Power;
                                var schedulers = list.Where(u => u.ServiceNatureID == item.ServiceNatureID && u.TugID == ship.TugID)
                                    .OrderBy(u => u.OrderID).OrderBy(u => u.ServiceNatureID)
                                    .Select(u => new
                                    {
                                        u.TugID,
                                        u.TugName1,
                                        u.TugName2,
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
                                        u.PositionTypeID
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


                                        if(subItem.BillingItemValue[0] == 'A')
                                        
                                            upTotalPrice += (double)bit.Price;
                                        else if (subItem.BillingItemValue[0] == 'B')
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


        static public MyInvoice NewInvoice(int orderId, string customerBillingScheme,
            int billingTypeId, string billingTypeValue, string billingTypeLabel,
            int timeTypeId, string timeTypeValue, string timeTypeLabel, double discount)
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            double grandTotal = 0.0;

            MyInvoice _invoice = new MyInvoice();

            _invoice.OrderID = orderId;
            //_invoice.OrderCode = ;
            //_invoice

            int billingTemplateId = Convert.ToInt32(customerBillingScheme.Split('%')[0].Split('~')[0]);

            List<V_BillingItemTemplate> listBillingItemTemplate = GetCustomerBillSchemeItems(billingTemplateId);

            _invoice.BillingTypeID = billingTypeId;
            _invoice.BillingTypeValue = billingTypeValue;
            _invoice.BillingTypeLabel = billingTypeLabel;
            _invoice.TimeTypeID = timeTypeId;
            _invoice.TimeTypeValue = timeTypeValue;
            _invoice.TimeTypeLabel = timeTypeLabel;
            _invoice.Discount = discount;

            var list = db.V_OrderScheduler.Where(u => u.OrderID == orderId).OrderBy(u => u.ServiceNatureID).Select(u => u);

            var services = list.Select(u => new { u.ServiceNatureID, u.ServiceNatureLabel, u.ServiceWorkDate, u.ServiceWorkPlace }).Distinct().ToList();

            if (services != null)
            {
                Dictionary<int, MyService> dicService = new Dictionary<int, MyService>();
                Dictionary<int, List<MyScheduler>> dicScheduler = new Dictionary<int, List<MyScheduler>>();
                foreach (var service in services)
                {
                    MyService ms = new MyService();
                    ms.ServiceId = (int)service.ServiceNatureID;
                    ms.ServiceName = service.ServiceNatureLabel;
                    ms.ServiceWorkDate = service.ServiceWorkDate;
                    ms.ServiceWorkPlace = service.ServiceWorkPlace;
                    dicService.Add(ms.ServiceId, ms);

                    var schedulers = list.Where(u => u.ServiceNatureID == (int)service.ServiceNatureID)
                        .Select(u => new
                        {
                            u.IDX,
                            u.TugID,
                            u.TugName1,
                            u.TugName2,
                            u.TugSimpleName,
                            u.Power,
                            u.DepartBaseTime,
                            u.ArrivalBaseTime,
                            u.RopeUsed,
                            u.RopeNum,
                            u.Remark
                        }).ToList();

                    if (schedulers != null)
                    {
                        List<MyScheduler> lstScheduler = new List<MyScheduler>();    

                        foreach (var scheduler in schedulers)
                        {
                            MyScheduler mySch = new MyScheduler();
                            mySch.SchedulerID = scheduler.IDX;
                            mySch.TugID = (int)scheduler.TugID;
                            mySch.TugCnName = scheduler.TugName1;
                            mySch.TugEnName = scheduler.TugName2;
                            mySch.TugSimpleName = scheduler.TugSimpleName;
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

                            #region 全包
                            if (_invoice.BillingTypeID == 6 || _invoice.BillingTypeValue == "0" || _invoice.BillingTypeLabel == "全包")
                            {
                                V_BillingItemTemplate tmp = listBillingItemTemplate.FirstOrDefault(u => u.ItemID == service.ServiceNatureID);
                                if(tmp != null)
                                {
                                    mySch.UnitPrice = mySch.Price = mySch.SubTotaHKS = (double)tmp.UnitPrice;
                                }
                                else
                                {
                                    mySch.UnitPrice = mySch.Price = mySch.SubTotaHKS = 0;
                                }
                                mySch.DiscountSubTotalHKS = Math.Round(mySch.SubTotaHKS * _invoice.Discount, 2);
                                mySch.TotalHKs = mySch.DiscountSubTotalHKS;
                                grandTotal += mySch.TotalHKs;
                            }
                            #endregion

                            #region 半包
                            if (_invoice.BillingTypeID == 7 || _invoice.BillingTypeValue == "1" || _invoice.BillingTypeLabel == "全包加特别条款")
                            {
                                double top_total_price = 0.0, mid_total_price = 0.0, bottom_total_price = 0.0;

                                V_BillingItemTemplate tmp = listBillingItemTemplate.FirstOrDefault(u => u.ItemID == service.ServiceNatureID);
                                if (tmp != null)
                                {
                                    mySch.UnitPrice = mySch.Price = (double)tmp.UnitPrice;
                                }
                                else
                                {
                                    mySch.UnitPrice = mySch.Price = 0;
                                }

                                top_total_price += mySch.Price;


                                List<CustomField> banbaoShowItems = GetBanBaoShowItems();
                                List<MyBillingItem> lstMyBillingItems = new List<MyBillingItem>();

                                #region 条目费用计算

                                if (banbaoShowItems != null)
                                {
                                    foreach (CustomField item in banbaoShowItems)
                                    {
                                        tmp = listBillingItemTemplate.FirstOrDefault(u => u.ItemID == item.IDX);

                                        MyBillingItem mbi = new MyBillingItem();
                                        if(tmp == null){

                                            if (item.IDX == 17 || item.CustomValue == "B10" || item.CustomLabel == "25%港外附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price * 0.25, 2);
                                            }
                                            else if (item.IDX == 18 || item.CustomValue == "B11" || item.CustomLabel == "50% 18时至22时附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price * 0.5, 2);
                                            }
                                            else if (item.IDX == 19 || item.CustomValue == "B12" || item.CustomLabel == "100% 22时至08时附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price, 2);
                                            }
                                            else if (item.IDX == 20 || item.CustomValue == "B13" || item.CustomLabel == "100%假日附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price, 2);
                                            }
                                            else if (item.IDX == 21 || item.CustomValue == "B14" || item.CustomLabel == "100%台风附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price, 2);
                                            }
                                            else if (item.IDX == 22 || item.CustomValue == "C15" || item.CustomLabel == "使用3600BHP以上的拖轮+15%")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price * 0.15, 2);
                                            }
                                            else
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = -1;
                                                mbi.ItemValue = "";
                                                mbi.ItemLabel = "";
                                                mbi.UnitPrice = 0;
                                                mbi.Price = 0;
                                            }

                                            if (item.CustomValue.StartsWith("B")) { mid_total_price += (double)mbi.Price; }
                                            if (item.CustomValue.StartsWith("C")) { bottom_total_price += (double)mbi.Price; }
                                        }
                                        else{
                                            mbi.Currency = tmp.Currency;
                                            mbi.ItemID = tmp.ItemID;
                                            mbi.ItemValue = tmp.ItemValue;
                                            mbi.ItemLabel = tmp.ItemLabel;
                                            mbi.UnitPrice = tmp.UnitPrice;

                                            if (tmp.ItemID == 23 || tmp.ItemValue == "C80" || tmp.ItemLabel == "燃油附加费")
                                            {
                                                mbi.Price = tmp.UnitPrice * mySch.WorkTimeConsumption; 
                                            }
                                            else if (tmp.ItemID == 24 || tmp.ItemValue == "C81" || tmp.ItemLabel == "拖缆费")
                                            {
                                                mbi.Price = tmp.UnitPrice * mySch.RopeNum;
                                            }
                                            else
                                            {
                                                mbi.Price = tmp.UnitPrice;
                                            }

                                            if (item.CustomValue.StartsWith("B")) { mid_total_price += (double)mbi.Price; }
                                            if (item.CustomValue.StartsWith("C")) { bottom_total_price += (double)mbi.Price; }

                                            mbi.TypeID = tmp.TypeID;
                                            mbi.TypeValue = tmp.TypeValue;
                                            mbi.TypeLabel = tmp.TypeLabel;
                                        }

                                        lstMyBillingItems.Add(mbi);
                                    }
                                }
                                #endregion

                                mySch.BillingItems = lstMyBillingItems;

                                mySch.SubTotaHKS = top_total_price + mid_total_price;
                                mySch.DiscountSubTotalHKS = Math.Round(mySch.SubTotaHKS * _invoice.Discount, 2);
                                //totalPrice += mySch.DiscountSubTotalHKS;

                                mySch.TotalHKs = mySch.DiscountSubTotalHKS + bottom_total_price;
                                grandTotal += mySch.TotalHKs;

                            }
                            #endregion

                            #region 条款
                            if (_invoice.BillingTypeID == 8 || _invoice.BillingTypeValue == "2" || _invoice.BillingTypeLabel == "条款")
                            {
                                double top_total_price = 0.0, mid_total_price = 0.0, bottom_total_price = 0.0;

                                V_BillingItemTemplate tmp = listBillingItemTemplate.FirstOrDefault(u => u.ItemID == service.ServiceNatureID);
                                if (tmp != null)
                                {
                                    mySch.UnitPrice = (double)tmp.UnitPrice;
                                    mySch.Price = mySch.UnitPrice * mySch.WorkTimeConsumption;
                                }
                                else
                                {
                                    mySch.UnitPrice = mySch.Price = 0;
                                }

                                top_total_price += mySch.Price;


                                List<CustomField> banbaoShowItems = GetTiaoKuanShowItems();
                                List<MyBillingItem> lstMyBillingItems = new List<MyBillingItem>();

                                #region 条目费用计算

                                if (banbaoShowItems != null)
                                {
                                    foreach (CustomField item in banbaoShowItems)
                                    {
                                        tmp = listBillingItemTemplate.FirstOrDefault(u => u.ItemID == item.IDX);

                                        MyBillingItem mbi = new MyBillingItem();
                                        if (tmp == null)
                                        {

                                            if (item.IDX == 17 || item.CustomValue == "B10" || item.CustomLabel == "25%港外附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price * 0.25, 2);
                                            }
                                            else if (item.IDX == 18 || item.CustomValue == "B11" || item.CustomLabel == "50% 18时至22时附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price * 0.5, 2);
                                            }
                                            else if (item.IDX == 19 || item.CustomValue == "B12" || item.CustomLabel == "100% 22时至08时附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price, 2);
                                            }
                                            else if (item.IDX == 20 || item.CustomValue == "B13" || item.CustomLabel == "100%假日附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price, 2);
                                            }
                                            else if (item.IDX == 21 || item.CustomValue == "B14" || item.CustomLabel == "100%台风附加费")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price, 2);
                                            }
                                            else if (item.IDX == 22 || item.CustomValue == "C15" || item.CustomLabel == "使用3600BHP以上的拖轮+15%")
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = item.IDX;
                                                mbi.ItemValue = item.CustomValue;
                                                mbi.ItemLabel = item.CustomLabel;
                                                mbi.UnitPrice = mbi.Price = Math.Round(mySch.Price * 0.15, 2);
                                            }
                                            else
                                            {
                                                mbi.Currency = "港币";
                                                mbi.ItemID = -1;
                                                mbi.ItemValue = "";
                                                mbi.ItemLabel = "";
                                                mbi.UnitPrice = 0;
                                                mbi.Price = 0;
                                            }

                                            if (item.CustomValue.StartsWith("B")) { mid_total_price += (double)mbi.Price; }
                                            if (item.CustomValue.StartsWith("C")) { bottom_total_price += (double)mbi.Price; }
                                        }
                                        else
                                        {
                                            mbi.Currency = tmp.Currency;
                                            mbi.ItemID = tmp.ItemID;
                                            mbi.ItemValue = tmp.ItemValue;
                                            mbi.ItemLabel = tmp.ItemLabel;
                                            mbi.UnitPrice = tmp.UnitPrice;

                                            if (tmp.ItemID == 23 || tmp.ItemValue == "C80" || tmp.ItemLabel == "燃油附加费")
                                            {
                                                mbi.Price = tmp.UnitPrice * mySch.WorkTimeConsumption;
                                            }
                                            else if (tmp.ItemID == 24 || tmp.ItemValue == "C81" || tmp.ItemLabel == "拖缆费")
                                            {
                                                mbi.Price = tmp.UnitPrice * mySch.RopeNum;
                                            }
                                            else
                                            {
                                                mbi.Price = tmp.UnitPrice;
                                            }

                                            if (item.CustomValue.StartsWith("B")) { mid_total_price += (double)mbi.Price; }
                                            if (item.CustomValue.StartsWith("C")) { bottom_total_price += (double)mbi.Price; }

                                            mbi.TypeID = tmp.TypeID;
                                            mbi.TypeValue = tmp.TypeValue;
                                            mbi.TypeLabel = tmp.TypeLabel;
                                        }

                                        lstMyBillingItems.Add(mbi);
                                    }
                                }
                                #endregion

                                mySch.BillingItems = lstMyBillingItems;

                                mySch.SubTotaHKS = top_total_price + mid_total_price;
                                mySch.DiscountSubTotalHKS = Math.Round(mySch.SubTotaHKS * _invoice.Discount, 2);
                                //totalPrice += mySch.DiscountSubTotalHKS;

                                mySch.TotalHKs = mySch.DiscountSubTotalHKS + bottom_total_price;
                                grandTotal += mySch.TotalHKs;
                            }
                            #endregion

                            lstScheduler.Add(mySch);
                        }
                        dicScheduler.Add((int)service.ServiceNatureID, lstScheduler);

                    }

                }

                _invoice.ServiceNature = dicService;
                _invoice.Schedulers = dicScheduler;
            }

            _invoice.GrandTotalHKS = grandTotal;
            

            return _invoice;
        }


        static public List<V_BillingTemplate> GetCustomerBillSchemes(int custId)
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            List<V_BillingTemplate> list = new List<V_BillingTemplate>();
            if(custId == -1)
                list = db.V_BillingTemplate.Where(u => u.CustomerCode == "-1").OrderBy(u => u.BillingTemplateName).ToList();
            else
                list = db.V_BillingTemplate.Where(u =>u.CustomerCode == "-1" || u.CustomerID == custId).OrderBy(u => u.BillingTemplateName).ToList();

            return list;
        }

        static public List<V_BillingItemTemplate> GetCustomerBillSchemeItems(int billSchemeId)
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            List<V_BillingItemTemplate> list = db.V_BillingItemTemplate.Where(u => u.BillingTemplateID == billSchemeId).OrderBy(u => u.TypeValue).OrderBy(u=>u.ItemValue).ToList();

            return list;
        }

        /// <summary>
        /// 获取半包类型账单的条目显示项
        /// </summary>
        /// <returns></returns>
        static public List<CustomField> GetBanBaoShowItems()
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            List<CustomField> list = db.CustomField.Where(u => u.CustomName == "BillingItemTemplate.ItemID" && u.FormulaStr.Substring(1,1) == "1").OrderBy(u => u.CustomValue).ToList();

            return list;
        }

        /// <summary>
        /// 获取条款类型账单的条目显示项
        /// </summary>
        /// <returns></returns>
        static public List<CustomField> GetTiaoKuanShowItems()
        {
            TugDataModel.TugDataEntities db = new TugDataModel.TugDataEntities();

            List<CustomField> list = db.CustomField.Where(u => u.CustomName == "BillingItemTemplate.ItemID" && u.FormulaStr.Substring(2, 1) == "1").OrderBy(u => u.CustomValue).ToList();

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
                switch (orderField)
                {
                    case "":
                        {
                            //if(orderMethod.ToLower().Equals("asc"))
                            //    orders = orders.OrderBy(u => u.IDX).ToList();
                            //else
                            orders = orders.OrderByDescending(u => u.OrderID).ToList();
                        }
                        break;
                    case "CustomerName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.CustomerName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.CustomerName).ToList();
                        }
                        break;
                    case "OrderCode":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.OrderCode).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.OrderCode).ToList();
                        }
                        break;

                    case "WorkDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkDate).ToList();
                        }
                        break;
                    case "WorkTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkTime).ToList();
                        }
                        break;
                    case "EstimatedCompletionTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.EstimatedCompletionTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.EstimatedCompletionTime).ToList();
                        }
                        break;
                    case "ShipName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ShipName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ShipName).ToList();
                        }
                        break;


                    case "ServiceNatureNames":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ServiceNatureNames).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ServiceNatureNames).ToList();
                        }
                        break;
                    case "WorkStateLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkStateLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkStateLabel).ToList();
                        }
                        break;

                    case "BillingCode":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingCode).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingCode).ToList();
                        }
                        break;

                    case "BillingName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingName).ToList();
                        }
                        break;

                    case "BillingTypeLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingTypeLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingTypeLabel).ToList();
                        }
                        break;
                    case "TimeTypeLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.TimeTypeLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.TimeTypeLabel).ToList();
                        }
                        break;
                    case "Amount":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Amount).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Amount).ToList();
                        }
                        break;
                    case "BillingRemark":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingRemark).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingRemark).ToList();
                        }
                        break;
                    case "Month":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Month).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Month).ToList();
                        }
                        break;
                    case "TimesNo":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.TimesNo).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.TimesNo).ToList();
                        }
                        break;
                    case "Status":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Status).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Status).ToList();
                        }
                        break;
                    case "Phase":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Phase).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Phase).ToList();
                        }
                        break;
                    case "BillingCreateDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingCreateDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingCreateDate).ToList();
                        }
                        break;
                    case "BillingLastUpDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingLastUpDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingLastUpDate).ToList();
                        }
                        break;


                    default:
                        break;
                }

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
                ParameterExpression parameter = Expression.Parameter(typeof(V_OrderBilling));

                if (rules != null)
                {
                    foreach (JObject item in rules)
                    {
                        string field = (string)item["field"];
                        string op = (string)item["op"];
                        string data = (string)item["data"];

                        #region 根据各字段条件进行条件表达式拼接
                        switch (field)
                        {
                            #region IsGuest
                            case "IsGuest":
                                {
                                    Expression cdt = null;

                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                if (data != "全部")
                                                {
                                                    //orders = orders.Where(u => u.IsGuest == data).ToList();
                                                    cdt = Expression.Equal(Expression.PropertyOrField(parameter, "IsGuest"), Expression.Constant(data));
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region CustomerName
                            case "CustomerName":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.CustomerName.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "CustomerName"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.CustomerName.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "CustomerName"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.CustomerName.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "CustomerName"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.CustomerName.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "CustomerName"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region OrderCode
                            case "OrderCode":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.Code.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "OrderCode"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.Code.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "OrderCode"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.Code.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "OrderCode"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.Code.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "OrderCode"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region WorkDate
                            case "WorkDate":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.WorkDate == data.Trim()).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.Equal(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                //orders = orders.Where(u => u.WorkDate.CompareTo(data.Trim()) == -1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                //orders = orders.Where(u => u.WorkDate.CompareTo(data.Trim()) == -1 || u.WorkDate.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                //orders = orders.Where(u => u.WorkDate.CompareTo(data.Trim()) == 1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThan(tmp, Expression.Constant(typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                //orders = orders.Where(u => u.WorkDate.CompareTo(data.Trim()) == 1 || u.WorkDate.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region WorkTime
                            case "WorkTime":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.WorkTime == data.Trim()).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.Equal(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                //orders = orders.Where(u => u.WorkTime.CompareTo(data.Trim()) == -1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                //orders = orders.Where(u => u.WorkTime.CompareTo(data.Trim()) == -1 || u.WorkTime.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                //orders = orders.Where(u => u.WorkTime.CompareTo(data.Trim()) == 1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThan(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                //orders = orders.Where(u => u.WorkTime.CompareTo(data.Trim()) == 1 || u.WorkTime.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "WorkTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region EstimatedCompletionTime
                            case "EstimatedCompletionTime":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.EstimatedCompletionTime == data.Trim()).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), Expression.Constant(data.Trim()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                //orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data.Trim()) == -1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                //orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data.Trim()) == -1 || u.EstimatedCompletionTime.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                //orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data.Trim()) == 1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThan(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                //orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data.Trim()) == 1 || u.EstimatedCompletionTime.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "EstimatedCompletionTime"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region ShipName
                            case "ShipName":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.ShipName.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "ShipName"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.ShipName.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "ShipName"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.ShipName.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "ShipName"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.ShipName.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "ShipName"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region ServiceNatureNames
                            case "ServiceNatureNames":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.ServiceNatureNames.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "ServiceNatureNames"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.ServiceNatureNames.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "ServiceNatureNames"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.ServiceNatureNames.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "ServiceNatureNames"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.ServiceNatureNames.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "ServiceNatureNames"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region WorkStateLabel
                            case "WorkStateLabel":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                int workStateId = Convert.ToInt32(data.Split('~')[0]);
                                                if (workStateId != -1)
                                                {
                                                    //orders = orders.Where(u => u.WorkStateID == workStateId).ToList();
                                                    cdt = Expression.Equal(Expression.PropertyOrField(parameter, "WorkStateID"), Expression.Constant(workStateId, typeof(Nullable<int>)));
                                                }

                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion



                            #region BillingCode
                            case "BillingCode":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "BillingCode"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingCode"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingCode"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingCode"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region BillingName
                            case "BillingName":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "BillingName"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingName"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingName"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingName"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region BillingTypeLabel
                            case "BillingTypeLabel":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                int workStateId = Convert.ToInt32(data.Split('~')[0]);
                                                if (workStateId != -1)
                                                {
                                                    //orders = orders.Where(u => u.WorkStateID == workStateId).ToList();
                                                    cdt = Expression.Equal(Expression.PropertyOrField(parameter, "BillingTypeID"), Expression.Constant(workStateId, typeof(Nullable<int>)));
                                                }

                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region TimeTypeLabel
                            case "TimeTypeLabel":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                int workStateId = Convert.ToInt32(data.Split('~')[0]);
                                                if (workStateId != -1)
                                                {
                                                    //orders = orders.Where(u => u.WorkStateID == workStateId).ToList();
                                                    cdt = Expression.Equal(Expression.PropertyOrField(parameter, "TimeTypeID"), Expression.Constant(workStateId, typeof(Nullable<int>)));
                                                }

                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region Amount
                            case "Amount":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "Amount"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "Amount"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "Amount"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "Amount"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "Amount"), Expression.Constant(Convert.ToDouble(data.Trim()), typeof(Nullable<double>)));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion


                            #region BillingRemark
                            case "BillingRemark":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.Remark.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "BillingRemark"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.Remark.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingRemark"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.Remark.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingRemark"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.Remark.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "BillingRemark"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region Month
                            case "Month":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "Month"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "Month"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "Month"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "Month"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region TimesNo
                            case "TimesNo":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "TimesNo"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "TimesNo"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "TimesNo"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "TimesNo"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "TimesNo"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region Status
                            case "Status":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().CompareTo(data.Trim().ToLower()) == 0).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "Status"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().StartsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "Status"), typeof(string).GetMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().EndsWith(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "Status"), typeof(string).GetMethod("EndsWith", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                //orders = orders.Where(u => u.WorkPlace.ToLower().Contains(data.Trim().ToLower())).ToList();
                                                cdt = Expression.Call(Expression.PropertyOrField(parameter, "Status"), typeof(string).GetMethod("Contains"), Expression.Constant(data.Trim().ToLower()));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region Phase
                            case "Phase":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "Phase"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "Phase"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.LessThanOrEqual(Expression.PropertyOrField(parameter, "Phase"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.GreaterThan(Expression.PropertyOrField(parameter, "Phase"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                //orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data.Trim()) || u.SmallTugNum == Convert.ToInt32(data.Trim())).ToList();
                                                cdt = Expression.GreaterThanOrEqual(Expression.PropertyOrField(parameter, "Phase"), Expression.Constant(Convert.ToInt32(data.Trim()), typeof(Nullable<int>)));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region BillingCreateDate
                            case "BillingCreateDate":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.CreateDate == data.Trim()).ToList();
                                                cdt = Expression.Equal(Expression.PropertyOrField(parameter, "BillingCreateDate"), Expression.Constant(data.Trim()));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                //orders = orders.Where(u => u.CreateDate.CompareTo(data.Trim()) == -1).ToList();
                                                //cdt = Expression.LessThan(Expression.PropertyOrField(parameter, "CreateDate"), Expression.Constant(data.Trim()));
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingCreateDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                //orders = orders.Where(u => u.CreateDate.CompareTo(data.Trim()) == -1 || u.CreateDate.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingCreateDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                //orders = orders.Where(u => u.CreateDate.CompareTo(data.Trim()) == 1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingCreateDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThan(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                //orders = orders.Where(u => u.CreateDate.CompareTo(data.Trim()) == 1 || u.CreateDate.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingCreateDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(0));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion

                            #region BillingLastUpDate
                            case "BillingLastUpDate":
                                {
                                    Expression cdt = null;
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                //orders = orders.Where(u => u.LastUpDate == data.Trim()).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingLastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.Equal(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == -1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingLastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThan(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == -1 || u.LastUpDate.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingLastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.LessThanOrEqual(tmp, Expression.Constant(0, typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == 1).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingLastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThan(tmp, Expression.Constant(typeof(Int32)));
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                //orders = orders.Where(u => u.LastUpDate.CompareTo(data.Trim()) == 1 || u.LastUpDate.CompareTo(data.Trim()) == 0).ToList();
                                                Expression tmp = Expression.Call(Expression.PropertyOrField(parameter, "BillingLastUpDate"), typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) }), Expression.Constant(data.Trim().ToLower(), typeof(String)));
                                                cdt = Expression.GreaterThanOrEqual(tmp, Expression.Constant(typeof(Int32)));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (cdt != null)
                                    {
                                        condition = Expression.AndAlso(condition, cdt);
                                    }
                                }
                                break;
                            #endregion



                            default:
                                break;
                        }
                        #endregion

                    }

                }

                #region 执行查询
                if (condition != null)
                {
                    var lamda = Expression.Lambda<Func<V_OrderBilling, bool>>(condition, parameter);
                    orders = db.V_OrderBilling.Where(lamda).Select(u => u).ToList<V_OrderBilling>();
                }
                else
                {
                    orders = db.V_OrderBilling.Select(u => u).ToList<V_OrderBilling>();
                }
                #endregion


                #region 对搜索结果根据排序字段和方式进行排序
                switch (orderField)
                {
                    case "":
                        {
                            //if(orderMethod.ToLower().Equals("asc"))
                            //    orders = orders.OrderBy(u => u.IDX).ToList();
                            //else
                            orders = orders.OrderByDescending(u => u.OrderID).ToList();
                        }
                        break;
                    case "CustomerName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.CustomerName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.CustomerName).ToList();
                        }
                        break;
                    case "OrderCode":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.OrderCode).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.OrderCode).ToList();
                        }
                        break;

                    case "WorkDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkDate).ToList();
                        }
                        break;
                    case "WorkTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkTime).ToList();
                        }
                        break;
                    case "EstimatedCompletionTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.EstimatedCompletionTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.EstimatedCompletionTime).ToList();
                        }
                        break;
                    case "ShipName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ShipName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ShipName).ToList();
                        }
                        break;


                    case "ServiceNatureNames":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ServiceNatureNames).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ServiceNatureNames).ToList();
                        }
                        break;
                    case "WorkStateLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkStateLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkStateLabel).ToList();
                        }
                        break;

                    case "BillingCode":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingCode).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingCode).ToList();
                        }
                        break;

                    case "BillingName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingName).ToList();
                        }
                        break;

                    case "BillingTypeLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingTypeLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingTypeLabel).ToList();
                        }
                        break;
                    case "TimeTypeLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.TimeTypeLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.TimeTypeLabel).ToList();
                        }
                        break;
                    case "Amount":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Amount).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Amount).ToList();
                        }
                        break;
                    case "BillingRemark":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingRemark).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingRemark).ToList();
                        }
                        break;
                    case "Month":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Month).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Month).ToList();
                        }
                        break;
                    case "TimesNo":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.TimesNo).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.TimesNo).ToList();
                        }
                        break;
                    case "Status":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Status).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Status).ToList();
                        }
                        break;
                    case "Phase":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Phase).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Phase).ToList();
                        }
                        break;
                    case "BillingCreateDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingCreateDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingCreateDate).ToList();
                        }
                        break;
                    case "BillingLastUpDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BillingLastUpDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BillingLastUpDate).ToList();
                        }
                        break;


                    default:
                        break;
                }

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
