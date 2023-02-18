using RootCapsule.Core.Types;
using RootCapsule.ModelData.Fields;
using System;

namespace RootCapsule.Model.Fields
{
    //developing: save\load
    public class Fertilizer: ISerializableObject<FertilizerData>
    {
        public float GrowthModifier { get; }
        public float VitalityModifier { get; }
        public float ProductivityModifier { get; }
        public float ResistModifier { get; }

        int uses;

        public event Action FertilizerOver;

        public Fertilizer(int uses, float growthModifier = 1f, float vitalityModifier = 1f, float productivityModifier = 1f, float resistModifier = 1f)
        {
            GrowthModifier = growthModifier;
            VitalityModifier = vitalityModifier;
            ProductivityModifier = productivityModifier;
            ResistModifier = resistModifier;

            this.uses = uses;
        }

        public Fertilizer(FertilizerData data)
        {
            DeserializeState(data);
        }

        public void Use()
        {
            uses--;

            if (uses < 0) throw new InvalidOperationException(uses + " uses left! Destroy " + typeof(Fertilizer).ToString());

            if (uses == 0) FertilizerOver?.Invoke();
        }

        public FertilizerData SerializeState()
        {
            return new FertilizerData();
        }

        public void DeserializeState(FertilizerData data)
        {
            this.uses = 5;
        }
    }
}