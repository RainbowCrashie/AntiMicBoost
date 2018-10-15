using NAudio.CoreAudioApi;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AntiMicBoostGui
{
    public class Core
    {
        private static readonly TimeSpan VolumeCheckSpan = new TimeSpan(0, 0, 5);
        
        public Action<string> RecordLevelChangedCallback { get; set; }
        public float DesiredVolume { get; set; }

        public async Task Start()
        {
            await ObserveForever();
        }

        public AudioEndpointVolume GetMicLevelProperty()
        {
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();

            var currentDevice = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All)
                .First(d => d.State == DeviceState.Active);

            return currentDevice.AudioEndpointVolume;
        }

        private  void CheckLevelAndCorrect()
        {
            var level = GetMicLevelProperty().MasterVolumeLevelScalar;

            if (level != DesiredVolume)
            {
                GetMicLevelProperty().MasterVolumeLevelScalar = DesiredVolume;
                Log($"Microphone boost detected! Changed record level to {DesiredVolume * 100}%", true);
                RecordLevelChangedCallback($"Microphone boost detected! Changed record level to {DesiredVolume * 100}%");
            }
        }

        private async Task ObserveForever()
        {
            while (true)
            {
                CheckLevelAndCorrect();

                await Task.Delay(VolumeCheckSpan);
            }
        }

        private static void Log(string message, bool important = false)
        {
            if (!important)
                Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine(DateTime.Now.ToString("[MM-dd(ddd) HH:mm:ss] ") + message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

