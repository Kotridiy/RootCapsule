using RootCapsule.Core;
using RootCapsule.Model.Fields;
using System.ComponentModel;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace RootCapsule.Control.SceneControl
{
    // developing: All player actions, control by player body
    class FieldsSceneController : SceneController
    {
        [SerializeField, EditorBrowsable(EditorBrowsableState.Always)] 

        protected override void OnPrimaryPressed()
        {
            Arable arable = GetArableOnMouse();
            if (arable != null && arable.AliveOnArable == null)
            {
                //TEST CODE
                PlantType type = new PlantType.Builder()
                {
                    Id = "Test",
                    GrowthTime = 10,
                    LifeTime = 10,
                    HarvestMin = 1,
                    HarvestMax = 1,
                    SeedsMin = 1,
                    SeedsMax = 1,
                    HarvestPrice = 1,
                    SeedPrice = 1,
                    Mutability = 1,
                    Influence = 1,
                    Resistance = 1,
                    Capacity = 1
                }.Build();
                var seed = new Seed(type, new SeedStat());
                arable.PlantSeed(seed);
            }
        }

        Arable GetArableOnMouse()
        {
            var ray = Camera.main.ScreenPointToRay(UnityInput.mousePosition);
            var hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                Arable arable = hit.transform.GetComponent<Arable>();
                if (arable != null) return arable;
            }
            return null;
        }
    }
}
