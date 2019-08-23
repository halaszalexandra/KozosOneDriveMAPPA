using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LogXExplorer.Module.BusinessObjects.Database
{

    public partial class TransportOrder
    {
        Session mySession;
        public TransportOrder(Session session) : base(session)
        {
            mySession = session;
        }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void OnSaving()
        {
            base.OnSaving();
        }
        //private object JoinTogetherLhu(int lHU2X, int lHU2Y)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
