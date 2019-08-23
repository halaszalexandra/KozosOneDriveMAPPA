using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace LogXExplorer.Module.BusinessObjects.Database
{

    public partial class CommonTrHeader
    {
        Session mySession;
        public CommonTrHeader(Session session) : base(session) {
            mySession = session;
        }
        public override void AfterConstruction() { base.AfterConstruction();
            RecordingDate = DateTime.Now;
            this.Status = 0;

            //TODO
            
        }
        protected override void OnSaving()
        {
            if (Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
            {
                // Client  
                if (Identity == "" || Identity == null)
                {
                    this.Identity = GetNewIdentity();
                }

            }
            else
            {
                // Server 

            }
            base.OnSaving();
        }

        private string GetNewIdentity()
        {
            string Identity = "";

            CriteriaOperator cop = new BinaryOperator("Oid", CommonType.Oid);
            CommonTrType ct = (CommonTrType)mySession.FindObject(typeof(CommonTrType), cop);

            if (ct != null)
            {

                Sorszam talaltSorszam = null;
                
                // Ha a tranzakció típusa évfüggő
                if (ct.DateDepended)
                {
                    UInt16 year = Convert.ToUInt16(RecordingDate.Year);

                    CriteriaOperator copSr = new GroupOperator(GroupOperatorType.And, new BinaryOperator("Type", ct), new BinaryOperator("Year", year));
                    talaltSorszam = (Sorszam)mySession.FindObject(typeof(Sorszam), copSr);
                }
                // Ha a tranzakció típusa NEM évfüggő
                else
                {
                    CriteriaOperator copSr = new GroupOperator(GroupOperatorType.And, new BinaryOperator("Type", ct), new BinaryOperator("Year", 0));
                    talaltSorszam = (Sorszam)mySession.FindObject(typeof(Sorszam), copSr);
                }


                if (talaltSorszam == null)
                {
                    using (NestedUnitOfWork uow = mySession.BeginNestedUnitOfWork())
                    {

                        CriteriaOperator copType = new BinaryOperator("Type", CommonType);
                        CommonTrType ctType = uow.FindObject<CommonTrType>(copType);

                        Sorszam newsr = new Sorszam(uow);
                        newsr.Type = ctType;

                        if (ctType.DateDepended)
                        {
                            newsr.Year = Convert.ToUInt16(RecordingDate.Year);
                        }
                        else
                        {
                            newsr.Year = 0;
                        }
                        uow.CommitChanges();

                        talaltSorszam = newsr;
                    }
                }


                talaltSorszam.LastNum++;
                Int32 newNumber = talaltSorszam.LastNum;
                StringBuilder sb = new StringBuilder();
                sb.Append(ct.Prefix);
                sb.Append("/");
                sb.AppendFormat(String.Format("{0:D6}", newNumber));

                Identity = sb.ToString();
            }

            return Identity;
        }

        //protected override XPCollection CreateCollection(XPMemberInfo property)
        //{
        //    XPCollection result = base.CreateCollection(property);
        //    if (property.Name == "CommonTrDetails")
        //    {
        //        Console.WriteLine("hahó");
        //    }
        //    return result;
        //}

        
        public double GetPerformedSumQuantity()
        {
            double sum = 0;

            foreach (CommonTrDetail detail in CommonTrDetails)
            {

                sum += detail.PerformedQty;
            }
            return sum;
        }

        public double GetRequiredSumQuantity()
        {
            double sum = 0;

            foreach (CommonTrDetail detail in CommonTrDetails)
            {
                sum += detail.Quantity;
            }
            return sum;
        }

    }

}
