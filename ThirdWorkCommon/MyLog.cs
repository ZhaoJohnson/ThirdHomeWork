using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ThirdWorkCommon
{
    public static class MyLog
    {
        private static object objectLock = new object();
        private static readonly string TheBasePath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///  通用输出并记录，主线程
        /// </summary>
        /// <param name="message"></param>
        public static void OutputAndSaveTxt(string message)
        {
            if (!File.Exists(Path.Combine(TheBasePath, "Alloutput.txt")))
                File.Create(Path.Combine(TheBasePath, "Alloutput.txt"));
            ReaderWriterLock rwl = new System.Threading.ReaderWriterLock();
            lock (objectLock)
            {
                rwl.AcquireWriterLock(1000);

                Thread.Sleep(new Random().Next(1000, 2000));
                Console.WriteLine(message);
                File.AppendAllLines(Path.Combine(TheBasePath, "Alloutput.txt"), new List<string>() { message });
                //File.AppendAllText(Path.Combine(TheBasePath, "Alloutput.txt"), message);
                Thread.Sleep(new Random().Next(1000, 2000));

                rwl.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// 异常调用此方法记录
        /// </summary>
        /// <param name="message"></param>
        public static void SaveEx(string message)
        {
            Console.WriteLine(message);

            string LogPath = TheBasePath + "MyLogs";
            string dateTodayfileName = DateTime.Now.ToString("mmmm_dd_yyyy") + "logs.txt";
            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);
            DirectoryInfo mylogpath = new DirectoryInfo(LogPath);
            var FullPath = Path.Combine(LogPath, dateTodayfileName);

            FileStream myfs = new FileStream(FullPath, FileMode.OpenOrCreate);
            using (StreamWriter mysw = new StreamWriter(myfs))
            {
                mysw.WriteLine(message);
                mysw.Flush();
                mysw.Close();
                myfs.Close();
            }
        }
    }
}