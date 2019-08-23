using LogXExplorer.Module.BusinessObjects.Database;
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
    public class LogXPublicServices : ILogXPublicServices
    {
        public string CreateNewComission(string classCode)
        {
            return LogXServer.getInstance().CreateNewComission(classCode);
        }

        //public Product CreateNewProduct(String Identifier, String Name)
        //{
        //    return LogXServer.getInstance().CreateNewProduct(Identifier,Name);
        //}

        public string CreateNewStorage(string classCode)
        {
            return LogXServer.getInstance().CreateNewStorage(classCode);
        }

        public string GetAbcClassName(string classCode)
        {
            return LogXServer.getInstance().GetAbcClassName(classCode);
        }

        public void GetProductsStock()
        {
            LogXServer.getInstance().GetProductsStock();
        }

        public void GetProductsStockDetails()
        {
            LogXServer.getInstance().GetProductsStockDetails();
        }
    }
}
