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
        private Plant plantPrefab;

        protected override void OnPrimaryPressed()
        {
            Arable arable = GetArableOnMouse();
            if (arable != null && arable.AliveOnArable == null)
            {
                //TEST CODE
                var seed = new Seed(new PlantType(), new SeedStat());
                PlantPlant(arable, seed);
            }
        }

        private Arable GetArableOnMouse()
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

        private void PlantPlant(Arable arable, Seed seed)
        {
            var newPlant = Instantiate(plantPrefab, arable.transform);
            newPlant.Initialize(seed.PlantType, seed.SeedStat, arable.Fertilizer);
            arable.AliveOnArable = newPlant;
        }
    }
}
