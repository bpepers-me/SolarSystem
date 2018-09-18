using System.Collections;
using System.Collections.Generic;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

// Add this MonoBehaviour on client workers only
[WorkerType(WorkerPlatform.UnityClient)]
public class FloatingOrigin : MonoBehaviour
{
    public static Vector3d offset;

    void OnEnable()
    {
        offset = Vector3d.zero;
    }

	void Update ()
    {
        Vector3 currentPosition = transform.position;

        float distance = currentPosition.magnitude;
        if (distance > 10000f)
        {
            var sun = GameObject.Find("Sun");
            sun.transform.localPosition -= currentPosition;

            offset += new Vector3d(currentPosition.x, currentPosition.y, currentPosition.z) * Scales.unityFactor;
            transform.position = Vector3.zero;
        }
	}
}
