using System.Xml.Serialization;


namespace ConsoleSnakeCompetition.Utilities
{
    public class AppSettings : ISettings
    {
        private static readonly Lazy<AppSettings> instance = new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance => instance.Value;

        private readonly string settingsFile = Path.GetFullPath("Resources/Settings/settings.xml");

        public string GameName { get; set; } = "The Best Game";

        public int Speed { get; set; } = 1;

        public int MinSpeedValue { get; set; } = 5;
        public int MaxSpeedValue { get; set; } = 800;
        public int StepSpeedCount { get; set; } = 50;

        public string ThemeColor { get; set; } = "Blue";

        private AppSettings()
        {
        }

        public int GetDelayMS()
        {
            var scaleStep = (MaxSpeedValue - MinSpeedValue) / (double)(StepSpeedCount - 1);
            return (int)(MaxSpeedValue - scaleStep * (Speed - 1));
        }

        public void LoadSettings()
        {
            if (File.Exists(settingsFile))
            {
                using (StreamReader reader = new StreamReader(settingsFile))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                    AppSettings loadedSettings = (AppSettings)serializer.Deserialize(reader);
                    CopyProperties(loadedSettings);
                }
            }
        }

        public void SaveSettings()
        {
            using (StreamWriter writer = new StreamWriter(settingsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                serializer.Serialize(writer, this);
            }
        }

        private void CopyProperties(AppSettings other)
        {
            Speed = other.Speed;
            ThemeColor = other.ThemeColor;
        }
    }
}


