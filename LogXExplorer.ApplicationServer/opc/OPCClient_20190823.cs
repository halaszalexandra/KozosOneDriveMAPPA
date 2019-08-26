using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LogXExplorer.Module;
using LogXExplorer.Module.BusinessObjects.Database;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogXExplorer.ApplicationServer;
using System.Collections.Concurrent;
using log4net;
//using Opc.Ua.Client;
//using static System.Collections.Generic.Dictionary<TKey, TValue>;
//using static System.Collections.Generic;

namespace LogXExplorer.ApplicationServer.opc
{
    public class OPCClient {
        //https://logging.apache.org/log4net/release/manual/configuration.html
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(OPCClient));

        // Változók
        private UAClientHelperAPI myClientHelperAPI;
        private Opc.Ua.Client.Session opcSession;
        private int iocpkSzama = 0;
        
        //iocpid + "_in", iocpid + "_out"
        Dictionary<String, BlockingCollection<TransportOrder>> zsakok = new Dictionary<String, BlockingCollection<TransportOrder>>();

        //Lockok
        private Object findLocationLock = new Object();

        //Configból kiolvasott értékek
        #region Configból kiolvasott értékek
        private readonly string secPolicy = ConfigurationManager.AppSettings["secPolicy"];
        private readonly string opcserver = ConfigurationManager.AppSettings["opcserver"];
        private readonly string opcuser = ConfigurationManager.AppSettings["opcuser"];
        private readonly string opcpwd = ConfigurationManager.AppSettings["opcpwd"];
        private readonly string endpointUrl = ConfigurationManager.AppSettings["EndpointUrl"];
        private readonly string discoveryUrl = ConfigurationManager.AppSettings["DiscoveryUrl"];
        private readonly string opcSetNodId = ConfigurationManager.AppSettings["OpcSetNodId"];
        private readonly string opcSetObjId = ConfigurationManager.AppSettings["OpcSetObjId"];
        private readonly string opcGetNodId = ConfigurationManager.AppSettings["OpcGetNodId"];
        private readonly string opcGetObjId = ConfigurationManager.AppSettings["OpcGetObjId"];
        private readonly string opcModNodId = ConfigurationManager.AppSettings["OpcModNodId"];
        private readonly string opcModObjId = ConfigurationManager.AppSettings["OpcModObjId"];
        private readonly string opcDelNodId = ConfigurationManager.AppSettings["OpcDelNodId"];
        private readonly string opcDelObjId = ConfigurationManager.AppSettings["OpcDelObjId"];
        private readonly string opcQueryQSize = ConfigurationManager.AppSettings["OpcQueryQSize"];
        private readonly string opcTransportStatusChanges = ConfigurationManager.AppSettings["opcTransportStatusChanges"];
        #endregion

        private int plcMaxQSize = 100;
        private Thread mainThread = null;
        private Boolean isRunning = false;
        private int sleepMillis = 100;
        private int qPointer = 0;


        #region Start OPC
        public void init()
        {
            try
            {
/*                
                log.Debug
                log.Info
                log.Error
                log.Fatal
*/

                myClientHelperAPI = new UAClientHelperAPI();
                EndpointDescription mySelectedEndpoint = null;
                log.Info("Start OPC connection...");
                //mySelectedEndpoint = CreateEndpointDescription(endpointUrl, secPolicy, MessageSecurityMode.None);
                mySelectedEndpoint = getEndpointDescription(endpointUrl);
                if (opcSession == null && mySelectedEndpoint != null)
                {
                    myClientHelperAPI.KeepAliveNotification += new Opc.Ua.Client.KeepAliveEventHandler(plcKeepAliveEventHandler);
                    //myClientHelperAPI.CertificateValidationNotification += new CertificateValidationEventHandler(Notification_ServerCertificate);

                    //Call connect
                    myClientHelperAPI.Connect(mySelectedEndpoint, opcuser.Length > 0, opcuser, opcpwd);
                    //myClientHelperAPI.Connect(mySelectedEndpoint.EndpointUrl, mySelectedEndpoint.SecurityPolicyUri, MessageSecurityMode.None, false, null, null);
                    opcSession = myClientHelperAPI.Session;
                    log.Info("OPC connect successfull");
                    

                    //load jobs from database
                    loadJobsFromDb();

                    //Új szál indítása
                    this.plcMaxQSize = int.Parse(ConfigurationManager.AppSettings["plc_max_q_size"]);
                    this.sleepMillis = int.Parse(ConfigurationManager.AppSettings["thread_sleepmillis"]);
                    this.isRunning = true;
                    this.mainThread = new Thread(this.Ciklus);
                    this.mainThread.Start();
                    
                }
             
            }
            catch (Exception exp)
            {
                log.Info("OPC not connected" + exp);
                
            }
        }
      
        
        #region Stop OPC
        public void destroy()
        {
            try
            {
                this.isRunning = false;
                if (opcSession != null)
                {
                    //Call connect
                    myClientHelperAPI.Disconnect();
                    opcSession = null;
                    log.Info("OPC disconnect successfull");
                }
            }
            catch (Exception exp)
            {
                log.Info("opcDisConnect error = " +exp);
            }
           
        }
        #endregion

