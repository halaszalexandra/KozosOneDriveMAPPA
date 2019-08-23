using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogXExplorer.Module.BusinessObjects.DatamodelCode
{

    [Serializable, NonPersistent]
    public class MyBasePersistentObject : IXPSimpleObject
    {
        public MyBasePersistentObject(Session session)
        {
            this.session = session;
            classInfo = session.GetClassInfo(this);
        }

        [NonSerialized]
        private XPClassInfo classInfo;
        [NonSerialized]
        private Session session;

        [Key(true)]
        public Guid OID;

        #region IXPClassInfoProvider Members  
        XPClassInfo IXPClassInfoProvider.ClassInfo
        {
            get { return classInfo; }
        }
        #endregion

        #region IXPDictionaryProvider Members  
        XPDictionary DevExpress.Xpo.Metadata.Helpers.IXPDictionaryProvider.Dictionary
        {
            get { return session.Dictionary; }
        }
        #endregion

        #region ISessionProvider Members  
        Session ISessionProvider.Session
        {
            get { return session; }
        }
        #endregion

        #region IDataLayerProvider Members  
        IDataLayer IDataLayerProvider.DataLayer
        {
            get { return session.DataLayer; }
        }

        IObjectLayer IObjectLayerProvider.ObjectLayer
        {
            get { return session.ObjectLayer; }
        }
        #endregion

    }
}