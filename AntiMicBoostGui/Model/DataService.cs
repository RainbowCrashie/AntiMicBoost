using System;
using System.IO;

namespace AntiMicBoostGui.Model
{
    public class DataService : IDataService
    {
        private static readonly string ConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "config.cfg");

        public void LoadData(Action<DataItem, Exception> callback)
        {
            var item = new DataItem();
            if (File.Exists(ConfigPath))
            {
                item = new XmlSerializer<DataItem>().DeserializeFromFile(ConfigPath);
            }
            else
            {
                item.DesiredVolume = (int)(new Core().GetMicLevelProperty().MasterVolumeLevelScalar * 100);
                item.OpenOnSystemStartup = true;
                item.StartMinimized = false;
            }
            callback(item, null);
        }

        public void SaveData(DataItem data)
        {
            new XmlSerializer<DataItem>().SerializeToFile(data, ConfigPath);
        }
    }

    public class XmlSerializer<T> where T : class
    {
        public void SerializeToFile(T subject, string filepath)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (StreamWriter wr = new StreamWriter(filepath))
            {
                xs.Serialize(wr, subject);
            }
        }

        public T DeserializeFromFile(string filepath)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (StreamReader rd = new StreamReader(filepath))
            {
                return xs.Deserialize(rd) as T;
            }
        }
    }
}