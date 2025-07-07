namespace Jlib.Controls
{
    /// <summary>
    /// 通知
    /// </summary>
    public class Notification
    {

        public string Message { get; private set; }
        public EnumNotificationType Type { get; private set; }

        public Notification(string message, EnumNotificationType type)
        {
            Message = message;
            Type = type;
        }

    }

}
