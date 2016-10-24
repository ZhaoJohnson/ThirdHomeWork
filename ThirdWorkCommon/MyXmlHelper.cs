using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ThirdWorkCommon
{
    public static class MyXmlHelper
    {
        private static string TheBasePath = AppDomain.CurrentDomain.BaseDirectory;

        public static List<Action> ReadToActionList(string appsettingName, string singleNode, string propertyName)
        {
            var myxmlName = ConfigurationSettings.AppSettings[appsettingName].ToString();
            string fullPath = Path.Combine(TheBasePath, myxmlName);
            XmlDocument xmlRead = new XmlDocument();
            xmlRead.Load(fullPath);
            XmlNode selectSingleNode = xmlRead.SelectSingleNode(singleNode);
            if (selectSingleNode == null) return null;
            XmlNodeList xmlNodeList = selectSingleNode.ChildNodes;
            List<Action> result = (from XmlNode list in xmlNodeList where list.Name == propertyName from XmlElement child in list.ChildNodes select (Action)(() => Console.WriteLine(child.InnerText))).ToList();

            return result;
        }

        public static T Deserialize<T>(string XmlFilename)
        {
            string path = Path.Combine(TheBasePath, XmlFilename);
            Type targetType = typeof(T);
            if (string.IsNullOrEmpty(path) || !File.Exists(path)
                || targetType == null)
            {
                return default(T);
            }
            object obj = null;
            //if (File.Exists(path))
            //{
            //    Console.WriteLine("找不到文件");
            //}
            try
            {
                XmlSerializerFactory xmlSerializerFactory = new XmlSerializerFactory();
                XmlSerializer xmlSerializer =
                    xmlSerializerFactory.CreateSerializer(targetType, targetType.Name);
                Stream stream = new FileStream(path, FileMode.Open);
                obj = xmlSerializer.Deserialize(stream);
                stream.Close();
            }
            catch
            {
            }
            return (T)obj;
        }

        public static T DeserializeXMLFileToObject<T>(string XmlFilename)
        {
            string path = Path.Combine(TheBasePath, XmlFilename);
            T returnObject = default(T);
            if (string.IsNullOrEmpty(path)) return default(T);

            try
            {
                StreamReader xmlStream = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObject = (T)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                MyLog.OutputAndSaveTxt(ex.Message);
            }
            return returnObject;
        }

        public static bool Serializer<T>(T t, string XmlFilename)
        {
            string path = Path.Combine(TheBasePath, XmlFilename);
            FileStream xmlfile = new FileStream(path, FileMode.OpenOrCreate);

            //创建序列化对象
            XmlSerializer xml = new XmlSerializer(typeof(T));
            try
            {    //序列化对象
                xml.Serialize(xmlfile, t);
                xmlfile.Close();
            }
            catch (InvalidOperationException)
            {
                throw;
            }

            return true;
        }
    }
}