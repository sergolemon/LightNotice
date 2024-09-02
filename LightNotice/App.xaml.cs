using Plugin.Maui.Audio;

namespace LightNotice
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            var stoppingTokenSource = new CancellationTokenSource();
            var stoppingToken = stoppingTokenSource.Token;

            Task.Run(async () => {
                using Stream soundStream = await FileSystem.OpenAppPackageFileAsync("Kamaliya_Svitlo_Ye.mp3");
                using IAudioPlayer audioPlayer = AudioManager.Current.CreatePlayer(soundStream);

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (Battery.Default.PowerSource == BatteryPowerSource.AC && !audioPlayer.IsPlaying)
                    {
                        audioPlayer.Play();
                    }
                    else if (Battery.Default.PowerSource != BatteryPowerSource.AC && audioPlayer.IsPlaying)
                    {
                        audioPlayer.Stop();
                    }
                    else
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }

                audioPlayer.Stop();
            }, stoppingToken);

            base.OnStart();
        }
    }
}
