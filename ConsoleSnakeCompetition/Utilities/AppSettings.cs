﻿using System.Xml.Serialization;


namespace ConsoleSnakeCompetition.Utilities
{
    public class AppSettings
    {
        private static readonly Lazy<AppSettings> instance = new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance => instance.Value;

        public string GameName { get; set; } = "The Best Game";

        public int Speed { get; set; } = 1;

        public int MinSpeedValue { get; set; } = 50;
        public int MaxSpeedValue { get; set; } = 400;
        public int StepSpeedCount { get; set; } = 20;

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
            if (File.Exists("settings.xml"))
            {
                using (StreamReader reader = new StreamReader("settings.xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                    AppSettings loadedSettings = (AppSettings)serializer.Deserialize(reader);
                    CopyProperties(loadedSettings);
                }
            }
        }

        public void SaveSettings()
        {
            using (StreamWriter writer = new StreamWriter("settings.xml"))
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

