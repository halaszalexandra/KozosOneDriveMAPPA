using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LogXExplorer.Module.BusinessObjects.Database
{

    public partial class CommonTrDetail
    {
        public CommonTrDetail(Session session) : base(session) {

        }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public double GetRecentQuantity()
        {
            double recent = Quantity - PerformedQty;
            return recent;
        }


        protected override void OnSaving()
        {
            ArrangeRowsNum();
            base.OnSaving();
            //ArrangeRowsNum();
        }

        protected override void OnDeleted()
        {
            ArrangeRowsNum();
            base.OnDeleted();
        }


        private void ArrangeRowsNum()
        {
            int newSorszam = 0;

            IList<CommonTrDetail> ctrdList = CommonTrHeader.CommonTrDetails;

            foreach (CommonTrDetail item in ctrdList)
            {
                if (!item.IsDeleted)
                {
                    newSorszam++;
                    if (item.ItemNum == 0)
                    {
                        item.ItemNum = newSorszam;
                        item.Save();
                    }
                }
            }
        }
    }
}
