using System;
using System.Management;
using System.Text.RegularExpressions;

namespace Jlib
{

    /// <summary>
    /// 设备变动事件处理委托
    /// </summary>
    /// <param name="IsInsert"></param>
    /// <param name="pid"></param>
    /// <param name="vid"></param>
    /// <param name="id"></param>
    public delegate void DeviceChangedEventHandler(bool IsInsert,string pid,string vid,string id );

    /// <summary>
    /// 硬件设备监控类
    /// </summary>
    public class HardwareMonitor
    {
        private static readonly string InsertQuery = @"
        SELECT * FROM __InstanceCreationEvent 
        WITHIN 2 
        WHERE TargetInstance ISA 'Win32_USBControllerDevice'";

        private static readonly string RemoveQuery = @"
        SELECT * FROM __InstanceDeletionEvent 
        WITHIN 2 
        WHERE TargetInstance ISA 'Win32_USBControllerDevice'";

        private ManagementEventWatcher insertWatcher;
        private ManagementEventWatcher removeWatcher;

        public event DeviceChangedEventHandler DeviceChangedEvent;


        public void StartMonitoring()
        {
            //创建设备插入监听器
           insertWatcher = new ManagementEventWatcher(InsertQuery);
            insertWatcher.EventArrived += (sender, e) =>
            {
                var device = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                var str = device.GetPropertyValue("Dependent")?.ToString();
                if (IsPhysicalDevice(str, out string vid,out string pid, out string sid))
                {
                    Console.WriteLine($"插入物理设备:{str} VID/PID: {vid}/{pid}, ID: {sid}");
                    DeviceChangedEvent?.BeginInvoke(true, vid,pid, sid, null, null); // 异步调用事件
                }
                else
                {
                    Console.WriteLine($"插入虚拟设备或非USB设备: ID: {sid}");
                }
            };

            // 创建设备移除监听器
            removeWatcher = new ManagementEventWatcher(RemoveQuery);
            removeWatcher.EventArrived += (sender, e) =>
            {
                Console.WriteLine("设备已移除");
                // 此处可添加移除处理逻辑

                var device = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                var str = device.GetPropertyValue("Dependent")?.ToString().Trim();
                if (IsPhysicalDevice(str, out string vid, out string pid, out string sid))
                {
                    Console.WriteLine($"移除物理设备:{str} VID/PID: {vid}/{pid}, ID: {sid}");
                    DeviceChangedEvent?.BeginInvoke(false, vid,pid, sid, null,null); // 异步调用事件
                }
                else
                {
                    Console.WriteLine($"移除虚拟设备或非USB设备: ID: {sid}");
                }


            };

            // 启动监听
            insertWatcher.Start();
            removeWatcher.Start();
            Console.WriteLine("开始监听硬件变动...");
        }

        public void StopMonitoring()
        {
            insertWatcher?.Stop();
            removeWatcher?.Stop();
            Console.WriteLine("已停止硬件监听");
        }

        private  bool IsPhysicalDevice(string deviceIdString, out string vid,out string pid, out string sid)
        {
            vid = "";
            pid = "";
            sid = "";

            // 正则表达式（详细说明见下方）
            string pattern = @"DeviceID=""(?<type>USB|HID)\\.*?VID_(?<vid>[0-9A-F]{4})&PID_(?<pid>[0-9A-F]{4}).*?\\\\(?<sid>[^""]+)""";

            Match match = Regex.Match(deviceIdString, pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
                return false;
            string deviceType = match.Groups["type"].Value;
                vid = match.Groups["vid"].Value;
                pid = match.Groups["pid"].Value;
                sid = match.Groups["sid"].Value;
            return true;
        }
    }
}
