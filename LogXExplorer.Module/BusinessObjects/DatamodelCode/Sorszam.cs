using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LogXExplorer.Module.BusinessObjects.Database
{

    public partial class Sorszam
    {
        public Sorszam(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        public Int32 GetNewNumber()
        {
            Int32 newNumber = 0;

            this.LastNum++;
            newNumber = LastNum;
            return newNumber;
        }
    }
}
