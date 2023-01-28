using System;

namespace RootCapsule.Core
{
    // developing: loading from repository 
    public struct PlantType
    {

        public string Id { get; }
        public int GrowthTime { get; }
        public int LifeTime { get; }
        public int HarvestMin { get; }
        public int HarvestMax { get; }
        public int SeedsMin { get; }
        public int SeedsMax { get; }
        public int HarvestPrice { get; }
        public int SeedPrice { get; }
        public int Mutability { get; }
        public int Influence { get; }
        public int Resistance { get; }
        public int Capacity { get; }
        public bool Refillable { get; }

        public PlantType(Builder b)
        {
            Id = b.Id ?? throw new ArgumentNullException(nameof(b.Id));
            GrowthTime = ValidateMin(b.GrowthTime, 1, nameof(b.GrowthTime));
            LifeTime = ValidateMin(b.LifeTime, 1, nameof(b.LifeTime));
            HarvestMin = ValidateMin(b.HarvestMin, 0, nameof(b.HarvestMin));
            HarvestMax = ValidateMin(b.HarvestMax, 1, nameof(b.HarvestMax));
            SeedsMin = ValidateMin(b.SeedsMin, 0, nameof(b.SeedsMin));
            SeedsMax = ValidateMin(b.SeedsMax, 1, nameof(b.SeedsMax));
            HarvestPrice = ValidateMin(b.HarvestPrice, 0, nameof(b.HarvestPrice));
            SeedPrice = ValidateMin(b.SeedPrice, 0, nameof(b.SeedPrice));
            Mutability = ValidateMin(b.Mutability, 1, nameof(b.Mutability));
            Influence = ValidateMin(b.Influence, 1, nameof(b.Influence));
            Resistance = ValidateMin(b.Resistance, 1, nameof(b.Resistance));
            Capacity = ValidateMin(b.Capacity, 1, nameof(b.Capacity));
            Refillable = b.Refillable;
        }

        public class Builder
        {
            public string Id { get; set; }
            public int GrowthTime { get; set; }
            public int LifeTime { get; set; }
            public int HarvestMin { get; set; }
            public int HarvestMax { get; set; }
            public int SeedsMin { get; set; }
            public int SeedsMax { get; set; }
            public int HarvestPrice { get; set; }
            public int SeedPrice { get; set; }
            public int Mutability { get; }
            public int Influence { get; set; }
            public int Resistance { get; set; }
            public int Capacity { get; set; }
            public bool Refillable { get; set; }

            public PlantType Build()
            {
                return new PlantType(this);
            }
        }

        private static int ValidateMin(int value, int min, string paramName)
        {
            return value >= min ? value : throw new ArgumentException(paramName);
        }
    }
}