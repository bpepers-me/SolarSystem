using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public struct PlanetData
    {
        public string name;

        public float x;                 // heliocentric coordinates (in AU) with the z plane defined as the earth orbit
        public float y;                 // around the sun which means earth is always at a z of zero.  values taken at
        public float z;                 // Jan 1, 2018 00:00:00 using http://cosinekitty.com/solar_system.html

        public float mass;              // relative to earth (from https://nssdc.gsfc.nasa.gov/planetary/factsheet/planet_table_ratio.html)
        public float diameter;          // relative to earth
        public float rotationPeriod;    // relative to earth
        public float orbitalPeriod;     // relative to earth

        public float distanceFromSun()
        {
            return Mathf.Sqrt(x * x + y * y);
        }
    }

    public static class PlanetInfo
    {
        public static PlanetData GetData(uint planetIndex)
        {
            PlanetData data = new PlanetData();

            // mercury
            if (planetIndex == 0)
            {
                data.name = "Mercury";
                data.x = -0.3890948f;
                data.y = -0.0173439f;
                data.z = 0.0344211f;
                data.mass = 0.0553f;
                data.diameter = 0.383f;
                data.rotationPeriod = 58.8f;
                data.orbitalPeriod = .241f;
            }

            // venus
            if (planetIndex == 1)
            {
                data.name = "Venus";
                data.x = 0.0801084f;
                data.y = -0.7227354f;
                data.z = -0.0143865f;
                data.mass = 0.815f;
                data.diameter = 0.949f;
                data.rotationPeriod = -244.0f;
                data.orbitalPeriod = .615f;
            }

            // earth
            if (planetIndex == 2)
            {
                data.name = "Earth";
                data.x = -0.1844608f;
                data.y = 0.9658580f;
                data.z = 0.0000000f;
                data.mass = 1.0f;
                data.diameter = 1.0f;
                data.rotationPeriod = 1.0f;
                data.orbitalPeriod = 1.0f;
            }

            // mars
            if (planetIndex == 3)
            {
                data.name = "Mars";
                data.x = -1.5808145f;
                data.y = -0.3995236f;
                data.z = 0.0305856f;
                data.mass = 0.107f;
                data.diameter = 0.532f;
                data.rotationPeriod = 1.025957f;
                data.orbitalPeriod = 1.88f;
            }

            // jupiter
            if (planetIndex == 4)
            {
                data.name = "Jupiter";
                data.x = -4.2561706f;
                data.y = -3.3723786f;
                data.z = 0.1093285f;
                data.mass = 317.8f;
                data.diameter = 11.21f;
                data.rotationPeriod = .415f;
                data.orbitalPeriod = 11.9f;
            }

            // saturn
            if (planetIndex == 5)
            {
                data.name = "Saturn";
                data.x = 0.0898178f;
                data.y = -10.0823233f;
                data.z = 0.1718396f;
                data.mass = 95.2f;
                data.diameter = 9.45f;
                data.rotationPeriod = .445f;
                data.orbitalPeriod = 29.4f;
            }

            // uranus
            if (planetIndex == 6)
            {
                data.name = "Uranus";
                data.x = 17.7211786f;
                data.y = 9.0654912f;
                data.z = -0.1960038f;
                data.mass = 14.5f;
                data.diameter = 4.01f;
                data.rotationPeriod = -.720f;
                data.orbitalPeriod = 83.7f;
            }

            // neptune
            if (planetIndex == 7)
            {
                data.name = "Neptune";
                data.x = 28.7078888f;
                data.y = -8.4582224f;
                data.z = -0.4842037f;
                data.mass = 17.1f;
                data.diameter = 3.88f;
                data.rotationPeriod = .673f;
                data.orbitalPeriod = 163.7f;
            }

            // pluto
            if (planetIndex == 8)
            {
                data.name = "Pluto";
                data.x = 10.8929283f;
                data.y = -31.5782334f;
                data.z = 0.2721556f;
                data.mass = 0.0025f;
                data.diameter = 0.186f;
                data.rotationPeriod = 6.41f;
                data.orbitalPeriod = 247.9f;
            }

            return data;
        }
    }
}
