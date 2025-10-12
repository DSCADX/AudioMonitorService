
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System.Data;

namespace AudioMonitorService
{
    class AudioDeviceNotificationClient : IMMNotificationClient
    {
        private readonly MMDeviceEnumerator enumerator;

        public event Action<MMDevice>? DefaultDeviceChanged;

        public AudioDeviceNotificationClient()
        {
            enumerator = new MMDeviceEnumerator();
            enumerator.RegisterEndpointNotificationCallback(this);
        }

        public void OnDeviceStateChanged(string defaultDeviceId, DeviceState newState)
        {           
            if (newState == DeviceState.Active) 
            {
                MMDevice device = enumerator.GetDevice(defaultDeviceId);
                DefaultDeviceChanged?.Invoke(device);
                Console.WriteLine(newState.ToString() + "    " + device.FriendlyName);
            }
        }

        public void OnDeviceAdded(string defaultDeviceId)
        {
        }

        public void OnDeviceRemoved(string deviceId)
        {
        }

        public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
            if (flow == DataFlow.Render && role == Role.Multimedia)
            {
                MMDevice device = enumerator.GetDevice(defaultDeviceId);
                DefaultDeviceChanged?.Invoke(device);
            }
        }

        public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
        {
        }


        public MMDeviceCollection getDevicesList()
        {
            return enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
        }
    }
}
