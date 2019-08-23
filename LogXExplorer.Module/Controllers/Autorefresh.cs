using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

namespace LogXExplorer.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class Autorefresh : ViewController
    {

        System.Timers.Timer timer;
        bool autorefreshActive = false;

        public Autorefresh()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            timer = new System.Timers.Timer(10000);
            timer.SynchronizingObject = (ISynchronizeInvoke)Frame.Template;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            timer.Stop();
            timer.Dispose();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (autorefreshActive)
            {
                //MessageBox.Show("refresh");
                View.ObjectSpace.Refresh();
                View.Refresh();
            }
        }

        private void LogX_AutoRefreshON_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            autorefreshActive = true;
            
        }

        private void LogX_AutoRefreshOff_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            autorefreshActive = false;

        }
    }
}