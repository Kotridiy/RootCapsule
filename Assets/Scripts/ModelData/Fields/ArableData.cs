using RootCapsule.Core;
using System;

namespace RootCapsule.ModelData.Fields
{
    [Serializable]
    public struct ArableData
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public PlantData? Plant { get; set; }
        public DeadPlantData? DeadPlant { get; set; }
        public FertilizerData? Fertilizer { get; set; }
    }
}
