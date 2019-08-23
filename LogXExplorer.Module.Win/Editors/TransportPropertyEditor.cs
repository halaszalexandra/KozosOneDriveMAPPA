using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;

namespace LogXExplorer.Module.Win
{

    [PropertyEditor(typeof(int), false)]
    public class TransportTypePropEd : EnumIntPropertyEditor<TransportTypeEnum>
    {
        public TransportTypePropEd(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }
    }


    [PropertyEditor(typeof(int), false)]
    public class TransportStatusPropEd : EnumIntPropertyEditor<TransportStatusEnum>
    {
        public TransportStatusPropEd(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }
    }

    [PropertyEditor(typeof(int), false)]
    public class TransportInOutPropEd : EnumIntPropertyEditor<TransportInOutEnum>
    {
        public TransportInOutPropEd(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }
    }
}
