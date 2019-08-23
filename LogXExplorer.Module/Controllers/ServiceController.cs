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
using LogXExplorer.Module.BusinessObjects.Database;
using LogXExplorer.Module;
using LogXExplorer.Module.comm;

namespace LogXExplorer.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ServiceController : ViewController
    {

        int numberOfAisle;
        byte tomb;
        int numberOfLine = 2;
        int numberOfColumn;
        int numberOFRow;
        uint barCode = 0;
        int ciklus = 0;
        AbcType aType;
        AbcType bType;
        AbcType cType;
        LoadCarrier loadCarriers;
        StorageLocation storageLocation;
        AbcType abcType;
        LoadCarrierType LcT;
        //OpcClient myOpcClient;

        public ServiceController()
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

        private void ResetTestData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            DialogResult rs = MessageBox.Show("Visszaállítás indulhat?", "T", MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                //Tárolási helyek visszaállítása
                ResetStorageLocation();

                //Transport sorok törlése
                DeleteTransports();

                //Transport soszám visszaállítás
                ResetTransportID();

                //Készletek törlése
                DeleteStock();

                //Készlet napló törlése
                DeleteStockHistories();

                //Termék készletének nullázása
                ResetProductQuantities();

                //Bizonylat tételsorok visszaállítása
                ResetCommonTrDetail();

                //Bizonylat fejlécek visszaállítása
                ResetCommonTrHeader();

                //Modul adatok törlése
                ResetIocp();

                View.ObjectSpace.CommitChanges();
                View.Refresh();

                MessageBox.Show("Visszaállítás megtörtént!");
            }
        }

        #region Tárolási helyek visszaállítása
        private void ResetStorageLocation()
        {
            //CriteriaOperator cSto = new BinaryOperator("Aisle", 1);
            IList listSlocation = View.ObjectSpace.GetObjects(typeof(StorageLocation));

            int counter = listSlocation.Count;

            for (int i = 0; i < counter; i++)
            {
                StorageLocation sloc = (StorageLocation)listSlocation[i];

                CriteriaOperator cop = new BinaryOperator("Oid", sloc.Oid);
                LoadCarrier lc = (LoadCarrier)View.ObjectSpace.FindObject(typeof(LoadCarrier), cop);

                sloc.StatusCode = 1;
                sloc.LoadCarrier = lc;
                sloc.LcIsEmpty = true;
            }

            //View.ObjectSpace.CommitChanges();
        }
        #endregion

        #region Transport sorok törlése
        private void DeleteTransports()
        {
            IList listTpo = View.ObjectSpace.GetObjects(typeof(TransportOrder));
            int counter = listTpo.Count;
            for (int i = 0; i < counter; i++)
            {
                TransportOrder tpo = (TransportOrder)listTpo[0];
                tpo.Delete();
            }
            ObjectSpace.CommitChanges();
        }

        #endregion

        #region Transport soszám visszaállítás
        private void ResetTransportID()
        {

            CriteriaOperator copType = new BinaryOperator("Type", "TPO");
            CommonTrType cTpId = (CommonTrType)View.ObjectSpace.FindObject(typeof(CommonTrType), copType);


            CriteriaOperator copSorszam = new BinaryOperator("Oid", cTpId.Oid);
            Sorszam sorszam = (Sorszam)View.ObjectSpace.FindObject(typeof(Sorszam), copSorszam);

            if (sorszam != null)
            {
                sorszam.LastNum = 0;
            }

            //View.ObjectSpace.CommitChanges();
        }
        #endregion

        #region Készletek törlése
        private void DeleteStock()
        {

            IList listStock = View.ObjectSpace.GetObjects(typeof(Stock));
            int counter = listStock.Count;
            for (int i = 0; i < counter; i++)
            {
                Stock stock = (Stock)listStock[0];
                stock.Delete();
            }
        }
        #endregion

        #region Készlet napló törlése
        private void DeleteStockHistories()
        {
            CriteriaOperator cSth = new BinaryOperator("Section", "A1");
            IList listStockHistory = View.ObjectSpace.GetObjects(typeof(StockHistory), cSth, true);

            int counter = listStockHistory.Count;

            for (int i = 0; i < counter; i++)
            {
                StockHistory stockH = (StockHistory)listStockHistory[0];
                stockH.Delete();
            }

            //View.ObjectSpace.CommitChanges();
        }
        #endregion

        #region Termék készletének nullázása
        private void ResetProductQuantities()
        {
            IList listProduct = View.ObjectSpace.GetObjects(typeof(Product));

            int counter = listProduct.Count;

            for (int i = 0; i < counter; i++)
            {
                Product product = (Product)listProduct[i];
                product.ReservedQty = 0;
                product.NormalQty = 0;
                product.BlockedQty = 0;
                product.DispousedQty = 0;
                product.Save();
            }
        }
        #endregion

        #region Bizonylat tételsorok visszaállítása;
        private void ResetCommonTrDetail()
        {
            double filterValue = 0;
            CriteriaOperator cTrD = new BinaryOperator("PerformedQty", filterValue, BinaryOperatorType.Greater);
            IList listCommonTrDetail = View.ObjectSpace.GetObjects(typeof(CommonTrDetail), cTrD, false);

            int counter = listCommonTrDetail.Count;

            for (int i = 0; i < counter; i++)
            {
                CommonTrDetail c = (CommonTrDetail)listCommonTrDetail[i];
                c.PerformedQty = 0;
                c.CalcLcNumber = 0;
            }

            //View.ObjectSpace.CommitChanges();

        }
        #endregion

        #region Bizonylat fejlécek visszaállítása
        private void ResetCommonTrHeader()
        {

            CriteriaOperator cTrH = CriteriaOperator.Parse("Status >= 1");
            IList listCommonTrHeader = View.ObjectSpace.GetObjects(typeof(CommonTrHeader), cTrH, false);

            int counter = listCommonTrHeader.Count;

            for (int i = 0; i < counter; i++)
            {
                CommonTrHeader ch = (CommonTrHeader)listCommonTrHeader[i];
                ch.Status = 0;
            }

            //View.ObjectSpace.CommitChanges();
        }
        #endregion

        #region Modul adatok törlése
        private void ResetIocp()
        {

            CriteriaOperator cIocp = new BinaryOperator("Name", "MODUL1");
            Iocp iocp = (Iocp)View.ObjectSpace.FindObject(typeof(Iocp), cIocp);

            iocp.ActiveCtrD = null;
            iocp.ActiveCTrH = null;
            iocp.ActiveType = null;
            iocp.ActiveLc = null;
            iocp.ActiveProduct = null;
            iocp.CalcLcNumber = 0;
            iocp.LcCallingOK = false;
            iocp.Qexchange = null;
            iocp.StoredQty = 0;
            //View.ObjectSpace.CommitChanges();
        }
        #endregion

        private void GenerateTestCtrH_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            DialogResult rs = MessageBox.Show("Generálás indulhat?", "T", MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(CommonTrHeader));
                IList<Product> products = objectSpace.GetObjects<Product>();

                //for (int i = 0; i < 2; i++)
                //{
                //    Product pr = objectSpace.FindObject<Product>(new BinaryOperator("Oid", products[i].Oid));
                //    StorageHeader sh = objectSpace.CreateObject<StorageHeader>();
                //    sh.Reference = "MANCI";
                //    sh.Save();
                //    objectSpace.CommitChanges();
                //    CommonTrDetail detail = objectSpace.CreateObject<CommonTrDetail>();
                //    detail.Product = pr;
                //    detail.Quantity = 200;
                //    detail.CommonTrHeader = (CommonTrHeader)sh;

                //    objectSpace.CommitChanges();
                //}


                foreach (Product product in products)
                {
                    //Product pr = objectSpace.FindObject<Product>(new BinaryOperator("Oid", product.Oid));
                    //StorageHeader sh = objectSpace.CreateObject<StorageHeader>();
                    //sh.Reference = "MANCI";
                    //sh.Save();
                    //objectSpace.CommitChanges();
                    //CommonTrDetail detail = objectSpace.CreateObject<CommonTrDetail>();
                    //detail.Product = pr;
                    ////detail.Quantity = pr.QtyExchanges.;
                    //detail.CommonTrHeader = (CommonTrHeader)sh;

                    //objectSpace.CommitChanges();
                }
            }
        }

        private void LogX_TarhelyGeneralas_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            numberOfAisle = 2;
            numberOfColumn = 50;
            numberOFRow = 10;
            abcType = View.ObjectSpace.FindObject<AbcType>(new BinaryOperator("Code", "A"));
            LcT = View.ObjectSpace.FindObject<LoadCarrierType>(new BinaryOperator("Name", "MAGAS"));


            for (int i = 0; i < numberOfAisle; i++)
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Oid = ?", i + 1);
                Aisle aisle = View.ObjectSpace.FindObject<Aisle>(criteria);

                for (int j = 0; j < numberOfLine; j++)
                {
                    tomb++;

                    for (int k = 0; k < numberOfColumn; k++)
                    {

                        for (int l = 0; l < numberOFRow; l++)
                        {

                            //Láda létrehozása
                            loadCarriers = View.ObjectSpace.CreateObject<LoadCarrier>();
                            barCode++;
                            loadCarriers.BarCode = barCode;
                            loadCarriers.RFID1 = Convert.ToString(1000 + barCode);
                            loadCarriers.RFID2 = Convert.ToString(2000 + barCode);
                            loadCarriers.LcType = LcT;

                            //Tárolóhely létrehozása
                            storageLocation = View.ObjectSpace.CreateObject<StorageLocation>();
                            storageLocation.Aisle = aisle;
                            storageLocation.Block = tomb;
                            storageLocation.Column = Convert.ToUInt16(k + 1);
                            storageLocation.Row = Convert.ToByte(l + 1);
                            storageLocation.Name = storageLocation.Aisle.Name + "_" + storageLocation.Block + "_" + storageLocation.Column + "_" + storageLocation.Row;
                            storageLocation.AbcClass = abcType; ;
                            storageLocation.LHU1X = 10;
                            storageLocation.LHU1Y = 10;
                            storageLocation.LHU2X = 10;
                            storageLocation.LHU2Y = 10;
                            storageLocation.LoadCarrier = loadCarriers;
                            storageLocation.LcIsEmpty = true;
                        }
                    }
                }
            }
            //View.ObjectSpace.CommitChanges();
            MessageBox.Show("Finished!" + ciklus.ToString());
        }

        private void LogX_abcKat_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IList<StorageLocation> slList = View.ObjectSpace.GetObjects<StorageLocation>();
            aType = View.ObjectSpace.FindObject<AbcType>(new BinaryOperator("Code", "A"));
            bType = View.ObjectSpace.FindObject<AbcType>(new BinaryOperator("Code", "B"));
            cType = View.ObjectSpace.FindObject<AbcType>(new BinaryOperator("Code", "C"));

            foreach (StorageLocation sl in slList)
            {
                if (sl.Oid > 0 && sl.Oid <= 300 || sl.Oid > 500 && sl.Oid <= 800 || sl.Oid > 1000 && sl.Oid <= 1300 || sl.Oid > 1500 && sl.Oid <= 1800)
                {
                    sl.AbcClass = aType;
                }
                else if (sl.Oid > 300 && sl.Oid <= 450 || sl.Oid > 800 && sl.Oid <= 950 || sl.Oid > 1300 && sl.Oid <= 1450 || sl.Oid > 1800 && sl.Oid <= 1950)
                {
                    sl.AbcClass = bType;
                }
                else
                {
                    sl.AbcClass = cType;
                }

                if (sl.LoadCarrier != null)
                {
                    sl.StatusCode = 1;
                }
            }

            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
            MessageBox.Show("Finished!");
        }


        private void callSzia_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //kulon memoria, nincs static eleres sem a kliens oldalrol a server fele!!!
            //RobiTeszt rt = RobiTeszt.GetInstance();
            //rt.RobiTesztHello();

            LogXPrivateServiceClientProxy proxy = new LogXPrivateServiceClientProxy();
            string customerName = proxy.GetCustomerName(1);
            MessageBox.Show(customerName);

            //string vissza = proxy.DoWork("param1 ", "param2 ");
            //ushort sorszam = proxy.GetNewSorszam("TPO"); 
            //MessageBox.Show(sorszam.ToString());

            // LogXServerClientProxy proxy = new LogXServiceClientProxy();
            //LogXServiceClientProxy proxy1 = new LogXServiceClientProxy();
            //string vissza = proxy.DoWork("HELLO", "LEO");

            //string vissza = proxy.GetAbcClassName("A");
            // MessageBox.Show(vissza);
        }
    }
}
