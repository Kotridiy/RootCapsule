using RootCapsule.Core;
using System;

namespace RootCapsule.Model.Fields
{
    public class Seed
    {
        public PlantType PlantType { get; private set; }
        public SeedStat SeedStat { get; private set; }

        public Seed(PlantType plantType, SeedStat seedStat = default)
        {
            PlantType = plantType;
            SeedStat = seedStat;
        }
    }
}