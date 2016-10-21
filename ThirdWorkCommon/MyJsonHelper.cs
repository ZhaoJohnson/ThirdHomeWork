using System;
using System.IO;
using Newtonsoft.Json;

namespace ThirdWorkCommon
{
    public class MyJsonHelper
    {

        static readonly string TheBasePath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 将对象序列化为Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void object2Json<T>(T t)
        {
            string myjsonName = typeof(T).Name + ".json";
            string tjson = JsonConvert.SerializeObject(t);

            if (!File.Exists(Path.Combine(TheBasePath, myjsonName)))
                File.Create(Path.Combine(TheBasePath, myjsonName));
            File.WriteAllText(Path.Combine(TheBasePath, myjsonName), tjson);
        }
        /// <summary>
        /// 反序列化创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T Json2Object<T>(T t)
        {
            string myjsonName = typeof(T).Name + ".json";
            var fillJson = File.ReadAllText(Path.Combine(TheBasePath, myjsonName));
            return JsonConvert.DeserializeObject<T>(fillJson);
        }
    }
}

