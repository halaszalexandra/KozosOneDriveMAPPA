using LogXExplorer.Module.BusinessObjects.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogXExplorer.ApplicationServer.opc
{
    public interface IPLCHandler
    {
        void init(String endpointUrl, String opcuser, String opcpwd);

        void destroy();

        List<string> ReadValues(List<String> nodeIdStrings);

        IList<object> CallMethod(String methodIdString, String objectIdString, List<string[]> inputData);

        IList<object> sendPLCTransaction(TransportOrder transportOrder);
    }
}
