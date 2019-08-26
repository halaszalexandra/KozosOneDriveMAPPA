using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Configuration;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp;
using System.Collections;
using System.ServiceModel;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.MiddleTier;
using LogXExplorer.Module.Controllers;
using LogXExplorer.Module;
using LogXExplorer.Module.comm;
using Opc.Ua;
using System.Threading;
using LogXExplorer.Module.BusinessObjects.Database;
using DevExpress.Data.Filtering;
using LogXExplorer.ApplicationServer.opc;
using LogXExplorer.ApplicationServer.ws;
using LogXExplorer.Module.BusinessObjects;
using log4net;

//ezt a mezot nem viszi at a webservice
//[XmlIgnore]
//private string alma

namespace LogXExplorer.ApplicationServer
{
    //ennélkül nem megy a wshost singleton módban.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class LogXServer : ILogXPrivateServices, ILogXPublicServices
    {
        private static LogXServer instance = null;
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(OPCClient));

        private ServerApplication serverApplication = null;
        private IObjectSpace serverObjectSpace = null;

        private WcfXafServiceHost xafServiceHost;
        private System.ServiceModel.ServiceHost wsHostPrivate = null;
        private System.ServiceModel.ServiceHost wsHostPublic = null;
        private LogXPrivateServices privateWSServices = null;
        private LogXPublicServices publicWSServices = null;
        private OPCClient opcClient = null;
        List<Aisle> aisleList = new List<Aisle>();
        List<string> abcTypeList = new List<string>();
        List<int> storageHeightList = new List<int>();

        //Lockok
        private Object doWorkLock = new Object();
        private Object sorszamLock = new Object();
        private Object bookingLock = new Object();
        private Object locationStatusLock = new Object();
        private Object findEmptyLcLock = new Object();
        private Object findLocationLock = new Object();
        
        //Konstruktor
        public LogXServer()
        {
            log.Info("LogXServer constructor has been called!");
        }

