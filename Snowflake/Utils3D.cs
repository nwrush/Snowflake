using System;
using System.Text;

using Mogre;
using Math = Mogre.Math;

namespace Snowflake {
    public class Utils3D {

        /// <summary>
        /// Rotates the specified vector clockwise by the specified angle, in radians
        /// </summary>
        /// <param name="vec">The vector2 to rotate</param>
        /// <param name="radians">The number of radians to rotate clockwise by</param>
        /// <returns>The rotated vector</returns>
        public static Vector2 RotateVector2(Vector2 vec, float rad) {
            return new Vector2(vec.x * Math.Cos(rad) + vec.y * Math.Sin(rad), vec.x * Math.Sin(rad) - vec.y * Math.Cos(rad));
        }
    }
}
