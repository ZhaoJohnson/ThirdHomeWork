//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using ThirdWorkCommon;
//using ThirdWorkInterFace.IBusiness;
//using ThirdWorkModel;
//using ThirdWorkModel.CommonModel;

//namespace ThirdWorkBusiness
//{
//    public class ReadSoryBusiness<T> : IReadSoryBusiness
//        where T : HeroModel
//    {
//        private static readonly object ObjectLock = new object();
//        private static bool _standby = false;
//        private readonly HeroModel _loadHeroModel;

//        public ReadSoryBusiness(T _t)
//        {
//            _loadHeroModel = _t;
//        }

//        /// <summary>
//        /// 准备好了一份故事Task
//        /// </summary>
//        /// <returns></returns>
//        public List<Action> LoadStoryTask()
//        {
//            List<Action> taskList = new List<Action>();
//            foreach (string item in _loadHeroModel.HeroPosition)
//            {
//                taskList.Add(SingleRead(item));
//            }
//            return taskList;
//        }

//        /// <summary>
//        /// 创建新线程播报故事
//        /// </summary>
//        /// <param name="message"></param>
//        /// <returns></returns>
//        private Action SingleRead(string message)
//        {
//            Action result;
//            Thread.Sleep(new Random().Next(1000, 2000));
//            lock (ObjectLock)
//            {
//                string time = DateTime.Now.ToString();
//                Thread.Sleep(new Random().Next(1000, 2000));
//                 result = () =>
//                {
//                    Console.ForegroundColor = RadomColor();
//                    Thread.Sleep(new Random().Next(1000, 2000));
//                    var thisPositionStory = LoadXmlStory().MyFullStory.FirstOrDefault(p => p.HeroPosition == message) ?? null;
//                    if (thisPositionStory != null)
//                    {
//                        foreach (var item in thisPositionStory.LevelUpStory)
//                        {
//                            Thread.Sleep(new Random().Next(1000, 2000));
//                            MyLog.OutputAndSaveTxt(_loadHeroModel.MyHero + "：" + item);
//                        }
//                    }
//                    MyLog.OutputAndSaveTxt($"{_loadHeroModel.MyHero}:完成了剧情《{message}》,时间在{time}");
//                    Thread.Sleep(new Random().Next(1000, 2000));
//                };

//                if (!_standby)
//                {
//                    MyLog.OutputAndSaveTxt($"因为{_loadHeroModel.MyHero}的到来:天龙八部就此拉开序幕");
//                    Thread.Sleep(new Random().Next(1000, 2000));
//                    _standby = true;
//                }
//            }
//            return result;
//        }

//        /// <summary>
//        /// 通过XML读取故事子内容
//        /// </summary>
//        /// <returns></returns>
//        private static FullStoryModel LoadXmlStory()
//        {
//            AppSettingsReader AppRead = new AppSettingsReader();
//            var settingXml = AppRead.GetValue("StoryXml", typeof(string)).ToString();
//            return MyXmlHelper.DeserializeXMLFileToObject<FullStoryModel>(settingXml);
//        }

//        private ConsoleColor RadomColor()
//        {
//            switch (_loadHeroModel.MyHero)
//            {
//                case "乔峰":
//                    return ConsoleColor.DarkYellow;

//                case "段誉":
//                    return ConsoleColor.Green;

//                case "虚竹":
//                    return ConsoleColor.Red;
//                default:
//                    return ConsoleColor.White;
//            }
//        }

//        public void Dispose()
//        {
//        }
//    }
//}