using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdWorkInterFace.IBusiness;
using ThirdWorkModel;

namespace ThirdWorkBusiness
{
    public class HeroStory : IHeroStory
    {
        public Hero MyHero { get; set; }
        public List<Task> MyHeroStory { get; set; }
        public void Dispose()
        {
        }
    }
}
