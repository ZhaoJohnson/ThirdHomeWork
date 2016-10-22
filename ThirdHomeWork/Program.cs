using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThirdWorkBusiness;
using ThirdWorkCommon;
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
                Console.WriteLine("请按任意键退出");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                MyLog.SaveEx(ex.Message);
            }
        }
    }
}
