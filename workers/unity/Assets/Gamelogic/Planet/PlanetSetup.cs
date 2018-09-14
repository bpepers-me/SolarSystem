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
            float diameter = planetData.diameter * 12742000f / 1.496e11f * 10000f;
            transform.localScale = new Vector3(diameter, diameter, diameter);

            Material material = (Material)Resources.Load("Materials/" + planetData.name);
            GetComponent<Renderer>().material = material;
        }
    }
}
