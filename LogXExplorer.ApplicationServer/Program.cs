using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Configuration;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp;
using System.Collections;
using System.ServiceModel;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.MiddleTier;
using LogXExplorer.Module.Controllers;
using LogXExplorer.Module;

namespace LogXExplorer.ApplicationServer {
    class Program {
        static Program() {
        }
     
        static void Main(string[] args) {

            //start
            LogXServer.staticInit();
            //várakozás
            Console.WriteLine("Press <ENTER> to terminate service.");
            Console.ReadLine();
            //stop
            LogXServer.staticDestroy();

        }

    }
}
