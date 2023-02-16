using Newtonsoft.Json;
using System;

namespace RootCapsule.Core.Types
{
    // developing: mutation, crossing sum
    [Serializable]
    public class SeedStat
    {
        public StatMultiplier GrowthSpeed { get; }
        public StatMultiplier Vitality { get; }
        public StatMultiplier Productivity { get; }
        public StatMultiplier Resistance { get; }
        public StatMultiplier MutationPower { get; }
        public StatMultiplier Reprodaction { get; }
        public StatMultiplier Capacious { get; }

        #region Constructors
        public SeedStat(Builder builder)
        {
            GrowthSpeed = builder.GrowthSpeed;
            Vitality = builder.Vitality;
            Productivity = builder.Productivity;
            Resistance = builder.Resistance;
            MutationPower = builder.MutationPower;
            Reprodaction = builder.Reprodaction;
            Capacious = builder.Capacious;
        }

        [JsonConstructor]
        public SeedStat(StatMultiplier growthSpeed, StatMultiplier vitality, StatMultiplier productivity, StatMultiplier resistance, StatMultiplier mutationPower, StatMultiplier reprodaction, StatMultiplier capacious)
        {
            GrowthSpeed = growthSpeed;
            Vitality = vitality;
            Productivity = productivity;
            Resistance = resistance;
            MutationPower = mutationPower;
            Reprodaction = reprodaction;
            Capacious = capacious;
        }
        #endregion

        public int CulcGrowthTime(PlantType type)
        {
            return (int)Math.Floor(type.GrowthTime * GrowthSpeed.GetMultiplier(-1));
        }

        public int CulcLifeTime(PlantType type)
        {
            return (int)Math.Floor(type.LifeTime * Vitality.GetMultiplier());
        }

        public int CulcHarvestMin(PlantType type)
        {
            return (int)Math.Floor(type.HarvestMin * Productivity.GetMultiplier());
        }

        public int CulcHarvestMax(PlantType type)
        {
            return (int)Math.Floor(type.HarvestMax * Productivity.GetMultiplier());
        }

        public int CulcSeedsMin(PlantType type)
        {
            return (int)Math.Floor(type.SeedsMin * Reprodaction.GetMultiplier());
        }

        public int CulcResistance(PlantType type)
        {
            return (int)Math.Floor(type.Resistance * Resistance.GetMultiplier());
        }

        public int CulcMutability(PlantType type)
        {
            return (int)Math.Floor(type.Mutability * MutationPower.GetMultiplier());
        }

        public int CulcSeedsMax(PlantType type)
        {
            return (int)Math.Floor(type.SeedsMax * Reprodaction.GetMultiplier());
        }

        public int CulcInfluence(PlantType type)
        {
            return (int)Math.Floor(type.Influence * GrowthSpeed.GetMultiplier());
        }

        public int CulcCapacity(PlantType type)
        {
            return (int)Math.Floor(type.Capacity * Capacious.GetMultiplier());
        }

        public class Builder
        {
            public StatMultiplier GrowthSpeed { get; set; }
            public StatMultiplier Vitality { get; set; }
            public StatMultiplier Productivity { get; set; }
            public StatMultiplier Resistance { get; set; }
            public StatMultiplier MutationPower { get; set; }
            public StatMultiplier Reprodaction { get; set; }
            public StatMultiplier Capacious { get; set; }

            public SeedStat Build()
            {
                return new SeedStat(this);
            }
        }
    }


    public struct StatMultiplier
    {
        public const float GREAT_REDUCER = 0.7f;
        public const float SMALL_REDUCER = 8.5f;
        public const float SMALL_MAGNIFIER = 1.25f;
        public const float GREAT_MAGNIFIER = 1.5f;

        public float Value { get; }

        public float GetMultiplier(int sign = 1)
        {
            if (sign != 1 && sign != -1) throw new ArgumentException(nameof(sign) + " must have only '-1' or '1' value!");

            var mul = (int)Math.Round(Value);
            mul = (mul - 3) * sign;

            switch (mul)
            {
                case -3:
                case -2:
                    return GREAT_REDUCER;
                case -1:
                    return SMALL_REDUCER;
                case 0:
                    return 1;
                case 1:
                    return SMALL_MAGNIFIER;
                case 2:
                case 3:
                    return GREAT_MAGNIFIER;

                default:
                    throw new Exception("Multiplier has wrong value! Value = " + Value);
            }
        }

        public StatMultiplier(float value = 3f)
        {
            Value = (value < 0.5f) ? 0.5f : ((value > 5.4f) ? 5.4f : value);
        }
    }
}