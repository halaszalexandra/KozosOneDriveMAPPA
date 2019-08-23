using DevExpress.Persistent.Base;

namespace LogXExplorer.Module.Win
{
    public enum TransportStatusEnum
    {
        [ImageName("State_Priority_Low")]
        Aktiv = 0,
        [ImageName("State_Priority_Normal")]
        Mozgásban = 2,
        [ImageName("State_Priority_High")]
        Felvevőponton = 4,
        [ImageName("State_Priority_Low")]
        Valami = 6,
        [ImageName("State_Priority_Normal")]
        Majdnem = 8,
        [ImageName("State_Priority_High")]
        Végrehajtva = 10

    }
}
