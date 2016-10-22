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
using static System.Net.Mime.MediaTypeNames;

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
            var t = Task.Factory.StartNew(() =>
             {
                 while (true)
                 {
                     Thread.Sleep(500);
                     int stop = new Random().Next(0, 10000);
                     Console.WriteLine(stop.ToString());
                     //int.Parse(DateTime.Now.Year.ToString())
                     if (stop == stop)
                     {
                         Thread.Sleep(1000);
                         token.CancelAfter(1000);
                         throw new Exception("天降雷霆灭世，天龙八部的故事就此结束...");
                     }
                 }
             });
            Task.WaitAny(taskList.ToArray());
            MyLog.OutputAndSaveTxt("有人已经准备好了");
            Thread.Sleep(5000);
            taskList.Remove(t);
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
        //执行一个可以取消的Task
        private static Task NewCancellableTask(CancellationToken token)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    token.ThrowIfCancellationRequested();
                }
            });
        }
        public void Dispose()
        {
        }
    }
}
