using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace LogXExplorer.Module.comm
{
    [ServiceContract]
    public interface ILogXPrivateServices
    {
        //minden amit kliens-server kommunikációval akarsz híni, itt kell először deklarálni.
        //ide kell tenni valamennyi konkurrens hivast.

        //Teszt
        [OperationContract]
        String DoWork(String param1, String param2);

        [OperationContract]
        String GetCustomerName(int custID);

        [OperationContract]
        void LcNumPreCalculation(int CtrH);

        [OperationContract]
        void CreateStockHistory(int direction, int LoadCarrierId, int ProductId, int ctrDId, double quantity,string section, int type);

        //Bizonylat státusz állítás
        [OperationContract]
        bool ChangeCommonTrHeaderStatus(Int32 CtrhID, Int32 status);

        

        //Tételsorok teljesítése 
        [OperationContract]
        void PerformDetail(int CtrH);


        [OperationContract]
        ushort GetNewSorszam(string commonType);


        [OperationContract]
        void CallLoadCarriers(int ctrH, string commonType, int iocp, int weight);


        [OperationContract]
        void SendLoadCarrierBack(int ctrH, int ctrD, int lc, int IocpId, int weight);
    }
}
