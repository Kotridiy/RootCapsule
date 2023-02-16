using System;
using UnityEngine;

namespace RootCapsule.ModelData.Fields
{
    [Serializable]
    public struct VectorData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    [Serializable]
    public struct QuaternionData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
    }
}