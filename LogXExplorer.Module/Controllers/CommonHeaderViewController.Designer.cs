namespace LogXExplorer.Module.Controllers
{
    partial class CommonHeaderViewController
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
            this.LogX_sh_AdatokElfogadva = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_ch_AdatokElfogadva = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_rh_AdatokElfogadva = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_ih_AdatokElfogadva = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_SelectCustomer = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.LogX_SelectCommonType = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // LogX_sh_AdatokElfogadva
            // 
            this.LogX_sh_AdatokElfogadva.Caption = "Adatok elfogadva";
            this.LogX_sh_AdatokElfogadva.Category = "RecordEdit";
            this.LogX_sh_AdatokElfogadva.ConfirmationMessage = null;
            this.LogX_sh_AdatokElfogadva.Id = "LogX_sh_AdatokElfogadva";
            this.LogX_sh_AdatokElfogadva.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.CommonTrHeader);
            this.LogX_sh_AdatokElfogadva.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.LogX_sh_AdatokElfogadva.ToolTip = null;
            this.LogX_sh_AdatokElfogadva.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.LogX_sh_AdatokElfogadva.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_sh_AdatokElfogadva_Execute);
            // 
            // LogX_ch_AdatokElfogadva
            // 
            this.LogX_ch_AdatokElfogadva.Caption = "Adatok elfogadva";
            this.LogX_ch_AdatokElfogadva.Category = "RecordEdit";
            this.LogX_ch_AdatokElfogadva.ConfirmationMessage = null;
            this.LogX_ch_AdatokElfogadva.Id = "LogX_ch_AdatokElfogadva";
            this.LogX_ch_AdatokElfogadva.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.CommonTrHeader);
            this.LogX_ch_AdatokElfogadva.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.LogX_ch_AdatokElfogadva.ToolTip = null;
            this.LogX_ch_AdatokElfogadva.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.LogX_ch_AdatokElfogadva.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_ch_AdatokElfogadva_Execute);
            // 
            // LogX_rh_AdatokElfogadva
            // 
            this.LogX_rh_AdatokElfogadva.Caption = "Adatok elfogadva";
            this.LogX_rh_AdatokElfogadva.Category = "RecordEdit";
            this.LogX_rh_AdatokElfogadva.ConfirmationMessage = null;
            this.LogX_rh_AdatokElfogadva.Id = "LogX_rh_AdatokElfogadva";
            this.LogX_rh_AdatokElfogadva.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.CommonTrHeader);
            this.LogX_rh_AdatokElfogadva.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.LogX_rh_AdatokElfogadva.ToolTip = null;
            this.LogX_rh_AdatokElfogadva.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.LogX_rh_AdatokElfogadva.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_rh_AdatokElfogadva_Execute);
            // 
            // LogX_ih_AdatokElfogadva
            // 
            this.LogX_ih_AdatokElfogadva.Caption = "Adatok elfogadva";
            this.LogX_ih_AdatokElfogadva.Category = "RecordEdit";
            this.LogX_ih_AdatokElfogadva.ConfirmationMessage = null;
            this.LogX_ih_AdatokElfogadva.Id = "LogX_ih_AdatokElfogadva";
            this.LogX_ih_AdatokElfogadva.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.CommonTrHeader);
            this.LogX_ih_AdatokElfogadva.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.LogX_ih_AdatokElfogadva.ToolTip = null;
            this.LogX_ih_AdatokElfogadva.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.LogX_ih_AdatokElfogadva.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_ih_AdatokElfogadva_Execute);
            // 
            // LogX_SelectCustomer
            // 
            this.LogX_SelectCustomer.AcceptButtonCaption = null;
            this.LogX_SelectCustomer.CancelButtonCaption = null;
            this.LogX_SelectCustomer.Caption = "Partner választás";
            this.LogX_SelectCustomer.ConfirmationMessage = null;
            this.LogX_SelectCustomer.Id = "LogX_SelectCustomer";
            this.LogX_SelectCustomer.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.LogX_SelectCustomer.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.LogX_SelectCustomer.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.LogX_SelectCustomer.ToolTip = null;
            this.LogX_SelectCustomer.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.LogX_SelectCustomer.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.LogX_SelectCustomer_CustomizePopupWindowParams);
            this.LogX_SelectCustomer.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.LogX_SelectCustomer_Execute);
            // 
            // LogX_SelectCommonType
            // 
            this.LogX_SelectCommonType.AcceptButtonCaption = null;
            this.LogX_SelectCommonType.CancelButtonCaption = null;
            this.LogX_SelectCommonType.Caption = "Tranzakció típus választás";
            this.LogX_SelectCommonType.ConfirmationMessage = null;
            this.LogX_SelectCommonType.Id = "LogX_SelectCommonType";
            this.LogX_SelectCommonType.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.LogX_SelectCommonType.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.LogX_SelectCommonType.ToolTip = null;
            this.LogX_SelectCommonType.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.LogX_SelectCommonType.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.LogX_SelectCommonType_CustomizePopupWindowParams);
            this.LogX_SelectCommonType.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.LogX_SelectCommonType_Execute);
            // 
            // CommonHeaderViewController
            // 
            this.Actions.Add(this.LogX_sh_AdatokElfogadva);
            this.Actions.Add(this.LogX_ch_AdatokElfogadva);
            this.Actions.Add(this.LogX_rh_AdatokElfogadva);
            this.Actions.Add(this.LogX_ih_AdatokElfogadva);
            this.Actions.Add(this.LogX_SelectCustomer);
            this.Actions.Add(this.LogX_SelectCommonType);
            this.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.CommonTrHeader);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction LogX_sh_AdatokElfogadva;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_ch_AdatokElfogadva;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_rh_AdatokElfogadva;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_ih_AdatokElfogadva;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction LogX_SelectCustomer;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction LogX_SelectCommonType;
    }
}
