namespace WebUI.Data.Enums
{
    // define all enums used by EF models here

    public enum Frequency
    {
        Monthly = 1,
        Biweekly = 2,
        Weekly = 3
    }

    public enum BillingType
    {
        Hourly = 1,
        NonBillable = 2,
        FixedPrice = 3
    }
}
