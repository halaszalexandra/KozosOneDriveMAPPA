using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;

namespace LogXExplorer.Module.Win
{

    [PropertyEditor(typeof(int), false)]
    public class CtrhStatusPropEd : EnumIntPropertyEditor<CtrhStatusEnum>
    {
        public CtrhStatusPropEd(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }
    }


    [PropertyEditor(typeof(int), false)]
    public class CtrhPriorityPropEd : EnumIntPropertyEditor<CtrhPriorityEnum>
    {
        public CtrhPriorityPropEd(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }
    }
}
