namespace Jlib
{
    public class HardWareDevice 
    {
        public string ProductId = "";
        public string VendorId = "";
        public string SerialNumber = "";
        public EnumDeviceType DeviceType = EnumDeviceType.Unknown;
        public string DeviceID
        {
            get
            {
                return $"{VendorId}-{ProductId}-{SerialNumber}";
            }
        }
        public HardWareDevice(string vid, string pid,string snumer)
        {
            ProductId = pid;
            VendorId = vid;
            SerialNumber = snumer;
        }
        public void SetDeviceType(EnumDeviceType deviceType)
        {
            DeviceType = deviceType;
        }

    }
}
