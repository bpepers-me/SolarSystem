using Improbable;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Assets.Gamelogic.Core;

namespace Assets.Gamelogic.Planets
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class PlanetSetup : MonoBehaviour
    {
        [Require]
        private PlanetIndex.Reader planetIndexReader;

        void OnEnable()
        {
            var planetData = PlanetInfo.GetData(planetIndexReader.Data.index);

            Material material = (Material)Resources.Load("Materials/" + planetData.name + "_mat");
            GetComponent<Renderer>().material = material;

            if (planetData.name == "Saturn")
            {
                var folder = new GameObject("Rings");
                folder.transform.parent = transform;
            }
        }
    }
}
