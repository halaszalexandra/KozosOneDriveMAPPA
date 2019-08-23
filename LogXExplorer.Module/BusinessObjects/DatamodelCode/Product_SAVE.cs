//using System;
//using DevExpress.Xpo;
//using DevExpress.Xpo.Metadata;
//using DevExpress.Data.Filtering;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Reflection;
//using DevExpress.Persistent.BaseImpl;

//namespace LogXExplorer.Module.BusinessObjects.Database
//{

//    public partial class ProductSAVE
//    {
//        public ProductSAVE(Session session) : base(session) { }
//        public override void AfterConstruction() { base.AfterConstruction(); }

//        //[System.Xml.Serialization.XmlIgnore]
//        private XPCollection<AuditDataItemPersistent> auditTrail;
//        public XPCollection<AuditDataItemPersistent> AuditTrail
//        {
//            get
//            {
//                if (auditTrail == null)
//                {
//                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
//                }
//                return auditTrail;
//            }
//        }

//    }

//}
