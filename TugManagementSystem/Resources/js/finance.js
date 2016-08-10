
function BillingItem(SchedulerID, ItemID, UnitPrice, Currency) {
    this.SchedulerID = SchedulerID;
    this.ItemID = ItemID;
    this.UnitPrice = UnitPrice;
    this.Currency = Currency;
    //this.IsVisible = IsVisible;
}


function SummaryItem(SchedulerID, Amount,FuelPrice, Currency, Hours)
{
    this.SchedulerID = SchedulerID;
    this.FuelPrice = FuelPrice;
    this.Amount = Amount;
    this.Hours = Hours;
    this.Currency = Currency;
}


function SpecialBillingItem(ServiceDate, ServiceNature, CustomerShipName, TugNumber, ServiceUnitPrice, FeulUnitPrice) {
    this.ServiceDate = ServiceDate;
    this.ServiceNature = ServiceNature;
    this.CustomerShipName = CustomerShipName;
    this.TugNumber = TugNumber;
    this.ServiceUnitPrice = ServiceUnitPrice;
    this.FeulUnitPrice = FeulUnitPrice;
}