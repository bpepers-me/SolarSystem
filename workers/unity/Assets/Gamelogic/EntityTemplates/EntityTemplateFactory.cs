using Assets.Gamelogic.Core;
using Improbable;
using Improbable.Core;
using Improbable.Player;
using Improbable.Unity.Core.Acls;
using Improbable.Worker;
using Quaternion = UnityEngine.Quaternion;
using Vector3d = UnityEngine.Vector3d;
using UnityEngine;
using Improbable.Unity.Entity;
using Improbable.Ship;
using Improbable.Collections;

namespace Assets.Gamelogic.EntityTemplates
{
    public class EntityTemplateFactory : MonoBehaviour
    {
        public static Entity CreatePlayerCreatorTemplate()
        {
            var playerCreatorEntityTemplate = EntityBuilder.Begin()
                .AddPositionComponent(Improbable.Coordinates.ZERO.ToUnityVector(), CommonRequirementSets.PhysicsOnly)
                .AddMetadataComponent(entityType: SimulationSettings.PlayerCreatorPrefabName)
                .SetPersistence(true)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .AddComponent(new PlayerCreation.Data(), CommonRequirementSets.PhysicsOnly)
                .AddComponent(new ClientEntityStore.Data(new Map<string, EntityId>()), CommonRequirementSets.PhysicsOnly)
                .Build();

            return playerCreatorEntityTemplate;
        }

        public static Entity CreatePlayerTemplate(string clientId, EntityId playerCreatorId, Vector3d position)
        {
			var rotation = Quaternion.identity;
			var scale = new Vector3d(1.0, 1.0, 1.0);
            var spatialPosition = position / Scales.spatialFactor;

            var playerTemplate = EntityBuilder.Begin()
                .AddPositionComponent((Vector3)spatialPosition, CommonRequirementSets.SpecificClientOnly(clientId))
                .AddMetadataComponent(entityType: SimulationSettings.PlayerPrefabName)
                .SetPersistence(false)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .AddComponent(new TransformInfo.Data(position.ToImprobable(), rotation.ToImprobable(), scale.ToImprobable()), CommonRequirementSets.SpecificClientOnly(clientId))
                .AddComponent(new ShipControls.Data(0, 0), CommonRequirementSets.SpecificClientOnly(clientId))
                .AddComponent(new ClientAuthorityCheck.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
                .AddComponent(new ClientConnection.Data(SimulationSettings.TotalHeartbeatsBeforeTimeout, clientId, playerCreatorId), CommonRequirementSets.PhysicsOnly)
                .Build();

            return playerTemplate;
        }

        public static Entity CreatePlanetTemplate(uint planetIndex)
        {
            PlanetData data = PlanetInfo.GetData(planetIndex);
			
			float initialAngle = Random.Range(0f, 360f); // TODO: calculate this from position
            double orbitRadius = data.distanceFromSun() * Scales.au2km;
			float orbitPeriod = data.orbitalPeriod * 365.25f;
			float rotationPeriod = data.rotationPeriod;
            double diameter = data.diameter * Scales.earthDiameter;

			var position = new Vector3d(0, 0, 0);
			var rotation = Quaternion.identity;
			var scale = new Vector3d(diameter, diameter, diameter);
            var spatialPosition = position / Scales.spatialFactor;

            var planetTemplate = EntityBuilder.Begin()
                .AddPositionComponent((Vector3)spatialPosition, CommonRequirementSets.PhysicsOnly)
                .AddMetadataComponent(entityType: SimulationSettings.PlanetPrefabName)
                .SetPersistence(true)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .AddComponent(new TransformInfo.Data(position.ToImprobable(), rotation.ToImprobable(), scale.ToImprobable()), CommonRequirementSets.PhysicsOnly)
                .AddComponent(new OrbitInfo.Data(initialAngle, orbitRadius, orbitPeriod, rotationPeriod), CommonRequirementSets.PhysicsOnly)
                .AddComponent(new PlanetIndex.Data(planetIndex), CommonRequirementSets.PhysicsOnly)
                .Build();

            return planetTemplate;
        }

        public static Entity CreateAsteroidTemplate(float initialAngle, double orbitRadius, double diameter)
        {
			float orbitPeriod = 365.25f; // TODO: pick a good orbit period for the asteroid belt
			float rotationPeriod = 1f / 24f / 60f; // TODO: have different asteroids rotate at different speeds

            double x = Mathf.Sin(Mathf.Deg2Rad * initialAngle) * orbitRadius;
            double y = Random.Range(-200f, 200f);
            double z = Mathf.Cos(Mathf.Deg2Rad * initialAngle) * orbitRadius;

			var position = new Vector3d(x, y, z);
			var rotation = Quaternion.identity;
			var scale = new Vector3d(diameter, diameter, diameter);
            var spatialPosition = position / Scales.spatialFactor;

			var asteroidTemplate = EntityBuilder.Begin()
                .AddPositionComponent((Vector3)spatialPosition, CommonRequirementSets.PhysicsOnly)
                .AddMetadataComponent(entityType: SimulationSettings.AsteroidPrefabName)
                .SetPersistence(true)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .AddComponent(new TransformInfo.Data(position.ToImprobable(), rotation.ToImprobable(), scale.ToImprobable()), CommonRequirementSets.PhysicsOnly)
                .AddComponent(new OrbitInfo.Data(initialAngle, orbitRadius, orbitPeriod, rotationPeriod), CommonRequirementSets.PhysicsOnly)
                .Build();

            Debug.Log(orbitRadius.ToString() + " " + spatialPosition.ToString());
            return asteroidTemplate;
        }
    }
}
