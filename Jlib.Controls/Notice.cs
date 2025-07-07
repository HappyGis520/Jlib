using System;

namespace Jlib.Controls
{
    /// <summary>
    /// 通知消息扩展类
    /// </summary>
    public class Notice : Notification
    {
        public string SoundFile { get; private set; } = "";
        public bool Speak { get; private set; } = false;
        public bool Beep { get; private set; } = true;
        public Notice(string message, EnumNotificationType type, bool speak = false, bool beep = true,string soundFile = "") 
            : base(message, type)
        {
            SoundFile = soundFile;
            Speak = speak;
            Beep = beep;
        }

    }

}
