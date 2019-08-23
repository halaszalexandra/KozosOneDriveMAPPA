using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogXExplorer.Module.BusinessObjects.DatamodelCode
{
    public class MyXPOBaseObject : XPObject
    {

        //[NonSerialized]
        [XmlIgnore]
        private XPClassInfo classInfo;

        //[NonSerialized]
        [XmlIgnore]
        private Session session;

        public MyXPOBaseObject(Session _session) {
            this.session = _session;
            this.classInfo = session.GetClassInfo(this);
        }

    }



}


