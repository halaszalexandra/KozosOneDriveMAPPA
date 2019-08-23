namespace LogXExplorer.Module.Controllers
{
    partial class TransportViewController
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
            this.LogX_LcArrivedIntoLocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LogX_LcArrivedIntoLocation
            // 
            this.LogX_LcArrivedIntoLocation.Caption = "Láda megérkezik";
            this.LogX_LcArrivedIntoLocation.Category = "Edit";
            this.LogX_LcArrivedIntoLocation.ConfirmationMessage = null;
            this.LogX_LcArrivedIntoLocation.Id = "LogX_LcArrivedIntoLocation";
            this.LogX_LcArrivedIntoLocation.ToolTip = null;
            //this.LogX_LcArrivedIntoLocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_LcArrivedIntoLocation_Execute);
            // 
            // TransportViewController
            // 
            this.Actions.Add(this.LogX_LcArrivedIntoLocation);
            this.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.TransportOrder);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction LogX_LcArrivedIntoLocation;
    }
}
