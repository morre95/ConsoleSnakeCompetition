using System;
using System.Xml.Serialization;


namespace ConsoleSnakeCompetition.Utilities
{
    public class AppSettings
    {
        private static readonly Lazy<AppSettings> instance = new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance => instance.Value;

        private readonly string settingsFile = Path.GetFullPath("Resources/Settings/settings.xml");

        public int Speed { get; set; } = 50;

        public int IncreaseSpeedEvery { get; set; } = 120;

        public int BestOf { get; set; } = 3;

        public int MinSpeedValue { get; set; } = 1;
        public int MaxSpeedValue { get; set; } = 1000;
        public int StepStepCount { get; set; } = 100;

        public string Player1Name { get; set; } = "Player 1";
        public bool Player1Colorized { get; set; } = true;
        public bool Player1ColorInverted { get; set; } = false;
        public char Player1Symbol { get; set; } = '#';
        public int Player1StartLength { get; set; } = 5;
        public bool Player1DieWhenEaten { get; set; } = true;
         

        public string Player2Name { get; set; } = "Player 2";
        public bool Player2Colorized { get; set; } = true;
        public bool Player2ColorInverted { get; set; } = true;
        public char Player2Symbol { get; set; } = '&';
        public int Player2StartLength { get; set; } = 5;
        public bool Player2DieWhenEaten { get; set; } = true;

        public bool ComputerColorized { get; set; } = false;
        public bool ComputerColorInverted { get; set; } = false;
        public char ComputerSymbol { get; set; } = '|';
        public int ComputerStartLength { get; set; } = 5;

        public string ThemeColor { get; set; } = "Gray";

        public string FoodColor { get; set; } = "Gray";


        private AppSettings()
        {

        }

        public int GetDelayMS()
        {
            var scaleStep = (MaxSpeedValue - MinSpeedValue) / (double)(StepStepCount - 1);
            return (int)(MaxSpeedValue - scaleStep * (Speed - 1));
        }

        public ConsoleColor GetFoodColor()
        {
            return ToEnum<ConsoleColor>(FoodColor);
        }

        public static T ToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
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
            BestOf = other.BestOf;

            Player1Colorized = other.Player1Colorized;
            Player1ColorInverted = other.Player1ColorInverted;
            Player1Symbol = other.Player1Symbol;
            Player1StartLength = other.Player1StartLength;

            Player2Colorized = other.Player2Colorized;
            Player2ColorInverted = other.Player2ColorInverted;
            Player2Symbol = other.Player2Symbol;
            Player2StartLength = other.Player2StartLength;

            ComputerColorized = other.ComputerColorized;
            ComputerColorInverted = other.ComputerColorInverted;
            ComputerSymbol = other.ComputerSymbol;
            ComputerStartLength = other.ComputerStartLength;

            ThemeColor = other.ThemeColor;
            FoodColor = other.FoodColor;
        }
    }
}