        #region Endpoint
        //private EndpointDescription CreateEndpointDescription(string url, string secPolicy, MessageSecurityMode msgSecMode)
        //{
        //    // create the endpoint description.
        //    EndpointDescription endpointDescription = new EndpointDescription();

        //    // submit the url of the endopoint
        //    endpointDescription.EndpointUrl = url;

        //    // specify the security policy to use.
        //    endpointDescription.SecurityPolicyUri = secPolicy;
        //    endpointDescription.SecurityMode = msgSecMode;
        //    // specify the transport profile.
        //    endpointDescription.TransportProfileUri = Profiles.UaTcpTransport;
        //    return endpointDescription;
        //}

        //url="opc.tcp://192.168.0.15:4840"
        private EndpointDescription getEndpointDescription(string url)
        {
            EndpointDescription ret = null;
            EndpointDescriptionCollection endpoints = myClientHelperAPI.GetEndpoints(url);
            if (endpoints != null && endpoints.Count > 0) {
                ret = endpoints[0];
            }
            return ret;
        }




        #endregion

        private void loadJobsFromDb()
        {
            DevExpress.Xpo.Session session = new Session();
            XPQuery<Iocp> iocps = session.Query<Iocp>();
            var list = from i in iocps
                       // A Type megmondja, hogy milyen típusú az IOCP;  (0 = KomissióPont, 1 = Manipulátor)
                       where (i.IocpType == "0")
                       select i;

            iocpkSzama = list.Count();
            //int loop= 0;
            foreach (Iocp iocp in list)
            {
                //minden pultnak külön ki és be q-ja van, így nem kell végigvárnia egy beküldésnek az összes
                //kihívást ugyanazon pulton.
                BlockingCollection<TransportOrder> iocpQ_in = new BlockingCollection<TransportOrder>();
                BlockingCollection<TransportOrder> iocpQ_out = new BlockingCollection<TransportOrder>();
                foreach (TransportOrder tpo in iocp.TransportOrders) {
                    if (tpo.IsSendOpc) {
                        if (tpo.Type == 0){
                            iocpQ_out.Add(tpo);
                        }else {
                            iocpQ_in.Add(tpo);
                        }
                    }
                }
                zsakok.Add(iocp.Oid + "_in", iocpQ_in);   //(boundedCapacity: 100)
                zsakok.Add(iocp.Oid + "_out", iocpQ_out);   //(boundedCapacity: 100)
            }
            //SetCollection(zsakok[loop], iocp.Oid);
            //loop++;
        }
            #endregion


        #region Fő Ciklus
        public void Ciklus()
        {
            while (this.isRunning)
            {
                Boolean wasProcessed = false;
                //1. get status from PLC
                wasProcessed = wasProcessed || processPLCEvents();

                //2. lekérdezzük a plc q méretét
                int plcQSize = queryPLCQueueSize();
System.Console.WriteLine("plcQSize: " + plcQSize);
                if ((plcQSize > 0 ) && (plcQSize  < plcMaxQSize)) {

                    //3. process next job
                    bool sendJob = sendNextJobToPLC();
                    wasProcessed = wasProcessed || sendJob;
                }

                if (!wasProcessed)
                {
                    Thread.Sleep(this.sleepMillis);
                }
                else {
                    Thread.Sleep(1000);
                }
            }
        }
        #endregion

        private int queryPLCQueueSize() {
            int ret = -1;
            List<String> retVals = myClientHelperAPI.ReadValues(PLCTransaction.getOneInputParam(opcQueryQSize));
            if (retVals.Count > 0) {
                String firstVal = retVals.ElementAt<String>(0);
                ret = int.Parse(firstVal);
            }
            return ret;
        }


        private Boolean processPLCEvents() {
            Boolean ret = false;
            //Transport sorok státusváltozásának figylése legalább 5 öt próbáljon
            for (int i = 0; (i < 5) && this.isRunning; i++)
            {
                Int64 changedTpoAndStatus = ChangedTpoID(opcTransportStatusChanges);
                if (changedTpoAndStatus != 0)
                {
                    ret = true;
                    int lowerWord = (int)(0xFFFFFFFF & changedTpoAndStatus);
                    int higherWord = (int)((changedTpoAndStatus >> 32) & 0xFFFFFFFF);
                    Int32 tpoID = higherWord;
                    Int32 status = lowerWord;

                    if (tpoID > 0) {
                        //Lekezelni a státuszt 
                        TransportStatuszvaltozasKezeles(tpoID, status);
                        //Jelezni a PLC-nek, hogy leolvastuk az értéket
                        DeleteChangesTpNode();
                    }

                }
            }
            return ret;
        }


