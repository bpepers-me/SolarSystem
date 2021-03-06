﻿using Assets.Gamelogic.Core;
using Assets.Gamelogic.EntityTemplates;
using Improbable;
using Improbable.Worker;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
	public class SnapshotMenu : MonoBehaviour
	{
		[MenuItem("Improbable/Snapshots/Generate Default Snapshot")]
		private static void GenerateDefaultSnapshot()
		{
			var snapshotEntities = new Dictionary<EntityId, Entity>();
			var currentEntityId = 1;

			snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreatePlayerCreatorTemplate());

			// create the planets
            for (uint i = 0; i < 9; ++i)
            {
                snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreatePlanetTemplate(i));
            }

			// create the asteroid belt
            for (int i = 0; i < 1000; ++i)
            {
                float angle = Random.Range(-180f, 180f);

                // asteroids are in a belt that's from 2 to 3.2 AU from the sun
                double orbitRadius = Random.Range(2.0f, 3.2f) * Scales.au2km;

                // TODO: make this a distribution where most are near 1 km in size but a tiny few can be as large as 100 km
                double diameter = Random.Range(10000f, 60000f);

                snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateAsteroidTemplate(angle, orbitRadius, diameter));
            }

            SaveSnapshot(snapshotEntities);
		}

		private static void SaveSnapshot(IDictionary<EntityId, Entity> snapshotEntities)
		{
			File.Delete(SimulationSettings.DefaultSnapshotPath);
			using (SnapshotOutputStream stream = new SnapshotOutputStream(SimulationSettings.DefaultSnapshotPath))
			{
				foreach (var kvp in snapshotEntities)
				{
					var error = stream.WriteEntity(kvp.Key, kvp.Value);
					if (error.HasValue)
					{
						Debug.LogErrorFormat("Failed to generate initial world snapshot: {0}", error.Value);
						return;
					}
				}
			}

			Debug.LogFormat("Successfully generated initial world snapshot at {0}", SimulationSettings.DefaultSnapshotPath);
		}
	}
}
