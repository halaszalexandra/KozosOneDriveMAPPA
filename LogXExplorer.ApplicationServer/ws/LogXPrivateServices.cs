using LogXExplorer.Module.comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LogXExplorer.ApplicationServer.ws 
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class LogXPrivateServices : ILogXPrivateServices
    {
        public void CallLoadCarriers(int ctrH, string commonType, int iocp, int weight)
        {
            LogXServer.getInstance().CallLoadCarriers(ctrH, commonType, iocp, weight);
        }

        public bool ChangeCommonTrHeaderStatus(int CtrhID, int status)
        {
            return LogXServer.getInstance().ChangeCommonTrHeaderStatus(CtrhID, status);
        }

        public void CreateStockHistory(int direction, int LoadCarrierId, int ProductId, int ctrDId, double quantity, string section, int type)
        {
            throw new NotImplementedException();
        }

        public void SendLoadCarrierBack(int ctrH, int ctrD, int lc, int IocpId, int weight)
        {
            LogXServer.getInstance().SendLoadCarrierBack(ctrH, ctrD, lc, IocpId, weight);
        }

        public string DoWork(string param1, string param2)
        {
            return LogXServer.getInstance().DoWork(param1, param2);
        }
        public string GetCustomerName(int custID)
        {
            return LogXServer.getInstance().GetCustomerName(custID);
        }

        public ushort GetNewSorszam(string commonType)
        {
            return LogXServer.getInstance().GetNewSorszam(commonType);
        }

        public void LcNumPreCalculation(int CtrH)
        {
            LogXServer.getInstance().LcNumPreCalculation(CtrH);
        }

        public void PerformDetail(int CtrH)
        {
            throw new NotImplementedException();
        }
    }
}
