using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            LegendService<HeroStoryBusiness> Service = new LegendService<HeroStoryBusiness>();
            Service.ShowTest();
        }
    }
}
