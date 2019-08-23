using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogXExplorer.Module.BusinessObjects
{
    public class BaseDictWrapper
    {

        private static String TYPENAME = "TYPENAME";
        public Dictionary<String, Object> dict { get; set; }
        static Type xpoBaseCollectionType = typeof(DevExpress.Xpo.XPBaseCollection);
        static List<string> enabledBaseProps = new List<string>();
        static BaseDictWrapper()
        {
            //itt kell felsorolni azokat a mezoket amit megis at akarunk hozni
            //az osokbol a wrapperbe !!!!!
            enabledBaseProps.Add("Id");
            enabledBaseProps.Add("Oid");
            enabledBaseProps.Add("Guid");
        }

        public BaseDictWrapper() {
            dict = new Dictionary<string, object>();
        }

        public Type getWrappedType(){
            return Type.GetType((string)dict[TYPENAME]);
        }

        public void setWrappedType(Type wrappedType) {
            dict.Add(TYPENAME, wrappedType.FullName);
        }


        static public BaseDictWrapper wrap(Object xpoObj)
        {
            BaseDictWrapper wrapper = new BaseDictWrapper();
            Type origType = xpoObj.GetType();
            wrapper.setWrappedType(origType);

            Type origBaseType = origType.BaseType;
            PropertyInfo[] origProps = origType.GetProperties();
            PropertyInfo[] baseProps = origBaseType.GetProperties();

            //filter base xpo props
            List<string> basePropNameList = new List<string>();
            foreach (PropertyInfo prop in baseProps)
            {
                //az enabled listan levoket nem tesszik tiltolistara
                if (!enabledBaseProps.Contains(prop.Name))
                {
                    basePropNameList.Add(prop.Name);
                }
            }
            //copy instance properties into wrapper
            foreach (PropertyInfo prop in origProps)
            {
                if (!basePropNameList.Contains(prop.Name))
                {
                    //xpcollection vagy egyéb field
                    if (prop.PropertyType.BaseType.Equals(xpoBaseCollectionType)) {
                        XPBaseCollection xpColl = (XPBaseCollection)prop.GetValue(xpoObj);
                        //wrapper.dict.Add (prop.Name + "_" + TYPENAME, prop.PropertyType.FullName);
                        if (xpColl.Count > 0) {
                            List<BaseDictWrapper> wrapperCollection = new List<BaseDictWrapper>();
                            foreach (Object xpoo in xpColl)
                            {
                                BaseDictWrapper wrappedItem = wrap(xpoo);
                                wrapperCollection.Add(wrappedItem);
                            }
                            wrapper.dict.Add(prop.Name, wrapperCollection);
                        }
                    }
                    else {
                        wrapper.dict.Add(prop.Name, prop.GetValue(xpoObj));
                    }

                }
            }
            return wrapper;
        }



        static public Object unwrap(BaseDictWrapper wrapperObj, Type xpoType, Session session)
        {
            Object xpoInstance = null;
            if (session != null){
                xpoInstance = Activator.CreateInstance(xpoType, new Object[] { session });
            }else{
                xpoInstance = Activator.CreateInstance(xpoType);
            }

            foreach (String keyPropName in wrapperObj.dict.Keys)
            {
                PropertyInfo xpoProp = xpoType.GetProperty(keyPropName);
                if (xpoProp != null)
                {
                     if (xpoProp.PropertyType.BaseType.Equals(xpoBaseCollectionType)){
                        //nincs setter!!! csak getter van az xpocollection-ön.
                        //XPCollection<T> xpCollection = null;
                        //if (session != null) {
                        //xpCollection = new XPCollection<T>(session);
                        //}else{
                        //xpCollection = new XPCollection<T>();
                        //}

                        //kell az XPCollection full type, a generic <ValamiType> miatt, eltér a típusuk.
                        //string collFullTypeName = (String)wrapperObj.dict[keyPropName + "_" + TYPENAME];
                        //Type collFullType = typeof(DevExpress.Xpo.XPCollection);
                        Object typeLessXPOCollection = xpoProp.GetValue(xpoInstance);
                        Type xpoCollType = typeLessXPOCollection.GetType();
                        MethodInfo method = xpoCollType.GetMethod("Add");  //, BindingFlags.Public

                        List<BaseDictWrapper> wrapperCollection = (List<BaseDictWrapper>)wrapperObj.dict[keyPropName];
                        foreach (BaseDictWrapper collItemWrapper in wrapperCollection) {
                            Object xpoTypeCollItem = unwrap(collItemWrapper, collItemWrapper.getWrappedType(), session);
                            method.Invoke(typeLessXPOCollection, new Object[] { xpoTypeCollItem });     //xpCollection.Add(xpoTypeCollItem);
                        }
                        //nincs setter az xpocollection-ön.
                        //xpoProp.SetValue(xpoInstance, xpCollection);  
                    }
                    else {
                        xpoProp.SetValue(xpoInstance, wrapperObj.dict[keyPropName]);
                    }
                }
            }
            return xpoInstance;
        }


    }
}
