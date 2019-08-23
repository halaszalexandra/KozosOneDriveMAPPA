using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using LogXExplorer.Module.BusinessObjects.Database;

namespace LogXExplorer.Module.comm
{
    [ServiceContract]
    public interface ILogXPublicServices
    {
        //minden amit kliens-server kommunikációval akarsz híni, itt kell először deklarálni.
        //ide kell tenni valamennyi konkurrens hivast.

        //Teszt       
        [OperationContract]
        String GetAbcClassName(String classCode);


        /*************************** 
         * Insert
        *****************************/
        ////Új termék létrehozása
        //[OperationContract]
        //Product CreateNewProduct(String Identifier, String Name);

        //Új betárolási bizonylat létrehozása
        [OperationContract]
        String CreateNewStorage(String classCode);

        //Új kitárolási bizonylat létrehozása
        [OperationContract]
        String CreateNewComission(String classCode);



        /*************************** 
         * Lekérdezések
        *****************************/
        //Termékek készlete vagy az összes termék készlet visszaadása nem részletes
        [OperationContract]
        void GetProductsStock();

        //Termékek készlete vagy az összes termék készlet visszaadása részletes
        [OperationContract]
        void GetProductsStockDetails();
    }
}
