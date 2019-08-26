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

        public List<int> GetLcHeights()
        {
            List<int> ret = new List<int>();
            foreach (QtyExchange qty in this.QtyExchanges)
            {
                if (qty.In == true)
                {
                    ret.Add(qty.LcType.Height);
                }
            }
            return ret;
        }
    }
}
