namespace LogXExplorer.Module.Controllers
{
    partial class ServiceController
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
            this.ResetTestData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.callSzia = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ResetTestData
            // 
            this.ResetTestData.Caption = "Reset Test Data";
            this.ResetTestData.Category = "Edit";
            this.ResetTestData.ConfirmationMessage = null;
            this.ResetTestData.Id = "ResetTestData";
            this.ResetTestData.ToolTip = null;
            this.ResetTestData.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ResetTestData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ResetTestData_Execute);
            // 
            // callSzia
            // 
            this.callSzia.Caption = "call Szia";
            this.callSzia.ConfirmationMessage = null;
            this.callSzia.Id = "callSzia";
            this.callSzia.ToolTip = null;
            this.callSzia.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.callSzia_Execute);
            // 
            // ServiceController
            // 
            this.Actions.Add(this.ResetTestData);
            this.Actions.Add(this.callSzia);
            this.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.BusinessSettings);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ResetTestData;
        private DevExpress.ExpressApp.Actions.SimpleAction callSzia;
    }
}
