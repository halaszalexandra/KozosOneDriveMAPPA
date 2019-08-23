namespace LogXExplorer.Module.Controllers
{
    partial class CommonDetailViewController
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
            this.LogX_ItemNumUp = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_ItemNumDown = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LogX_ItemNumUp
            // 
            this.LogX_ItemNumUp.Caption = "LogX_ItemNumUp";
            this.LogX_ItemNumUp.ConfirmationMessage = null;
            this.LogX_ItemNumUp.Id = "LogX_ItemNumUp";
            this.LogX_ItemNumUp.ToolTip = null;
            this.LogX_ItemNumUp.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_ItemNumUp_Execute);
            // 
            // LogX_ItemNumDown
            // 
            this.LogX_ItemNumDown.Caption = "LogX_ItemNumDown";
            this.LogX_ItemNumDown.ConfirmationMessage = null;
            this.LogX_ItemNumDown.Id = "LogX_ItemNumDown";
            this.LogX_ItemNumDown.ToolTip = null;
            this.LogX_ItemNumDown.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_ItemNumDown_Execute);
            // 
            // CommonDetailViewController
            // 
            this.Actions.Add(this.LogX_ItemNumUp);
            this.Actions.Add(this.LogX_ItemNumDown);
            this.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.CommonTrDetail);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction LogX_ItemNumUp;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_ItemNumDown;
    }
}
