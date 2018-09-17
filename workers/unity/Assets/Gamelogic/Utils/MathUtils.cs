using Improbable;
using UnityEngine;

public static class MathUtils {

    public static UnityEngine.Vector3d FromImprobable(this Improbable.Vector3d vector)
    {
        return new UnityEngine.Vector3d(vector.x, vector.y, vector.z);
    }

    public static Improbable.Vector3d ToImprobable(this UnityEngine.Vector3d vector)
    {
        return new Improbable.Vector3d(vector.x, vector.y, vector.z);
    }

    public static Improbable.Vector3d ToImprobable(this UnityEngine.Vector3 vector)
    {
        return new Improbable.Vector3d(vector.x, vector.y, vector.z);
    }

    public static Improbable.Coordinates ToImprobableCoordinates(this UnityEngine.Vector3d vector)
    {
        return new Improbable.Coordinates(vector.x, vector.y, vector.z);
    }

    public static Improbable.Coordinates ToImprobableCoordinates(this UnityEngine.Vector3 vector)
    {
        return new Improbable.Coordinates(vector.x, vector.y, vector.z);
    }

    public static Quaternion FromImprobable(this Improbable.Core.Quaternion quaternion)
    {
        return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }

    public static Improbable.Core.Quaternion ToImprobable(this Quaternion quaternion)
    {
        return new Improbable.Core.Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
}
