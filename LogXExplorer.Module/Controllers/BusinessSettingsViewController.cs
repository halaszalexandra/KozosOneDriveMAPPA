using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using LogXExplorer.Module.BusinessObjects.Database;

namespace LogXExplorer.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BusinessSettingsViewController : ViewController
    {

        LoadCarrierType lctIn;
        LoadCarrierType lctOut;

        UnitType unitType;
        AbcType abcType;
        UInt32 w = 0;
        UInt32 l = 0;
        UInt32 h = 0;
        double we = 0;

      
       

public BusinessSettingsViewController()
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



        //OverlayTextPainter overlayLabel;
        //OverlayImagePainter overlayButton;

       

        #region Import DATA
        private void LogX_ImportDatas_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            DialogResult rs = MessageBox.Show("Import adatok beolvasása indulhat?", "Törzs adatok beolvasása...", MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                string importMappa = ConfigurationManager.AppSettings["ImportFolder"];

                SplashScreenManager.ShowForm(typeof(Splash));


                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Partnerek beolvása...");
                Import_DefPartner();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 5);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Tranzakció típusok beolvasása...");
                Import_CommonTrType();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 10);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "IOCP-k létrehozása...");
                CreateIOCP();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 25);

                //SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Státusz beolvasása...");
                //Import_Status();
                //SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 30);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Abc típusok beolvasása...");
                Import_AbcTypes();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 30);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Gyorsulási osztályok beolvasása...");
                Import_AccelerateType();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 45);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Mennyiségi egységek beolvasása...");
                Import_UnitType();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 59);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Termékhordó típusok beolvasása...");
                Import_LoadCarrier_Type();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 65);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Folyosók beolvasása...");
                Import_Aisles();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 73);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Termékhordók beolvasása...");
                Import_LoadCarrier();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 80);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Tárolási helyek beolvasása...");
                Import_StorageLocation();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 86);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "ProductGroup létrehozása...");
                Import_ProductGroup();
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 90);

                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Termékek beolvasása...");
                Import_Product();
                //Thread.Sleep(1000);
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 100);
                SplashScreenManager.CloseForm(false);
                MessageBox.Show("Táblázatok importálása megtörtént!");
                
            }
        }
        #endregion

        #region Termékcsoport beolvasása / ProductGroup
        private void Import_ProductGroup()
        {
            int rekord = 0;

            bool existfile = File.Exists(@"C:\Projects\LogXExplorer\Imports\LogX_ProductGroup.csv");

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_ProductGroup.csv", FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fileStream);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    if (rekord > 0)
                    {

                        if (values[0] != "")
                        {
                            ProductGroup cus = View.ObjectSpace.CreateObject<ProductGroup>();
                            cus.Name = values[0];
                            cus.Save();
                        }
                    }
                    rekord++;
                }
                View.ObjectSpace.CommitChanges();
            }
        }
        #endregion

        #region Alapértelmezett partner / Default Partner
        private void Import_DefPartner()
        {
            int rekord = 0;

            bool existfile = File.Exists(@"C:\Projects\LogXExplorer\Imports\LogX_Customer.csv");

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_Customer.csv", FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fileStream);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    if (rekord > 0)
                    {

                        if (values[0] != "")
                        {
                            Customer cus = View.ObjectSpace.CreateObject<Customer>();
                            cus.Name = values[0];
                            cus.Save();
                        }
                    }
                    rekord++;
                }
                View.ObjectSpace.CommitChanges();
            }
        }
        #endregion

        #region IOCP beolvasása / IOCP
        private void CreateIOCP()
        {
            CreateIocp("MODUL1", 1, 0);
            CreateIocp("MODUL2", 2, 0);
            CreateIocp("MP1", 3, 4);
            CreateIocp("MP2", 4, 4);
            CreateIocp("MP3", 5, 4);
            CreateIocp("MP4", 6, 4);
            CreateIocp("MP5", 7, 4);
            CreateIocp("MP6", 8, 4);
            CreateIocp("MP7", 9, 4);
            CreateIocp("MP8", 10, 4);
            CreateIocp("MP9", 11, 4);
            CreateIocp("MP10", 12, 4);
            CreateIocp("MP11", 13, 4);
            CreateIocp("MP12", 14, 4);
            CreateIocp("MP13", 15, 4);
            CreateIocp("MP14", 16, 4);
            CreateIocp("MP15", 17, 4);
            CreateIocp("MP16", 18, 4);
            ObjectSpace.CommitChanges();
        }

        private void CreateIocp(string name, byte targetTag, byte lcNum)
        {
            Iocp iocp = ObjectSpace.CreateObject<Iocp>();
            iocp.Name = name;
            iocp.TargetTag = targetTag;
            iocp.targetLcNum = lcNum;
        }
        #endregion

        #region Folyosók beolvasása  / Aisle
        private void Import_Aisles()
        {
            int rekord = 0;

            bool existfile = File.Exists(@"C:\Projects\LogXExplorer\Imports\LogX_Aisle.csv");

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_Aisle.csv", FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fileStream);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    if (rekord > 0)
                    {

                        if (values[0] != "")
                        {
                            ProductGroup cus = View.ObjectSpace.CreateObject<ProductGroup>();
                            cus.Name = values[0];
                            cus.Save();
                        }
                    }
                    rekord++;
                }
                View.ObjectSpace.CommitChanges();
            }
        }
        #endregion

        #region Mennyisége Egységek beolvasás / UnitType
        private void Import_UnitType()
        {

            int rekord = 0;

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_UnitType.csv", FileMode.Open, FileAccess.Read))
            {

                StreamReader reader = new StreamReader(fileStream);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    if (rekord > 0)
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("Name", values[0]);
                        UnitType existedType = (UnitType)View.ObjectSpace.FindObject(typeof(UnitType), criteria, true);

                        if (values[0] != "" || existedType == null)
                        {
                            UnitType unittype = View.ObjectSpace.CreateObject<UnitType>();
                            unittype.Name = values[0];
                            unittype.Save();
                        }
                    }
                    rekord++;
                }
                View.ObjectSpace.CommitChanges();

            }
        }
        #endregion

        #region Láda típusok beolvasása / LoadCarrierType
        private void Import_LoadCarrier_Type()
        {

            int rekord = 0;

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_LoadCarrierType.csv", FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fileStream);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    if (rekord > 0)
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("Name=?", values[0]);
                        LoadCarrierType existedLct = (LoadCarrierType)View.ObjectSpace.FindObject(typeof(LoadCarrierType), criteria, true);


                        if (existedLct == null)
                        {
                            LoadCarrierType lct = View.ObjectSpace.CreateObject<LoadCarrierType>();
                            lct.Name = values[0];
                            lct.Types = values[0];
                            lct.Length = Convert.ToInt32(values[2]);
                            lct.Width = Convert.ToInt32(values[3]);
                            lct.Height = Convert.ToInt32(values[4]);
                            lct.Weight = Convert.ToInt32(values[5]);

                            lct.Save();
                        }
                    }
                    rekord++;
                }
                View.ObjectSpace.CommitChanges();
            }
        }
        #endregion

        #region Termékek beolvasása  /  Product
        private void Import_Product()
        {

            int rekord = 0;
            //ProgressBarControl progressBarControl1 = new ProgressBarControl();
            //Session prSession = new Session();

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_Product.csv", FileMode.Open, FileAccess.Read))
            {

                StreamReader reader = new StreamReader(fileStream);

                abcType = View.ObjectSpace.FindObject<AbcType>(new BinaryOperator("Code", "A"));

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    if (rekord > 0)
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("Identifier=?", values[0]);
                        Product existedProduct = (Product)View.ObjectSpace.FindObject(typeof(Product), criteria, true);
                        unitType = View.ObjectSpace.FindObject<UnitType>(new BinaryOperator("Name", "Doboz"));

                        if (values[1] != "" && (existedProduct == null || values[0] == ""))
                        {
                            UInt32 seged = GetparsedValue(values[2]);
                            Product product = View.ObjectSpace.CreateObject<Product>();
                            product.Identifier = values[0];
                            product.Name = values[1];
                            product.Weight = Convert.ToInt32(seged);
                            product.AbcClass = abcType;

                            
                            Aisle aisle = View.ObjectSpace.FindObject<Aisle>(new BinaryOperator("Name", values[12]));
                            product.DefaultAisle = aisle;
                            //product.Aisles.Add(aisle);

                            ProductGroup productGroup = View.ObjectSpace.FindObject<ProductGroup>(new BinaryOperator("Name", values[26]));
                            product.ProductGroup = productGroup;
                            product.Save();


                            lctIn = View.ObjectSpace.FindObject<LoadCarrierType>(new BinaryOperator("Name", values[8]));
                            lctOut = View.ObjectSpace.FindObject<LoadCarrierType>(new BinaryOperator("Name", values[10]));

                            //ObjectSpace.CommitChanges();

                            w = GetparsedValue(values[5]);
                            l = GetparsedValue(values[4]);
                            h = GetparsedValue(values[6]);
                            we = GetparsedValue(values[14]);

                            // Betárolási láda QuexChange
                            if (lctIn != null)
                            {
                                UInt32 seged1 = GetparsedValue(values[9]);
                                createQexchange(product, lctIn, 1, Convert.ToDouble(seged1), true, true, w, l, h, we);
                            }


                            // Kitárolási láda QuexChange
                            if (lctOut == null)
                            {
                                UInt32 seged2 = GetparsedValue(values[11]);
                                createQexchange(product, lctOut, 1, Convert.ToDouble(seged2), false, true, w, l, h, we);
                            }

                        }
                    }
                    rekord++;

                }
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, 90);
                View.ObjectSpace.CommitChanges();

            }
        }


        private UInt32 GetparsedValue(string value)
        {
            UInt32 returnValue = 0;
            bool success = UInt32.TryParse(value, out returnValue);

            if (success)
            {
                return returnValue;
            }
            else
            {
                return 0;
            }
        }

        private void createQexchange(Product product, LoadCarrierType lct, double sourceQty, double targetQty, bool io, bool def, UInt32 width, UInt32 length, UInt32 height, double weight)
        {
            QtyExchange qtyExchange = View.ObjectSpace.CreateObject<QtyExchange>();
            qtyExchange.Product = product;
            qtyExchange.LcType = lct;
            qtyExchange.SourceQty = sourceQty;
            qtyExchange.TargetUnit = unitType;
            qtyExchange.TargetQty = targetQty;
            qtyExchange.In = io;
            qtyExchange.Out = !io;
            qtyExchange.Default = def;
            qtyExchange.PackageWidth = width;
            qtyExchange.PackageLength = length;
            qtyExchange.PackageHeight = height;
            qtyExchange.PackageWeight = weight;

        }

        private void createAisle(Product product, LoadCarrierType lct, double sourceQty, double targetQty)
        {
            QtyExchange qtyExchange = View.ObjectSpace.CreateObject<QtyExchange>();
            qtyExchange.Product = product;
            qtyExchange.LcType = lct;
            qtyExchange.SourceQty = sourceQty;
            qtyExchange.TargetUnit = unitType;
            qtyExchange.TargetQty = targetQty;
            qtyExchange.In = true;
            qtyExchange.Out = false;
        }
        #endregion

        #region  Abc típusok beolvasás  / ABC type
        private void Import_AbcTypes()
        {
            
            int rekord = 0;

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_AbcType.csv", FileMode.Open, FileAccess.Read))
            {

                    StreamReader reader = new StreamReader(fileStream);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var values = line.Split(';');

                        if (rekord > 0)
                        {
                            CriteriaOperator criteria = CriteriaOperator.Parse("Code", values[0]);
                            AbcType existedAbcType = (AbcType)View.ObjectSpace.FindObject(typeof(AbcType), criteria, true);

                            if (values[0] != "")
                            {
                                AbcType abctype = View.ObjectSpace.CreateObject<AbcType>();
                                abctype.Code = values[0];
                                abctype.Name = values[1];
                                abctype.Save();
                            }

                        }
                        rekord++;
                    }
                    View.ObjectSpace.CommitChanges();
                
            }
        }


        #endregion

        #region Gyorsulási osztályok beolvasása  / Accelerate Type
        private void Import_AccelerateType()
        {
           
            int rekord = 0;

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_AccelerateType.csv", FileMode.Open, FileAccess.Read))
            {

                StreamReader reader = new StreamReader(fileStream);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var values = line.Split(';');

                        if (rekord > 0)
                        {
                            CriteriaOperator criteria = CriteriaOperator.Parse("Code", values[0]);
                            AccelarateType existedAccType = (AccelarateType)View.ObjectSpace.FindObject(typeof(AccelarateType), criteria, true);

                            if (values[0] != "")
                            {
                                AccelarateType acctype = View.ObjectSpace.CreateObject<AccelarateType>();
                                acctype.Name = values[0];
                                acctype.Accelerate = Convert.ToByte(values[1]);
                                acctype.Code = Convert.ToChar(values[2]);
                                acctype.Save();
                            }

                        }
                        rekord++;
                    }
                    View.ObjectSpace.CommitChanges();
                
            }
        }
        #endregion

        //#region Státuszok beolvasása  / Status
        //private void Import_Status()
        //{
        //    var fileContent = string.Empty;
        //    var filePath = string.Empty;
        //    int rekord = 0;

        //    using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_Status.csv", FileMode.Open, FileAccess.Read))
        //    {
 
        //            StreamReader reader = new StreamReader(fileStream);

        //            while (!reader.EndOfStream)
        //            {
        //                string line = reader.ReadLine();
        //                var values = line.Split(';');

        //                if (rekord > 0)
        //                {
        //                    CriteriaOperator criteria = CriteriaOperator.Parse("Code", values[0]);
        //                    Status existedStatus = (Status)View.ObjectSpace.FindObject(typeof(Status), criteria, true);

        //                    if (values[0] != "")
        //                    {
        //                        Status status = View.ObjectSpace.CreateObject<Status>();
        //                        status.Code = values[0];
        //                        status.Name = values[1];

        //                        status.Save();
        //                    }

        //                }
        //                rekord++;
        //            }
        //            View.ObjectSpace.CommitChanges();
                
        //    }
        //}

        //#endregion

        #region Tárhelyek beolvasása / StorageLocation

        private void Import_StorageLocation()
        {
            
            int rekord = 0;

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_StorageLocation.csv", FileMode.Open, FileAccess.Read))
            {

                StreamReader reader = new StreamReader(fileStream);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    if (rekord > 0)
                    {
                        CriteriaOperator criteriaAisle = new BinaryOperator("Name", values[0]);
                        Aisle aisle = (Aisle)View.ObjectSpace.FindObject(typeof(Aisle), criteriaAisle, true);


                        CriteriaOperator criteriaAbc = new BinaryOperator("Code", values[5]);
                        AbcType abc = (AbcType)View.ObjectSpace.FindObject(typeof(AbcType), criteriaAbc, true);

                        if (values[0] != "")
                        {
                            StorageLocation storage = View.ObjectSpace.CreateObject<StorageLocation>();
                            storage.Aisle = aisle;
                            storage.Block = Convert.ToByte(values[1]);
                            storage.Column = Convert.ToInt32(values[2]);
                            storage.Row = Convert.ToByte(values[3]);
                            storage.Name = values[4];
                            storage.AbcClass = abc;
                            storage.LHU1X = Convert.ToInt32(values[6]);
                            storage.LHU1Y = Convert.ToInt32(values[7]);
                            storage.LHU2X = Convert.ToInt32(values[8]);
                            storage.LHU2Y = Convert.ToInt32(values[9]);
                            storage.Height = Convert.ToInt32(values[10]);
                            storage.StatusCode = 0;

                            storage.Save();
                        }

                    }
                    rekord++;
                }
                View.ObjectSpace.CommitChanges();
            } 
        }
        #endregion

        #region Termékhordók beolvasása / LoadCarrier
        private void Import_LoadCarrier()
        {
            
            int rekord = 0;

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_LoadCarrier.csv", FileMode.Open, FileAccess.Read))
            {

                    StreamReader reader = new StreamReader(fileStream);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var values = line.Split(';');

                        if (rekord > 0)
                        {
                            CriteriaOperator criteriaLctType = new BinaryOperator("Types", values[1]);
                            LoadCarrierType lct = (LoadCarrierType)View.ObjectSpace.FindObject(typeof(LoadCarrierType), criteriaLctType, true);


                            if (values[0] != "")
                            {
                                LoadCarrier lc = View.ObjectSpace.CreateObject<LoadCarrier>();
                                lc.BarCode = Convert.ToDecimal(values[0]);
                                lc.LcType = lct;
                                lc.RFID1 = Convert.ToString(values[2]);
                                lc.RFID2 = Convert.ToString(values[3]);
                                lc.StatusCode = 0;
                                lc.isEmpty = true;


                                lc.Save();
                            }

                        }
                        rekord++;
                    }
                    View.ObjectSpace.CommitChanges();
                
            }
        }
        #endregion

        #region Tranzakció típusok beolvasása /  CommonTrType
        private void Import_CommonTrType()
        {
           
            int rekord = 0;

            using (var fileStream = new FileStream(@"C:\Projects\LogXExplorer\Imports\LogX_CommonTrType_Trans.csv", FileMode.Open, FileAccess.Read))
            {

                    StreamReader reader = new StreamReader(fileStream);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var values = line.Split(';');

                        if (rekord > 0)
                        {
                            CriteriaOperator criteriaDefPartner = new BinaryOperator("Name", values[3]);
                            Customer c = (Customer)View.ObjectSpace.FindObject(typeof(Customer), criteriaDefPartner, true);


                            if (values[0] != "")
                            {
                                CommonTrType ctrt = View.ObjectSpace.CreateObject<CommonTrType>();
                                ctrt.Type = values[0];
                                ctrt.Prefix = values[1];
                                ctrt.DateDepended = Convert.ToBoolean(values[2]);
                                ctrt.DefaultPartner = c;
                                ctrt.Name = values[4];
                                ctrt.Creatable = Convert.ToBoolean(values[5]);
                                ctrt.Save();

                                Sorszam sorszam = View.ObjectSpace.CreateObject<Sorszam>();
                                sorszam.Type = ctrt;
                                sorszam.LastNum = 0;
                                if (ctrt.DateDepended)
                                {
                                    sorszam.Year = 2019;
                                }else
                                {
                                    sorszam.Year = 0;
                                }
                                sorszam.Save();
                        }
                    }
                        rekord++;
                    }
                    View.ObjectSpace.CommitChanges();
                
            }
        }
        #endregion

        private void ImportLoadCarrierToStorageLocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IList listSlocation = View.ObjectSpace.GetObjects(typeof(StorageLocation));
            SplashScreenManager.ShowForm(typeof(Splash));
            SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProcessText, "Tárhelyek feltöltése üres ládákkal");

            int maxSize = 100;
            int counter = listSlocation.Count;
            for (int i = 0; i < counter; i++)
            {
                int szazalek = Convert.ToInt32((i + 1) / counter * 100);

                StorageLocation sloc = (StorageLocation)listSlocation[i];

                CriteriaOperator cop = new BinaryOperator("Oid", i + 1);
                LoadCarrier lc = (LoadCarrier)View.ObjectSpace.FindObject(typeof(LoadCarrier), cop);

                sloc.StatusCode = 1;
                sloc.LoadCarrier = lc;
                sloc.LcIsEmpty = true;
                SplashScreenManager.Default.SendCommand(Splash.SplashScreenCommand.SetProgress, Math.Min(maxSize,szazalek));
            }

            ObjectSpace.CommitChanges();
            SplashScreenManager.CloseForm(false);
            MessageBox.Show("Táblázatok importálása megtörtént!");

        }
    }
}



