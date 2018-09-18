using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace UnityEngine
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaterniond : IEnumerable<double>, IEquatable<Quaterniond>
    {
        #region Fields
        
        /// <summary>
        /// x-component
        /// </summary>
        public double x;
        
        /// <summary>
        /// y-component
        /// </summary>
        public double y;
        
        /// <summary>
        /// z-component
        /// </summary>
        public double z;
        
        /// <summary>
        /// w-component
        /// </summary>
        public double w;

        #endregion


        #region Constructors
        
        /// <summary>
        /// Component-wise constructor
        /// </summary>
        public Quaterniond(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        
        /// <summary>
        /// all-same-value constructor
        /// </summary>
        public Quaterniond(double v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
            this.w = v;
        }
        
        /// <summary>
        /// copy constructor
        /// </summary>
        public Quaterniond(Quaterniond q)
        {
            this.x = q.x;
            this.y = q.y;
            this.z = q.z;
            this.w = q.w;
        }
        
        /// <summary>
        /// vector-and-scalar constructor (CAUTION: not angle-axis, use FromAngleAxis instead)
        /// </summary>
        public Quaterniond(Vector3d v, double s)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = s;
        }
        
        /// <summary>
        /// Create a quaternion from two normalized axis (http://lolengine.net/blog/2013/09/18/beautiful-maths-quaternion-from-vectors)
        /// </summary>
        public Quaterniond(Vector3d u, Vector3d v)
        {
            var localW = Vector3d.Cross(u, v);
            var dot = Vector3d.Dot(u, v);
            var q = new Quaterniond(localW.x, localW.y, localW.z, 1.0 + dot).Normalized();
            this.x = q.x;
            this.y = q.y;
            this.z = q.z;
            this.w = q.w;
        }

        /// <summary>
        /// Create a quaternion from euler angles
        /// </summary>
        public Quaterniond(Vector3d eulerAngle)
        {
            var c = new Vector3d(Math.Cos(eulerAngle.x / 2), Math.Cos(eulerAngle.y / 2), Math.Cos(eulerAngle.z / 2));
            var s = new Vector3d(Math.Sin(eulerAngle.x / 2), Math.Sin(eulerAngle.y / 2), Math.Sin(eulerAngle.z / 2));
            this.x = s.x * c.y * c.z - c.x * s.y * s.z;
            this.y = c.x * s.y * c.z + s.x * c.y * s.z;
            this.z = c.x * c.y * s.z - s.x * s.y * c.z;
            this.w = c.x * c.y * c.z + s.x * s.y * s.z;
        }
 
        public static Quaterniond FromEulerAngles(Vector3d rotation)
		{
			Quaterniond xRotation = FromAxisAngle(new Vector3d(1, 0, 0), rotation.x);
			Quaterniond yRotation = FromAxisAngle(new Vector3d(0, 1, 0), rotation.y);
			Quaterniond zRotation = FromAxisAngle(new Vector3d(0, 0, 1), rotation.z);

			//return xRotation * yRotation * zRotation;
			return zRotation * yRotation * xRotation;
		}

		/// <summary>
		/// Build a Quaternion from the given axis and angle
		/// </summary>
		/// <param name="axis">The axis to rotate about</param>
		/// <param name="angle">The rotation angle in radians</param>
		/// <returns></returns>
		public static Quaterniond FromAxisAngle(Vector3d axis, double angle)
		{
			if (axis.sqrMagnitude == 0.0)
			{
				return Identity;
			}

            Quaterniond result = Identity;

			angle *= 0.5f;
			axis.Normalize();
            result.x = axis.x * Math.Sin(angle);
            result.y = axis.y * Math.Sin(angle);
            result.z = axis.z * Math.Sin(angle);
            result.w = (double)System.Math.Cos(angle);

			return result.Normalized();
		}
        
        #endregion


        #region Explicit Operators

        /// <summary>
        /// Explicitly converts this to a Quaternion
        /// </summary>
        public static explicit operator Quaternion(Quaterniond v)
        {
            return new Quaternion((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        }

        #endregion

        #region Indexer
        
        /// <summary>
        /// Gets/Sets a specific indexed component (a bit slower than direct access).
        /// </summary>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        #endregion


        #region Properties
        
        /// <summary>
        /// Returns an array with all values
        /// </summary>
        public double[] Values()
        {
            return new[] { x, y, z, w };
        }
        
        /// <summary>
        /// Returns the number of components (4).
        /// </summary>
        public int Count()
        {
            return 4;
        }
        
        /// <summary>
        /// Returns the euclidean length of this quaternion.
        /// </summary>
        public double Length()
        {
            return Math.Sqrt((x*x + y*y) + (z*z + w*w));
        }
        
        /// <summary>
        /// Returns the squared euclidean length of this quaternion.
        /// </summary>
        public double LengthSqr()
        {
            return (x*x + y*y) + (z*z + w*w);
        }
        
        /// <summary>
        /// Returns a copy of this quaternion with length one (undefined if this has zero length).
        /// </summary>
        public Quaterniond Normalized()
        {
            return this / (double)Length();
        }
        
        /// <summary>
        /// Returns a copy of this quaternion with length one (returns zero if length is zero).
        /// </summary>
        public Quaterniond NormalizedSafe()
        {
            return this == Zero ? Identity : this / (double)Length();
        }
        
        /// <summary>
        /// Returns the represented angle of this quaternion.
        /// </summary>
        public double Angle()
        {
            return Math.Acos((double)w) * 2.0;
        }
        
        /// <summary>
        /// Returns the represented axis of this quaternion.
        /// </summary>
        public Vector3d Axis
        {
            get
            {
                var s1 = 1 - w * w;
                if (s1 < 0) return new Vector3d(0, 0, 1);
                var s2 = 1 / Math.Sqrt(s1);
                return new Vector3d(x * s2, y * s2, z * s2);
            }
        }
        
        /// <summary>
        /// Returns the represented yaw angle of this quaternion.
        /// </summary>
        public double Yaw()
        {
            return Math.Asin(-2.0 * (x * z - w * y));
        }
        
        /// <summary>
        /// Returns the represented pitch angle of this quaternion.
        /// </summary>
        public double Pitch()
        {
            return Math.Atan2(2.0 * (y * z + w * x), (w * w - x * x - y * y + z * z));
        }
        
        /// <summary>
        /// Returns the represented roll angle of this quaternion.
        /// </summary>
        public double Roll()
        {
            return Math.Atan2(2.0 * (x * y + w * z), (w * w + x * x - y * y - z * z));
        }
        
        /// <summary>
        /// Returns the represented euler angles (pitch, yaw, roll) of this quaternion.
        /// </summary>
        public Vector3d EulerAngles()
        {
            return new Vector3d(Pitch(), Yaw(), Roll());
        }

        /// <summary>
        /// Returns the conjugated quaternion
        /// </summary>
        public Quaterniond Conjugate()
        {
            return new Quaterniond(-x, -y, -z, w);
        }
        
        /// <summary>
        /// Returns the inverse quaternion
        /// </summary>
        public Quaterniond Inverse()
        {
            return Conjugate() / LengthSqr();
        }

        #endregion


        #region Static Properties
        
        /// <summary>
        /// Predefined all-zero quaternion
        /// </summary>
        public static Quaterniond Zero = new Quaterniond(0.0, 0.0, 0.0, 0.0);
        
        /// <summary>
        /// Predefined all-ones quaternion
        /// </summary>
        public static Quaterniond Ones = new Quaterniond(1.0, 1.0, 1.0, 1.0);
        
        /// <summary>
        /// Predefined identity quaternion
        /// </summary>
        public static Quaterniond Identity = new Quaterniond(0.0, 0.0, 0.0, 1.0);
        
        /// <summary>
        /// Predefined unit-X quaternion
        /// </summary>
        public static Quaterniond UnitX = new Quaterniond(1.0, 0.0, 0.0, 0.0);
        
        /// <summary>
        /// Predefined unit-Y quaternion
        /// </summary>
        public static Quaterniond UnitY = new Quaterniond(0.0, 1.0, 0.0, 0.0);
        
        /// <summary>
        /// Predefined unit-Z quaternion
        /// </summary>
        public static Quaterniond UnitZ = new Quaterniond(0.0, 0.0, 1.0, 0.0);
        
        /// <summary>
        /// Predefined unit-W quaternion
        /// </summary>
        public static Quaterniond UnitW = new Quaterniond(0.0, 0.0, 0.0, 1.0);
        
        /// <summary>
        /// Predefined all-MaxValue quaternion
        /// </summary>
        public static Quaterniond MaxValue = new Quaterniond(double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue);
        
        /// <summary>
        /// Predefined all-MinValue quaternion
        /// </summary>
        public static Quaterniond MinValue = new Quaterniond(double.MinValue, double.MinValue, double.MinValue, double.MinValue);
        
        /// <summary>
        /// Predefined all-Epsilon quaternion
        /// </summary>
        public static Quaterniond Epsilon = new Quaterniond(double.Epsilon, double.Epsilon, double.Epsilon, double.Epsilon);
        
        /// <summary>
        /// Predefined all-NaN quaternion
        /// </summary>
        public static Quaterniond NaN = new Quaterniond(double.NaN, double.NaN, double.NaN, double.NaN);
        
        /// <summary>
        /// Predefined all-NegativeInfinity quaternion
        /// </summary>
        public static Quaterniond NegativeInfinity = new Quaterniond(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        
        /// <summary>
        /// Predefined all-PositiveInfinity quaternion
        /// </summary>
        public static Quaterniond PositiveInfinity = new Quaterniond(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

        #endregion


        #region Operators
        
        /// <summary>
        /// Returns true iff this equals rhs component-wise.
        /// </summary>
        public static bool operator==(Quaterniond lhs, Quaterniond rhs)
        {
            return lhs.Equals(rhs);
        }
        
        /// <summary>
        /// Returns true iff this does not equal rhs (component-wise).
        /// </summary>
        public static bool operator!=(Quaterniond lhs, Quaterniond rhs)
        {
            return !lhs.Equals(rhs);
        }
        
        /// <summary>
        /// Returns proper multiplication of two quaternions.
        /// </summary>
        public static Quaterniond operator*(Quaterniond p, Quaterniond q)
        {
            return new Quaterniond(p.w * q.x + p.x * q.w + p.y * q.z - p.z * q.y, p.w * q.y + p.y * q.w + p.z * q.x - p.x * q.z, p.w * q.z + p.z * q.w + p.x * q.y - p.y * q.x, p.w * q.w - p.x * q.x - p.y * q.y - p.z * q.z);
        }
        
        /// <summary>
        /// Returns a vector rotated by the quaternion.
        /// </summary>
        public static Vector3d operator*(Quaterniond q, Vector3d v)
        {
            var qv = new Vector3d(q.x, q.y, q.z);
            var uv = Vector3d.Cross(qv, v);
            var uuv = Vector3d.Cross(qv, uv);
            return v + ((uv * q.w) + uuv) * 2;
        }
        
        /// <summary>
        /// Returns a vector rotated by the inverted quaternion.
        /// </summary>
        public static Vector3d operator*(Vector3d v, Quaterniond q)
        {
            return q.Inverse() * v;
        }
        
        #endregion


        #region Functions
        
        /// <summary>
        /// Returns an enumerator that iterates through all components.
        /// </summary>
        public IEnumerator<double> GetEnumerator()
        {
            yield return x;
            yield return y;
            yield return z;
            yield return w;
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through all components.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        /// <summary>
        /// Returns a string representation of this quaternion using ', ' as a seperator.
        /// </summary>
        public override string ToString()
        {
            return ToString(", ");
        }
        
        /// <summary>
        /// Returns a string representation of this quaternion using a provided seperator.
        /// </summary>
        public string ToString(string sep)
        {
            return ((x + sep + y) + sep + (z + sep + w));
        }
        
        /// <summary>
        /// Returns a string representation of this quaternion using a provided seperator and a format provider for each component.
        /// </summary>
        public string ToString(string sep, IFormatProvider provider)
        {
            return ((x.ToString(provider) + sep + y.ToString(provider)) + sep + (z.ToString(provider) + sep + w.ToString(provider)));
        }
        
        /// <summary>
        /// Returns a string representation of this quaternion using a provided seperator and a format for each component.
        /// </summary>
        public string ToString(string sep, string format)
        {
            return ((x.ToString(format) + sep + y.ToString(format)) + sep + (z.ToString(format) + sep + w.ToString(format)));
        }
        
        /// <summary>
        /// Returns a string representation of this quaternion using a provided seperator and a format and format provider for each component.
        /// </summary>
        public string ToString(string sep, string format, IFormatProvider provider)
        {
            return ((x.ToString(format, provider) + sep + y.ToString(format, provider)) + sep + (z.ToString(format, provider) + sep + w.ToString(format, provider)));
        }
        
        /// <summary>
        /// Returns true iff this equals rhs component-wise.
        /// </summary>
        public bool Equals(Quaterniond rhs)
        {
            return ((x.Equals(rhs.x) && y.Equals(rhs.y)) && (z.Equals(rhs.z) && w.Equals(rhs.w)));
        }
        
        /// <summary>
        /// Returns true iff this equals rhs type- and component-wise.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Quaterniond && Equals((Quaterniond) obj);
        }
        
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((((((x.GetHashCode()) * 397) ^ y.GetHashCode()) * 397) ^ z.GetHashCode()) * 397) ^ w.GetHashCode();
            }
        }
        
        /// <summary>
        /// Rotates this quaternion from an axis and an angle (in radians).
        /// </summary>
        public Quaterniond Rotated(double angle, Vector3d v)
        {
            return this * FromAxisAngle(angle, v);
        }

        #endregion


        #region Static Functions
        
        /// <summary>
        /// Converts the string representation of the quaternion into a quaternion representation (using ', ' as a separator).
        /// </summary>
        public static Quaterniond Parse(string s)
        {
            return Parse(s, ", ");
        }
        
        /// <summary>
        /// Converts the string representation of the quaternion into a quaternion representation (using a designated separator).
        /// </summary>
        public static Quaterniond Parse(string s, string sep)
        {
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 4) throw new FormatException("input has not exactly 4 parts");
            return new Quaterniond(double.Parse(kvp[0].Trim()), double.Parse(kvp[1].Trim()), double.Parse(kvp[2].Trim()), double.Parse(kvp[3].Trim()));
        }
        
        /// <summary>
        /// Converts the string representation of the quaternion into a quaternion representation (using a designated separator and a type provider).
        /// </summary>
        public static Quaterniond Parse(string s, string sep, IFormatProvider provider)
        {
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 4) throw new FormatException("input has not exactly 4 parts");
            return new Quaterniond(double.Parse(kvp[0].Trim(), provider), double.Parse(kvp[1].Trim(), provider), double.Parse(kvp[2].Trim(), provider), double.Parse(kvp[3].Trim(), provider));
        }
        
        /// <summary>
        /// Converts the string representation of the quaternion into a quaternion representation (using a designated separator and a number style).
        /// </summary>
        public static Quaterniond Parse(string s, string sep, NumberStyles style)
        {
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 4) throw new FormatException("input has not exactly 4 parts");
            return new Quaterniond(double.Parse(kvp[0].Trim(), style), double.Parse(kvp[1].Trim(), style), double.Parse(kvp[2].Trim(), style), double.Parse(kvp[3].Trim(), style));
        }
        
        /// <summary>
        /// Converts the string representation of the quaternion into a quaternion representation (using a designated separator and a number style and a format provider).
        /// </summary>
        public static Quaterniond Parse(string s, string sep, NumberStyles style, IFormatProvider provider)
        {
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 4) throw new FormatException("input has not exactly 4 parts");
            return new Quaterniond(double.Parse(kvp[0].Trim(), style, provider), double.Parse(kvp[1].Trim(), style, provider), double.Parse(kvp[2].Trim(), style, provider), double.Parse(kvp[3].Trim(), style, provider));
        }
        
        /// <summary>
        /// Tries to convert the string representation of the quaternion into a quaternion representation (using ', ' as a separator), returns false if string was invalid.
        /// </summary>
        public static bool TryParse(string s, out Quaterniond result)
        {
            return TryParse(s, ", ", out result);
        }
        
        /// <summary>
        /// Tries to convert the string representation of the quaternion into a quaternion representation (using a designated separator), returns false if string was invalid.
        /// </summary>
        public static bool TryParse(string s, string sep, out Quaterniond result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(s)) return false;
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 4) return false;
            double x = 0.0, y = 0.0, z = 0.0, w = 0.0;
            var ok = ((double.TryParse(kvp[0].Trim(), out x) && double.TryParse(kvp[1].Trim(), out y)) && (double.TryParse(kvp[2].Trim(), out z) && double.TryParse(kvp[3].Trim(), out w)));
            result = ok ? new Quaterniond(x, y, z, w) : Zero;
            return ok;
        }
        
        /// <summary>
        /// Tries to convert the string representation of the quaternion into a quaternion representation (using a designated separator and a number style and a format provider), returns false if string was invalid.
        /// </summary>
        public static bool TryParse(string s, string sep, NumberStyles style, IFormatProvider provider, out Quaterniond result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(s)) return false;
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 4) return false;
            double x = 0.0, y = 0.0, z = 0.0, w = 0.0;
            var ok = ((double.TryParse(kvp[0].Trim(), style, provider, out x) && double.TryParse(kvp[1].Trim(), style, provider, out y)) && (double.TryParse(kvp[2].Trim(), style, provider, out z) && double.TryParse(kvp[3].Trim(), style, provider, out w)));
            result = ok ? new Quaterniond(x, y, z, w) : Zero;
            return ok;
        }
        
        /// <summary>
        /// Returns the inner product (dot product, scalar product) of the two quaternions.
        /// </summary>
        public static double Dot(Quaterniond lhs, Quaterniond rhs)
        {
            return ((lhs.x * rhs.x + lhs.y * rhs.y) + (lhs.z * rhs.z + lhs.w * rhs.w));
        }
        
        /// <summary>
        /// Creates a quaternion from an axis and an angle (in radians).
        /// </summary>
        public static Quaterniond FromAxisAngle(double angle, Vector3d v)
        {
            var s = Math.Sin((double)angle * 0.5);
            var c = Math.Cos((double)angle * 0.5);
            return new Quaterniond((double)((double)v.x * s), (double)((double)v.y * s), (double)((double)v.z * s), (double)c);
        }
        
        /// <summary>
        /// Returns the cross product between two quaternions.
        /// </summary>
        public static Quaterniond Cross(Quaterniond q1, Quaterniond q2)
        {
            return new Quaterniond(q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y, q1.w * q2.y + q1.y * q2.w + q1.z * q2.x - q1.x * q2.z, q1.w * q2.z + q1.z * q2.w + q1.x * q2.y - q1.y * q2.x, q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z);
        }
        
        /// <summary>
        /// Calculates a proper spherical interpolation between two quaternions (only works for normalized quaternions).
        /// </summary>
        public static Quaterniond Mix(Quaterniond x, Quaterniond y, double a)
        {
            var cosTheta = (double)Dot(x, y);
            if (cosTheta > 1 - float.Epsilon)
                return Lerp(x, y, a);
            else
            {
                var angle = Math.Acos((double)cosTheta);
                return ( (Math.Sin((1 - (double)a) * angle) * x + Math.Sin((double)a * angle) * y) / Math.Sin(angle) );
            }
        }
        
        /// <summary>
        /// Calculates a proper spherical interpolation between two quaternions (only works for normalized quaternions).
        /// </summary>
        public static Quaterniond SLerp(Quaterniond x, Quaterniond y, double a)
        {
            var z = y;
            var cosTheta = (double)Dot(x, y);
            if (cosTheta < 0) { z = -y; cosTheta = -cosTheta; }
            if (cosTheta > 1 - float.Epsilon)
                return Lerp(x, z, a);
            else
            {
                var angle = Math.Acos((double)cosTheta);
                return ( (Math.Sin((1 - (double)a) * angle) * x + Math.Sin((double)a * angle) * z) / Math.Sin(angle) );
            }
        }
        
        /// <summary>
        /// Applies squad interpolation of these quaternions
        /// </summary>
        public static Quaterniond Squad(Quaterniond q1, Quaterniond q2, Quaterniond s1, Quaterniond s2, double h)
        {
            return Mix(Mix(q1, q2, h), Mix(s1, s2, h), 2 * (1 - h) * h);
        }

        #endregion


        #region Component-Wise Static Functions
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static Quaterniond Lerp(Quaterniond min, Quaterniond max, Quaterniond a)
        {
            return new Quaterniond(min.x * (1-a.x) + max.x * a.x, min.y * (1-a.y) + max.y * a.y, min.z * (1-a.z) + max.z * a.z, min.w * (1-a.w) + max.w * a.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static Quaterniond Lerp(Quaterniond min, Quaterniond max, double a)
        {
            return new Quaterniond(min.x * (1-a) + max.x * a, min.y * (1-a) + max.y * a, min.z * (1-a) + max.z * a, min.w * (1-a) + max.w * a);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static Quaterniond Lerp(Quaterniond min, double max, Quaterniond a)
        {
            return new Quaterniond(min.x * (1-a.x) + max * a.x, min.y * (1-a.y) + max * a.y, min.z * (1-a.z) + max * a.z, min.w * (1-a.w) + max * a.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static Quaterniond Lerp(Quaterniond min, double max, double a)
        {
            return new Quaterniond(min.x * (1-a) + max * a, min.y * (1-a) + max * a, min.z * (1-a) + max * a, min.w * (1-a) + max * a);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static Quaterniond Lerp(double min, Quaterniond max, Quaterniond a)
        {
            return new Quaterniond(min * (1-a.x) + max.x * a.x, min * (1-a.y) + max.y * a.y, min * (1-a.z) + max.z * a.z, min * (1-a.w) + max.w * a.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static Quaterniond Lerp(double min, Quaterniond max, double a)
        {
            return new Quaterniond(min * (1-a) + max.x * a, min * (1-a) + max.y * a, min * (1-a) + max.z * a, min * (1-a) + max.w * a);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static Quaterniond Lerp(double min, double max, Quaterniond a)
        {
            return new Quaterniond(min * (1 - a.x) + max * a.x, min * (1 - a.y) + max * a.y, min * (1 - a.z) + max * a.z, min * (1 - a.w) + max * a.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from the application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static Quaterniond Lerp(double min, double max, double a)
        {
            return new Quaterniond(min * (1-a) + max * a);
        }

        #endregion


        #region Component-Wise Operator Overloads
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator+ (identity).
        /// </summary>
        public static Quaterniond operator+(Quaterniond v)
        {
            return v;
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator- (-v).
        /// </summary>
        public static Quaterniond operator-(Quaterniond v)
        {
            return new Quaterniond(-v.x, -v.y, -v.z, -v.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator+ (lhs + rhs).
        /// </summary>
        public static Quaterniond operator+(Quaterniond lhs, Quaterniond rhs)
        {
            return new Quaterniond(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator+ (lhs + rhs).
        /// </summary>
        public static Quaterniond operator+(Quaterniond lhs, double rhs)
        {
            return new Quaterniond(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator+ (lhs + rhs).
        /// </summary>
        public static Quaterniond operator+(double lhs, Quaterniond rhs)
        {
            return new Quaterniond(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator- (lhs - rhs).
        /// </summary>
        public static Quaterniond operator-(Quaterniond lhs, Quaterniond rhs)
        {
            return new Quaterniond(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator- (lhs - rhs).
        /// </summary>
        public static Quaterniond operator-(Quaterniond lhs, double rhs)
        {
            return new Quaterniond(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator- (lhs - rhs).
        /// </summary>
        public static Quaterniond operator-(double lhs, Quaterniond rhs)
        {
            return new Quaterniond(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator* (lhs * rhs).
        /// </summary>
        public static Quaterniond operator*(Quaterniond lhs, double rhs)
        {
            return new Quaterniond(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator* (lhs * rhs).
        /// </summary>
        public static Quaterniond operator*(double lhs, Quaterniond rhs)
        {
            return new Quaterniond(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
        }
        
        /// <summary>
        /// Returns a Quaterniond from component-wise application of operator/ (lhs / rhs).
        /// </summary>
        public static Quaterniond operator/(Quaterniond lhs, double rhs)
        {
            return new Quaterniond(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
        }

        #endregion

    }
}
