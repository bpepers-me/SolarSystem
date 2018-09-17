using Improbable;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using Improbable.Worker;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public class TransformReceiver : MonoBehaviour
    {
        [Require] private TransformInfo.Reader transformReader;

        void OnEnable()
        {
            var position = transformReader.Data.position.FromImprobable();
			var rotation = transformReader.Data.rotation.FromImprobable();
			var scale = transformReader.Data.scale.FromImprobable();

			var unityPosition = (Vector3)(position / Scales.unityFactor);
			var unityScale = (Vector3)(scale / Scales.unityFactor);

			transform.localPosition = unityPosition;
            transform.localRotation = rotation;
			transform.localScale = unityScale;

            transformReader.ComponentUpdated.Add(OnTransformUpdated);
        }

        void OnDisable()
        {
            transformReader.ComponentUpdated.Remove(OnTransformUpdated);
        }

        void OnTransformUpdated(TransformInfo.Update update)
        {
            if (transformReader.Authority == Authority.NotAuthoritative)
            {
                if (update.position.HasValue)
                {
					var position = update.position.Value.FromImprobable();
					var unityPosition = (Vector3)(position / Scales.unityFactor);
                    transform.localPosition = unityPosition;
                }
                if (update.rotation.HasValue)
                {
                    transform.localRotation = update.rotation.Value.FromImprobable();
                }
                if (update.scale.HasValue)
                {
					var scale = update.scale.Value.FromImprobable();
					var unityScale = (Vector3)(scale / Scales.unityFactor);
                    transform.localScale = unityScale;
                }
            }
        }
    }
}