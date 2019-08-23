namespace LogXExplorer.Module.Controllers
{
    partial class IocpNewController
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
            this.LogX_CallCtrh = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.LogX_GetProductFromLc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_SendLcBack = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_TypeChangeBack = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_CallLc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_TypeChangeRemoval = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_TypeChangeInventory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_TypeChangeComission = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_TypeChangeStorage = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_CloseCtrH = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_LcIsHere = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_PutProductIntoLc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LogX_TypeChangeService = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LogX_CallCtrh
            // 
            this.LogX_CallCtrh.AcceptButtonCaption = null;
            this.LogX_CallCtrh.CancelButtonCaption = null;
            this.LogX_CallCtrh.Caption = "Ügylet választás";
            this.LogX_CallCtrh.Category = "RecordEdit";
            this.LogX_CallCtrh.ConfirmationMessage = null;
            this.LogX_CallCtrh.Id = "LogX_CallCtrh";
            this.LogX_CallCtrh.ToolTip = null;
            this.LogX_CallCtrh.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_CallCtrh.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.LogX_CallCtrh_PopupWindow);
            this.LogX_CallCtrh.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.LogX_CallCtrh_Execute);
            // 
            // LogX_GetProductFromLc
            // 
            this.LogX_GetProductFromLc.Caption = "Termék kivét ládából";
            this.LogX_GetProductFromLc.Category = "RecordEdit";
            this.LogX_GetProductFromLc.ConfirmationMessage = null;
            this.LogX_GetProductFromLc.Id = "LogX_GetProductFromLc";
            this.LogX_GetProductFromLc.ToolTip = null;
            this.LogX_GetProductFromLc.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_GetProductFromLc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_GetProductFromLc_Execute);
            // 
            // LogX_SendLcBack
            // 
            this.LogX_SendLcBack.Caption = "Láda beküldése a tárolási helyre";
            this.LogX_SendLcBack.Category = "RecordEdit";
            this.LogX_SendLcBack.ConfirmationMessage = null;
            this.LogX_SendLcBack.Id = "LogX_SendLcBack";
            this.LogX_SendLcBack.ToolTip = null;
            this.LogX_SendLcBack.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_SendLcBack.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_SendLcBack_Execute);
            // 
            // LogX_TypeChangeBack
            // 
            this.LogX_TypeChangeBack.Caption = "Vissza";
            this.LogX_TypeChangeBack.Category = "Edit";
            this.LogX_TypeChangeBack.ConfirmationMessage = null;
            this.LogX_TypeChangeBack.Id = "LogX_TypeChangeBack";
            this.LogX_TypeChangeBack.ToolTip = null;
            this.LogX_TypeChangeBack.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_TypeChangeBack.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_TypeChangeBack_Execute);
            // 
            // LogX_CallLc
            // 
            this.LogX_CallLc.Caption = "Ládák kihívása";
            this.LogX_CallLc.Category = "RecordEdit";
            this.LogX_CallLc.ConfirmationMessage = null;
            this.LogX_CallLc.Id = "LogX_CallLc";
            this.LogX_CallLc.ToolTip = null;
            this.LogX_CallLc.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_CallLc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_CallLc_Execute);
            // 
            // LogX_TypeChangeRemoval
            // 
            this.LogX_TypeChangeRemoval.Caption = "Kitárolás";
            this.LogX_TypeChangeRemoval.Category = "Edit";
            this.LogX_TypeChangeRemoval.ConfirmationMessage = null;
            this.LogX_TypeChangeRemoval.Id = "LogX_TypeChangeRemoval";
            this.LogX_TypeChangeRemoval.ToolTip = null;
            this.LogX_TypeChangeRemoval.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_TypeChangeRemoval.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_TypeChangeRemoval_Execute);
            // 
            // LogX_TypeChangeInventory
            // 
            this.LogX_TypeChangeInventory.Caption = "Leltár";
            this.LogX_TypeChangeInventory.Category = "Edit";
            this.LogX_TypeChangeInventory.ConfirmationMessage = null;
            this.LogX_TypeChangeInventory.Id = "LogX_TypeChangeInventory";
            this.LogX_TypeChangeInventory.ToolTip = null;
            this.LogX_TypeChangeInventory.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_TypeChangeInventory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_TypeChangeInventory_Execute);
            // 
            // LogX_TypeChangeComission
            // 
            this.LogX_TypeChangeComission.Caption = "Komissió";
            this.LogX_TypeChangeComission.Category = "Edit";
            this.LogX_TypeChangeComission.ConfirmationMessage = null;
            this.LogX_TypeChangeComission.Id = "LogX_TypeChangeComission";
            this.LogX_TypeChangeComission.ToolTip = null;
            this.LogX_TypeChangeComission.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_TypeChangeComission.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_TypeChangeComission_Execute);
            // 
            // LogX_TypeChangeStorage
            // 
            this.LogX_TypeChangeStorage.Caption = "Betárolás";
            this.LogX_TypeChangeStorage.Category = "Edit";
            this.LogX_TypeChangeStorage.ConfirmationMessage = null;
            this.LogX_TypeChangeStorage.Id = "LogX_TypeChangeStorage";
            this.LogX_TypeChangeStorage.ToolTip = null;
            this.LogX_TypeChangeStorage.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_TypeChangeStorage.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_TypeChangeStorage_Execute);
            // 
            // LogX_CloseCtrH
            // 
            this.LogX_CloseCtrH.Caption = "Ügylet zárása";
            this.LogX_CloseCtrH.Category = "RecordEdit";
            this.LogX_CloseCtrH.ConfirmationMessage = null;
            this.LogX_CloseCtrH.Id = "LogX_CloseCtrH";
            this.LogX_CloseCtrH.ToolTip = null;
            this.LogX_CloseCtrH.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_CloseCtrH.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_CloseCtrH_Execute);
            // 
            // LogX_LcIsHere
            // 
            this.LogX_LcIsHere.Caption = "Megérkezett a láda";
            this.LogX_LcIsHere.Category = "RecordEdit";
            this.LogX_LcIsHere.ConfirmationMessage = null;
            this.LogX_LcIsHere.Id = "LogX_LcIsHere";
            this.LogX_LcIsHere.ToolTip = null;
            this.LogX_LcIsHere.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_LcIsHere.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_LcIsHere_Execute);
            // 
            // LogX_PutProductIntoLc
            // 
            this.LogX_PutProductIntoLc.Caption = "Termék betét ládába";
            this.LogX_PutProductIntoLc.Category = "RecordEdit";
            this.LogX_PutProductIntoLc.ConfirmationMessage = null;
            this.LogX_PutProductIntoLc.Id = "LogX_PutProductIntoLc";
            this.LogX_PutProductIntoLc.ToolTip = null;
            this.LogX_PutProductIntoLc.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_PutProductIntoLc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_PutProductIntoLc_Execute);
            // 
            // LogX_TypeChangeService
            // 
            this.LogX_TypeChangeService.Caption = "Szervíz";
            this.LogX_TypeChangeService.Category = "Edit";
            this.LogX_TypeChangeService.ConfirmationMessage = null;
            this.LogX_TypeChangeService.Id = "LogX_TypeChangeService";
            this.LogX_TypeChangeService.ToolTip = null;
            this.LogX_TypeChangeService.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.LogX_TypeChangeService.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LogX_TypeChangeService_Execute);
            // 
            // IocpNewController
            // 
            this.Actions.Add(this.LogX_CallCtrh);
            this.Actions.Add(this.LogX_GetProductFromLc);
            this.Actions.Add(this.LogX_SendLcBack);
            this.Actions.Add(this.LogX_TypeChangeBack);
            this.Actions.Add(this.LogX_CallLc);
            this.Actions.Add(this.LogX_TypeChangeRemoval);
            this.Actions.Add(this.LogX_TypeChangeInventory);
            this.Actions.Add(this.LogX_TypeChangeComission);
            this.Actions.Add(this.LogX_TypeChangeStorage);
            this.Actions.Add(this.LogX_CloseCtrH);
            this.Actions.Add(this.LogX_LcIsHere);
            this.Actions.Add(this.LogX_PutProductIntoLc);
            this.Actions.Add(this.LogX_TypeChangeService);
            this.TargetObjectType = typeof(LogXExplorer.Module.BusinessObjects.Database.Iocp);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction LogX_CallCtrh;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_GetProductFromLc;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_SendLcBack;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_TypeChangeBack;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_CallLc;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_TypeChangeRemoval;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_TypeChangeInventory;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_TypeChangeComission;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_TypeChangeStorage;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_CloseCtrH;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_LcIsHere;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_PutProductIntoLc;
        private DevExpress.ExpressApp.Actions.SimpleAction LogX_TypeChangeService;
    }
}
