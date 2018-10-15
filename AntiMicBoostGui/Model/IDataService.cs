using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiMicBoostGui.Model
{
    public interface IDataService
    {
        void LoadData(Action<DataItem, Exception> callback);
        void SaveData(DataItem data);
    }
}
