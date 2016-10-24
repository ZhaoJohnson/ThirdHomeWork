﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        private static readonly object ObjectLock = new object();
        private static bool _standby = false;

        //token.Cancel();标记
        public void Show()
        {
            #region Setting
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            AppSettingsReader AppRead = new AppSettingsReader();
            var settingXml = AppRead.GetValue("ProjectSettingXml", typeof(string)).ToString();
            #endregion

            #region MyRegionTest
            foreach (HeroModel item in LoadHero(LoadSetting(settingXml)))
            {

                foreach (var oneAct in LoadStoryAction(item))
                {
                    taskList.Add(Task.Factory.StartNew(oneAct));
                }
            }
            #endregion

            Task.Factory.ContinueWhenAny(taskList.ToArray(), t =>
            {
                Console.WriteLine("有人已经干过了");
            });



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
        private static SettingModel LoadSetting(string fileName)
        {
            return MyXmlHelper.DeserializeXMLFileToObject<SettingModel>(fileName) ?? null;
        }

        /// <summary>
        /// 通过配置文件创建对象
        /// </summary>
        /// <param name="_SettingModel">设置</param>
        /// <returns></returns>
        private static List<HeroModel> LoadHero(SettingModel _SettingModel)
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

        private Task ShowStory(HeroModel _HeroModel)
        {
            return new Task(() =>
            {
                List<Task> taskList = new List<Task>();
                TaskFactory taskFactory = new TaskFactory();

                ReadSoryBusiness<HeroModel> bu = new ReadSoryBusiness<HeroModel>(_HeroModel);
                foreach (var act in bu.LoadStoryTask())
                {
                    taskList.Add(new Task(act));
                    taskFactory.StartNew(act);
                }
                taskFactory.ContinueWhenAny(taskList.ToArray(), t =>
                {
                    Thread.Sleep(new Random().Next(1000, 2000));
                    MyLog.OutputAndSaveTxt($"因为{_HeroModel.MyHero}的到来:天龙八部就此拉开序幕");
                });                //独立剧情完成后，执行一次
                taskFactory.ContinueWhenAll(taskList.ToArray(), t =>
                {
                    Thread.Sleep(new Random().Next(1000, 2000));
                    MyLog.OutputAndSaveTxt($"{_HeroModel.MyHero}已经通关了");
                });
            });
        }
        private List<Action> LoadStoryAction(HeroModel _heroModel)
        {
            List<Action> taskList = new List<Action>();
            foreach (string item in _heroModel.HeroPosition)
            {
                taskList.Add(SingleHero(_heroModel, item));
            }
            return taskList;
        }

        private Action SingleHero(HeroModel _heroModel, string message)
        {
            Action result;
           // Thread.Sleep(new Random().Next(1000, 2000));
            lock (ObjectLock)
            {
                string time = DateTime.Now.ToString();
               // Thread.Sleep(new Random().Next(1000, 2000));
                result = () =>
                 {
                     Console.ForegroundColor = RadomColor(_heroModel);
                     //Thread.Sleep(new Random().Next(1000, 2000));
                     var thisPositionStory = LoadXmlStory().MyFullStory.FirstOrDefault(p => p.HeroPosition == message) ?? null;
                     if (thisPositionStory != null)
                     {
                         foreach (var item in thisPositionStory.LevelUpStory)
                         {
                            // Thread.Sleep(new Random().Next(1000, 2000));
                             MyLog.OutputAndSaveTxt(_heroModel.MyHero + "：" + item);
                         }
                     }
                     MyLog.OutputAndSaveTxt($"{_heroModel.MyHero}:完成了剧情《{message}》,时间在{time}");
                    // Thread.Sleep(new Random().Next(1000, 2000));
                 };

                //if (!_standby)
                //{
                //    MyLog.OutputAndSaveTxt($"因为{_heroModel.MyHero}的到来:天龙八部就此拉开序幕");
                //    Thread.Sleep(new Random().Next(1000, 2000));
                //    _standby = true;
                //}
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

        private ConsoleColor RadomColor(HeroModel _heroModel)
        {
            switch (_heroModel.MyHero)
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