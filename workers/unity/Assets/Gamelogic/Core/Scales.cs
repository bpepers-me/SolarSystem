﻿using UnityEngine;
using System.Collections;

public static class Scales
{
    public const double spatialFactor = 1000000.0;  // SpatialOS units are in millions of km's so divide by this to get those units
    public const double unityFactor = 1000.0;       // Unity units are in thousands of km's so divide by this to get those units

    public const double au2km = 149597870.7;        // convert AU to km
    public const double earthDiameter = 12742.0;    // in km

    public const float earthMass = 5.9722e24f;          // in kg
    public const float earthTilt = 7.155f;              // in degrees (to sun's equator)
    public const float earthRotationPeriod = 1f;        // in days
    public const float earthOrbitPeriod = 365.256f;     // in days

    public const float gravityColliderMult = 30;
    public const float messageDuration = 8f;
    public const float maxSpaceShipSpeed = 16.28f;
    public const float sunMass2EarthMass = 3.00347E-06f;
    public const float earthMass2SunMass = 1 / sunMass2EarthMass;
    public static float massScale = 16;
    public const float au2mu = 1000f;
    public const float mu2au = 1 / au2mu;
    public const float solarSystemEdge = 31f * au2mu;
    public const float y2tmu = 720f;
    public const float tmu2y = 1 / y2tmu;
    public const float y2sec = 31556926f;
    public static float GM = Mathf.Pow(2 * Mathf.PI / y2tmu, 2f) * Mathf.Pow(au2mu, 3f);
    private static bool pause = false;
    private static float TimaScaleFactor = 2.0f;
    private static float currentTimeScale = 1.0f;
    private const float minTimeScale = 0.125f;
    private const float maxTimeScale = 64f;
    private static float minCurrentTimeScale = 0.125f;
    private static float maxCurrentTimeScale = 64f;

    public static float MaxTimeScale
    {
        get { return maxCurrentTimeScale; }
        set { maxCurrentTimeScale = value; }
    }

    public static float MinTimeScale
    {
        get { return minCurrentTimeScale; }
        set { minCurrentTimeScale = value; }
    }

    public static float CurrentTimeScale
    {
        get
        {
            return currentTimeScale;
        }
        private set
        {
            currentTimeScale = value;
        }
    }

    public static bool Pause
    {        //Setting Time.timeScale to zero wouldn't work because it would also stop camera and other functions
        get { return pause; }
        set { pause = value; }
    }

    public static void ResetTimeScale()
    {
        CurrentTimeScale = 1.0f;
    }

    public static void IncreaseTimeScale()              //In order the change to take effect I use ClampTimeScale() in GUI update
    {
        CurrentTimeScale *= TimaScaleFactor;

        CurrentTimeScale = Mathf.Clamp(CurrentTimeScale, minCurrentTimeScale, maxCurrentTimeScale);
    }

    public static void DecreaseTimeScale()
    {
        CurrentTimeScale /= TimaScaleFactor;
        CurrentTimeScale = Mathf.Clamp(CurrentTimeScale, minCurrentTimeScale, maxCurrentTimeScale);
    }

    public static void ClampTimeScale()
    {
        CurrentTimeScale = Mathf.Clamp(CurrentTimeScale, minCurrentTimeScale, maxCurrentTimeScale);

        Time.timeScale = CurrentTimeScale;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    public static void ResetMaximumTimeScale()
    {
        maxCurrentTimeScale = maxTimeScale;
    }
}
