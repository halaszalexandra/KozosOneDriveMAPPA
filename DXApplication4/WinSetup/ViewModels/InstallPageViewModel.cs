namespace WinSetup.ViewModels
{
    class InstallPageViewModel : IWizardPageViewModel
    {
        public bool IsComplete
        {
            get;
            set;
        }
        public bool CanReturn { get { return !IsComplete; } }
    }
}