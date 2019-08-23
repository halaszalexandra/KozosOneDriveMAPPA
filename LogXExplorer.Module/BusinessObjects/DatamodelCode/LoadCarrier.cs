using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace LogXExplorer.Module.BusinessObjects.Database
{

    public partial class LoadCarrier
    {
        public LoadCarrier(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }


        public double TermekTaroltMennyiseg(int productId)
        {
            double taroltMennyiseg = 0;
            IList<Stock> stockList = Stocks;

            foreach (Stock stock in stockList)
            {
                if (stock.Product.Oid == productId)
                {
                    taroltMennyiseg += stock.NormalQty;
                }
            }
            return taroltMennyiseg;
        }


        public byte LeglassabbGyorsulasVissz(byte defAcc)
        {
            byte minValue = 0;
            int ciklus = 0;

            foreach (Stock st in Stocks)
            {
                ciklus++;

                if (st.Product.AccelerateType != null)
                {
                    if (ciklus == 1)
                    {
                        minValue = st.Product.AccelerateType.Accelerate;
                    }
                    else
                    {
                        if (minValue > st.Product.AccelerateType.Accelerate)
                        {
                            minValue = st.Product.AccelerateType.Accelerate;
                        }
                    }
                }
            }

            if (minValue == 0)
            {
                minValue = defAcc;
            }

            return minValue;
        }


        //TODO: Kiválasztani a legjobb besorolást
        //public char LegjobbAbcTipus()
        //{
        //    char minValue;
        //    int ciklus = 0;

        //    foreach (Stock st in Stocks)
        //    {
        //        ciklus++;

        //        if (ciklus == 1)
        //        {
        //            minValue = st.Product.AbcClass.Code;
        //        }
        //        else
        //        {
        //            //if (minValue > st.Product.AbcClass.Code)
        //            //{
        //            //    minValue = st.Product.AbcClass.Code;

        //            //}
        //        }
        //    }
        //    return minValue;
        //}


    

}

}
