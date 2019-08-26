using LogXExplorer.Module.BusinessObjects.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogXExplorer.ApplicationServer.opc
{
    public class PLCTransaction
    {
        //private static UInt64 JoinTogetherTransaction(int transId, int detailId, int sumDetails)
        //{
        //    UInt64 transaction = 0;

        //    transId = Convert.ToInt32(transId);
        //    detailId = Convert.ToUInt16(detailId);
        //    sumDetails = Convert.ToInt16(sumDetails);

        //    string TransIdB = Convert.ToString(transId, 2);
        //    string DetailIdB = Convert.ToString(detailId, 2);
        //    string SumDetailsB = Convert.ToString(sumDetails, 2);

        //    string output = TransIdB.PadLeft(32, '0') + DetailIdB.PadLeft(16, '0') + SumDetailsB.PadLeft(16, '0');

        //    transaction = Convert.ToUInt64(calculateBinaryString(output));
        //    return transaction;
        //}

        //private static UInt64 JoinTogetherTypeAccPrTime(byte notUsed, byte type, byte accelerate, byte priority, UInt32 time)
        //{
        //    UInt64 typeAccPrTime = 0;

        //    notUsed = Convert.ToByte(notUsed);
        //    type = Convert.ToByte(type);
        //    accelerate = Convert.ToByte(accelerate);
        //    priority = Convert.ToByte(priority);
        //    time = Convert.ToUInt32(time);

        //    string notUsedB = Convert.ToString(notUsed, 2);
        //    string typeB = Convert.ToString(type, 2);
        //    string accelerateB = Convert.ToString(accelerate, 2);
        //    string priorityB = Convert.ToString(priority, 2);
        //    string timeB = Convert.ToString(time, 2);

        //    string output = notUsedB.PadLeft(8, '0') + typeB.PadLeft(8, '0') + accelerateB.PadLeft(8, '0') + priorityB.PadLeft(8, '0') + timeB.PadLeft(32, '0');

        //    typeAccPrTime = Convert.ToUInt64(calculateBinaryString(output));
        //    return typeAccPrTime;
        //}

        //private static UInt32 JoinTogetherSource(int line, int row, int column)
        //{
        //    UInt32 source = 0;

        //    line = Convert.ToInt32(line);
        //    row = Convert.ToInt16(row);
        //    column = Convert.ToInt16(column);

        //    string lineB = Convert.ToString(line, 2);
        //    string rowB = Convert.ToString(row, 2);
        //    string columnB = Convert.ToString(column, 2);

        //    string output = lineB.PadLeft(8, '0') + rowB.PadLeft(8, '0') + columnB.PadLeft(16, '0');

        //    source = Convert.ToUInt32(calculateBinaryString(output));
        //    return source;
        //}

        //private static UInt64 JoinTogetherTarget(int notUsed1, int line, int row, int column, int notUsed2, int modul)
        //{
        //    UInt64 target = 0;

        //    notUsed1 = Convert.ToInt16(notUsed1);
        //    line = Convert.ToByte(line);
        //    row = Convert.ToByte(row);
        //    column = Convert.ToInt16(column);
        //    notUsed2 = Convert.ToByte(notUsed2);
        //    modul = Convert.ToByte(column);

        //    string notUsed1B = Convert.ToString(notUsed1, 2);
        //    string lineB = Convert.ToString(line, 2);
        //    string rowB = Convert.ToString(row, 2);
        //    string columnB = Convert.ToString(column, 2);
        //    string notUsed2B = Convert.ToString(notUsed2, 2);
        //    string modulB = Convert.ToString(modul, 2);

        //    string output = notUsed1B.PadLeft(16, '0') + lineB.PadLeft(8, '0') + rowB.PadLeft(8, '0') + columnB.PadLeft(16, '0') + notUsed2B.PadLeft(8, '0') + modulB.PadLeft(8, '0');

        //    target = Convert.ToUInt64(calculateBinaryString(output));
        //    return target;
        //}

        //private static Int64 JoinTogetherLhu(Int32 lhux, Int32 lhuy)
        //{
        //    Int64 lhu = 0;
        //    lhux = Convert.ToInt32(lhux);
        //    lhuy = Convert.ToInt32(lhuy);

        //    string lhuxB = Convert.ToString(lhux, 2);
        //    string lhuyB = Convert.ToString(lhuy, 2);

        //    string output = lhuxB.PadLeft(32, '0') + lhuyB.PadLeft(32, '0');

        //    lhu = Convert.ToInt64(calculateBinaryString(output));
        //    return lhu;
        //}

        //private static double calculateBinaryString(string inputBinary)
        //{
        //    double returnValue = 0;
        //    for (int i = inputBinary.Length - 1; i > 0; i--)
        //    {
        //        if (inputBinary.Trim()[i] == '1')
        //        {
        //            returnValue += Math.Pow(2, Convert.ToDouble(inputBinary.Length - i - 1));
        //        }
        //    }
        //    return returnValue;
        //}


        public static List<string[]> getTransactionData(TransportOrder transportOrder) {
            List<string[]> ret = new List<string[]>();


            //Int16 tpoStatusInt = Convert.ToString((Int16)transportOrder.TpId);
            //tpoStatusInt = (tpoStatusInt << 32) & tpo.msgStatus;

            //string msgID = transportOrder.TpId.ToString();
            //string msgID = Convert.ToString((Int16)transportOrder.TpId);
            //string msgCtrhidCurrdSumd = Convert.ToString(JoinTogetherTransaction(transportOrder.CommonTrHeader.Oid, transportOrder.CommonDetail.Oid, transportOrder.CommonTrHeader.CommonTrDetails.Count));
            //string msgTypeAccPrTime = Convert.ToString(JoinTogetherTypeAccPrTime(0, Convert.ToByte(transportOrder.Type), transportOrder.LC.LeglassabbGyorsulasVissz(6), Convert.ToByte(transportOrder.CommonTrHeader.Priority), 0));
            //string msgLoadCarrier = Convert.ToString(transportOrder.LC.BarCode);
            //string msgRFID1 = Convert.ToString(transportOrder.LC.RFID1);
            //string msgRFID2 = Convert.ToString(transportOrder.LC.RFID2);
            //string msgSource;
            //string msgSourceLHU1;
            //string msgSourceLHU2;
            //string msgTarget;
            //string msgTargetLHU1 ;
            //string msgTargetLHU2 ;
            string msgWeight= Convert.ToString(transportOrder.Weight);

            UInt64 msgCtrhidCurrdSumd = 0;
            UInt64 msgTypeAccPrTime;
            UInt32 msgSource;
            Int64 msgSourceLHU1;
            Int64 msgSourceLHU2;
            UInt32 msgTarget;
            Int64 msgTargetLHU1;
            Int64 msgTargetLHU2;


            msgCtrhidCurrdSumd = (UInt32)transportOrder.CommonTrHeader.Oid;
            msgCtrhidCurrdSumd = (msgCtrhidCurrdSumd << 32) & (UInt16)transportOrder.CommonDetail.Oid;
            msgCtrhidCurrdSumd = (msgCtrhidCurrdSumd << 16) & (UInt16)transportOrder.CommonTrHeader.CommonTrDetails.Count;

            msgTypeAccPrTime = 0;
            msgTypeAccPrTime = (msgTypeAccPrTime << 8) & (byte)transportOrder.Type;
            msgTypeAccPrTime = (msgTypeAccPrTime << 8) & (byte)transportOrder.LC.LeglassabbGyorsulasVissz(6);
            msgTypeAccPrTime = (msgTypeAccPrTime << 8) & (byte)transportOrder.CommonTrHeader.Priority;
            msgTypeAccPrTime = (msgTypeAccPrTime << 32) & (UInt32)0;


            if (transportOrder.SourceLocation != null)
            {
                msgSource = transportOrder.SourceLocation.Block;
                msgSource = (msgSource << 8) & (byte)transportOrder.SourceLocation.Row;
                msgSource = (msgSource << 16) & (byte)transportOrder.SourceLocation.Column;

                msgSourceLHU1 = transportOrder.SourceLocation.LHU1X;
                msgSourceLHU1 = (msgSourceLHU1 << 32) & (Int32)transportOrder.SourceLocation.LHU1Y;

                msgSourceLHU2 = transportOrder.SourceLocation.LHU2X;
                msgSourceLHU2 = (msgSourceLHU2 << 32) & (Int32)transportOrder.SourceLocation.LHU2Y;
            }
            else
            {
                msgSource = 0;
                msgSourceLHU1 = 0;
                msgSourceLHU2 = 0;
            }

            if (transportOrder.TargetLocation != null)
            {
                msgTarget = transportOrder.TargetLocation.Block;
                msgTarget = (msgTarget << 8) & (byte)transportOrder.TargetLocation.Row;
                msgTarget = (msgTarget << 16) & (byte)transportOrder.TargetLocation.Column;

                msgTargetLHU1 = transportOrder.TargetLocation.LHU1X;
                msgTargetLHU1 = (msgTargetLHU1 << 32) & (Int32)transportOrder.TargetLocation.LHU1Y;

                msgTargetLHU2 = transportOrder.TargetLocation.LHU2X;
                msgTargetLHU2 = (msgTargetLHU2 << 32) & (Int32)transportOrder.TargetLocation.LHU2Y;
            }
            else
            {
                msgTarget = 0;
                msgTargetLHU1 = 0;
                msgTargetLHU2 = 0;
            }



            string[] t0 = new string[2] { Convert.ToString((UInt16)transportOrder.TpId), "UInt16" };
            string[] t1 = new string[2] { Convert.ToString((UInt64)msgCtrhidCurrdSumd), "UInt64" };
            string[] t2 = new string[2] { Convert.ToString((UInt64)msgTypeAccPrTime), "UInt64" };
            string[] t3 = new string[2] { Convert.ToString((UInt32)transportOrder.LC.BarCode), "UInt32" };
            string[] t4 = new string[2] { transportOrder.LC.RFID1, "UInt64" };
            string[] t5 = new string[2] { transportOrder.LC.RFID2, "UInt64" };
            string[] t6 = new string[2] { Convert.ToString((UInt32)msgSource), "UInt32" };
            string[] t7 = new string[2] { Convert.ToString((Int64)msgSourceLHU1), "Int64" };
            string[] t8 = new string[2] { Convert.ToString((Int64)msgSourceLHU2), "Int64" };
            string[] t9 = new string[2] { Convert.ToString((UInt32)msgTarget), "UInt32" };
            string[] t10 = new string[2] { Convert.ToString((Int64)msgTargetLHU1), "Int64" };
            string[] t11 = new string[2] { Convert.ToString((Int64)msgTargetLHU2), "Int64" };
            string[] t12 = new string[2] { msgWeight, "UInt32" };

            ret.Add(t0);
            ret.Add(t1);
            ret.Add(t2);
            ret.Add(t3);
            ret.Add(t4);
            ret.Add(t5);
            ret.Add(t6);
            ret.Add(t7);
            ret.Add(t8);
            ret.Add(t9);
            ret.Add(t10);
            ret.Add(t11);
            ret.Add(t12);
            return ret;
        }


        public static List<String> getOneInputParam(String onlyVal) {
            List<String> ret = new List<string>();
            ret.Add(onlyVal);
            return ret;
        }
    }
}
