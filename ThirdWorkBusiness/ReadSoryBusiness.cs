using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThirdWorkCommon;
using ThirdWorkModel;
using ThirdWorkModel.CommonModel;

namespace ThirdWorkBusiness
{
    public class ReadSoryBusiness<T>
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
                //taskList.Add(new Task(() =>
                //{
                //    Console.WriteLine($"{LoadHeroModel.MyHero}正在练级中.....");
                //    Thread.Sleep(new Random().Next(1000, 2000));
                //}));
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
            Task result = taskFactory.StartNew(() =>
            {
                Thread.Sleep(new Random().Next(1000, 2000));
                var thisPositionStory = LoadXmlStory().MyFullStory.FirstOrDefault(p => p.HeroPosition == message) ?? null;
                if (thisPositionStory != null)
                {
                    foreach (var item in thisPositionStory.LevelUpStory)
                    {
                        Thread.Sleep(new Random().Next(1000, 2000));
                        Console.WriteLine(LoadHeroModel.MyHero + "：" + item);
                    }
                }
                Console.WriteLine($"{LoadHeroModel.MyHero}:现在成为了{message},时间在{time}");
                Thread.Sleep(new Random().Next(1000, 2000));
            });
            Task.WaitAny(new Task[] { result });
            lock (objectLock)
            {
                if (!Standby)
                {
                    Console.WriteLine("ReadSoryBusiness天龙八部就此拉开序幕");
                    Thread.Sleep(new Random().Next(1000, 2000));
                    Standby = true;
                }
            }
            return result;
        }
        private static FullStoryModel LoadXmlStory()
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            var settingXml = AppRead.GetValue("StoryXml", typeof(string)).ToString();
            return MyXmlHelper.DeserializeXMLFileToObject<FullStoryModel>(settingXml);
        }
    }
}