        private Boolean sendNextJobToPLC() {
            Boolean ret = false;
            //minden zsákba belenéz ha nincs az előző zsákban job.
            for (int ctr = 0; ctr < zsakok.Count && !ret; ctr++)
            {
                Dictionary<String, BlockingCollection<TransportOrder>>.KeyCollection keys = zsakok.Keys;
                String key = keys.ElementAt(qPointer);
                BlockingCollection<TransportOrder> queue = zsakok[key];
                TransportOrder job = null;
                Boolean hasJob = queue.TryTake(out job);
                if (hasJob && (job != null))
                {
                    //send job to plc
                    sendPLCTransaction(job);
                }
                else {
                    //ez nem kell, limitálatlan méretű q-k kellenek!!!
                    //ennek feltétele az, hogy az indulás után minden job a server oldalon kerüljön rögzítésre
                    //a db-ben és a memóriában is azonnal !!!
                    //meg kell probalni betolteni a db-bol N darab jobot az adott q-ba. 
                }
                qPointer++;
                if (qPointer >= zsakok.Count)
                {
                    qPointer = 0;
                }
            }
            return ret;
        }


        /*
         Minden beérkező jobot
         1. eltárol az adatbázisban a kommisiózó pult azonosítójával a jobot.
         2. beleteszi a saját blocking queue végére (minden pult in, out q)
         A queue-t a komissiózó pult azonosítója azonosítja
         */
        public void addJob(TransportOrder tpo) {
            String key = tpo.Iocp.Oid + tpo.Type == 0 ? "_out" : "_in";
            BlockingCollection<TransportOrder> q = zsakok[key];
            q.Add(tpo);
        }


       
        private void TransportStatuszvaltozasKezeles(int tpoID, int status)
        {
            DevExpress.Xpo.Session session = new Session();

            //ha a státusz igényli, akkor törölni kell a tpo-t a db-ből! !! ++++++++++++++++++++
            //ha a státusz igényli, akkor törölni kell a tpo-t a plc-ből is!!! ++++++++++++++++++++

            //Megkeresni a transzport sort
            TransportOrder transportOrder = session.FindObject<TransportOrder>(new BinaryOperator("Oid",tpoID));


            //LoadCarrier lc = Session.FindObject<LoadCarrier>(new BinaryOperator("Oid", LcType));
            //LoadCarrier loadcarrier = 

            //Figyelni hogy ki vagy be mozog a láda (0=Ki, 1=Be)

            //Kifelé jön
            if (transportOrder.Type == 0)
            {
                switch (status)
                {
                    case 0: { break; }
                    case 1: { break; }
                    case 2:
                    {
                            // Lokáció felszabadítása
                            //LogXServer.getInstance().ChangeLocationStatus(transportOrder.SourceLocation,2);
                            LogXServer.getInstance().LokacioFelszabadítas(transportOrder.SourceLocation);
                            
                         break;
                    }
                    case 3: { break; }
                    case 4: { break; }
                    case 5: { break; }
                    case 6: { break; }
                    case 7: { break; }
                    case 8: { break; }
                    case 9: { break; }
                    case 10:
                        {

                            break;
                        }
                }
            }
            else
            {
                switch (status)
                {
                    case 0: { break; }
                    case 1: { break; }
                    case 2:
                    {
                            //Booking: Ha befelé megy a láda akkor kell a készleteket könyvelni
                            LogXServer.getInstance().BookingStorageHistory(transportOrder.CommonDetail.Oid);
                            break;
                    }
                    case 3: { break; }
                    case 4: { break; }
                    case 5: { break; }
                    case 6: { break; }
                    case 7: { break; }
                    case 8: { break; }
                    case 9: { break; }
                    case 10:
                        {

                            break;
                        }
                }

            }
        }

   


        #region Adatok küldése 
        public void sendPLCTransaction(TransportOrder transportOrder)
        {
            List<string[]> inputData = PLCTransaction.getTransactionData(transportOrder);
            IList<object> outputValues = myClientHelperAPI.CallMethod(opcSetNodId, opcSetObjId, inputData);
        }


