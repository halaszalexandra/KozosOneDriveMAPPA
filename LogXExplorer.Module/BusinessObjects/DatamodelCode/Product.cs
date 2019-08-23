using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LogXExplorer.Module.BusinessObjects.Database
{

    public partial class Product
    {
        public Product(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public double  GetStockByAisle(Aisle aisle)
        {
            double ret = 0;
            foreach (Stock stock in this.Stocks)
            {
                if(stock.StorageLocation.Aisle == aisle)
                {
                    ret += stock.NormalQty;
                }
            }
            return ret;
        }
       
    }
}
