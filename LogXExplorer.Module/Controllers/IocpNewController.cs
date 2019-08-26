using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using LogXExplorer.Module.BusinessObjects.Database;
using LogXExplorer.Module.LogXObjects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Collections;
using System.Management;
using LogXExplorer.Module.comm;
//using MessagingToolkit.Barcode;

using System.Runtime.Serialization;

namespace LogXExplorer.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class IocpNewController : ViewController
    {
        Iocp iocp;
        



        public IocpNewController()
        {
            InitializeComponent();
            RegisterActions(components);
        // Target required Views (via the TargetXXX properties) and create their Actions.

        }
        protected override void OnActivated()
        {
            base.OnActivated();


            //this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);

            //View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            //IList usbDevices = GetUSBDevices();
            //foreach (USBDeviceInfo usbDevice in usbDevices)
            //{
            //    Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
            //    usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            //}

            iocp = (Iocp)View.CurrentObject;
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
            //View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
        }

        protected void OnCurrentObjectChanged(object sender, ObjectChangeEventArgs e)
        {
            MessageBox.Show("aaa");
        }



        #region Ügylet választása
        
        //Választóablak megnyítása
        private void LogX_CallCtrh_PopupWindow(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            Iocp iocp = (Iocp)View.CurrentObject;

            if (iocp.ActiveType != null)
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(CommonTrHeader));
                string vid = Application.FindLookupListViewId(typeof(CommonTrHeader));
                CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace, typeof(CommonTrHeader), vid);
                collectionSource.Criteria["My criteria"] = new GroupOperator(GroupOperatorType.And, new BinaryOperator("CommonType", iocp.ActiveType.Oid), new InOperator("Status", new int[] { 5, 20 }));
                e.View = Application.CreateListView(vid, collectionSource, true);
            }
        }

        //Kiválasztott ügyleten történő események
        private void LogX_CallCtrh_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            foreach (CommonTrHeader ctrh in e.PopupWindowViewSelectedObjects)
            {
                if (ctrh != null)
                {
                    CommonTrHeader selectedItem = View.ObjectSpace.FindObject<CommonTrHeader>(new BinaryOperator("Oid", ctrh.Oid));
                    if (selectedItem != null)
                    {
                        try
                        {
                            Iocp iocp = (Iocp)View.CurrentObject;
                            LogXPrivateServiceClientProxy proxy = new LogXPrivateServiceClientProxy();
                            bool siker = proxy.ChangeCommonTrHeaderStatus(selectedItem.Oid, 15);
                            if (siker)
                            {
                                iocp.ActiveCTrH = selectedItem;
                            }
                            

                            if (selectedItem.CommonType.Type == "BETAR")
                            {
                                try
                                {
                                    proxy.LcNumPreCalculation(selectedItem.Oid);
                                }
                                catch
                                {
                                    throw;
                                }
                                finally
                                {
                                    ObjectSpace.CommitChanges();
                                    ObjectSpace.Refresh();
                                }
                            }

                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                        }
                    }
                }
            }
        }
        #endregion


        #region Ládaszám változtatás
        //Ládaszám növelés
        //private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        //{
        //    CommonTrDetail ctrDetail = null;

        //    ctrDetail = (CommonTrDetail)gridViewCommonDetail.GetRow(gridViewCommonDetail.FocusedRowHandle) as CommonTrDetail;
        //    ctrDetail.CalcLcNumber++;
        //    iocp.CalcLcNumber++;
        //}

        //Ládaszám csökkentés
        //private void repositoryItemButtonEdit3_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        //{
        //    CommonTrDetail ctrDetail = null;
        //    ctrDetail = (CommonTrDetail)gridViewCommonDetail.GetRow(gridViewCommonDetail.FocusedRowHandle) as CommonTrDetail;
        //    ctrDetail.CalcLcNumber--;

        //    iocp.CalcLcNumber--;

        //}
        #endregion
