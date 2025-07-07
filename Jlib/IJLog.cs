/*******************************************************************
 * * 功   能：  日志输出接口
 * * 作   者：  Jason
 * * 编程语言： C# 
 * *******************************************************************/
namespace Jlib
{
    public interface IJLog
    {
        void Debug(string msg, string time = "");
        void Error(string msg, string time = "");
        void Fatal(string msg, string time = "");
        void Info(string msg, string time = "");
        void Warm(string msg, string time = "");
    }
}