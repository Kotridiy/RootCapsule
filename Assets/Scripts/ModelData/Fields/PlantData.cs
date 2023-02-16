using RootCapsule.Core.Types;
using System;

namespace RootCapsule.ModelData.Fields
{
    [Serializable]
    public struct PlantData
    {
        public PlantType PlantType { get; set; }
        public SeedStat SeedStat { get; set; }
        public PlantState PlantState { get; set; }
        public VectorData[,] PartsPositions { get; set; }
        public QuaternionData[,] PartsRotations { get; set; }
    }
}