        private static void serverApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
            e.Updater.Update();
            e.Handled = true;
        }


        #region Start,Stop
        /********************* 
        *     INIT!!!
       **********************/
        public static void staticInit()
        {
            instance = new LogXServer();
            instance.init();
        }
        public static void staticDestroy()
        {
            instance.destroy();
        }

        public void init()
        {
            try
            {
                log.Info("LogXServer init() begins..!");
                DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
                DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;

                //INIT XAF AND DEFAULT DB SERVER
                initXAFServer();
                log.Info("LogXServer.xafServer inited.");

                //INIT WS PRIVATE INTERFACES HTTP SELFHOSTING
                initWSHostPrivateInterfaces();

                //INIT WS PUBLIC INTERFACES HTTP SELFHOSTING
                initWSHostPublicInterfaces();
                log.Info("LogXServer.ws interfaces are inited.");

                //INIT OPC
                opcClient = new OPCClient();
                opcClient.init();
                log.Info("LogXServer opcClient inited");

                SetAisleList();
                SetAbcTypeList();
                SetStorageHeightList();
                log.Info("LogXServer default lists loaded");


                log.Info("LogXServer init() end!");
            }
            catch (Exception e)
            {
                log.Fatal("Error in LogXServer.init(): ", e);
            }
        }


        #region Inicializálás
        private void initXAFServer()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            ValueManager.ValueManagerType = typeof(MultiThreadValueManager<>).GetGenericTypeDefinition();

            SecurityAdapterHelper.Enable();
            serverApplication = new ServerApplication();
            serverApplication.ApplicationName = "LogXExplorer";
            serverApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && serverApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                serverApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            serverApplication.Modules.BeginInit();
            serverApplication.Modules.Add(new DevExpress.ExpressApp.Security.SecurityModule());
            serverApplication.Modules.Add(new LogXExplorer.Module.LogXExplorerModule());
            serverApplication.Modules.Add(new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule());
            //serverApplication.Modules.Add(new LogXExplorer.Module.Win.LogXExplorerWindowsFormsModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.AuditTrail.AuditTrailModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.CloneObject.CloneObjectModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.Dashboards.DashboardsModule() { DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData) });
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.Notifications.NotificationsModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.Notifications.Win.NotificationsWindowsFormsModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2() { ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2) });
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.ReportsV2.Win.ReportsWindowsFormsModuleV2());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.Validation.ValidationModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule());
            //serverApplication.Modules.Add(new DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule());
            serverApplication.Modules.EndInit();

            XPObjectSpaceProvider objectSpaceProvider = null;
            serverApplication.DatabaseVersionMismatch += new EventHandler<DatabaseVersionMismatchEventArgs>(serverApplication_DatabaseVersionMismatch);
            serverApplication.CreateCustomObjectSpaceProvider += (s, e) =>
            {
                objectSpaceProvider = new XPObjectSpaceProvider(e.ConnectionString, e.Connection);
                e.ObjectSpaceProviders.Add(objectSpaceProvider);
                e.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(serverApplication.TypesInfo, null));
            };
            serverApplication.ConnectionString = connectionString;
            serverApplication.Setup();
            serverApplication.CheckCompatibility();
            //serverApplication.Dispose();

            //            //korábbi ConnectionHelper kiváltása
            //            XpoDefault.DataLayer = objectSpaceProvider.DataLayer;
            //            XpoDefault.GetDataLayer
            this.serverObjectSpace = serverApplication.ObjectSpaceProvider.CreateObjectSpace();

            Func <IDataServerSecurity> dataServerSecurityProvider = () =>
            {
                SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), new AuthenticationStandard());
                security.SupportNavigationPermissionsForTypes = false;
                return security;
            };

            //+++++ configból a connection stringet: net.tcp://127.0.0.1:1451/DataServer
            xafServiceHost = new WcfXafServiceHost(connectionString, dataServerSecurityProvider);
            xafServiceHost.AddServiceEndpoint(typeof(IWcfXafDataServer), WcfDataServerHelper.CreateNetTcpBinding(), "net.tcp://127.0.0.1:1451/DataServer");
            xafServiceHost.Open();

            //test                
            //ConnectionHelper.Connect(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, true);
            //log.Info("ConnectionHelper connected.");

        }

        private void initWSHostPrivateInterfaces()
        {
            //minden kéréshez új példányt kreál!!!
            //wsInterfaceHost = new System.ServiceModel.ServiceHost(typeof(LogXServer));

            privateWSServices = new LogXPrivateServices();
            wsHostPrivate = new System.ServiceModel.ServiceHost(privateWSServices);
            wsHostPrivate.Open();
            log.Debug("WS Privateinterface is up and running at:");
            foreach (var ea in wsHostPrivate.Description.Endpoints)
            {
                log.Debug(ea.Address);
            }
        }

        private void initWSHostPublicInterfaces()
        {
            //minden kéréshez új példányt kreál!!!
            //wsInterfaceHost = new System.ServiceModel.ServiceHost(typeof(LogXServer));

            publicWSServices = new LogXPublicServices();
            wsHostPublic = new System.ServiceModel.ServiceHost(publicWSServices);
            wsHostPublic.Open();
            log.Debug("WS Public interface is up and running at:");
            foreach (var ea in wsHostPublic.Description.Endpoints)
            {
                log.Debug(ea.Address);
            }
        }
        #endregion

        /******************** 
        *    Destroy!!!!
       *********************/
        public void destroy()
        {
            try
            {
                //példány konstruktor
                //logoljuk, mert kíváncsiak vagyunk, hogy csak egyszer hívódik e meg.
                log.Info("LogXServer.destroy() begin..");
                xafServiceHost.Close();
                log.Info("XAF service closed.");
                wsHostPrivate.Close();
                log.Info("WS Private service stopped.");
                wsHostPublic.Close();
                log.Info("WS Public service stopped.");
                opcClient.destroy();
                serverApplication.Dispose();
                log.Info("LogXServer.destroy() end.");
            }
            catch (Exception e)
            {
                log.Fatal("Error in LogXServer.destroy()", e);
            }
        }

        public static LogXServer getInstance()
        {
            return instance;
        }
        #endregion

        #region Alapértelmezett folyosók listája
        private void SetAisleList()
        {
            IObjectSpace os = null;

            try
            {
                os = GetNewObjectSpace();

                IQueryable<Aisle> aisles = os.GetObjectsQuery<Aisle>();
                var list = from aisle in aisles
                           select aisle;
                foreach (Aisle item in list)
                {
                    aisleList.Add(item);
                }
            }
            catch
            {
                log.Error("ObjectSpace not found");
            }
            finally
            {
                DisposeObjectSpace(os);
            }
            
        }
        #endregion

        #region Alapértelmezett ABC kategóriák lista
        private void SetAbcTypeList()
        {
            IObjectSpace os = null;

            try
            {
                os = GetNewObjectSpace();
                IQueryable<AbcType> abcList = os.GetObjectsQuery<AbcType>();
                var list = from abc in abcList
                           select abc;
                foreach (var item in list)
                {
                    abcTypeList.Add(item.Code);
                }
            }
            catch
            {
                log.Error("ObjectSpace not found");
            }
            finally
            {
                DisposeObjectSpace(os);
            }
            
        }
        #endregion

        #region Méretek listája
        private void SetStorageHeightList()
        {
            IObjectSpace os = null;

            try
            {
                os = GetNewObjectSpace();

                IQueryable<StorageLocation> stoList = os.GetObjectsQuery<StorageLocation>();
                var list = from s in stoList
                           group s by s.Height into cc
                           where cc.Count() >= 1
                           select new { height = cc.Key, Count = cc.Count() };
                foreach (var loc in list)
                {
                    storageHeightList.Add(loc.height);
                }
            }
            catch
            {
                log.Error("ObjectSpace not found");
            }
            finally
            {
                DisposeObjectSpace(os);
            }
        }
        #endregion

        #region ObjectSpace create,dispose
        public IObjectSpace GetNewObjectSpace()
        {
            IObjectSpace os = serverApplication.CreateObjectSpace();
            return os;
        }

        public void DisposeObjectSpace(IObjectSpace os)
        {
            os.Dispose();
        }
        #endregion

        /****************************************************************************************************************************************       
        PRIVATE PARTS
        **************************************************************************************************************************************/

        //Proxin keresztül hívódnak
        #region DoWork teszt
        public String DoWork(String param1, String param2)
        {
            lock (doWorkLock)
            {
                //itt a kenyes muvelet
                log.Info("DoWork service has been called.");
            }
            return param1 + param2;
        }
        #endregion

        #region Partnernév visszaadó Teszt
        public string GetCustomerName(int custID)
        {
            string name = "";
            IObjectSpace os = null;
            try
            {
                os = GetNewObjectSpace();

                IQueryable<Customer> customers = os.GetObjectsQuery<Customer>();
                var list = from c in customers
                           select c;
                foreach (Customer item in list)
                {
                    name = item.Name;
                }
            }
            catch
            {
                log.Error("ObjectSpace not found");
            }
            finally
            {
                DisposeObjectSpace(os);
            }
            return name;
        }
        #endregion

        #region Előkalkuláció - hány ládára van szükség
        /* A függvény kiszámolja, egy adott betárolási bizonylat minden tételsorához, hogy hány ládára van szükség a betároláshoz, az alapértelmezett ládában.
         */
        public void LcNumPreCalculation(int CtrH)
        {
            IObjectSpace os = null;

            try
            {
                os = GetNewObjectSpace();

                IQueryable<CommonTrDetail> details = os.GetObjectsQuery<CommonTrDetail>();
                    var list = (from c in details
                                where (c.CommonTrHeader.Oid == CtrH)
                                orderby c.ItemNum ascending
                                select c);

                QtyExchange qtyX = null;

                // végigmegyünk az ügylet összes tételsorán
                foreach (CommonTrDetail ctrd in list)
                {
                    int kalkulaltLadaszamTetelsoron = 0;
                    double tetelsorIgenyelt = ctrd.Quantity;
                    double tetelsorTeljesitett = ctrd.PerformedQty;
                    double maradekMennyiseg = tetelsorIgenyelt - tetelsorTeljesitett;
                    double maxBetarolhatoDb = 0;


                    if (maradekMennyiseg > 0)
                    {
                        // megkeressük az alapértelmezett betárolást
                        IQueryable<QtyExchange> atvaltasok = os.GetObjectsQuery<QtyExchange>();
                        var atvlist =   (from q in atvaltasok
                                        where (q.Product.Oid == ctrd.Product.Oid && q.In == true && q.Default == true)
                                        select q).Take(1);
                    
                        foreach (QtyExchange QtyEx in atvlist) qtyX = QtyEx;
                    
                        // megnézzük, hogy az alapértelmezett ládába, hány darabot tárolhatunk el
                        // Source = Hány egység (doboz,zacskó...)    Target= egységenként hány db  ------- 3 dobozban dobozonként 100 egység , összesen 300
                        maxBetarolhatoDb = qtyX.SourceQty * qtyX.TargetQty;

                        //Kiszámoljuk, hogy mennyi láda kell az aktuális darabszámra.
                        kalkulaltLadaszamTetelsoron = Convert.ToInt32(maradekMennyiseg / maxBetarolhatoDb);

                        // ha van maradék, plusz egy ládát kell kihivni.
                        if (maradekMennyiseg > (kalkulaltLadaszamTetelsoron * maxBetarolhatoDb))
                        {
                            kalkulaltLadaszamTetelsoron++;
                        }
                    }

                    if (kalkulaltLadaszamTetelsoron > 0)
                    {
                        ctrd.CalcLcNumber = (UInt32)kalkulaltLadaszamTetelsoron;

                    }
                    os.CommitChanges();
                }
            }
            catch
            {
                log.Error("hiba");
            }
            finally
            {
                DisposeObjectSpace(os);
            }
        }
        #endregion

        #region Ládák kihívása
        // Bejővő paraméterek (Ügylet száma, Komissiózó pont azonosítója, Komissiózó pont plc azonosítója)
        public void CallLoadCarriers(int ctrH, string commonType, int iocp, int weight)
        {
            IObjectSpace os = null;
            try
            {
                os = GetNewObjectSpace();
                //Megkeressük a bizonylat tételsorait
                
                IQueryable<CommonTrDetail> details = os.GetObjectsQuery<CommonTrDetail>();
                var list = (from c in details
                            where (c.CommonTrHeader.Oid == ctrH)
                            orderby c.ItemNum ascending
                            select c);

                //Végigmegyünk minden tételsoron és ládákat keresünk az ügylettípus szerint:
                foreach (CommonTrDetail ctrD in list)
                {
                    //BETÁROLÁS - annyi ÜRES!!! ládát kell hívnunk amennyit előkalkuláltunk
                    if (commonType == "BETAR")
                    {
                        for (int i = 0; i < ctrD.CalcLcNumber; i++)
                        {
                            // Keresünk egy üres ládát lehetőleg onnan ahová vissza is akarjuk küldeni és le is foglaljuk az erőforrást.
                            StorageLocation sourceLocation = FindEmptyLoadcarrier(ctrD.Product);

                            if (sourceLocation != null)
                            {
                                CreateTransportOrder(2, ctrH, ctrD.Oid, sourceLocation.LoadCarrier.Oid, iocp, weight, sourceLocation, null);
                            }
                            else
                            {
                                throw new Exception("Hiba - Nem talált kihívható ládát!");
                            }
                        }
                    }

                    //KITÁROLÁS ÉS KOMISSIÓ - Allokálnunk kell és FIFO szerint kihívni a terméket tartalmazó ládákat
                    if (commonType == "KITAR" || commonType == "KOMISSIO")
                    {
                        double szuksegesMennyiseg = ctrD.Quantity - ctrD.PerformedQty;
                        double maradekMennyiseg = szuksegesMennyiseg;
                        double allokaltMennyiseg = 0;

                        if (szuksegesMennyiseg > 0)
                        {
                            foreach (Stock stock in ctrD.Product.Stocks)
                            {
                                if (stock.StorageLocation != null && stock.StorageLocation.StatusCode == 1)
                                {
                                    allokaltMennyiseg = Math.Min(stock.NormalQty, maradekMennyiseg);
                                    if (allokaltMennyiseg > 0)
                                    {
                                        LoadCarrier lc = stock.LC;
                                        stock.StorageLocation.StatusCode = 2;
                                        maradekMennyiseg -= allokaltMennyiseg;

                                        CreateTransportOrder(2, ctrH, ctrD.Oid, lc.Oid, iocp, weight, stock.StorageLocation, null);
                                        ctrD.CalcLcNumber++;
                                    }
                                }
                            }
                        }
                    }

                    //LELTÁR minden ládát kihozunk amia terméket tartalmazza
                    if (commonType == "LELTAR" )
                    {
                        foreach (Stock stock in ctrD.Product.Stocks)
                        {
                            if (stock.StorageLocation != null && stock.StorageLocation.StatusCode == 1)
                            {
                                ChangeLocationStatus(stock.StorageLocation, 3);
                                CreateTransportOrder(2, ctrH, ctrD.Oid, stock.LC.Oid, iocp, weight, stock.StorageLocation, null);
                                ctrD.CalcLcNumber++;
                            }
                        }
                    }
                }
            }
            catch
            {
                log.Error("hiba");
            }
            finally
            {
                DisposeObjectSpace(os);
            }
        }
        #endregion

        #region Üres láda keresése tárolóhelyen betároláshoz
        public StorageLocation FindEmptyLoadcarrier(Product product)
        {
            StorageLocation retSl = null;
            
            //Megpróbálunk arról a tárolási helyről ládát kihívni ahová esetleg vissza is küldenénk

            bool talalt = false;
            List<int> meretek = product.GetLcHeights();
            List<Aisle> folyosok = GetProductAisleByStock(product);
            List<string> abcBesorolasok = GetAbcListByProduct(product.AbcClass.Code);

            if (folyosok.Count == 0)
            {
                folyosok = aisleList;
            }                
            

            for (int i = 0; i < meretek.Count && !talalt; i++)
            {
                for (int j = 0; j < folyosok.Count && !talalt; j++)
                {
                    for (int k = 0; k < abcBesorolasok.Count && !talalt; k++)
                    {
                        retSl = AdottTarolohelykeresesFolyoson(meretek[i], folyosok[j], abcBesorolasok[k],1);
                        if (retSl != null)
                        {
                            ChangeLocationStatus(retSl, 3);
                            talalt = true;
                        }
                    }
                }
            }
            return retSl;
        }
        #endregion

        #region Láda beküldése
        public void SendLoadCarrierBack(int ctrH, int ctrD, int lc, int IocpId, int weight)
        {
            IObjectSpace os = null;

            try
            {
                os = GetNewObjectSpace();

                LoadCarrier lca = os.FindObject<LoadCarrier>(new BinaryOperator("Oid", lc));
                StorageLocation targetLocation = FindEmptyLocation(lca);

                if (targetLocation != null)
                {
                    CreateTransportOrder(1, ctrH, ctrD, lca.Oid, IocpId, weight, null,targetLocation);
                }
                else
                {
                    throw new Exception("Nem talált tárolóhelyet");
                }
            }
            catch
            {
                log.Error("ObjectSpace not found");
            }
            finally
            {
                DisposeObjectSpace(os);
            }
         
        }
        #endregion

        #region Üres tárolóhely keresése láda beküldésnél
        public StorageLocation FindEmptyLocation(LoadCarrier loadCarrier)
        {
            lock (findLocationLock)
            {
                /*
                * Üres tárolóhelyet úgy kell megtalálni, hogy meg kell próbálni azokon a folyosókon ahol a termék tárolható egyenlő mértékben elosztani.
                * Ez azt jelenti, hogy meg kell nézni hogy eddig melyik folyosón mennyit tároltunk és növekvő sorrendben megpróbálni kiszolgálni.
                * A kiválasztást a láda mérete és a termék ABC besorolása is befolyásolja. 
                * Üres láda beküldésénél ezeket csak részben kell figyelembe venni.
                */
                StorageLocation sl = null;

                bool talalt = false;
                List<int> meretek = null;
                List<Aisle> folyosok = null;
                List<string> abcBesorolasok = null;

                if (loadCarrier.Stocks.Count == 0)
                {
                    // Folyosók default listája, inicialitzáláskor feltöltődik 
                    meretek = GetStorageHeightListByLc(loadCarrier.LcType.Height);
                    folyosok = aisleList;
                    abcBesorolasok = abcTypeList;
                }
                else
                {
                    //Ez azért kell, mert ha a ládában több termék van, akkor is csak 1 termék adataiból indulunk ki, mégpedig a legelső termék adataiból
                    for (int i = 0; i < 1; i++)
                    {
                        Product pr = loadCarrier.Stocks[i].Product;
                        meretek = GetStorageHeightListByLc(loadCarrier.LcType.Height);
                        folyosok = GetProductAisleByStock(pr);
                        abcBesorolasok = GetAbcListByProduct(pr.AbcClass.Code);
                    }
                }


                for (int i = 0; i < meretek.Count || !talalt; i++)
                {
                    for (int j = 0; j < folyosok.Count || !talalt; j++)
                    {
                        for (int k = 0; k < abcBesorolasok.Count || !talalt; k++)
                        {
                            sl = AdottTarolohelykeresesFolyoson(meretek[i], folyosok[j], abcBesorolasok[k],0);
                            if (sl != null)
                            {
                                ChangeLocationStatus(sl, 3);
                                talalt = true;
                            }
                        }
                    }
                }
                return sl;
            }
        }
        #endregion

        #region Tárolóhely megtalálása adott méretben, adott folyosón , adott ABC osztállyal

        /* Láda visszaküldés esetén üres tárolóhelyet keresünk (StatusCode = 0), adott folyosón, adott ládaméretben, adott abc kategóriának megfelelően.
            * Ha üres ládát küldünk vissza, akkor azABC besorolás nem számít.
            * A függvény adott paraméterekkel 1 rekordot ad vissza.
            */
        private StorageLocation AdottTarolohelykeresesFolyoson(int height, Aisle aisle, string abcTypeCode, int statusCode)
        {
            StorageLocation ret = null;
            IQueryable<StorageLocation> locations = serverObjectSpace.GetObjectsQuery<StorageLocation>();
            var list = (from c in locations
                        where (c.StatusCode == statusCode && c.Aisle==aisle && c.Height == height && (c.AbcClass.Code == abcTypeCode || abcTypeCode == null))
                        orderby c.Column, c.Row, c.Block ascending
                        select c).Take(1);
            foreach (StorageLocation item in list)
            {
                ret = item;
            }
            return ret;
        }
        #endregion

        #region Transzport sor létrehozása az adatbázisba
        public void CreateTransportOrder(int type,  int ctrH, int ctrD, int lc,   int IocpId , int weight, StorageLocation sourceLocation, StorageLocation targetLocation)
        {
            

            //Megkeressük az erőforrásokat
            CommonTrHeader ctrHeader = serverObjectSpace.FindObject<CommonTrHeader>(new BinaryOperator("Oid", ctrH));
            CommonTrDetail cdetail = serverObjectSpace.FindObject<CommonTrDetail>(new BinaryOperator("Oid", ctrD));
            LoadCarrier loadCarrier = serverObjectSpace.FindObject<LoadCarrier>(new BinaryOperator("Oid", lc));
            Iocp iocp = serverObjectSpace.FindObject<Iocp>(new BinaryOperator("Oid", IocpId));

            //Osztumk egy új sorszámot
            ushort UjTransportID = GetNewSorszam("TPO");

            //Létrehozzuk az új transzportot az adatbázisban

            TransportOrder transportOrder = serverObjectSpace.CreateObject<TransportOrder>();
            transportOrder.Iocp = iocp;
            transportOrder.TpId = UjTransportID;
            transportOrder.LC = loadCarrier;
            
            transportOrder.CommonTrHeader = ctrHeader;
            transportOrder.CommonDetail = cdetail;
            transportOrder.Type = type;
            transportOrder.TargetTag = iocp.TargetTag;
            transportOrder.SourceLocation =sourceLocation;
            transportOrder.TargetLocation=targetLocation;
            transportOrder.Weight=weight;
            serverObjectSpace.CommitChanges();
            // Hozzáadjuk a megfelelő iocp zsák queue-hoz
            this.opcClient.addJob(transportOrder);
        }
        #endregion

        #region Tárolóhely státusz állítás
        public bool ChangeLocationStatus(StorageLocation location, int status)
        {
            bool succes = false;
            log.Debug("Location status is changed: " + status);
            lock (locationStatusLock)
            {
                location.StatusCode = status;
                location.Save();
                succes = true;
            }
            return succes;
        }
        #endregion

        #region Bizonylat státusz állítás
        private Object lockCtrhStatus = new Object();
        public bool  ChangeCommonTrHeaderStatus(int CtrhID, int status)
        {
            bool ret = false;

            lock (lockCtrhStatus)
            {   
                CriteriaOperator criteria = CriteriaOperator.Parse("[Oid] = ?", CtrhID);
                CommonTrHeader cTrH = serverObjectSpace.FindObject<CommonTrHeader>(criteria);

                if(cTrH.Status< status)
                {
                    cTrH.Status = status;
                    serverObjectSpace.CommitChanges();
                    ret = true;
                    return ret;
                }
                else
                {
                    throw new Exception ("Az ügylet státusza időközben megváltozott!");
                }
            }
        }
        #endregion

        #region Új sorszám kérése
        public ushort GetNewSorszam(string commonType)
        {
            decimal ujSorszam = 0;
            
            CommonTrType cType = serverObjectSpace.FindObject<CommonTrType>(new BinaryOperator("Type", commonType));
            Sorszam tpoSorszam = null;

            lock (sorszamLock)
            {
                DateTime date = DateTime.Now;
                //Megkeressük, hogy a sorszámot aszerint, hogyy évfüggő vagy nem
                if (cType.DateDepended)
                {
                    CriteriaOperator cop = new GroupOperator(GroupOperatorType.And, new BinaryOperator("Type", cType.Oid), new BinaryOperator("Year", date.Year));
                    tpoSorszam = (Sorszam)serverObjectSpace.FindObject(typeof(Sorszam), cop);
                }
                else
                {
                    CriteriaOperator cop = new GroupOperator(GroupOperatorType.And, new BinaryOperator("Type", cType.Oid), new BinaryOperator("Year", 0));
                    tpoSorszam = (Sorszam)serverObjectSpace.FindObject(typeof(Sorszam), cop);
                }

                // Ha nem létezik sorszám létrehozunk egyet
                if (tpoSorszam == null)
                {
                    tpoSorszam = serverObjectSpace.CreateObject<Sorszam>();
                    tpoSorszam.Type = cType;

                    if (cType.DateDepended)
                    {
                        tpoSorszam.Year = Convert.ToUInt16(date.Year);
                    }
                    else
                    {
                        tpoSorszam.Year = 0;
                    }

                    tpoSorszam.LastNum = 1;
                    ujSorszam = 1;
                }
                // Ha létezik akkor kérünk egy új sorszámot
                else
                {
                    ujSorszam = tpoSorszam.GetNewNumber();

                }
                serverObjectSpace.CommitChanges();
            }
            return Convert.ToUInt16(ujSorszam);
        }
        #endregion

        #region Készlet napló sor létrehozása
        // A készlet napló létrehozása még nem mozgratja a termék kélszletét, csak azt jelzi, hogy a ládába bekerült vagy kikerült a termék
        // A termék készlet könyvelése a transzport sor státuszváltozása alapján történik
        private void CreateStockHistory(int direction, int LoadCarrierId, int ProductId, int ctrDId, double quantity, string section, int type)
        {
            Session workSession = new Session();
            //Láda
            LoadCarrier Lc = workSession.FindObject<LoadCarrier>(new BinaryOperator("Oid", LoadCarrierId));
            //Termék
            Product product = workSession.FindObject<Product>(new BinaryOperator("Oid", ProductId));
            //Tételsor
            CommonTrDetail ctrD = workSession.FindObject<CommonTrDetail>(new BinaryOperator("Oid", ctrDId));

            StockHistory stockhistory = new StockHistory(workSession);
            stockhistory.Product = product;
            stockhistory.LC = Lc;
            stockhistory.Section = section;
            stockhistory.Quantity = quantity;
            stockhistory.CommonDetail = ctrD;
            stockhistory.Time = DateTime.Now;
            stockhistory.Type = type;
            stockhistory.Direction = direction;
            stockhistory.Processed = false;
            stockhistory.Booked = false;
            stockhistory.Save();
        }
        #endregion

        #region Készletnapló tétel létrehozása
        void ILogXPrivateServices.CreateStockHistory(int direction, int LoadCarrierId, int ProductId, int ctrDId, double quantity, string section, int type)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Tételsor teljesítés
        public void PerformDetail(int CtrH)
        {
            Session session = new Session();
            CommonTrHeader ctrh = session.FindObject<CommonTrHeader>(new BinaryOperator("Oid", CtrH));
            foreach (CommonTrDetail detail in ctrh.CommonTrDetails)
            {
                double PerformedQty = 0;
                foreach (StockHistory sh in detail.StockHistories)
                {
                    if (!sh.Processed)
                    {
                        PerformedQty += sh.Quantity;
                        sh.Processed = true;
                    }
                }
                detail.PerformedQty += PerformedQty;
            }
        }
        #endregion

        #region Készletek könyvelése
        public void BookingStorageHistory(int ctrDID)
        {

            lock (bookingLock)
            {
                DevExpress.Xpo.Session session = new Session();
                CommonTrDetail commonTrDetail = session.FindObject<CommonTrDetail>(new BinaryOperator("Oid", ctrDID));

                foreach (StockHistory sh in commonTrDetail.StockHistories)
                {
                    if (!sh.Booked)
                    {
                        switch (sh.Type)
                        {
                            case 0:
                                sh.Product.NormalQty = (sh.Direction == 1) ? sh.Product.NormalQty + sh.Quantity : sh.Product.NormalQty - sh.Quantity;
                                sh.Booked = true;
                                break;

                            case 1:
                                sh.Product.ReservedQty = (sh.Direction == 1) ? sh.Product.ReservedQty + sh.Quantity : sh.Product.ReservedQty - sh.Quantity;
                                sh.Booked = true;
                                break;

                            case 2:
                                sh.Product.BlockedQty = (sh.Direction == 1) ? sh.Product.BlockedQty + sh.Quantity : sh.Product.BlockedQty - sh.Quantity;
                                sh.Booked = true;
                                break;

                            case 3:
                                sh.Product.DispousedQty = (sh.Direction == 1) ? sh.Product.DispousedQty + sh.Quantity : sh.Product.DispousedQty - sh.Quantity;
                                sh.Booked = true;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Manipulator
        //private void LogX_Manipulator_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{

        //    //DialogResult rs = MessageBox.Show("Mannipulátor kiszolgálás indulhat?", "T", MessageBoxButtons.YesNo);
        //    //if (rs == DialogResult.Yes)
        //    //{

        //    //    Iocp iocp = (Iocp)e.CurrentObject;
        //    //    Product product = iocp.TargetProduct;
        //    //    IList<Stock> stocks = product.Stocks;


        //    //    if (product != null && iocp.targetLcNum > 0)
        //    //    {
        //    //        //ComissionHeader ch = ObjectSpace.CreateObject<ComissionHeader>();
        //    //        //ch.Reference = iocp.TargetTag.ToString();
        //    //        //ch.Manipulator = true;
        //    //        //ch.Save();
        //    //        //ObjectSpace.CommitChanges();
        //    //        //ch.Status = FindStatus(15);


        //    //        CommonTrDetail detail = ObjectSpace.CreateObject<CommonTrDetail>();
        //    //        detail.Product = product;
        //    //        CriteriaOperator copQtyE = new GroupOperator(GroupOperatorType.And, new BinaryOperator("In", true), new BinaryOperator("Product", product.Oid));
        //    //        QtyExchange qtyEx = View.ObjectSpace.FindObject<QtyExchange>(copQtyE);
        //    //        detail.Quantity = qtyEx.TargetQty * iocp.targetLcNum;
        //    //        //detail.CommonTrHeader = (CommonTrHeader)ch;

        //    //        for (int i = 0; i < iocp.targetLcNum; i++)
        //    //        {
        //    //            Stock stock = stocks[i];
        //    //            CreateStockHistory("T", stock.LC.Oid, detail.Product.Oid, qtyEx.Oid, detail.Oid, detail.Quantity);
        //    //            GetFromLoadCarrier(stock.LC.Oid, stock.Product.Oid, stock.NormalQty);
        //    //            detail.PerformedQty += stock.NormalQty;
        //    //            LokacioStatuszAllitas(stock.StorageLocation.Oid, 2);

        //    //            CreateTransports(false, ch.Oid, detail.Oid, stock.LC.Oid, stock.StorageLocation.Oid, 0,iocp.TargetTag, 0);
        //    //            BookingStorageHistory(stock.LC.Oid);

        //    //        }

        //    //        //ch.Status = FindStatus(50);
        //    //        ObjectSpace.CommitChanges();
        //    //    }
        //    //}
        //}
        #endregion

        #region Tárolóhely felszabadítás
        public void LokacioFelszabadítas(StorageLocation sl)
        {
            bool siker = ChangeLocationStatus(sl, 0);
            if (siker)
            {
                sl.LoadCarrier = null;
                sl.LcIsEmpty = false;
                sl.Save();
            }
        }
        #endregion

        #region ABC kategoria lista termék szerint 
        public List<string>  GetAbcListByProduct(string  abcTypeCode)
        {
            List<string> output = new List<string>();
            int index = abcTypeList.FindIndex(a => a.Equals(abcTypeCode));
            for (int i = 0; i < abcTypeList.Count; i++)
            {
                output.Add(abcTypeList[index]);
                index++;
                if (index >= abcTypeList.Count)
                {
                    index = 0;
                }
            }
            return output;
        }
        #endregion

        #region  Tárolóhely méretek növekvő listája ládaméret szerint
        public List<int> GetStorageHeightListByLc(int height)
        {
            List<int> output = new List<int>();
            int index = storageHeightList.FindIndex(a => a.Equals(height));
            for (int i = 0; i < storageHeightList.Count - index; i++)
            {
                output.Add(storageHeightList[index]);
                index++;
            }
            return output;
        }
        #endregion

        #region Folyosó lista termék szerint
        private List<Aisle> GetProductAisleByStock(Product product)
        {
            /* Feladat, hogy lehetőleg azokon a folyosókon amin a termék tárolható, egyenlő mértékben osszuk el a készleteket. Ehhez olyan listát készítünk, 
             * ami a folyosókat tartalmazza a bennük tárolt készlet mennyiség függvényében, növekvő sorrendben.
             */


            List<Aisle> ret = new List<Aisle>();
            List<KeyValuePair<Aisle, double>> d = new List<KeyValuePair<Aisle, double>>();

            //Feltöltöm a használható folyosókat 0 val. Minden terméknél meg van adva, hogy melyik folyosókon tárolható
            foreach (ProductProducts_AisleAisles pAisle in product.ProductProducts_AisleAisless)
            {
                //d.Add(pAisle.Aisles, 0);
                d.Add(new KeyValuePair<Aisle, double>(pAisle.Aisles, 0));
            }

            //Hozzárendelem a készlet tételeket a megfelelő folyosóhoz
            foreach (Stock stock in product.Stocks)
            {
                d.Add(new KeyValuePair<Aisle, double>(stock.StorageLocation.Aisle, stock.NormalQty));
            }

            //Összegzünk
            List<KeyValuePair<Aisle, double>> sums = new List<KeyValuePair<Aisle,double>>();
            foreach (KeyValuePair<Aisle, double> pair in d)
            {
                if (sums.FindIndex(x => x.Key == pair.Key) < 0)
                {
                  double total = d.Where(x => x.Key == pair.Key).Sum(x => x.Value);
                  sums.Add(new KeyValuePair<Aisle, double>(pair.Key, total));
                }
            }

            //Visszaadjuk csak a folyosókat a megfelelő sorrendben
            foreach (var q in sums)
            {
                ret.Add(q.Key);
            }
            return ret;
        }
        #endregion

        /****************************************************************************************************************************************       
        PUBLIC PARTS
        *****************************************************************************************************************************************/

        #region Abc osztály nevének visszaadása (Teszteléshez)
        public string GetAbcClassName(string classCode)
        {
            string ret = "item not found";
            Session session = new Session();
            AbcType abc = session.FindObject<AbcType>(new BinaryOperator("Code", classCode));
            //if (ret != null)
            {
                ret = abc.Name;
            }
            return ret;
        }
        #endregion

        #region Új termék létrehozása
        public Product CreateNewProduct(String Identifier, String Name)
        {
            Product product = new Product(new Session());
            product.Identifier = Identifier;
            product.Name = Name;
            product.Save();

            return product;
        }
        #endregion

        #region Új betárolási bizonylat létrehozása
        public string CreateNewStorage(string classCode)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Új kitárolási bizonylat létrehozása
        public string CreateNewComission(string classCode)
        {
            throw new NotImplementedException();
        }
        #endregion

        # region Termékek készlete vagy az összes termék készlet visszaadása nem részletes
        public void GetProductsStock()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Termékek készlete vagy az összes termék készlet visszaadása részletes
        public void GetProductsStockDetails()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
