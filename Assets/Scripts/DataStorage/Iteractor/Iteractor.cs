using System;
using System.Collections.Generic;

namespace RootCapsule.DataStorage.Iteractor
{
    public interface Iteractor<T> where T : struct
    {
        void SaveData(IEnumerable<T> data, out IEnumerable<Exception> exceptions);

        IEnumerable<T> LoadData(IEnumerable<string> dataIds, out IEnumerable<Exception> exceptions);
    }
}
