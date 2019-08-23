using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using LogXExplorer.Module.BusinessObjects.Database;

namespace LogXExplorer.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CommonHeaderViewController : ViewController
    {
        public CommonHeaderViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
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
        }

        private void LogX_sh_AdatokElfogadva_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            DialogResult rs = MessageBox.Show("Adatok elfogadva??", "T", MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                AllDataIsOK(e);
            }
        }


        private void LogX_ch_AdatokElfogadva_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            DialogResult rs = MessageBox.Show("Adatok elfogadva??", "T", MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                AllDataIsOK(e);
            }
        }

        private void LogX_rh_AdatokElfogadva_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            DialogResult rs = MessageBox.Show("Adatok elfogadva??", "T", MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                AllDataIsOK(e);
            }
        }

        private void LogX_ih_AdatokElfogadva_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            DialogResult rs = MessageBox.Show("Adatok elfogadva??", "T", MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                AllDataIsOK(e);
            }
        }

        private void AllDataIsOK(SimpleActionExecuteEventArgs e)
        {
            ArrayList SelectedCtrhs = new ArrayList();
            
            if ((e.SelectedObjects.Count > 0) &&
                ((e.SelectedObjects[0] is XafDataViewRecord) || (e.SelectedObjects[0] is XafInstantFeedbackRecord)))
            {
                foreach (var selectedObject in e.SelectedObjects)
                {
                    SelectedCtrhs.Add((CommonTrHeader)ObjectSpace.GetObject(selectedObject));
                }
            }
            else
            {
                SelectedCtrhs = (ArrayList)e.SelectedObjects;
            }
            foreach (CommonTrHeader selectedCtrh in SelectedCtrhs)
            {
                selectedCtrh.Status = 5;
            }
            View.ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void LogX_SelectCustomer_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            //Iocp iocp = (Iocp)View.CurrentObject;

            //if (iocp.ActiveType != null)
            //{
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Customer));
                string vid = Application.FindLookupListViewId(typeof(Customer));
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(Customer), vid);
                //Status status = objectSpace.FindObject<Status>(new InOperator("Code", new int[] { 5, 20 }));
                //collectionSource.Criteria["My criteria"] = new GroupOperator(GroupOperatorType.And, new BinaryOperator("CommonType", iocp.ActiveType), new BinaryOperator("Status", status));
                e.View = Application.CreateListView(vid, collectionSource, true);
            //}
        }

        private void LogX_SelectCustomer_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }

        private void LogX_SelectCommonType_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            foreach (CommonTrType selectedItem in e.PopupWindowViewSelectedObjects)
            {
                if(selectedItem != null)
                {
                    CommonTrHeader ctrh = (CommonTrHeader)View.CurrentObject;
                    CommonTrType ctrt = View.ObjectSpace.FindObject<CommonTrType>(new BinaryOperator("Oid",selectedItem.Oid));
                    ctrh.CommonType = ctrt;
                    //ObjectSpace.CommitChanges()
                }
            }
        }

        private void LogX_SelectCommonType_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(CommonTrType));
            string vid = Application.FindLookupListViewId(typeof(CommonTrType));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CommonTrType), vid);
            collectionSource.Criteria["Creatable"] = new BinaryOperator("Creatable",true);
            e.View = Application.CreateListView(vid, collectionSource, true);
        }
    }
}
