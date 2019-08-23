using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;

namespace LogXExplorer.Module.Win
{

    [PropertyEditor(typeof(int), false)]
    public class StoLocEnumPropertyEditor : EnumIntPropertyEditor<StoLocStatusEnum>
    {
        public StoLocEnumPropertyEditor(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }
    }
}
