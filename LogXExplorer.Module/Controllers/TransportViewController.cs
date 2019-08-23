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
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LogXExplorer.Module.BusinessObjects.Database;

namespace LogXExplorer.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TransportViewController : ViewController
    {
        public TransportViewController()
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

        //private void LogX_LcArrivedIntoLocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    List<SortProperty> sort = new List<SortProperty>();
        //    sort.Add(new SortProperty("TpId", SortingDirection.Ascending));
        //    //CriteriaOperator copTpo = new BinaryOperator("Type", 1);
        //    //IList tpoInList = View.ObjectSpace.GetObjects(typeof(TransportOrder), copTpo, sort, false);
        //    IList tpoInList = View.ObjectSpace.GetObjects(typeof(TransportOrder));
        //    bool existTpo = false;
        //    TransportOrder tpo = null;

        //    for (int i = 0; i < tpoInList.Count && !existTpo; i++)
        //    {
        //        tpo = (TransportOrder)tpoInList[i];

        //        if (tpo.msgTarget != "" || tpo.Iocp.IocpType == "1")
        //        {
        //            existTpo = true;
        //        }
        //    }

        //    if (existTpo)
        //    {
        //        StorageLocation sl;

        //        if (tpo.msgTarget != "")
        //        {
        //            sl = View.ObjectSpace.FindObject<StorageLocation>(new BinaryOperator("Oid", (StorageLocation)tpo.TargetLocation));

        //            if (tpo.LC.Stocks.Count > 0)
        //            {
        //                sl.LcIsEmpty = false;
        //                sl.StatusCode = 1;
        //                sl.LoadCarrier = tpo.LC;

        //                foreach (Stock stock in tpo.LC.Stocks)
        //                {
        //                    stock.StorageLocation = sl;
        //                }
        //            }
        //            else
        //            {
        //                sl.LcIsEmpty = true;
        //                sl.StatusCode = 1;
        //                sl.LoadCarrier = tpo.LC;
        //            }
        //        }
        //        else
        //        {
        //            sl = View.ObjectSpace.FindObject<StorageLocation>(new BinaryOperator("Oid", (StorageLocation)tpo.SourceLocation));
        //            sl.LcIsEmpty = false;
        //            sl.StatusCode = 0;
        //            sl.LoadCarrier = null;
        //        }
        //        tpo.Delete();
        //        View.ObjectSpace.CommitChanges();
        //        View.ObjectSpace.Refresh();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Nincs betárolásra váró termékhordó!");
        //    }
        //}
    }
}
