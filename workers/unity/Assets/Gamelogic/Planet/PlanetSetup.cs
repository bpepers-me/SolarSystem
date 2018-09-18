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
            var index = planetIndexReader.Data.index;
            var planetData = PlanetInfo.GetData(index);

            Material material = (Material)Resources.Load("Materials/" + planetData.name + "_mat");
            GetComponent<Renderer>().material = material;

            if (planetData.name == "Saturn")
            {
                Material ringMaterial = (Material)Resources.Load("Materials/Saturn_ring_mat");
                transform.GetChild(0).GetComponent<Renderer>().material = ringMaterial;
            }

            if (planetData.name == "Uranus")
            {
                Material ringMaterial = (Material)Resources.Load("Materials/Uranus_ring_mat");
                transform.GetChild(0).GetComponent<Renderer>().material = ringMaterial;
            }
        }
    }
}
