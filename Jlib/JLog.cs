/*******************************************************************
 * * 功   能：  日志输出
 * * 作   者：  Jason
 * * 编程语言： C# 
 * *******************************************************************/
using System;
using System.IO;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace Jlib
{
    /// <summary>
    /// 日志
    /// </summary>
    public class JLog : IJLog
    {
        private ILog MyLog = null;
        private ILoggerRepository repository;
        private string _ModuleName = string.Empty;
        public bool PrintEnabled
        {
            get
            {
                //return _PrintConfig != null && _PrintConfig.CurrentConfig.ContainsKey(_ModuleName) && _PrintConfig.CurrentConfig[_ModuleName].PrintEnabled == true;//暂时禁用实时配置是否起效功能
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName">输出日志的模块名称</param>
        /// <param name="config">输出日志的模块名称</param>
        /// <param name="ConfigFileName">日志输出配置文件</param>
        /// <exception cref="Exception"></exception>
        public JLog(string moduleName, string ConfigFileName = "JLeapLog")
        {
            try
            {
                _ModuleName = moduleName;
                var asm = typeof(JLog).Assembly;                                //获取当前程序集所在路径
                var local = asm.Location;
                local = Path.GetDirectoryName(local);
                ConfigFileName = local + $@"\JLeapLog.config";    //获取配置文件路径
               log4net.Config.XmlConfigurator.Configure(new FileInfo(ConfigFileName));
                if (File.Exists(ConfigFileName))
                {
                    if (repository == null|| !repository.Name.Equals(_ModuleName))
                        repository = LogManager.CreateRepository(_ModuleName);
                    XmlConfigurator.Configure(repository, new FileInfo(ConfigFileName));//从xml文件中配置
                    MyLog = LogManager.GetLogger(_ModuleName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Error(string msg, string time = "")
        {
            try
            {
                if (!PrintEnabled)
                    return;
                time = !string.IsNullOrEmpty(time) ? $"[{time}]" : time;
                MyLog?.Error($"{msg}");
            }
            catch (Exception)
            {
            }
        }

        public void Fatal(string msg, string time = "")
        {
            try
            {
                if (!PrintEnabled)
                    return;
                time = string.IsNullOrEmpty(time) ? $"[{time}]" : time;
                MyLog?.Fatal($"{msg}");
            }
            catch (Exception)
            {
            }
        }
        public void Warm(string msg, string time = "")
        {
            try
            {
                if (!PrintEnabled)
                    return;
                time = string.IsNullOrEmpty(time) ? $"[{time}]" : time;
                MyLog?.Warn($"{msg}");
            }
            catch (Exception)
            {

            }
        }
        public void Info(string msg, string time = "")
        {
            try
            {
                if (!PrintEnabled)
                    return;
                MyLog?.Info($"{msg}");
            }
            catch (Exception)
            {

            }
        }
        public void Debug(string msg, string time = "")
        {
            try
            {
                if (!PrintEnabled)
                    return;
                MyLog?.Debug($"{msg}");
            }
            catch (Exception)
            {

            }
        }
    }
}
