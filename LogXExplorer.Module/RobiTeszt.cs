using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogXExplorer.Module
{
    public class RobiTeszt
    {

        private static RobiTeszt instance = null;

        public RobiTeszt()
        {

            Console.WriteLine("RobiTeszt()");
        }

        public void  RobiTesztHello()
        {
            Console.WriteLine("RobiTesztHello()" + this.GetHashCode());
        }

        public static void Init()
        {
            instance = new RobiTeszt();

        }
        public static RobiTeszt GetInstance()
        {
            return instance;
        }

    }
}
