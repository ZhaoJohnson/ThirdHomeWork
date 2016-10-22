using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ThirdWorkBusiness;
using ThirdWorkCommon;
using ThirdWorkInterFace.IService;
using ThirdWorkModel;
using ThirdWorkModel.CommonModel;

namespace ThirdWorkService
{
    public class LegendService : ILegendService
    {
        private static bool Standby = true;
        private CancellationTokenSource token = new CancellationTokenSource();

        //token.Cancel();标记
        public void Show()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            AppSettingsReader AppRead = new AppSettingsReader();
            var settingXml = AppRead.GetValue("ProjectSettingXml", typeof(string)).ToString();
            //导入故事剧情
            foreach (HeroModel item in LoadHero(LoadSetting(settingXml)))
            {
                HeroStoryBusiness business = new HeroStoryBusiness(item);
                taskList.Add(taskFactory.StartNew(business.ShowStory()));
            }

            #region 监控雷劈线程

            var taskForStop = Task.Factory.StartNew(() =>
            {
                while (Standby)
                {
                    Thread.Sleep(1000);
                    int stop = new Random().Next(0, 10000);
                    Console.WriteLine();
                    try
                    {
                        if (stop == int.Parse(DateTime.Now.Year.ToString()))
                        {
                            Thread.Sleep(1000);
                            throw new Exception("天降雷霆灭世，天龙八部的故事就此结束...");
                        }
                    }
                    catch (Exception ex)
                    {
                        MyLog.SaveEx(ex.Message);
                        Thread.Sleep(1000);
                        Console.WriteLine("程序即将关闭");
                        Thread.Sleep(3000);
                        Environment.Exit(0);
                    }
                }
            });

            #endregion 监控雷劈线程

            //移除子线程，避免影响主线路
            taskList.Remove(taskForStop);

            Task.WaitAny(taskList.ToArray());
            MyLog.OutputAndSaveTxt("有人已经准备好了");
            Thread.Sleep(5000);
            Task.WaitAll(taskList.ToArray());
            MyLog.OutputAndSaveTxt($"中原群雄大战辽兵，忠义两难一死谢天,{DateTime.Now.ToString()}");
            //通过修正指正来停止监控线程
            Standby = false;
            watch.Stop();
            MyLog.OutputAndSaveTxt($"天龙八部全篇用时：{watch.ElapsedMilliseconds}ms");
        }

        /// <summary>
        /// 读取配置文件，获取参入人数
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private SettingModel LoadSetting(string fileName)
        {
            return MyXmlHelper.DeserializeXMLFileToObject<SettingModel>(fileName) ?? null;
        }

        /// <summary>
        /// 通过配置文件创建对象
        /// </summary>
        /// <param name="_SettingModel">设置</param>
        /// <returns></returns>
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