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
    public partial class CommonDetailViewController : ViewController
    {
        public CommonDetailViewController()
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

        private void LogX_ItemNumUp_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CommonTrDetail ctrd = (CommonTrDetail)e.CurrentObject;

            if (ctrd.ItemNum > 1)
            {
                CriteriaOperator cop = new GroupOperator(GroupOperatorType.And, new BinaryOperator("CommonTrHeader", ctrd.CommonTrHeader), new BinaryOperator("ItemNum", ctrd.ItemNum - 1));
                CommonTrDetail ctrdDown = (CommonTrDetail)View.ObjectSpace.FindObject<CommonTrDetail>(cop);
                ctrd.ItemNum--;
                ctrdDown.ItemNum++;
                View.ObjectSpace.CommitChanges();
            }
        }

        private void LogX_ItemNumDown_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CommonTrDetail ctrd = (CommonTrDetail)e.CurrentObject;

            CriteriaOperator copH = new BinaryOperator("Oid", ctrd.CommonTrHeader.Oid);
            CommonTrHeader ctrH = (CommonTrHeader)View.ObjectSpace.FindObject<CommonTrHeader>(copH);

            if (ctrd.ItemNum < ctrH.CommonTrDetails.Count())
            {
                CriteriaOperator cop = new GroupOperator(GroupOperatorType.And, new BinaryOperator("CommonTrHeader", ctrd.CommonTrHeader), new BinaryOperator("ItemNum", ctrd.ItemNum + 1));
                CommonTrDetail ctrdDown = (CommonTrDetail)View.ObjectSpace.FindObject<CommonTrDetail>(cop);
                ctrd.ItemNum++;
                ctrdDown.ItemNum--;
                View.ObjectSpace.CommitChanges();
            }
        }
    }
}
