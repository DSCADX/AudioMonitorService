# AudioMonitorService
This is a simple windows service that monitors when the selected audio device changes and then sets a volume previously defined for this device.

# Why?
I was tired of turning down the volume of my earpods each time that I connected because they always started at 80% volume, and if I forgot and played any sound, they left me deaf for some minutes.

# Installation
-Unzip  and copy the folder "AudioMonitorService" to your favorite location. Be sure that location won't change in the future.

-Run as administrator the file "Install.bat"

-Go again to the folder "AudioMonitorService" and edit the file "Devices.ini" to set volume values and enable the devices that you want to adjust volume on selected.

-Finally restart the service "AudioMonitorService" using Windows Services Administrator or restart your PC.

# Uninstall
-Run as administrator the file "Uninstall.bat"
