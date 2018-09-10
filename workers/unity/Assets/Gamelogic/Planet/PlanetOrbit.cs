using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public float eccentricity;
    public float pericenter;
    public float orbitalPeriod;
    public float radius;
    public float axialTilt;
    public float rotationPeriod;
    public float longOfAscendingNode;

    public float time;

    private float[] cosSinOmega = new float[2];
    private const int N = 300;
    private float[] angleArray = new float[N];
    private float surface;
    private float k;
    private float orbitDt;

    public float OrbitalPeriod()
    {
        return orbitalPeriod * Scales.y2tmu;
    }

    public float RotationPeriod()
    {
        return rotationPeriod * Scales.y2tmu;
    }

	void Start ()
    {
        surface = Mathf.Sqrt(-(1 + eccentricity) / Mathf.Pow(-1 + eccentricity, 3)) * Mathf.PI * pericenter * pericenter;
        k = 2 * surface / (Mathf.Pow(1 + eccentricity, 2) * OrbitalPeriod() * pericenter * pericenter);
        orbitDt = OrbitalPeriod() / (2 * (N - 1));

        ThetaRunge();
        time = Random.Range(0, OrbitalPeriod());

        cosSinOmega[0] = Mathf.Cos(longOfAscendingNode);
        cosSinOmega[1] = Mathf.Sin(longOfAscendingNode);
	}
	
	void FixedUpdate ()
    {
        time += Time.fixedDeltaTime;
        transform.localPosition = ParametricOrbit(ThetaInt(time));
    }

    public Vector3 ParametricOrbit(float th)
    {
        float Cost = Mathf.Cos(th);
        float Sint = Mathf.Sin(th);

        float x = (pericenter * (1 + eccentricity)) / (1 + eccentricity * Cost) * Cost;
        float z = (pericenter * (1 + eccentricity)) / (1 + eccentricity * Cost) * Sint;

        float xp = cosSinOmega[0] * x - cosSinOmega[1] * z;
        float yp = cosSinOmega[1] * x + cosSinOmega[0] * z;

        return new Vector3(xp, 0f, yp);
    }

    private float dthdt(float th)
    {
        return k * Mathf.Pow((1 + eccentricity * Mathf.Cos(th)), 2);
    }

    private void ThetaRunge()
    {
        float w = 0, k1, k2, k3, k4;
        for (int i = 0; i < N - 2; i++)
        {
            k1 = orbitDt * dthdt(w);
            k2 = orbitDt * dthdt(w + k1 / 2);
            k3 = orbitDt * dthdt(w + k2 / 2);
            k4 = orbitDt * dthdt(w + k3);
            w = w + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
            angleArray[i + 1] = w;
        }
        angleArray[N - 1] = Mathf.PI;
    }

    public float ThetaInt(float t)
    {
        float theta0 = 0;
        t = t % OrbitalPeriod();

        if (t <= OrbitalPeriod() / 2)
        {
            float i = t / orbitDt;
            float i0 = Mathf.Clamp(Mathf.Floor(i), 0, N - 1);
            float i1 = Mathf.Clamp(Mathf.Ceil(i), 0, N - 1);

            if (i0 == i1)
            {
                theta0 = angleArray[(int)i0];
            }
            else
            {
                theta0 = (angleArray[(int)i0] - angleArray[(int)i1]) / (i0 - i1) * i + (i0 * angleArray[(int)i1] - angleArray[(int)i0] * i1) / (i0 - i1);
            }
            return theta0;
        }
        else
        {
            t = -t + OrbitalPeriod();
            float i = t / orbitDt;
            float i0 = Mathf.Clamp(Mathf.Floor(i), 0, N - 1);
            float i1 = Mathf.Clamp(Mathf.Ceil(i), 0, N - 1);

            if (i0 == i1)
            {
                theta0 = -angleArray[(int)i0] + 2 * Mathf.PI;
            }
            else
            {
                theta0 = -((angleArray[(int)i0] - angleArray[(int)i1]) / (i0 - i1) * i + (i0 * angleArray[(int)i1] - angleArray[(int)i0] * i1) / (i0 - i1)) + 2 * Mathf.PI;
            }
            return theta0;
        }
    }
}
