using DevExpress.Persistent.Base;

namespace LogXExplorer.Module.Win
{
    public enum  StoLocStatusEnum
    {
        [ImageName("State_Priority_Low")]
        Üres = 0,
        [ImageName("State_Priority_Normal")]
        Használható_Üres = 1,
        [ImageName("State_Priority_Normal")]
        Használható_Készlettel = 2,
        [ImageName("State_Priority_High")]
        Foglalt = 3 
    }
}