            public void SetTransportOrderORIG(int tpoId)
        {
            Session session = new Session();
            TransportOrder transportOrder = session.FindObject<TransportOrder>(CriteriaOperator.Parse("[TpId] = ?", tpoId));
            List<string[]> inputData = new List<string[]>();

            //string msgID = transportOrder.TpId.ToString();
            //string msgCtrhidCurrdSumd = Convert.ToString(JoinTogetherTransaction(CommonTrHeader.Oid, CommonDetail.Oid, CommonTrHeader.CommonTrDetails.Count));
            //string msgTypeAccPrTime = Convert.ToString(JoinTogetherTypeAccPrTime(0, Type, LC.LeglassabbGyorsulasVissz(6), 1, 0));
            //string msgLoadCarrier = Convert.ToString(LC.Oid);
            //string msgSource;
            //string msgSourceLHU1;
            //string msgSourceLHU2;
            //string msgTarget;
            //string msgTargetLHU1;
            //string msgTargetLHU2;


            //if (transportOrder.SourceLocation != null)
            //{
            //    //msgSource = Convert.ToString(JoinTogetherSource(SourceLocation.Block, SourceLocation.Row, SourceLocation.Column));
            //    //msgSourceLHU1 = Convert.ToString(JoinTogetherLhu(SourceLocation.LHU1X, SourceLocation.LHU1Y));
            //    //msgSourceLHU2 = Convert.ToString(JoinTogetherLhu(SourceLocation.LHU2X, SourceLocation.LHU2Y));
            //}
            //else
            //{
            //    msgSource = "0";
            //    msgSourceLHU1 = "0";
            //    msgSourceLHU2 = "0";
            //}

            //if (transportOrder.TargetLocation != null)
            //{
            //    msgTarget = Convert.ToString(JoinTogetherTarget(0, TargetLocation.Block, TargetLocation.Row, TargetLocation.Column, 0, 0));
            //    msgTargetLHU1 = Convert.ToString(JoinTogetherLhu(0, 0));
            //    msgTargetLHU2 = Convert.ToString(JoinTogetherLhu(0, 0));
            //}
            //else
            //{
            //    msgTarget = Convert.ToString(JoinTogetherTarget(0, 0, 0, 0, 0, TargetTag));
            //    msgTargetLHU1 = "0";
            //    msgTargetLHU2 = "0";
            //}



            //TODO Súly kitöltése
           // msgWeight = "0";
        
            IList<object> outputValues = myClientHelperAPI.CallMethod(opcSetNodId, opcSetObjId, inputData);
            //transportOrder.DecomposeTransportMessage(outputValues);
        }
        #endregion

        #region Komissióponthoz tartozó RFID tag leolvasása
        //public string ReadModulRfIdTag(string OpcTagId)
        //{
        //    string output = "";
        //    List<String> values = new List<String>();
        //    try
        //    {
        //        //////////////////////////////////////////////////
        //        values = myClientHelperAPI.ReadValues(PLCTransaction.getOneInputParam(OpcTagId));
        //        output = values.ElementAt<String>(0);
        //        return output;
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
        //}
        #endregion

        #region Van Státuszváltozás Tag figyelése
        public Int64 ChangedTpoID(string OpcTagId)
        {
            Int64 returnTransportID = 0;
            string output = "";
            List<String> values = new List<String>();
            try
            {
                values = myClientHelperAPI.ReadValues(PLCTransaction.getOneInputParam(opcTransportStatusChanges));
                output = values.ElementAt<String>(0);
                returnTransportID = Convert.ToInt64(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //log.Error(exp);
            }
            return returnTransportID;
        }
        #endregion

        #region Leolvasott státuszváltozás jelzése a PLC-nek hogy törölje az utoljára leolvasott értéket
        public void DeleteChangesTpNode()
        {
            List<string[]> input = new List<string[]>();
            string[] Tp1 = new string[2] { "0", "UInt16" };
            input.Add(Tp1);

            try
            {
                IList<object> outputDel = myClientHelperAPI.CallMethod(opcDelNodId, opcDelObjId, input);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        //public void DeleteTransport(IObjectSpace objectSpace, int tpoId)
        //{
        //    TransportOrder transportOrder = objectSpace.FindObject<TransportOrder>(CriteriaOperator.Parse("[TpId] = ?", tpoId));
        //    List<string[]> input = new List<string[]>();
        //    string[] Tp1 = new string[2] { tpoId.ToString(), "UInt16" };
        //    input.Add(Tp1);

        //    try
        //    {
        //        if (transportOrder != null)
        //        {
        //            IList<object> outputDel = myClientHelperAPI.CallMethod(opcDelNodId, opcDelObjId, input);
        //            transportOrder.Delete();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        // }
        #endregion

            private void plcKeepAliveEventHandler(Opc.Ua.Client.Session sender, Opc.Ua.Client.KeepAliveEventArgs e)
            {
//Console.WriteLine("plcKeepAliveEventHandler " + e);
                try
                {
                    // check for events from discarded sessions.
                    if (!Object.ReferenceEquals(sender, opcSession))
                    {
                        return;
                    }

                    // check for disconnected session.
                    if (!ServiceResult.IsGood(e.Status))
                    {
                        // try reconnecting using the existing session state
                        opcSession.Reconnect();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }


        }
    }
