using System;
using AntiMicBoostGui.Model;

namespace AntiMicBoostGui.Design
{
    public class DesignDataService : IDataService
    {
        public void LoadData(Action<DataItem, Exception> callback)
        {
            var item = new DataItem
            {
                DesiredVolume = 95,
                OpenOnSystemStartup = true,
                StartMinimized = false
            };
            callback(item, null);
        }

        public void SaveData(DataItem data)
        {
            throw new NotImplementedException();
        }
    }
}