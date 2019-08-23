using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using LogXExplorer.Module.BusinessObjects.Database;

namespace LogXExplorer.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class IocpRefreshController : ViewController
    {
        System.Timers.Timer timer;
        bool autorefreshActive = true;
        Iocp iocp = null;
        //OpcClient myOpcClient;
        string newLcRfid = "";


        public IocpRefreshController()
        {
            InitializeComponent();
           // myOpcClient = new OpcClient();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            iocp = (Iocp)View.CurrentObject;
            timer = new System.Timers.Timer(10000);
            timer.SynchronizingObject = (ISynchronizeInvoke)Frame.Template;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
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
            timer.Stop();
            timer.Dispose();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (autorefreshActive)
            {
                //MessageBox.Show("refresh");
                View.ObjectSpace.Refresh();

                //Csak akkor ellenőrzünk ha jelenleg nincs az IOCP-n láda
                if(iocp.ActiveLc == null && iocp.RFIDtag != null)
                {
                   // newLcRfid = myOpcClient.ReadModulRfIdTag(iocp.RFIDtag);
                    if(newLcRfid != "" && newLcRfid != null)
                    {
                        LogX_LcIsHere(newLcRfid);
                    }
                }
                View.Refresh();
            }
        }



        #region Megérkezik a láda
        private void LogX_LcIsHere(string newLcRfid1)
        {
            LoadCarrier lc = null;
            CriteriaOperator c = new BinaryOperator("RFID1", newLcRfid1);
            lc = View.ObjectSpace.FindObject<LoadCarrier>(c);


            //if (lc != null)
            //{
            //    TransportOrder tpo = (TransportOrder)tpoList[0];
            //    iocp.ActiveLc = lc;
            //    iocp.ActiveCtrD = tpo.CommonDetail;
            //    iocp.ActiveProduct = tpo.CommonDetail.Product;
            //    StorageLocation sl = tpo.SourceLocation;


            //    CriteriaOperator copQtyE = new GroupOperator(GroupOperatorType.And, new BinaryOperator("In", true), new BinaryOperator("Product", iocp.ActiveProduct));
            //    QtyExchange qtye = View.ObjectSpace.FindObject<QtyExchange>(copQtyE);

            //    iocp.Qexchange = qtye;
            //    iocp.StoredUnit = qtye.SourceQty;
            //    double recentQty = iocp.ActiveCtrD.GetRecentQuantity();

            //    iocp.StoredQty = Math.Min(qtye.SourceQty * qtye.TargetQty, recentQty);
            //    LokacioFelszabadítas(sl);

            //    iocp.LcCallingOK = true;
            //    //myOpcClient.DeleteTransport(ObjectSpace,tpo.TpId);
            //    DeleteTransport(tpo.Oid);


            //    foreach (Stock stock in iocp.ActiveLc.Stocks)
            //    {
            //        stock.StorageLocation = null;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Nincs több láda!");
            //}
            //View.ObjectSpace.CommitChanges();
            //View.Refresh();
        }


        private void DeleteTransport(int tpoOid)
        {
            Iocp iocp = (Iocp)View.CurrentObject;

            CriteriaOperator ctpo = CriteriaOperator.Parse("[Oid] = ?", tpoOid);
            TransportOrder t = (TransportOrder)View.ObjectSpace.FindObject(typeof(TransportOrder), ctpo);

            if (t != null)
            {
                t.Delete();
                View.ObjectSpace.CommitChanges();
            }

        }
        #endregion

        #region Tárolóhely felszabadítás
        private void LokacioFelszabadítas(StorageLocation sl)
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("[Oid] = ?", sl.Oid);
            StorageLocation sloc = (StorageLocation)View.ObjectSpace.FindObject(typeof(StorageLocation), criteria);
            sloc.LoadCarrier = null;
            sloc.LcIsEmpty = false;
            sloc.StatusCode = 0;

            ObjectSpace.CommitChanges();
            View.Refresh();
        }
        #endregion




    }
}