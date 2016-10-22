using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThirdWorkCommon;
using ThirdWorkInterFace.IBusiness;
using ThirdWorkModel;
using ThirdWorkModel.CommonModel;

namespace ThirdWorkBusiness
{
    public class ReadSoryBusiness<T> : IReadSoryBusiness
        where T : HeroModel
    {
        private static object objectLock = new object();
        private static bool Standby = false;
        private HeroModel LoadHeroModel;

        public ReadSoryBusiness(T _t)
        {
            LoadHeroModel = _t;
        }

        /// <summary>
        /// 准备好了一份故事Task
        /// </summary>
        /// <returns></returns>
        public List<Task> LoadStoryTask()
        {
            List<Task> taskList = new List<Task>();
            foreach (string item in LoadHeroModel.HeroPosition)
            {
                taskList.Add(SingleRead(item));
            }
            return taskList;
        }

        /// <summary>
        /// 创建新线程播报故事
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private Task SingleRead(string message)
        {
            TaskFactory taskFactory = new TaskFactory();
            string time = DateTime.Now.ToString();
            Thread.Sleep(new Random().Next(1000, 2000));
            Task result;
            lock (objectLock)
            {
                result = taskFactory.StartNew(() =>
               {
                   Console.ForegroundColor = RadomColor();
                   Thread.Sleep(new Random().Next(1000, 2000));
                   var thisPositionStory = LoadXmlStory().MyFullStory.FirstOrDefault(p => p.HeroPosition == message) ?? null;
                   if (thisPositionStory != null)
                   {
                       foreach (var item in thisPositionStory.LevelUpStory)
                       {
                           Thread.Sleep(new Random().Next(1000, 2000));
                           MyLog.OutputAndSaveTxt(LoadHeroModel.MyHero + "：" + item);
                       }
                   }
                   MyLog.OutputAndSaveTxt($"{LoadHeroModel.MyHero}:完成了剧情《{message}》,时间在{time}");
                   Thread.Sleep(new Random().Next(1000, 2000));
               });
                Task.WaitAny(new Task[] { result });

                if (!Standby)
                {
                    MyLog.OutputAndSaveTxt($"因为{LoadHeroModel.MyHero}的到来:天龙八部就此拉开序幕");
                    Thread.Sleep(new Random().Next(1000, 2000));
                    Standby = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 通过XML读取故事子内容
        /// </summary>
        /// <returns></returns>
        private static FullStoryModel LoadXmlStory()
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            var settingXml = AppRead.GetValue("StoryXml", typeof(string)).ToString();
            return MyXmlHelper.DeserializeXMLFileToObject<FullStoryModel>(settingXml);
        }

        private ConsoleColor RadomColor()
        {
            switch (LoadHeroModel.MyHero)
            {
                case "乔峰":
                    return ConsoleColor.DarkYellow;

                case "段誉":
                    return ConsoleColor.Green;

                case "虚竹":
                    return ConsoleColor.Red;

                default:
                    return ConsoleColor.White;
            }
        }

        public void Dispose()
        {
        }
    }
}