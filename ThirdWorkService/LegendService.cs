using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdWorkBusiness;
using ThirdWorkCommon;
using ThirdWorkInterFace.IBusiness;
using ThirdWorkInterFace.IService;
using ThirdWorkModel;
using ThirdWorkModel.CommonModel;
using System.Configuration;

namespace ThirdWorkService
{
    public class LegendService<THero> : ILegendService
        where THero:HeroStoryBusiness,new()
    {
        public void ShowTest()
        {
            AppSettingsReader AppRead = new AppSettingsReader();
            var s=  AppRead.GetValue("ProjectSettingXml", typeof(string)).ToString();
            LoadHero(LoadSetting(s));
            
        }
        private SettingModel LoadSetting(string fileName)
        {
           return MyXmlHelper.DeserializeXMLFileToObject<SettingModel>(fileName)??null;
        }
        private List<HeroModel> LoadHero(SettingModel _SettingModel)
        {
            List<HeroModel> result = new List<HeroModel>();
            foreach (var item in _SettingModel.HeroName)
            {
                HeroModel model = new HeroModel();
                model= MyJsonHelper.Json2Object<HeroModel>(item + "Model");
                result.Add(model);
            }
            return result;
        }
        public void Dispose()
        {
        }
    }
}
