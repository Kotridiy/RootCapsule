using System;

namespace RootCapsule.ModelData.Fields
{
    [Serializable]
    public struct FieldData
    {
        public string ID { get; set; }

        public ArableData[] Arables { get; set; }
    }
}
