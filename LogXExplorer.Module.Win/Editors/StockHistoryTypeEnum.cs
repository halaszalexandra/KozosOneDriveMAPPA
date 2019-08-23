using DevExpress.Persistent.Base;

namespace LogXExplorer.Module.Win
{
    public enum StockHistoryTypeEnum
    {
        [ImageName("State_Priority_Low")]
        Normal = 0,
        [ImageName("State_Priority_Normal")]
        Foglalt = 1,
        [ImageName("State_Priority_Normal")]
        Blokkolt = 2,
        [ImageName("State_Priority_High")]
        Selejt = 3
    }
}
