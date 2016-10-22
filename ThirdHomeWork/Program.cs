using System;
using ThirdWorkCommon;
using ThirdWorkService;

namespace ThirdHomeWork
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                LegendService Service = new LegendService();
                Service.Show();
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