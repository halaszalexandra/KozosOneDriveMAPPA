using LogXExplorer.Module.BusinessObjects.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LogXExplorer.Module.comm
{
    public class LogXPublicServiceClientProxy : ClientBase<ILogXPublicServices>, ILogXPublicServices
    {
        public string CreateNewComission(string classCode)
        {
            throw new NotImplementedException();
        }


        //public Product CreateNewProduct(String Identifier, String Name)
        //{
        //    return base.Channel.CreateNewProduct(Identifier, Name);
        //}

        public string CreateNewStorage(string classCode)
        {
            throw new NotImplementedException();
        }

        public string GetAbcClassName(String abcCode)
        {
            return base.Channel.GetAbcClassName(abcCode);
        }

        public void GetProductsStock()
        {
            throw new NotImplementedException();
        }

        public void GetProductsStockDetails()
        {
            throw new NotImplementedException();
        }
    }
}
