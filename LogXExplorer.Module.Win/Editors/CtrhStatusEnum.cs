using DevExpress.Persistent.Base;

namespace LogXExplorer.Module.Win
{
    public enum CtrhStatusEnum
    {
        [ImageName("status_gray")]
        Uj = 0,
        [ImageName("status_red")]
        Elfogadott = 5,
        [ImageName("status_red")]
        Geplant = 10,
        [ImageName("status_yellow")]
        Teljesites_alatt = 15,
        [ImageName("status_blue")]
        InProduktion = 20,
        [ImageName("status_green")]
        Erledigt = 25,
        [ImageName("State_Priority_High")]
        Lezárt = 50,
        [ImageName("State_Priority_Low")]
        Elszámolt = 60,
        [ImageName("State_Priority_Low")]
        Sztornó = 99
    }
}
