using log4net;
using LogXExplorer.Module.BusinessObjects.Database;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogXExplorer.ApplicationServer.opc
{
    public class PLCHandler : IPLCHandler
    {
        private readonly string opcSetNodId = ConfigurationManager.AppSettings["OpcSetNodId"];
        private readonly string opcSetObjId = ConfigurationManager.AppSettings["OpcSetObjId"];
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PLCHandler));
        private UAClientHelperAPI myClientHelperAPI;
        private Opc.Ua.Client.Session opcSession;

        public PLCHandler() {
        }

        public void init(String endpointUrl, String opcuser, String opcpwd) {
            log.Debug("PLCHandler.init() begin");
            myClientHelperAPI = new UAClientHelperAPI();
            EndpointDescription mySelectedEndpoint = null;
            
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

                log.Debug("PLCHandler.init() end");
            }
        }

        public void destroy() {
            try
            {
                if (opcSession != null)
                {
                    //Call connect
                    myClientHelperAPI.Disconnect();
                    opcSession = null;
                    log.Info("PLCHandler disconnect successfull");
                }
            }
            catch (Exception exp)
            {
                log.Error("Error in PLCHandler.destroy()", exp);
            }

        }

        public List<string> ReadValues(List<String> nodeIdStrings) {
            return myClientHelperAPI.ReadValues(nodeIdStrings);
        }


        public IList<object> CallMethod(String methodIdString, String objectIdString, List<string[]> inputData) {
            return myClientHelperAPI.CallMethod(methodIdString, objectIdString, inputData);
        }

        public IList<object> sendPLCTransaction(TransportOrder transportOrder)
        {
            List<string[]> inputData = PLCTransaction.getTransactionData(transportOrder);
            IList<object> outputValues = CallMethod(opcSetNodId, opcSetObjId, inputData);
            return outputValues;
        }


        private EndpointDescription getEndpointDescription(string url)
        {
            EndpointDescription ret = null;
            EndpointDescriptionCollection endpoints = myClientHelperAPI.GetEndpoints(url);
            if (endpoints != null && endpoints.Count > 0)
            {
                ret = endpoints[0];
            }
            return ret;
        }


        private void plcKeepAliveEventHandler(Opc.Ua.Client.Session sender, Opc.Ua.Client.KeepAliveEventArgs e)
        {
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
                log.Error(ex);
            }
        }


    }

}

