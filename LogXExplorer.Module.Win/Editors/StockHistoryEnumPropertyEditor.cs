using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;

namespace LogXExplorer.Module.Win
{

    [PropertyEditor(typeof(int), false)]
    public class StockHistoryDirectionEnumPropertyEditor : EnumIntPropertyEditor<StockHistoryDirectionEnum>
    {
        public StockHistoryDirectionEnumPropertyEditor(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }
    }
    public class StockHistoryTypeEnumPropertyEditor : EnumIntPropertyEditor<StockHistoryTypeEnum>
    {
        public StockHistoryTypeEnumPropertyEditor(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }
    }
}
