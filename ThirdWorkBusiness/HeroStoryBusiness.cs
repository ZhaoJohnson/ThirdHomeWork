using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ThirdWorkCommon;
using ThirdWorkInterFace.IBusiness;
using ThirdWorkModel;

namespace ThirdWorkBusiness
{
    public class HeroStoryBusiness : IHeroStoryBusiness
    {
        private static object objectLock = new object();
        private HeroModel _HeroModel;

        public HeroStoryBusiness(HeroModel model)
        {
            _HeroModel = model;
        }

        /// <summary>
        /// 收集所有人的故事
        /// </summary>
        /// <returns></returns>
        public Action ShowStory()
        {
            return () =>
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
            };
        }

        public void Dispose()
        {
        }
    }
}