using DevExpress.Persistent.Base;

namespace LogXExplorer.Module.Win
{
    public enum CtrhStatusEnum
    {
        [ImageName("status_gray")]
        Neu = 0,
        [ImageName("status_red")]
        Offen = 5,
        [ImageName("status_red")]
        Geplant = 10,
        [ImageName("status_yellow")]
        Gesperrt = 15,
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
