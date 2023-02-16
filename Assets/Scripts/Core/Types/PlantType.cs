using Newtonsoft.Json;
using System;

namespace RootCapsule.Core.Types
{
    // developing: loading from repository
    [Serializable]
    public class PlantType
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

        #region Constructors
        public PlantType(Builder b) : this(
            b.Id,
            b.GrowthTime,
            b.LifeTime,
            b.HarvestMin,
            b.HarvestMax,
            b.SeedsMin,
            b.SeedsMax,
            b.HarvestPrice,
            b.SeedPrice,
            b.Mutability,
            b.Influence,
            b.Resistance,
            b.Capacity,
            b.Refillable
        ) { }

        [JsonConstructor]
        public PlantType(string id, int growthTime, int lifeTime, int harvestMin, int harvestMax, int seedsMin, int seedsMax, int harvestPrice, int seedPrice, int mutability, int influence, int resistance, int capacity, bool refillable)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            GrowthTime = ValidateMin(growthTime, 1, nameof(growthTime));
            LifeTime = ValidateMin(lifeTime, 1, nameof(lifeTime));
            HarvestMin = ValidateMin(harvestMin, 0, nameof(harvestMin));
            HarvestMax = ValidateMin(harvestMax, 1, nameof(harvestMax));
            SeedsMin = ValidateMin(seedsMin, 0, nameof(seedsMin));
            SeedsMax = ValidateMin(seedsMax, 1, nameof(seedsMax));
            HarvestPrice = ValidateMin(harvestPrice, 0, nameof(harvestPrice));
            SeedPrice = ValidateMin(seedPrice, 0, nameof(seedPrice));
            Mutability = ValidateMin(mutability, 1, nameof(mutability));
            Influence = ValidateMin(influence, 1, nameof(influence));
            Resistance = ValidateMin(resistance, 1, nameof(resistance));
            Capacity = ValidateMin(capacity, 1, nameof(capacity));
            Refillable = refillable;
        }
        #endregion

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
            public int Mutability { get; set; }
            public int Influence { get; set; }
            public int Resistance { get; set; }
            public int Capacity { get; set; }
            public bool Refillable { get; set; }

            public PlantType Build()
            {
                return new PlantType(this);
            }
        }

        static int ValidateMin(int value, int min, string paramName)
        {
            return value >= min ? value : throw new ArgumentException(paramName);
        }
    }
}