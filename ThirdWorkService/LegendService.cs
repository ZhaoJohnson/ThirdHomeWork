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
using System.Diagnostics;

namespace ThirdWorkService
{
    public class LegendService : ILegendService
    {
        private bool Standby = false;
        CancellationTokenSource token = new CancellationTokenSource();
        //token.Cancel();标记
        public void ShowTest()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            AppSettingsReader AppRead = new AppSettingsReader();
            var settingXml = AppRead.GetValue("ProjectSettingXml", typeof(string)).ToString();
            foreach (HeroModel item in LoadHero(LoadSetting(settingXml)))
            {
                HeroStoryBusiness business = new HeroStoryBusiness(item);
                taskList.Add(taskFactory.StartNew(business.ShowStory()));
            }
            Task.WaitAny(taskList.ToArray());
            MyLog.OutputAndSaveTxt("有人已经准备好了");
            Thread.Sleep(10000);
            Task.WaitAll(taskList.ToArray());
            MyLog.OutputAndSaveTxt($"中原群雄大战辽兵，忠义两难一死谢天,{DateTime.Now.ToString()}");
            watch.Stop();
            MyLog.OutputAndSaveTxt($"{watch.ElapsedMilliseconds}");


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
