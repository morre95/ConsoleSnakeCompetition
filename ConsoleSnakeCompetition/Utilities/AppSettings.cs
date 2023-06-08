using System.Xml.Serialization;


namespace ConsoleSnakeCompetition.Utilities
{
    public class AppSettings
    {
        private static readonly Lazy<AppSettings> instance = new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance => instance.Value;

        private readonly string settingsFile = Path.GetFullPath("Resources/Settings/settings.xml");

        public string GameName { get; set; } = "The Best Game";

        public int Speed { get; set; } = 50;

        public int BestOf { get; set; } = 3;

        public int MinSpeedValue { get; set; } = 1;
        public int MaxSpeedValue { get; set; } = 1000;
        public int StepStepCount { get; set; } = 100;

        public bool Player1Colorized { get; set; } = true;
        public bool Player1ColorInverted { get; set; } = false;
        public char Player1Symbol { get; set; } = '#';
        public int Player1StartLength { get; set; } = 5;

        public bool Player2Colorized { get; set; } = true;
        public bool Player2ColorInverted { get; set; } = true;
        public char Player2Symbol { get; set; } = '&';
        public int Player2StartLength { get; set; } = 5;

        public bool ComputerColorized { get; set; } = false;
        public bool ComputerColorInverted { get; set; } = false;
        public char ComputerSymbol { get; set; } = '|';
        public int ComputerStartLength { get; set; } = 5;

        public string ThemeColor { get; set; } = "Gray";

        private AppSettings()
        {
        }

        public int GetDelayMS()
        {
            var scaleStep = (MaxSpeedValue - MinSpeedValue) / (double)(StepStepCount - 1);
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


