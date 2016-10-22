using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThirdWorkBusiness;
using ThirdWorkModel;
using ThirdWorkService;

namespace ThirdHomeWork
{
    class Program
    {
        static void Main(string[] args)
        {
            
            try
            {
                LegendService Service = new LegendService();
                Service.ShowTest();
                Thread.Sleep(100000);
                Console.ReadKey();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
