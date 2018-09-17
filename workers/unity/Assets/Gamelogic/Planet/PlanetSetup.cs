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

            Material material = (Material)Resources.Load("Materials/" + planetData.name);
            GetComponent<Renderer>().material = material;
        }
    }
}
