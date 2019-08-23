using DevExpress.Persistent.Base;

namespace LogXExplorer.Module.Win
{
    public enum TransportTypeEnum
    {
        [ImageName("State_Priority_Low")]
        Mozgatás = 0,
        [ImageName("State_Priority_Normal")]
        Betárolás = 1,
        [ImageName("State_Priority_High")]
        Kitárolás = 2,
        [ImageName("State_Priority_Low")]
        Áttároláss = 3,
    }
}
