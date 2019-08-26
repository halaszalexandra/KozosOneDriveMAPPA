using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LogXExplorer.Module.comm
{
    class LogXPrivateServiceClientProxy : ClientBase<ILogXPrivateServices>, ILogXPrivateServices
    {
        //Teszt
        public string DoWork(String param1, String param2)
        {
            return base.Channel.DoWork(param1, param2);
        }

        public string GetCustomerName(int custID)
        {
            return base.Channel.GetCustomerName(custID);
        }


        //Bizonylat státusz állítás
        public bool ChangeCommonTrHeaderStatus(int CtrhID, int status)
        {
            return base.Channel.ChangeCommonTrHeaderStatus(CtrhID, status);
        }

        [OperationContract]
        public void CreateStockHistory(int direction, int LoadCarrierId, int ProductId, int ctrDId, double quantity, string section,int type)
        {
            base.Channel.CreateStockHistory(direction,LoadCarrierId,ProductId,ctrDId,quantity,section,type);
        }

        [OperationContract]
        public void LcNumPreCalculation(int CtrH)
        {
            base.Channel.LcNumPreCalculation(CtrH);
        }




        public void PerformDetail(int CtrH)
        {
            base.Channel.PerformDetail(CtrH);
        }

        public ushort GetNewSorszam(string commomnType)
        {
            return base.Channel.GetNewSorszam(commomnType);
        }
        public void CallLoadCarriers(int ctrH, string commonType, int iocp, int weight)
        {
           base.Channel.CallLoadCarriers(ctrH, commonType, iocp, weight);
        }


        public void SendLoadCarrierBack(int ctrH, int ctrD, int lc, int IocpId, int weight)
        {
            base.Channel.SendLoadCarrierBack(ctrH,ctrD,lc,IocpId,weight);
        }

    }
}
