using RootCapsule.Core.Types;
using System;

namespace RootCapsule.ModelData.Fields
{
    [Serializable]
    public struct DeadPlantData
    {
        public PlantType PlantType { get; set; }
        public VectorData[,] PartsPositions { get; set; }
        public QuaternionData[,] PartsRotations { get; set; }
    }
}
