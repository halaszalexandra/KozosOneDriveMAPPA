namespace LogXExplorer.Module.Controllers
{
    partial class BusinessSettingsViewController
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
            this.LogX_ImportDatas = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportLoadCarrierToStorageLocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LogX_ImportDatas
            // 
            this.LogX_ImportDatas.Caption = "Adatok beolvasása";
            this.LogX_ImportDatas.ConfirmationMessage = null;
            this.LogX_ImportDatas.Id = "LogX_ImportDatas";
            this.LogX_ImportDatas.ToolTip = null;
            this.LogX_ImportDatas.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_ImportDatas_Execute);
            // 
            // ImportLoadCarrierToStorageLocation
            // 
            this.ImportLoadCarrierToStorageLocation.Caption = "Lokáció helyek feltöltése";
            this.ImportLoadCarrierToStorageLocation.ConfirmationMessage = null;
            this.ImportLoadCarrierToStorageLocation.Id = "ImportLoadCarrierToStorageLocation";
            this.ImportLoadCarrierToStorageLocation.ToolTip = null;
            this.ImportLoadCarrierToStorageLocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportLoadCarrierToStorageLocation_Execute);
            // 
            // BusinessSettingsViewController
            // 
            this.Actions.Add(this.LogX_ImportDatas);
            this.Actions.Add(this.ImportLoadCarrierToStorageLocation);
            this.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.BusinessSettings);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction LogX_ImportDatas;
        private DevExpress.ExpressApp.Actions.SimpleAction ImportLoadCarrierToStorageLocation;
    }
}
