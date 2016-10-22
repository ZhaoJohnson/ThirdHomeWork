using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdWorkInterFace.IBusiness;
using ThirdWorkModel;

namespace ThirdWorkBusiness
{
    public class HeroStoryBusiness : IHeroStoryBusiness
    {
        public HeroModel MyHero { get; set; }
        public List<Task> MyHeroStory { get; set; }
        public void Dispose()
        {
        }
    }
}
