namespace LogXExplorer.Module.Controllers
{
    partial class Autorefresh
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LogX_AutoRefreshON = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_AutoRefreshOff = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LogX_AutoRefreshON
            // 
            this.LogX_AutoRefreshON.Caption = "Automatikus frissítés bekapcsolás";
            this.LogX_AutoRefreshON.Category = "View";
            this.LogX_AutoRefreshON.ConfirmationMessage = null;
            this.LogX_AutoRefreshON.Id = "LogX_AutoRefreshON";
            this.LogX_AutoRefreshON.ToolTip = null;
            this.LogX_AutoRefreshON.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_AutoRefreshON.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_AutoRefreshON_Execute);
            // 
            // LogX_AutoRefreshOff
            // 
            this.LogX_AutoRefreshOff.Caption = "Automatikus frissítés kikapcsolás";
            this.LogX_AutoRefreshOff.Category = "View";
            this.LogX_AutoRefreshOff.ConfirmationMessage = null;
            this.LogX_AutoRefreshOff.Id = "LogX_AutoRefreshOff";
            this.LogX_AutoRefreshOff.ToolTip = null;
            this.LogX_AutoRefreshOff.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_AutoRefreshOff.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_AutoRefreshOff_Execute);
            // 
            // Autorefresh
            // 
            this.Actions.Add(this.LogX_AutoRefreshON);
            this.Actions.Add(this.LogX_AutoRefreshOff);
            this.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.CommonTrHeader);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction LogX_AutoRefreshON;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_AutoRefreshOff;
    }
}
