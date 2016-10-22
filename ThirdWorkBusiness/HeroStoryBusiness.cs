using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                taskList.AddRange(bu.LoadStoryTask().ToArray());

                taskFactory.StartNew(taskList.ToArray);

                taskFactory.ContinueWhenAll(taskList.ToArray(), t =>
                {
                    //函数执行完后执行，执行一次
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
