using NAudio.CoreAudioApi;

namespace AudioMonitorService
{
    static class Control
    {
        readonly static string _devicesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Devices.ini");
        private static List<DeviceVolumeObj> devList=new List<DeviceVolumeObj>();
        private  static AudioDeviceNotificationClient client = new AudioDeviceNotificationClient();

        public static string Init()
        {
            try 
            {
                if (!File.Exists(_devicesFile))
                {
                    using (var myFile = File.Create(_devicesFile))
                    {

                    }
                }
                foreach (string record in File.ReadLines(_devicesFile))
                {
                    record.Trim();
                    string[] elements = record.Split(' ');
                    if (string.IsNullOrWhiteSpace(record) || record[0] == '#')
                    {
                        continue;
                    }
                    if (elements.Length <= 3)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(elements[0]) || string.IsNullOrWhiteSpace(elements[3]))
                    {
                        continue;
                    }
                    if (float.TryParse(elements[1], out float volume) && bool.TryParse(elements[2], out bool enable))
                    {
                        volume = Math.Clamp(volume, 0f, 1f);
                        devList.Add(new DeviceVolumeObj(string.Join(" ", elements, 3, elements.Length - 3), elements[0], enable, volume));
                    }
                    else
                    {
                        continue;
                    }
                }
                foreach (var device in client.getDevicesList())
                {
                    if (!devList.Any(e => e != null && e.deviceID.Equals(device.ID)))
                    {
                        devList.Add(new DeviceVolumeObj(device.FriendlyName, device.ID, false, device.AudioEndpointVolume.MasterVolumeLevelScalar != null ? device.AudioEndpointVolume.MasterVolumeLevelScalar : 0.35f));
                    }
                }
                SaveInifile();
                return "ok";
            } catch (Exception ex) 
            {
                return "Error "+ex.Message;
            }
        }
        public static void SaveInifile() 
        {
            string st = "";
            st += "# volume values 0.0000 to 1.0000\n";
            st += "# Columns\n";
            st += "# Device ID - Volume on set device - Enable to apply volume - Device name\n";
            st += "\n";
            foreach (var device in devList)
            {
                st += device.deviceID + " " + device.deviceVolume.ToString("0.0000") +" "+device.isEnable+ " " + device.deviceName +"\n";
            }
            File.WriteAllText(_devicesFile, st);
        }

        public static async Task<string> MonitoringDeviceAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            void DeviceChangedHandler(MMDevice device)
            {
                DeviceVolumeObj? dvObj = devList.Find(e => e.deviceID.Equals(device.ID));
                if (dvObj != null && dvObj.isEnable && device.AudioEndpointVolume.MasterVolumeLevelScalar >= dvObj.deviceVolume)
                {
                    device.AudioEndpointVolume.MasterVolumeLevelScalar = dvObj.deviceVolume;
                    tcs.TrySetResult("Set "+device.DeviceFriendlyName+" Volume: "+device.AudioEndpointVolume.MasterVolumeLevelScalar);
                }
                tcs.TrySetResult("");
            }
            client.DefaultDeviceChanged += DeviceChangedHandler;

            try
            {
                using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                {
                    return await tcs.Task;
                }
            }
            finally
            {
                client.DefaultDeviceChanged -= DeviceChangedHandler;
            }
        }
    }
}
