using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdWorkBusiness;
using ThirdWorkCommon;
using ThirdWorkInterFace.IService;
using ThirdWorkModel;
using ThirdWorkModel.CommonModel;
using System.Configuration;
using System.Threading;

namespace ThirdWorkService
{
    public class LegendService : ILegendService
    {
        private bool Standby = false;
        CancellationTokenSource token = new CancellationTokenSource();
        //token.Cancel();标记
        public void ShowTest()
        {
            //StoryModel storym = new StoryModel()
            //{
            //    HeroPosition = "小和尚",
            //    LevelUpStory = new string[]
            //      {
            //          "刚刚来到了如来寺",
            //          "巧遇扫地神僧",
            //          "我就没看了..."

            //      }
            //};
            //FullStoryModel fullstory = new FullStoryModel();
            //fullstory.MyFullStory = new StoryModel[]
            //{
            //    storym,
            //    new StoryModel
            //    {
            //    HeroPosition = "逍遥掌门",
            //    LevelUpStory = new string[]
            //      {
            //          "刚刚来到了如来寺",
            //          "巧遇扫地神僧",
            //          "我就没看了..."

            //      }
            //    }
            //};
            //MyXmlHelper.Serializer(fullstory, "DemonStory.xml");
            

            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            AppSettingsReader AppRead = new AppSettingsReader();
            var settingXml = AppRead.GetValue("ProjectSettingXml", typeof(string)).ToString();
            foreach (HeroModel item in LoadHero(LoadSetting(settingXml)))
            {
                HeroStoryBusiness business = new HeroStoryBusiness(item);
                //taskList.Add(new Task(() => business.ShowTest()));
                taskList.Add(taskFactory.StartNew(business.ShowStory()));
            }
            Task.WaitAny(taskList.ToArray());
            Console.WriteLine("有人已经准备好了");
            Thread.Sleep(10000);
            Task.WaitAll(taskList.ToArray());
            Console.WriteLine($"中原群雄大战辽兵，忠义两难一死谢天,{DateTime.Now.ToString()}");


        }
        private SettingModel LoadSetting(string fileName)
        {
            return MyXmlHelper.DeserializeXMLFileToObject<SettingModel>(fileName) ?? null;
        }
        private List<HeroModel> LoadHero(SettingModel _SettingModel)
        {
            List<HeroModel> result = new List<HeroModel>();
            foreach (var item in _SettingModel.HeroName)
            {
                HeroModel model = new HeroModel();
                model = MyJsonHelper.Json2Object<HeroModel>(item + "Model");
                result.Add(model);
            }
            return result;
        }
        public void Dispose()
        {
        }
    }
}
