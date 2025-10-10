namespace AudioMonitorService
{
    class DeviceVolumeObj
    {
        public string? deviceName { get; set; }
        public string? deviceID { get; set; }
        public bool isEnable { get; set; } = false;
        public float deviceVolume { get; set; } = 0.35f;

        public DeviceVolumeObj(string? deviceName, string? deviceID, bool isEnable, float deviceVolume)
        {
            this.deviceName = deviceName;
            this.deviceID = deviceID;
            this.deviceVolume = deviceVolume;
            this.isEnable = isEnable;
        }
    }
}
