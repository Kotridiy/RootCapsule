using RootCapsule.Core;
using RootCapsule.Core.Types;
using RootCapsule.Model.Fields;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityInput = UnityEngine.Input;

namespace RootCapsule.Control.SceneControl
{
    // developing: All player actions, control by player body
    class FieldsSceneController : SceneController
    {
        public Puppet Puppet;

        private void Update()
        {
            if (UnityInput.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        protected override void OnPrimaryPressed(Vector2 point)
        {
            Arable arable = GetArableOnPoint(point);
            if (arable != null && arable.AliveOnArable == null)
            {

                Puppet.GoToPosition(arable.transform.position);
                PlantSeed(arable);
            }
        }

        private static void PlantSeed(Arable arable)
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
            var seed = new Seed(type, new SeedStat.Builder().Build());
            arable.PlantSeed(seed);
        }

        Arable GetArableOnPoint(Vector2 point)
        {
            var ray = Camera.main.ScreenPointToRay(point);
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