// ----------------------------------------
//---------------------------------------------------------------------------------------------------------------------
        #region Ládák kihívása
        private void LogX_CallLc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Iocp iocp = (Iocp)View.CurrentObject;
            IList<CommonTrDetail> cTrD = new List<CommonTrDetail>();
            cTrD = iocp.ActiveCTrH.CommonTrDetails;

            LogXPrivateServiceClientProxy proxy = new LogXPrivateServiceClientProxy();
            proxy.CallLoadCarriers(iocp.ActiveCTrH.Oid, iocp.ActiveCTrH.CommonType.Type, iocp.Oid, iocp.WeightCurrent);
            //iocp.LcCallingOK = true;
            View.Refresh();
        }


        #region Megérkezik a láda
        private void LogX_LcIsHere_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Iocp iocp = (Iocp)View.CurrentObject;

            CriteriaOperator c = new GroupOperator(GroupOperatorType.And, new BinaryOperator("CommonTrHeader", iocp.ActiveCTrH), new BinaryOperator("TargetTag", iocp.TargetTag));
            IList tpoList = View.ObjectSpace.GetObjects(typeof(TransportOrder), c, false);

            if (tpoList.Count > 0)
            {
                TransportOrder tpo = (TransportOrder)tpoList[0];
                iocp.ActiveLc = tpo.LC;
                iocp.ActiveCtrD = tpo.CommonDetail;
                iocp.ActiveProduct = tpo.CommonDetail.Product;
                StorageLocation sl = tpo.SourceLocation;


                CriteriaOperator copQtyE = new GroupOperator(GroupOperatorType.And, new BinaryOperator("In", true), new BinaryOperator("Product", iocp.ActiveProduct));
                QtyExchange qtye = View.ObjectSpace.FindObject<QtyExchange>(copQtyE);

                iocp.Qexchange = qtye;
                iocp.StoredUnit = qtye.SourceQty;
                double recentQty = iocp.ActiveCtrD.GetRecentQuantity();

                //iocp.StoredQty = Math.Min(qtye.SourceQty * qtye.TargetQty, recentQty);
                //LokacioFelszabadítas(sl);

                iocp.LcCallingOK = true;
                //myOpcClient.DeleteTransport(ObjectSpace,tpo.TpId);
                DeleteTransport(tpo.Oid);


                foreach (Stock stock in iocp.ActiveLc.Stocks)
                {
                    stock.StorageLocation = null;
                }
            }
            else
            {
                MessageBox.Show("Nincs több láda!");
            }
            View.ObjectSpace.CommitChanges();
            View.Refresh();
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

        # region Termék berakása a ládába
        private void LogX_PutProductIntoLc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Iocp iocp = (Iocp)View.CurrentObject;
            double taroltMennyiseg = iocp.ActiveLc.TermekTaroltMennyiseg(iocp.ActiveProduct.Oid);
            double maxTarolhatoMennyiseg = iocp.Qexchange.SourceQty * iocp.Qexchange.TargetQty;
            double maradekTarolhato = maxTarolhatoMennyiseg - taroltMennyiseg;

            if (iocp.StoredQty <= maradekTarolhato)
            {
                //CreateStockHistory("J", iocp.ActiveLc.Oid, iocp.ActiveProduct.Oid, iocp.Qexchange.Oid, iocp.ActiveCtrD.Oid, iocp.StoredQty);
                PutIntoLoadCarrier();
                View.ObjectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
            else
            {
                MessageBox.Show(String.Format("Maximum: {0} mennyiség tárolható!", maradekTarolhato));
            }
        }
        #endregion

        #region Termék kivétel a ládából
        private void LogX_GetProductFromLc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Iocp iocp = (Iocp)View.CurrentObject;
            //CreateStockHistory("T", iocp.ActiveLc.Oid, iocp.ActiveProduct.Oid, iocp.Qexchange.Oid, iocp.ActiveCtrD.Oid, iocp.StoredQty);
            GetFromLoadCarrier(iocp.ActiveLc.Oid, iocp.ActiveProduct.Oid, iocp.StoredQty);
            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
            View.Refresh();


        }
        #endregion

     
        //CriteriaOperator c = new GroupOperator(GroupOperatorType.And, new BinaryOperator("StatusCode", "1"), new BinaryOperator("LC", iocp.ActiveLc));
        //IList CtrDStock = View.ObjectSpace.GetObjects(typeof(Stock), c, false);

        //CommonTrDetail cTrD = View.ObjectSpace.FindObject<CommonTrDetail>(new BinaryOperator("Oid", iocp.ActiveCtrD.Oid));

        //foreach (Stock st in CtrDStock)
        //{
        //    cTrD.PerformedQty += st.NormalQty;
        //    st.StatusCode = 0;
        //}

        //}
        #endregion



        #region Ügylet zárása
        private void LogX_CloseCtrH_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Iocp iocp = (Iocp)View.CurrentObject;

            int newStatus = CalcStatus(iocp.ActiveCTrH);

            iocp.ActiveCTrH.Status = newStatus; ;
            ObjectSpace.CommitChanges();


            iocp.ActiveCTrH = null;
            iocp.ActiveCtrD = null;
            iocp.LcCallingOK = false;
            iocp.ActiveLc = null;
            iocp.Barcode = "";
            iocp.StoredQty = 0;
            iocp.WeightCurrent = 0;
            iocp.WieghtBeforeStart = 0;

            ObjectSpace.CommitChanges();
            View.Refresh();
        }

        private int CalcStatus(CommonTrHeader ctrh)
        {
            int newStatus = ctrh.Status;

            double reqQty = ctrh.GetRequiredSumQuantity();
            double perfQty = ctrh.GetPerformedSumQuantity();


            if (perfQty == 0)
            {
                newStatus = 5;

            }
            else if (perfQty < reqQty)
            {
                newStatus = 20;
            }
            else
            {
                newStatus = 50;
            }

            return newStatus;
        }
        #endregion

        //#region Transport sor létrehozása
        //private void CreateTransports(bool back, int ctrH, int ctrd, int lc, int slSource, int slTarget, byte targetTag, byte Tipus,int InOut)
        //{
        //    StorageLocation sourceLocation = View.ObjectSpace.FindObject<StorageLocation>(new BinaryOperator("Oid", slSource));
        //    StorageLocation targetLocation = View.ObjectSpace.FindObject<StorageLocation>(new BinaryOperator("Oid", slTarget));
        //    LoadCarrier lca = View.ObjectSpace.FindObject<LoadCarrier>(new BinaryOperator("Oid", lc));
        //    CommonTrHeader ctrHeader = View.ObjectSpace.FindObject<CommonTrHeader>(new BinaryOperator("Oid", ctrH));
        //    CommonTrDetail cdetail = View.ObjectSpace.FindObject<CommonTrDetail>(new BinaryOperator("Oid", ctrd));


        //    ushort UjTransportID = 0;
        //   //GetNewSorszam("TPO");

        //    TransportOrder transportOrder = View.ObjectSpace.CreateObject<TransportOrder>();
        //    transportOrder.Iocp = (Iocp)View.CurrentObject;
        //    transportOrder.TpId = UjTransportID;
        //    transportOrder.LC = lca;
        //    transportOrder.CommonTrHeader = ctrHeader;
        //    transportOrder.CommonDetail = cdetail;
        //    transportOrder.Type = Tipus;
        //    transportOrder.TargetTag = targetTag;
        //    transportOrder.InOut = InOut;


        //    if (sourceLocation != null)
        //    {
        //        transportOrder.SourceLocation = sourceLocation;
        //    }

        //    if (targetLocation != null)
        //    {
        //        transportOrder.TargetLocation = targetLocation;
        //    }
        //    ObjectSpace.CommitChanges();
        //    //myOpcClient.SetTransportOrder(ObjectSpace,transportOrder.TpId);

        //}
        //private ushort UjTpID()
        //{
        //    Sorszam sorszam = null;
        //    decimal ujSorszam = 0;

        //    CriteriaOperator copType = new BinaryOperator("Type", "TPO");
        //    CommonTrType cTpId = (CommonTrType)View.ObjectSpace.FindObject(typeof(CommonTrType), copType);


        //    if (View.ObjectSpace.FindObject<Sorszam>(new BinaryOperator("Type", cTpId)) == null)
        //    {
        //        sorszam = View.ObjectSpace.CreateObject<Sorszam>();

        //        CommonTrType cType = View.ObjectSpace.FindObject<CommonTrType>(new BinaryOperator("Type", cTpId));

        //        sorszam.Type = cType;
        //        View.ObjectSpace.CommitChanges();
        //    }

        //    sorszam = View.ObjectSpace.FindObject<Sorszam>(new BinaryOperator("Type", cTpId));
        //    ujSorszam = sorszam.GetNewNumber();

        //    ObjectSpace.CommitChanges();

        //    return Convert.ToUInt16(ujSorszam);
        //}

        //#endregion


        #region Nem használt (egyenlőre)
        //private List<USBDeviceInfo> GetUSBDevices()
        //{
        //    List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

        //    ManagementObjectCollection collection;
        //    using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
        //        collection = searcher.Get();

        //    foreach (var device in collection)
        //    {
        //        devices.Add(new USBDeviceInfo(
        //        (string)device.GetPropertyValue("DeviceID"),
        //        (string)device.GetPropertyValue("PNPDeviceID"),
        //        (string)device.GetPropertyValue("Description")
        //        ));
        //    }
        //    collection.Dispose();
        //    return devices;
        //}
        //private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        //{

        //    Iocp iocp = (Iocp)e.Object;

        //    if (View.CurrentObject != null)
        //    {

        //        if (e.PropertyName == "Barcode")
        //        {
        //            int szam = iocp.Barcode.Length;
        //            MessageBox.Show(iocp.Barcode);
        //        }
        //        if (e.PropertyName == "StoredUnit")
        //        {

        //            // A képernyőn a BOM modellben beállított ImmediatePostData = true miatt frissül
        //            iocp.StoredQty = iocp.StoredUnit * iocp.Qexchange.SourceQty;
        //        }
        //    }
        //}

        //private void LogX_PrintCimke_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{

        //}


        //DateTime _lastKeystroke = new DateTime(0);
        //List<char> _barcode = new List<char>(10);

        //private void BarodeKeypress_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    //// check timing (keystrokes within 100 ms)
        //    //TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
        //    //if (elapsed.TotalMilliseconds > 100)
        //    //    _barcode.Clear();

        //    //// record keystroke & timestamp
        //    //_barcode.Add(e.KeyChar);
        //    //_lastKeystroke = DateTime.Now;+-+

        //    //// process barcode
        //    //if (e.KeyChar == 13 && _barcode.Count > 0)
        //    //{
        //    //    string msg = new String(_barcode.ToArray());
        //    //    queryData(msg);
        //    //    _barcode.Clear();
        //    //}
        //}
        #endregion






        private void PutIntoLoadCarrier()
        {
            Iocp iocp = (Iocp)View.CurrentObject;

            using (IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Stock)))
            {
                //Aktív láda megtalálása
                CriteriaOperator copLc = new BinaryOperator("Oid", iocp.ActiveLc.Oid);
                LoadCarrier activeLc = objectSpace.FindObject<LoadCarrier>(copLc);

                //Aktív termék megtalálása
                CriteriaOperator copProduct = new BinaryOperator("Oid", iocp.ActiveProduct.Oid);
                Product activeProduct = objectSpace.FindObject<Product>(copProduct);

                //Aktív átváltás megtalálása
                CriteriaOperator copQexc = new BinaryOperator("Oid", iocp.Qexchange.Oid);
                QtyExchange activeQexc = objectSpace.FindObject<QtyExchange>(copQexc);

                CriteriaOperator copStock = new GroupOperator(GroupOperatorType.And, new BinaryOperator("LC", activeLc), new BinaryOperator("Product", activeProduct));
                Stock findedStock = objectSpace.FindObject<Stock>(copStock);

                if (findedStock == null)
                {
                    Stock stock = objectSpace.CreateObject<Stock>();
                    stock.Product = activeProduct;
                    stock.LC = activeLc;
                    stock.Section = "A1";
                    stock.StatusCode = 1;
                    stock.NormalQty += iocp.StoredQty;
                    stock.ReservedQty = 0;
                    stock.BlockedQty = 0;
                }
                else
                {
                    findedStock.NormalQty += iocp.StoredQty;
                }
                objectSpace.CommitChanges();
            }
        }

        private void GetFromLoadCarrier(int LcId, int productID, double quantity)
        {
            Iocp iocp = (Iocp)View.CurrentObject;

            using (IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Stock)))
            {
                //Aktív láda megtalálása
                CriteriaOperator copLc = new BinaryOperator("Oid", LcId);
                LoadCarrier activeLc = objectSpace.FindObject<LoadCarrier>(copLc);

                //Aktív termék megtalálása
                CriteriaOperator copProduct = new BinaryOperator("Oid", productID);
                Product activeProduct = objectSpace.FindObject<Product>(copProduct);

                CriteriaOperator copStock = new GroupOperator(GroupOperatorType.And, new BinaryOperator("LC", activeLc), new BinaryOperator("Product", activeProduct));
                Stock findedStock = objectSpace.FindObject<Stock>(copStock);

                if (findedStock == null)
                {
                    MessageBox.Show("Ilyen termék nincs a ládában!");
                }
                else
                {
                    Stock stock = objectSpace.CreateObject<Stock>();
                    findedStock.Product = activeProduct;
                    findedStock.LC = activeLc;
                    findedStock.Section = "A1";
                    findedStock.StatusCode = 1;
                    findedStock.NormalQty -= quantity;
                    findedStock.ReservedQty = 0;
                    findedStock.BlockedQty = 0;
                }


                if (findedStock.NormalQty == 0)
                {
                    findedStock.Delete();
                }

                objectSpace.CommitChanges();
            }
        }

        #region Funkcióváltás
        private void LogX_TypeChangeInventory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ChangeType("LELTAR");
        }

        private void LogX_TypeChangeRemoval_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ChangeType("KITAR");
        }

        private void LogX_TypeChangeComission_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ChangeType("KOMISSIO");
        }

        private void LogX_TypeChangeStorage_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ChangeType("BETAR");
        }

        private void LogX_TypeChangeService_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ChangeType("SERVICE");
        }

        private void LogX_TypeChangeBack_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ChangeType("");
        }

        private void ChangeType(string Type)
        {

            CommonTrType ctrType = View.ObjectSpace.FindObject<CommonTrType>(new BinaryOperator("Type", Type));
            Iocp iocp = (Iocp)View.CurrentObject;


            //CriteriaOperator cop = new BinaryOperator("Type", Type);
            //CommonTrType ctrType = View.ObjectSpace.FindObject<CommonTrType>(cop);

            if (ctrType != null)
            {
                iocp.ActiveType = ctrType;
            }
            else
            {
                iocp.ActiveType = null;
            }
            ObjectSpace.CommitChanges();
        }
        #endregion

        private void LogX_SendLcBack_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }
    }
}

