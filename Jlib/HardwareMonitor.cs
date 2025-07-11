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
                var id = device.GetPropertyValue("Dependent")?.ToString();
                if (IsPhysicalDevice(id, out string vidPid, out id))
                {
                    Console.WriteLine($"插入物理设备: VID/PID: {vidPid}, ID: {id}");
                    DeviceChangedEvent?.BeginInvoke(true, vidPid, id, id, null, null); // 异步调用事件
                }
                else
                {
                    Console.WriteLine($"插入虚拟设备或非USB设备: ID: {id}");
                }
            };

            // 创建设备移除监听器
            removeWatcher = new ManagementEventWatcher(RemoveQuery);
            removeWatcher.EventArrived += (sender, e) =>
            {
                Console.WriteLine("设备已移除");
                // 此处可添加移除处理逻辑

                var device = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                var id = device.GetPropertyValue("Dependent")?.ToString().Trim();
                if(IsPhysicalDevice(id, out string vidPid, out id))
                {
                    Console.WriteLine($"移除物理设备: VID/PID: {vidPid}, ID: {id}");
                    DeviceChangedEvent?.BeginInvoke(false, vidPid, id, id,null,null); // 异步调用事件
                }
                else
                {
                    Console.WriteLine($"移除虚拟设备或非USB设备: ID: {id}");
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

        private  bool IsPhysicalDevice(string deviceIdString, out string vidPid, out string id)
        {
            vidPid = null;
            id = null;
            var regex = new Regex(@".+?DeviceID=""(.+?)""", RegexOptions.Compiled);

            var match = regex.Match(deviceIdString);
            if (!match.Success)
                return false;

            string eventType = match.Groups[0].Value;
            deviceIdString = match.Groups[1].Value.Replace(@"\\", @"\"); // 将双反斜杠转换为单反斜杠

            // 检查是否符合物理设备特征
            if (!deviceIdString.StartsWith("USB\\", StringComparison.Ordinal) ||
                deviceIdString.Contains("&MI_"))
            {
                return false;
            }

            // 提取VID/PID部分和设备ID
            var parts = deviceIdString.Split('\\');
            if (parts.Length < 3) return false;

            // 查找包含VID/PID的部分
            foreach (var part in parts)
            {
                if (part.StartsWith("VID_", StringComparison.Ordinal) &&
                    part.Contains("&PID_"))
                {
                    vidPid = part;
                    break;
                }
            }

            if (vidPid == null) return false;

            // 最后一个非空部分作为设备ID
            id = parts[parts.Length - 1];
            return !string.IsNullOrEmpty(id);
        }
    }
}
