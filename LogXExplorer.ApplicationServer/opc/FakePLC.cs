using log4net;
using LogXExplorer.Module.BusinessObjects.Database;
using LogXExplorer.Module.Win;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogXExplorer.ApplicationServer.opc
{
    public class FakePLC : IPLCHandler
    {

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

        private List<TransportOrder> queue = new List<TransportOrder>();
        private List<TransportOrder> changed = new List<TransportOrder>();
        private Thread mainThread = null;
        private bool isRunning = true;

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FakePLC));

        public List<string> ReadValues(List<string> inputParams)
        {
            List<String> ret = new List<string>();
            //plc q site
            if (compareParams(inputParams, opcQueryQSize)) {
                ret = getOneValueList(""+ queue.Count);
            }
            else
            //Transportorderid + status
            if (compareParams(inputParams, opcTransportStatusChanges))
            {
                //Transportorderid + status
                if (changed.Count > 0)
                {
                    TransportOrder tpo = changed[0];
                    Int64 tpoStatusInt = (Int64)tpo.TpId;
                    tpoStatusInt = (tpoStatusInt << 32) & tpo.msgStatus;
                    ret = getOneValueList(Convert.ToString(tpoStatusInt));
                }
                else {
                    ret = getOneValueList("0");
                }
            }
            return ret;
        }

        public IList<object> CallMethod(string methodIdString, string objectIdString, List<string[]> inputData)
        {
            IList<object> ret = new List<object>();
            if (methodIdString.Equals(opcDelNodId)) {
                //torolni kell a changed tpo-t a rekeszből
                if (changed.Count > 0) {
                    changed.RemoveAt(0);
                    //++++++++ majd amikor jön a tpo id is paraméterben akkor a nagy queu-ból is törölni kell
                    if (inputData != null && (inputData.Count > 0)) {
                        int tpoId = Convert.ToInt32(inputData[0]);
                        TransportOrder tpo = null;
                        for (int c=0; c<queue.Count && tpo == null; c++) {
                            if (queue[c].TpId == tpoId) {
                                tpo = queue[c];
                            }
                        }
                        if (tpo != null) {
                            queue.Remove(tpo);
                        }
                    }
                    
                }
            }
            return ret;
        }


        public IList<object> sendPLCTransaction(TransportOrder transportOrder) {
            queue.Add(transportOrder);
            return new List<object>();
        }






        private Boolean compareParams(List<string> inputParams, String compareTo) {
            bool ret = false;
            if (inputParams != null) {
                if (inputParams.Count > 0) {
                    ret = inputParams[0].Equals(compareTo);
                }
            }
            return ret;
        }

        private List<String> getOneValueList(Object val) {
            List<String> ret = new List<String>();
            if (val != null) {
                ret.Add(val.ToString());
            }
            return ret;

        }


        public void init(string endpointUrl, string opcuser, string opcpwd){
            this.mainThread = new Thread(this.run);
            this.mainThread.Start();
            log.Info("FakePLCEmulator has benn started!");
            //0, 2, 10
        }

        public void destroy()
        {
            isRunning = false;
        }


        public void run() {
            while (isRunning) {
                foreach (TransportOrder tpo in queue) {
                    if (tpo.msgStatus < 10) {       //(tpo.Type == 1 be, 2=ki) && 
                        tpo.msgStatus += 2;
                        changed.Add(tpo);
                    }
                }
                log.Debug("FakePLCEmulator.run()");
                Thread.Sleep(2000);
            }
        }
    }

}
