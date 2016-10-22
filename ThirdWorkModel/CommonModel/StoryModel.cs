using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdWorkModel.CommonModel
{
    public class StoryModel
    {
        public string HeroPosition { get; set; }
        public string[] LevelUpStory { get; set; }
    }
    public class FullStoryModel
    {
        public StoryModel[] MyFullStory { get; set; }
    }
}
