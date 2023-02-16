using RootCapsule.DataStorage.Repository;
using RootCapsule.ModelData.Fields;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RootCapsule.DataStorage.Iteractor
{
    public class FieldIteractor : Iteractor<FieldData>
    {
        const int TIMEOUT = 6000;
        IRepository repository;

        public FieldIteractor(IRepository repository)
        {
            this.repository = repository;
        }

        public void SaveData(IEnumerable<FieldData> data, out IEnumerable<Exception> exceptions)
        {
            List<Task> tasks = new List<Task>();
            var exceptionList = new List<Exception>();

            foreach (FieldData item in data)
            {
                var task = Task.Run(() => repository.SaveAsync(item.ID, item));
                tasks.Add(task);
            }
            if (!Task.WaitAll(tasks.ToArray(), TIMEOUT))
            {
                Debug.Log("Timeout!");
            }

            foreach (var task in tasks)
            {
                if (task.IsFaulted)
                {
                    exceptionList.Add(task.Exception.InnerException);
                }
            }

            exceptions = exceptionList.Count > 0 ? exceptionList : null;
        }

        public IEnumerable<FieldData> LoadData(IEnumerable<string> dataIds, out IEnumerable<Exception> exceptions)
        {
            var tasks = new List<Task<FieldData>>();
            var exceptionList = new List<Exception>();
            var data = new List<FieldData>();

            foreach (string id in dataIds)
            {
                var task = Task.Run(() => repository.LoadAsync<FieldData>(id));
                tasks.Add(task);
            }
            if (!Task<FieldData>.WaitAll(tasks.ToArray(), TIMEOUT))
            {
                Debug.Log("Timeout!");
            }

            foreach (var task in tasks)
            {
                if (task.IsFaulted)
                {
                    exceptionList.Add(task.Exception.InnerException);
                    break;
                }
                else if (task.IsCompleted)
                {
                    data.Add(task.Result);
                }
            }

            exceptions = exceptionList.Count > 0 ? exceptionList : null;
            return data;
        }


    }
}
